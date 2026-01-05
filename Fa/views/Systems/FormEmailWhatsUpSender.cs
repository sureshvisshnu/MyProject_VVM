using fa;
using fa;
using fa.api.Hms;
using fa.api.OrderManagement;
using fa.model.Hms.Master;
using fa.model.OrderManagement;
using fa.views;
using fa.views.Systems;
using fa.views.utils;
using FADataAccessLibrary.Api.Hms;
using Gnostice.Documents.Controls.WinForms;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Twilio;
using Twilio.Clients;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Fa.views.Systems
{
    public partial class FormEmailWhatsUpSender : FormBase
    {
        public long LabResultId { get; set; }
        public long SaleEntryId { get; set; }
        public string? PaperSelection { get; set; }
        // public RptLabTestResult? RptLabTestResult { get; set; }
        public ISharableDocument? Document { get; set; }
        public ShareDocumentType CurrentShareType = ShareDocumentType.None;

        public string? PdfStoredPath { get; set; }
        private bool HasOwnEmailServer = false;

        public FormEmailWhatsUpSender()
        {
            InitializeComponent();
        }

        public void ResetForm()
        {
            TextBoxSnderEmailAddress.Text = Global.Company.ContactInfo.Email;
            TextBoxSnderEmailAddress.Enabled = false;
            TextBoxMailPassWord.Text = string.Empty;
            TextBoxReceiverEmailAddress.Text = string.Empty;
            TextBoxCCEmailAddress.Text = string.Empty;
            TextBoxMailPassWord.Select();
        }
        private void LoadDocument()
        {
            try
            {
                if (Document == null)
                {
                    MessageBox.Show("No document selected.", "Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Cursor.Current = Cursors.WaitCursor;

                PdfStoredPath = Document.GeneratePdf();
                FileName.Text = Path.GetFileName(PdfStoredPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to generate document.\n" + ex.Message,
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return false;

            if (!phoneNumber.All(char.IsDigit))
                return false;

            return phoneNumber.Length >= 10 && phoneNumber.Length <= 15;
        }

        private bool IsValidIndianPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return false;

            return phoneNumber.StartsWith("+") && phoneNumber.Length >= 12;
        }
        private bool IsValidIndianPhoneNumber(string input, out string normalizedPhone)
        {
            normalizedPhone = "";

            if (string.IsNullOrWhiteSpace(input))
                return false;

            string digitsOnly = new string(input.Where(char.IsDigit).ToArray());

            if (digitsOnly.Length == 10)
                digitsOnly = "91" + digitsOnly;

            if (digitsOnly.Length < 10 || digitsOnly.Length > 15)
                return false;

            normalizedPhone = digitsOnly;
            return true;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Escape))
            {
                BtnWhatsUpEmailCancel.PerformClick();
                return false;
            }
            else if (keyData == (Keys.F10))
            {
                BtnWhatsUEmailExit.PerformClick();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void BtnWhatsUpEmailSend_Click(object sender, EventArgs e)
        {
            try
            {
                if (Document == null)
                {
                    MessageBox.Show("No document selected for sharing.", "Warning",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string generatedPdfPath = Document.GeneratePdf();

                SendToWhatsApp(generatedPdfPath);

                if (CheckBoxSendEmail.Checked)
                {
                    if (string.IsNullOrWhiteSpace(TextBoxReceiverEmailAddress.Text))
                    {
                        MessageBox.Show("Please enter a recipient email address.", "Email Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    SendByEmail(generatedPdfPath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Share Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void SendToWhatsApp(string pdfPath)
        {
            try
            {
                if (Document == null)
                    throw new Exception("No document selected for What'sApp sharing.");

                if (!File.Exists(pdfPath))
                    throw new FileNotFoundException("PDF not found for What'sApp sharing.");

                string receiverPhone = TextBoxReceiverWhatsUpNo.Text.Trim();

                if (!IsValidIndianPhoneNumber(receiverPhone, out string finalPhone))
                {
                    MessageBox.Show(
                        "Please enter a valid phone number.\nExample: 9876543210 or 919876543210",
                        "Validation Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    return;
                }

                Cursor.Current = Cursors.WaitCursor;

                string safeName = string.Join(
                    "_", Document.ContactName.Split(Path.GetInvalidFileNameChars())
                );

                string newPdfPath = Path.Combine(
                    Path.GetTempPath(),
                    $"{safeName}_{Document.FilePrefix}.pdf"
                );

                File.Copy(pdfPath, newPdfPath, true);

                string driveLink = GoogleDriveUploader.UploadPdfAndGetLink(newPdfPath);

                if (string.IsNullOrWhiteSpace(driveLink))
                {
                    MessageBox.Show(
                        "Failed to upload file to Google Drive.",
                        "Upload Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    return;
                }

                int validityDays = 3;

                string message =
                    $"Dear {Document.ContactName},\n" +
                    $"Your {Document.DocumentName} is ready.\n" +
                    $"You can download it using the link below:\n{driveLink}\n\n" +
                    $"Please note: The link is valid for {validityDays} days only.\n\n" +
                    $"Regards,\n{Global.Company.Name}";

                string encodedMessage = Uri.EscapeDataString(message);

                //string whatsappUrl = $"https://wa.me/{finalPhone}?text={encodedMessage}";
                string whatsappUrl = $"whatsapp://send?phone={finalPhone}&text={encodedMessage}";

                System.Diagnostics.Process.Start(new ProcessStartInfo
                {
                    FileName = whatsappUrl,
                    UseShellExecute = true
                });

                MessageBox.Show(
                    "What'sApp opened successfully.\n\nClick SEND to deliver the document.",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Failed to send What'sApp message.\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            finally
            {
                WhatsUpEmailErrorMsg.Text = "What'sApp opened successfully. Please confirm delivery.";
                Cursor.Current = Cursors.Default;
            }
        }
        private void SendByEmail(string pdfPath)
        {
            try
            {
                if (!File.Exists(pdfPath))
                    throw new FileNotFoundException("PDF not found for Email sharing.");

                string receiverEmail = TextBoxReceiverEmailAddress.Text.Trim();
                if (!IsValidEmail(receiverEmail))
                {
                    WhatsUpEmailErrorMsg.Text = "Please enter a valid Receiver Email address.";
                    TextBoxReceiverEmailAddress.Select();
                    return;
                }

                string ccEmail = TextBoxCCEmailAddress.Text.Trim();
                bool hasCC = !string.IsNullOrWhiteSpace(ccEmail);
                if (hasCC && !IsValidEmail(ccEmail))
                {
                    WhatsUpEmailErrorMsg.Text = "Please enter a valid CC Email address.";
                    TextBoxCCEmailAddress.Select();
                    return;
                }

                string appPassword = "";

                if (HasOwnEmailServer)
                {
                    appPassword = GetEmailAppPassword();
                    if (string.IsNullOrEmpty(appPassword))
                    {
                        if (MessageBox.Show("Email App password is not set in Workstation. Do you want to open Workstation Setup to set it now?",
                                          "Password Required",
                                          MessageBoxButtons.YesNo,
                                          MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            OpenWorkstationSetup();
                        }
                        return;
                    }
                }
                else
                {
                    appPassword = TextBoxMailPassWord.Text.Trim();
                    if (string.IsNullOrEmpty(appPassword))
                    {
                        MessageBox.Show("Please enter email password.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        TextBoxMailPassWord.Focus();
                        return;
                    }
                }

                Cursor.Current = Cursors.WaitCursor;

                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(Global.Company.ContactInfo.Email);
                    mail.To.Add(TextBoxReceiverEmailAddress.Text.Trim());

                    if (!string.IsNullOrEmpty(TextBoxCCEmailAddress.Text.Trim()))
                    {
                        mail.CC.Add(TextBoxCCEmailAddress.Text.Trim());
                    }

                    mail.Subject = "Your Lab Test Report";
                    mail.Body = $"Dear Patient,\n\nPlease find attached your Lab Test Report.\n\nRegards,\n{Global.Company.Name}";

                    mail.Attachments.Add(new Attachment(PdfStoredPath!));

                    var (smtpServer, port) = GetSmtpSettings(Global.Company.ContactInfo.Email);

                    using (SmtpClient smtp = new SmtpClient(smtpServer, port))
                    {
                        smtp.Credentials = new NetworkCredential(
                            Global.Company.ContactInfo.Email,
                            appPassword
                        );
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }
                }

                MessageBox.Show("Lab Test Report emailed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to send Lab Test Report.\n" + ex.Message, "Email Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);

                if (!email.Contains("@")) return false;
                if (email.Contains("..")) return false;

                string[] parts = email.Split('@');
                if (parts.Length != 2) return false;

                if (!parts[1].Contains(".")) return false;

                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        private void OpenWorkstationSetup()
        {
            try
            {
                FormWorkStationSetup workstationForm = new FormWorkStationSetup();
                workstationForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to open Workstation Setup: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private string GetEmailAppPassword()
        {
            try
            {
                RegistryKey? key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Ab2App");
                if (key != null)
                {
                    string? appPass = key.GetValue("DefaultEmailAppPass")?.ToString();
                    key.Close();

                    if (!string.IsNullOrEmpty(appPass))
                    {
                        return appPass;
                    }
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error reading email password from registry: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return string.Empty;
            }
        }
        private (string smtpServer, int port) GetSmtpSettings(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new Exception("Email address is empty.");

            string domain = email.Split('@')[1].Trim().ToLower();

            // Known providers
            if (domain == "gmail.com")
                return ("smtp.gmail.com", 587);

            if (domain == "yahoo.com")
                return ("smtp.mail.yahoo.com", 587);

            if (domain == "hotmail.com" || domain == "outlook.com" || domain == "live.com")
                return ("smtp-mail.outlook.com", 587);

            if (domain == "office365.com")
                return ("smtp.office365.com", 587);

            if (domain == "zoho.com")
                return ("smtp.zoho.com", 587);

            if (domain == "zoho.in")
                return ("smtp.zoho.in", 587);

            string[] smtpPatterns =
            {
                $"smtp.{domain}",
                $"mail.{domain}",
                $"smtp-mail.{domain}",
                $"smtp.mail.{domain}"
            };

            foreach (var server in smtpPatterns)
            {
                if (IsSmtpServerAvailable(server, 587))
                {
                    return (server, 587);
                }
            }

            return ($"smtp.{domain}", 587);
        }
        private bool IsSmtpServerAvailable(string smtpServer, int port)
        {
            try
            {
                using (var client = new TcpClient())
                {
                    var result = client.BeginConnect(smtpServer, port, null, null);
                    bool success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(2));

                    if (!success) return false;

                    client.EndConnect(result);
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        private void FormEmailWhatsUpSender_Load(object sender, EventArgs e)
        {
            ResetForm();
            //StoreLabTestResult(RptLabTestResult!)
            //Document = new SalesInvoiceSharable(SaleEntryId, PaperSelection);
            LoadDocument();
            //CheckEmailServerType();

        }
    }
    public class FileIoResponse
    {
        public bool success { get; set; }
        public string? key { get; set; }
        public string? link { get; set; }
    }
    public enum ShareDocumentType
    {
        None = 0,
        LabReport = 1,
        SalesInvoice = 2,
        // Future:
        Prescription = 3,
        Quotation = 4
    }

    public interface ISharableDocument
    {
        string DocumentName { get; }
        string FilePrefix { get; }
        string ContactName { get; }
        string GeneratePdf();
    }


    public class SalesInvoiceSharable : ISharableDocument
    {
        private readonly long _saleEntryId;
        private readonly string _paperSelection;

        public SalesInvoiceSharable(long saleEntryId, string paperSelection)
        {
            _saleEntryId = saleEntryId;
            _paperSelection = paperSelection;
        }

        public string DocumentName => "Sales Invoice";

        public string FilePrefix => "SALES_BILL";

        public string ContactName
        {
            get
            {
                try
                {
                    var sale = SalesManager.Instance.GetSaleEntry(_saleEntryId);
                    if (sale != null && !string.IsNullOrWhiteSpace(sale.CustomerName))
                        return sale.CustomerName.Trim();

                    return "Customer";
                }
                catch
                {
                    return "CashSales";
                }
            }
        }

        public string GeneratePdf()
        {
            //PrinterSetup.SalePrintSetup(
            //    _saleEntryId,
            //    false,
            //    Entrytype.SALE,
            //    "Sales",
            //    _paperSelection,
            //    true
            //);

            PrinterSetup.SalePrintAndWhatsUpSetup(
                    _saleEntryId,
                    false,
                    Entrytype.SALE,
                    false, false,
                    _paperSelection!, true);

            string lastPdf = PrinterSetup.LastGeneratedFilePathForWE;

            if (string.IsNullOrWhiteSpace(lastPdf) || !File.Exists(lastPdf))
                throw new FileNotFoundException("Sales Invoice PDF was not generated.");

            string safeName = string.Join(
                "_", ContactName.Split(Path.GetInvalidFileNameChars())
            );

            string newPath = Path.Combine(
                Path.GetTempPath(),
                $"{safeName}_{FilePrefix}_{_saleEntryId}.pdf"
            );

            File.Copy(lastPdf, newPath, true);
            return newPath;
        }
    }


}
