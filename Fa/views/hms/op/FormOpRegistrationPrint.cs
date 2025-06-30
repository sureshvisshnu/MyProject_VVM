using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTextSharp.text.pdf;
using fa.model.Common;
using System.Diagnostics;
using System.IO;
using iTextSharp.text.pdf.draw;
using fa.Printing;
using System.Drawing.Printing;
using fa.api.utils;
using fa.api.Hms;
using fa.model.Hms.Op;
namespace fa.views.hms.op
{
    public partial class FormOPRegistrationPrint : Form
    {
        public bool TokenSheet;
        public bool FeeReceipt;
        public string TknNo;
        public string PName;
        public string GetAge;
        public string RegFee;
        public string RecFee;
        public string PAdress;
        public string PNo;
        public string RefNo;
        public DateTime TknDate;

        public int PageW = 0;
        public int PageH = 0;

        public PictureBox CompanyPictureBoxLogo = new PictureBox();
        public TextBox TextBoxPatientId = new TextBox();
        public TextBox TextBoxRegtId = new TextBox();
        public DataGridView Dgv = new DataGridView();
        public string CompnyAddress = string.Empty;
        public string CompanysName = string.Empty;

        public FormOPRegistrationPrint()
        {
            InitializeComponent();
        }

        private void FormOPRegistrationPrint_Load(object sender, EventArgs e)
        {
            CheckBoxFeeReceipt.Checked = !FeeReceipt;
            if (FeeReceipt)
            {
                CheckBoxFeeReceipt.Enabled = false;
            }
            else
            {
                CheckBoxFeeReceipt.Enabled = true;
            }

            if (Global.Company.Logo != null)
            {
                MemoryStream Stream = new MemoryStream(Global.Company.Logo);
                CompanyPictureBoxLogo.Image = System.Drawing.Image.FromStream(Stream);
            }

            CompanysName = Global.Company.DisplayAs + "\n";

            Address GetAddress = Global.Company.Address;
            if (GetAddress != null)
            {
                CompnyAddress = (GetAddress.AddressLine1 == "") ? "" : GetAddress.AddressLine1;
                CompnyAddress = CompnyAddress + ((GetAddress.AddressLine2 == "") ? "" : GetAddress.AddressLine2);
                CompnyAddress = CompnyAddress + ((GetAddress.CityOrTown == "") ? "" : GetAddress.CityOrTown);
                CompnyAddress = CompnyAddress + ((GetAddress.District == "") ? "" : GetAddress.District);
                CompnyAddress = CompnyAddress + ((GetAddress.StateName == "") ? "" : GetAddress.StateName + " - ");
                CompnyAddress = CompnyAddress + ((GetAddress.PinCode == "") ? "" : GetAddress.PinCode + ".\n");
            }
        }

        private void CheckBoxToken_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBoxFeeReceipt.Checked == false && CheckBoxToken.Checked == false)
            {
                BtnPrint.Enabled = false;
            }
            else
            {
                BtnPrint.Enabled = true;
            }
        }

        private void CheckBoxFeeReceipt_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBoxFeeReceipt.Checked == false && CheckBoxToken.Checked == false)
            {
                BtnPrint.Enabled = false;
            }
            else
            {
                BtnPrint.Enabled = true;
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (CheckBoxFeeReceipt.Checked == true && CheckBoxToken.Checked == true)
            {
                OpTokenPrinting OpReceiptPrinting = new OpTokenPrinting();
                OpReceiptPrinting.PrintTokeninPDF(int.Parse(TextBoxRegtId.Text), true);
                OpReceiptPrinting.PrintReceiptFromOp(int.Parse(TextBoxRegtId.Text), true);
            }
            else if (CheckBoxFeeReceipt.Checked == false && CheckBoxToken.Checked == true)
            {
                OpTokenPrinting OpReceiptPrinting = new OpTokenPrinting();
                OpReceiptPrinting.PrintTokeninPDF(int.Parse(TextBoxRegtId.Text), true);

            }
            else if (CheckBoxFeeReceipt.Checked == true && CheckBoxToken.Checked == false)
            {
                OpTokenPrinting OpReceiptPrinting = new OpTokenPrinting();
                OpReceiptPrinting.PrintReceiptFromOp(int.Parse(TextBoxRegtId.Text), true);
            }
            Cursor.Current = Cursors.Default;
            BtnPrint.Focus();
        }

        private void PrintDocumentDirect_QueryPageSettings(object sender, System.Drawing.Printing.QueryPageSettingsEventArgs e)
        {
            int pw = 0;
            int ph = 0;

            pw = PageW;
            ph = PageH;

            PaperSize ps = new PaperSize("CS", pw, ph);
            var pset = new PageSettings();
            {
                pset.PaperSize = ps;
                pset.Margins = new Margins(0, 0, 0, 0);

                pset.PrinterResolution.X = 96;
                pset.PrinterResolution.Y = 96;

                e.PageSettings = pset;
            }

        }

        private void PrePrntTokenDesgn(System.Drawing.Printing.PrintPageEventArgs e)
        {
            // Construct string format and alignment 

            StringFormat StrTxtFrmtC = new StringFormat();
            StringFormat StrTxtFrmtR = new StringFormat();
            StringFormat StrTxtFrmtL = new StringFormat();

            // Set alignment, line alignment, and trimming properties of a string 

            StrTxtFrmtC.Alignment = StringAlignment.Center;
            StrTxtFrmtC.LineAlignment = StringAlignment.Center;
            StrTxtFrmtC.Trimming = StringTrimming.EllipsisCharacter;

            StrTxtFrmtR.Alignment = StringAlignment.Far;
            StrTxtFrmtR.LineAlignment = StringAlignment.Near;
            StrTxtFrmtR.Trimming = StringTrimming.Character;

            StrTxtFrmtL.Alignment = StringAlignment.Near;
            StrTxtFrmtL.LineAlignment = StringAlignment.Near;
            StrTxtFrmtL.Trimming = StringTrimming.Character;

            // Construct Brushes
            SolidBrush BluBrsh = new SolidBrush(Color.Blue);
            SolidBrush RedBrsh = new SolidBrush(Color.Red);
            SolidBrush GrnBrsh = new SolidBrush(Color.Green);
            SolidBrush WitBrsh = new SolidBrush(Color.White);
            SolidBrush BlkBrsh = new SolidBrush(Color.Black);
            SolidBrush GryBrsh = new SolidBrush(Color.LightGray);
            SolidBrush TrnspBrsh = new SolidBrush(Color.Transparent);

            // Construct Font 
            string RunningPath = AppDomain.CurrentDomain.BaseDirectory;
            string FONT = string.Format("{0}Resources\\CenturyGothic.ttf", Path.GetFullPath(Path.Combine(RunningPath, @"..\..\")));

            Font DrftFnt = new Font(FONT, 11, FontStyle.Regular);
            Font hdrFont = new Font(FONT, 10, FontStyle.Bold);
            Font TxtFnt = new Font(FONT, 7, FontStyle.Regular);
            Font TxtFntSmall = new Font(FONT, 6, FontStyle.Regular);

            // Create a Rectangle
            int RwPt = 0; // Y Point
            int ClPt = 0; // X Point
            int ClWd = 0; // Column Width
            int RwHg = 0; // Row Height

            ClPt = 20;
            ClWd = 191;
            RwHg = 50; //DrftFnt.Height + 2; // 40
            RwPt = 20;

            Rectangle rect = new Rectangle(ClPt, RwPt, ClWd, RwHg);

            Registration Registration = OpManager.Instance.GetRegisterByPatientId(int.Parse(TextBoxPatientId.Text));


            MemoryStream Barcode = new MemoryStream();
            var Image = BarCode.GenerateImageBarcode1(Registration.Patient.PatientNumber);
            Image.Save(Barcode, System.Drawing.Imaging.ImageFormat.Png);
            Image image = Image.FromStream(Barcode);

            rect.Height = DrftFnt.Height + 2;
            e.Graphics.DrawString("TOKEN", DrftFnt, BlkBrsh, rect, StrTxtFrmtC);
            e.Graphics.DrawRectangle(Pens.Transparent, rect.X, rect.Y, rect.Width, rect.Height);


            int sp = 0;
            sp = ClWd - 100;
            sp = sp / 2;
            rect.X = sp + 20;
            rect.Y = rect.Y + rect.Height;
            rect.Width = 100;
            rect.Height = TxtFnt.Height + 2;
            e.Graphics.DrawString("Date : " + String.Format("{0:d}", TknDate), TxtFnt, BlkBrsh, rect, StrTxtFrmtC);
            e.Graphics.DrawRectangle(Pens.Transparent, rect.X, rect.Y, rect.Width, rect.Height);

            sp = ClWd - 100;
            sp = sp / 2;
            rect.X = sp + 20;
            rect.Y = rect.Y + rect.Height;
            rect.Width = 100;
            rect.Height = TxtFnt.Height + 2;
            e.Graphics.DrawString("Token No : " + TknNo, TxtFnt, BlkBrsh, rect, StrTxtFrmtC);
            e.Graphics.DrawRectangle(Pens.Blue, rect.X, rect.Y, rect.Width, rect.Height);

            sp = ClWd - 100;
            sp = sp / 2;
            rect.X = sp + 20;
            rect.Y = rect.Y + rect.Height;
            rect.Width = 100;
            rect.Height = TxtFntSmall.Height + 2;
            e.Graphics.DrawString("Name : " + PName.ToString(), TxtFntSmall, BlkBrsh, rect, StrTxtFrmtL);
            e.Graphics.DrawRectangle(Pens.Transparent, rect.X, rect.Y, rect.Width, rect.Height);

            sp = ClWd - 100;
            sp = sp / 2;
            rect.X = sp + 20;
            rect.Y = rect.Y + rect.Height;
            rect.Width = 100;
            rect.Height = TxtFntSmall.Height + 2;
            e.Graphics.DrawString("Age  : " + GetAge.ToString(), TxtFntSmall, BlkBrsh, rect, StrTxtFrmtL);
            e.Graphics.DrawRectangle(Pens.Transparent, rect.X, rect.Y, rect.Width, rect.Height);

            sp = ClWd - 70;
            sp = sp / 2;
            rect.X = sp + 20;
            rect.Y = rect.Y + rect.Height;
            rect.Width = 70;
            rect.Height = 20;
            //e.Graphics.DrawString(Temp., TxtFnt, BlkBrsh, rect, StrTxtFrmtC);
            e.Graphics.DrawImage(image, rect.X, rect.Y, rect.Width, rect.Height);
            e.Graphics.DrawRectangle(Pens.Transparent, rect.X, rect.Y, rect.Width, rect.Height);

            sp = ClWd - 90;
            sp = sp / 2;
            rect.X = sp + 20;
            rect.Y = rect.Y + rect.Height;
            rect.Width = 90;
            rect.Height = TxtFntSmall.Height + 2;
            e.Graphics.DrawString("GET WELL SOON", TxtFntSmall, BlkBrsh, rect, StrTxtFrmtC);
            e.Graphics.DrawRectangle(Pens.Transparent, rect.X, rect.Y, rect.Width, rect.Height);
        }

        private void PrePrntReceiptDesgn(System.Drawing.Printing.PrintPageEventArgs e)
        {
            // Construct string format and alignment 

            StringFormat StrTxtFrmtC = new StringFormat();
            StringFormat StrTxtFrmtR = new StringFormat();
            StringFormat StrTxtFrmtL = new StringFormat();

            // Set alignment, line alignment, and trimming properties of a string 

            StrTxtFrmtC.Alignment = StringAlignment.Center;
            StrTxtFrmtC.LineAlignment = StringAlignment.Center;
            StrTxtFrmtC.Trimming = StringTrimming.EllipsisCharacter;

            StrTxtFrmtR.Alignment = StringAlignment.Far;
            StrTxtFrmtR.LineAlignment = StringAlignment.Near;
            StrTxtFrmtR.Trimming = StringTrimming.Character;

            StrTxtFrmtL.Alignment = StringAlignment.Near;
            StrTxtFrmtL.LineAlignment = StringAlignment.Near;
            StrTxtFrmtL.Trimming = StringTrimming.Character;

            // Construct Brushes
            SolidBrush BluBrsh = new SolidBrush(Color.Blue);
            SolidBrush RedBrsh = new SolidBrush(Color.Red);
            SolidBrush GrnBrsh = new SolidBrush(Color.Green);
            SolidBrush WitBrsh = new SolidBrush(Color.White);
            SolidBrush BlkBrsh = new SolidBrush(Color.Black);
            SolidBrush GryBrsh = new SolidBrush(Color.LightGray);
            SolidBrush TrnspBrsh = new SolidBrush(Color.Transparent);

            // Construct Font 
            string RunningPath = AppDomain.CurrentDomain.BaseDirectory;
            string FONT = string.Format("{0}Resources\\CenturyGothic.ttf", Path.GetFullPath(Path.Combine(RunningPath, @"..\..\")));

            Font MainFont = new Font(FONT, 7, FontStyle.Bold);
            Font DrftFnt = new Font(FONT, 11, FontStyle.Regular);
            Font hdrFont = new Font(FONT, 8, FontStyle.Bold);
            Font TxtFnt = new Font(FONT, 7, FontStyle.Regular);
            Font TxtFntSmall = new Font(FONT, 6, FontStyle.Regular);

            // Create a Rectangle
            int RwPt = 0; // Y Point
            int ClPt = 0; // X Point
            int ClWd = 0; // Column Width
            int RwHg = 0; // Row Height

            ClPt = 20;
            ClWd = 191;
            RwHg = 50; //DrftFnt.Height + 2; // 40
            RwPt = 20;

            Rectangle rect = new Rectangle(ClPt, RwPt, ClWd, RwHg);
            {
                rect.Width = 50;
                rect.Height = RwHg;
            }

            // capture company logo
            //e.Graphics.DrawString("Logo", DrftFnt, BlkBrsh, rect, StrTxtFrmtC);
            if (Global.Company.Logo != null)
            {
                MemoryStream logoImg = new MemoryStream(Global.Company.Logo);
                CompanyPictureBoxLogo.Image = System.Drawing.Image.FromStream(logoImg);
                Image logo = Image.FromStream(logoImg);
                e.Graphics.DrawImage(logo, rect.X, rect.Y, rect.Width, rect.Height);
                e.Graphics.DrawRectangle(Pens.Transparent, rect.X, rect.Y, rect.Width, rect.Height);
            }
            else
            {
                e.Graphics.DrawString("", DrftFnt, BlkBrsh, rect, StrTxtFrmtC);
                e.Graphics.DrawRectangle(Pens.Transparent, rect.X, rect.Y, rect.Width, rect.Height);

            }

            rect.X = 71;
            rect.Width = 140;
            rect.Height = MainFont.Height;

            e.Graphics.DrawString(CompanysName, MainFont, BlkBrsh, rect, StrTxtFrmtL);
            e.Graphics.DrawRectangle(Pens.Transparent, rect.X, rect.Y, rect.Width, rect.Height);

            rect.X = 71;
            rect.Y = rect.Y + rect.Height;
            rect.Width = 140;
            rect.Height = RwHg - rect.Height;

            e.Graphics.DrawString(CompnyAddress, TxtFntSmall, BlkBrsh, rect, StrTxtFrmtL);
            e.Graphics.DrawRectangle(Pens.Transparent, rect.X, rect.Y, rect.Width, rect.Height);

            rect.X = ClPt;
            rect.Y = rect.Y + rect.Height;
            rect.Width = ClWd;
            rect.Height = 10;

            e.Graphics.DrawString("", DrftFnt, BlkBrsh, rect, StrTxtFrmtC);
            e.Graphics.DrawRectangle(Pens.Transparent, rect.X, rect.Y, rect.Width, rect.Height);

            rect.X = ClPt;
            rect.Y = rect.Y + rect.Height;
            rect.Width = ClWd;
            rect.Height = hdrFont.Height + 2;

            e.Graphics.DrawString("OP Fee Receipt", hdrFont, BlkBrsh, rect, StrTxtFrmtC);
            e.Graphics.DrawRectangle(Pens.Transparent, rect.X, rect.Y, rect.Width, rect.Height);

            rect.X = ClPt;
            rect.Y = rect.Y + rect.Height;

            rect.Width = ClWd;
            rect.Height = DrftFnt.Height;

            e.Graphics.DrawString("", DrftFnt, BlkBrsh, rect, StrTxtFrmtC);
            e.Graphics.DrawRectangle(Pens.Transparent, rect.X, rect.Y, rect.Width, rect.Height);

            rect.X = ClPt;
            rect.Y = rect.Y + rect.Height;

            rect.Width = 95;
            rect.Height = TxtFnt.Height + 2;

            e.Graphics.DrawString("Date : " + String.Format("{0:d}", TknDate), TxtFnt, BlkBrsh, rect, StrTxtFrmtL);
            e.Graphics.DrawRectangle(Pens.Transparent, rect.X, rect.Y, rect.Width, rect.Height);

            rect.X = 116;
            // rect.Y = rect.Y + rect.Height;
            rect.Height = TxtFnt.Height + 2;

            e.Graphics.DrawString("Receipt # " + RefNo, TxtFnt, BlkBrsh, rect, StrTxtFrmtR);
            e.Graphics.DrawRectangle(Pens.Transparent, rect.X, rect.Y, rect.Width, rect.Height);

            rect.X = ClPt;
            rect.Y = rect.Y + rect.Height;

            rect.Width = ClWd;
            rect.Height = DrftFnt.Height;

            e.Graphics.DrawString("", DrftFnt, BlkBrsh, rect, StrTxtFrmtC);
            e.Graphics.DrawRectangle(Pens.Transparent, rect.X, rect.Y, rect.Width, rect.Height);

            rect.X = ClPt + 1;
            rect.Y = rect.Y + rect.Height;
            rect.Width = 80;
            rect.Height = TxtFnt.Height;

            e.Graphics.DrawRectangle(Pens.Black, rect.X, rect.Y, rect.Width, rect.Height);
            e.Graphics.FillRectangle(GryBrsh, rect);
            e.Graphics.DrawString("Patient Id ", TxtFnt, BlkBrsh, rect, StrTxtFrmtL);

            rect.X = 100 + 1;
            //rect.Y = 172;
            rect.Width = 111;
            rect.Height = TxtFnt.Height;

            e.Graphics.DrawString(PNo, TxtFnt, BlkBrsh, rect, StrTxtFrmtL);
            e.Graphics.DrawRectangle(Pens.Black, rect.X, rect.Y, rect.Width, rect.Height);

            rect.X = ClPt + 1;
            rect.Y = rect.Y + rect.Height;
            rect.Width = 80;
            rect.Height = TxtFnt.Height;

            e.Graphics.DrawRectangle(Pens.Black, rect.X, rect.Y, rect.Width, rect.Height);
            e.Graphics.FillRectangle(GryBrsh, rect);
            e.Graphics.DrawString("Patient Name ", TxtFnt, BlkBrsh, rect, StrTxtFrmtL);

            rect.X = 100 + 1;
            //rect.Y = rect.Y + rect.Height;
            rect.Width = 111;
            rect.Height = TxtFnt.Height;

            e.Graphics.DrawString(PName, TxtFnt, BlkBrsh, rect, StrTxtFrmtL);
            e.Graphics.DrawRectangle(Pens.Black, rect.X, rect.Y, rect.Width, rect.Height);

            rect.X = ClPt + 1;
            rect.Y = rect.Y + rect.Height;
            rect.Width = 80;
            rect.Height = (4 * TxtFnt.Height);

            e.Graphics.DrawRectangle(Pens.Black, rect.X, rect.Y, rect.Width, rect.Height);
            e.Graphics.FillRectangle(GryBrsh, rect);
            e.Graphics.DrawString("Address ", TxtFnt, BlkBrsh, rect, StrTxtFrmtL);

            rect.X = 100 + 1;
            //rect.Y = rect.Y + rect.Height;
            rect.Width = 111;
            rect.Height = (4 * TxtFnt.Height);

            e.Graphics.DrawString(PAdress, TxtFnt, BlkBrsh, rect, StrTxtFrmtL);
            e.Graphics.DrawRectangle(Pens.Black, rect.X, rect.Y, rect.Width, rect.Height);

            rect.X = ClPt + 1;
            rect.Y = rect.Y + rect.Height;
            rect.Width = 80;
            rect.Height = TxtFnt.Height;

            e.Graphics.DrawRectangle(Pens.Black, rect.X, rect.Y, rect.Width, rect.Height);
            e.Graphics.FillRectangle(GryBrsh, rect);
            e.Graphics.DrawString("Registration Fee ", TxtFnt, BlkBrsh, rect, StrTxtFrmtL);

            rect.X = 100 + 1;
            //rect.Y = rect.Y + rect.Height;
            rect.Width = 111;
            rect.Height = TxtFnt.Height;

            e.Graphics.DrawString(RegFee, TxtFnt, BlkBrsh, rect, StrTxtFrmtL);
            e.Graphics.DrawRectangle(Pens.Black, rect.X, rect.Y, rect.Width, rect.Height);

            rect.X = ClPt + 1;
            rect.Y = rect.Y + rect.Height;
            rect.Width = 80;
            rect.Height = TxtFnt.Height;

            e.Graphics.DrawRectangle(Pens.Black, rect.X, rect.Y, rect.Width, rect.Height);
            e.Graphics.FillRectangle(GryBrsh, rect);
            e.Graphics.DrawString("Fee Received ", TxtFnt, BlkBrsh, rect, StrTxtFrmtL);

            rect.X = 100 + 1;
            //rect.Y = rect.Y + rect.Height;
            rect.Width = 111;
            rect.Height = TxtFnt.Height;

            e.Graphics.DrawString(RecFee, TxtFnt, BlkBrsh, rect, StrTxtFrmtL);
            e.Graphics.DrawRectangle(Pens.Black, rect.X, rect.Y, rect.Width, rect.Height);

            rect.X = ClPt;
            rect.Y = rect.Y + rect.Height;
            rect.Width = ClWd;
            rect.Height = TxtFnt.Height;

            e.Graphics.DrawString("", TxtFnt, BlkBrsh, rect, StrTxtFrmtC);
            e.Graphics.DrawRectangle(Pens.Transparent, rect.X, rect.Y, rect.Width, rect.Height);

            rect.X = ClPt + 1;
            rect.Y = rect.Y + rect.Height;
            rect.Width = ClWd;
            rect.Height = TxtFnt.Height + 2;

            e.Graphics.DrawString("Printed on " + DateTime.Now, TxtFntSmall, BlkBrsh, rect, StrTxtFrmtC);
            e.Graphics.DrawRectangle(Pens.Transparent, rect.X, rect.Y, rect.Width, rect.Height);
        }

        private void PrePrntTokenReceiptDesgn(System.Drawing.Printing.PrintPageEventArgs e)
        {
            // Construct string format and alignment 

            StringFormat StrTxtFrmtC = new StringFormat();
            StringFormat StrTxtFrmtR = new StringFormat();
            StringFormat StrTxtFrmtL = new StringFormat();

            // Set alignment, line alignment, and trimming properties of a string 

            StrTxtFrmtC.Alignment = StringAlignment.Center;
            StrTxtFrmtC.LineAlignment = StringAlignment.Center;
            StrTxtFrmtC.Trimming = StringTrimming.EllipsisCharacter;

            StrTxtFrmtR.Alignment = StringAlignment.Far;
            StrTxtFrmtR.LineAlignment = StringAlignment.Far;
            StrTxtFrmtR.Trimming = StringTrimming.Character;

            StrTxtFrmtL.Alignment = StringAlignment.Near;
            StrTxtFrmtL.LineAlignment = StringAlignment.Near;
            StrTxtFrmtL.Trimming = StringTrimming.Character;

            // Construct Brushes
            SolidBrush BluBrsh = new SolidBrush(Color.Blue);
            SolidBrush RedBrsh = new SolidBrush(Color.Red);
            SolidBrush GrnBrsh = new SolidBrush(Color.Green);
            SolidBrush WitBrsh = new SolidBrush(Color.White);
            SolidBrush BlkBrsh = new SolidBrush(Color.Black);
            SolidBrush GryBrsh = new SolidBrush(Color.LightGray);
            SolidBrush TrnspBrsh = new SolidBrush(Color.Transparent);

            // Construct Font 
            string RunningPath = AppDomain.CurrentDomain.BaseDirectory;
            string FONT = string.Format("{0}Resources\\CenturyGothic.ttf", Path.GetFullPath(Path.Combine(RunningPath, @"..\..\")));

            Font MainFont = new Font(FONT, 7, FontStyle.Bold);
            Font DrftFnt = new Font(FONT, 11, FontStyle.Regular);
            Font hdrFont = new Font(FONT, 10, FontStyle.Bold);
            Font TxtFnt = new Font(FONT, 7, FontStyle.Regular);
            Font TxtFntSmall = new Font(FONT, 6, FontStyle.Regular);

            // Create a Rectangle
            int RwPt = 0; // Y Point
            int ClPt = 0; // X Point
            int ClWd = 0; // Column Width
            int RwHg = 0; // Row Height

            ClPt = 20;
            ClWd = 191;
            RwHg = 50; //DrftFnt.Height + 2; // 40
            RwPt = 20;

            Rectangle rect = new Rectangle(ClPt, RwPt, ClWd, RwHg);

            MemoryStream Barcode = new MemoryStream();
            var Image = BarCode.GenerateImageBarcode1(PNo);
            Image.Save(Barcode, System.Drawing.Imaging.ImageFormat.Png);
            Image image = Image.FromStream(Barcode);

            rect.Height = DrftFnt.Height + 2;
            e.Graphics.DrawString("TOKEN", DrftFnt, BlkBrsh, rect, StrTxtFrmtC);
            e.Graphics.DrawRectangle(Pens.Transparent, rect.X, rect.Y, rect.Width, rect.Height);

            int sp = 0;
            sp = ClWd - 100;
            sp = sp / 2;
            rect.X = sp + 20;
            rect.Y = rect.Y + rect.Height;
            rect.Width = 100;
            rect.Height = TxtFnt.Height + 2;
            e.Graphics.DrawString("Date : " + String.Format("{0:d}", TknDate), TxtFnt, BlkBrsh, rect, StrTxtFrmtC);
            e.Graphics.DrawRectangle(Pens.Transparent, rect.X, rect.Y, rect.Width, rect.Height);

            sp = ClWd - 100;
            sp = sp / 2;
            rect.X = sp + 20;
            rect.Y = rect.Y + rect.Height;
            rect.Width = 100;
            rect.Height = TxtFnt.Height + 2;
            e.Graphics.DrawString("Token No : " + TknNo, TxtFnt, BlkBrsh, rect, StrTxtFrmtC);
            e.Graphics.DrawRectangle(Pens.Blue, rect.X, rect.Y, rect.Width, rect.Height);

            sp = ClWd - 100;
            sp = sp / 2;
            rect.X = sp + 20;
            rect.Y = rect.Y + rect.Height;
            rect.Width = 100;
            rect.Height = TxtFntSmall.Height + 2;
            e.Graphics.DrawString("Name : " + PName.ToString(), TxtFntSmall, BlkBrsh, rect, StrTxtFrmtL);
            e.Graphics.DrawRectangle(Pens.Transparent, rect.X, rect.Y, rect.Width, rect.Height);

            sp = ClWd - 100;
            sp = sp / 2;
            rect.X = sp + 20;
            rect.Y = rect.Y + rect.Height;
            rect.Width = 100;
            rect.Height = TxtFntSmall.Height + 2;
            e.Graphics.DrawString("Age : " + GetAge.ToString(), TxtFntSmall, BlkBrsh, rect, StrTxtFrmtL);
            e.Graphics.DrawRectangle(Pens.Transparent, rect.X, rect.Y, rect.Width, rect.Height);

            sp = ClWd - 70;
            sp = sp / 2;
            rect.X = sp + 20;
            rect.Y = rect.Y + rect.Height;
            rect.Width = 70;
            rect.Height = 20;

            //e.Graphics.DrawString(Temp., TxtFnt, BlkBrsh, rect, StrTxtFrmtC);
            e.Graphics.DrawImage(image, rect.X, rect.Y, rect.Width, rect.Height);
            e.Graphics.DrawRectangle(Pens.Transparent, rect.X, rect.Y, rect.Width, rect.Height);

            sp = ClWd - 90;
            sp = sp / 2;
            rect.X = sp + 20;
            rect.Y = rect.Y + rect.Height;
            rect.Width = 90;
            rect.Height = TxtFnt.Height + 2;
            e.Graphics.DrawString("GET WELL SOON", TxtFntSmall, BlkBrsh, rect, StrTxtFrmtC);
            e.Graphics.DrawRectangle(Pens.Transparent, rect.X, rect.Y, rect.Width, rect.Height);


            rect.X = ClPt;
            rect.Y = rect.Y + rect.Height;

            rect.Width = ClWd;
            rect.Height = 10;

            e.Graphics.DrawString("", DrftFnt, BlkBrsh, rect, StrTxtFrmtC);
            e.Graphics.DrawRectangle(Pens.Transparent, rect.X, rect.Y, rect.Width, rect.Height);

            RwHg = 50; //DrftFnt.Height + 2; // 40
            RwPt = 20;
            rect.X = ClPt;
            rect.Y = rect.Y + rect.Height;
            rect.Width = 50;
            rect.Height = RwHg;

            //e.Graphics.DrawString("Logo", DrftFnt, BlkBrsh, rect, StrTxtFrmtC);
            if (Global.Company.Logo != null)
            {
                MemoryStream logoImg = new MemoryStream(Global.Company.Logo);
                CompanyPictureBoxLogo.Image = System.Drawing.Image.FromStream(logoImg);
                Image logo = Image.FromStream(logoImg);
                e.Graphics.DrawImage(logo, rect.X, rect.Y, rect.Width, rect.Height);
                e.Graphics.DrawRectangle(Pens.Transparent, rect.X, rect.Y, rect.Width, rect.Height);
            }
            else
            {
                e.Graphics.DrawString("", DrftFnt, BlkBrsh, rect, StrTxtFrmtC);
                e.Graphics.DrawRectangle(Pens.Transparent, rect.X, rect.Y, rect.Width, rect.Height);

            }

            rect.X = 71;
            rect.Width = 140;
            rect.Height = MainFont.Height;

            e.Graphics.DrawString(CompanysName, MainFont, BlkBrsh, rect, StrTxtFrmtL);
            e.Graphics.DrawRectangle(Pens.Transparent, rect.X, rect.Y, rect.Width, rect.Height);

            rect.X = 71;
            rect.Y = rect.Y + rect.Height;
            rect.Width = 140;
            rect.Height = RwHg - rect.Height;

            e.Graphics.DrawString(CompnyAddress, TxtFntSmall, BlkBrsh, rect, StrTxtFrmtL);
            e.Graphics.DrawRectangle(Pens.Transparent, rect.X, rect.Y, rect.Width, rect.Height);

            rect.X = ClPt;
            rect.Y = rect.Y + rect.Height;
            rect.Width = ClWd;
            rect.Height = 10;

            e.Graphics.DrawString("", DrftFnt, BlkBrsh, rect, StrTxtFrmtC);
            e.Graphics.DrawRectangle(Pens.Transparent, rect.X, rect.Y, rect.Width, rect.Height);

            rect.X = ClPt;
            rect.Y = rect.Y + rect.Height;
            rect.Width = ClWd;
            rect.Height = hdrFont.Height + 2;

            e.Graphics.DrawString("OP Fee Receipt", hdrFont, BlkBrsh, rect, StrTxtFrmtC);
            e.Graphics.DrawRectangle(Pens.Transparent, rect.X, rect.Y, rect.Width, rect.Height);

            rect.X = ClPt;
            rect.Y = rect.Y + rect.Height;

            rect.Width = ClWd;
            rect.Height = DrftFnt.Height;

            e.Graphics.DrawString("", DrftFnt, BlkBrsh, rect, StrTxtFrmtC);
            e.Graphics.DrawRectangle(Pens.Transparent, rect.X, rect.Y, rect.Width, rect.Height);

            rect.X = ClPt;
            rect.Y = rect.Y + rect.Height;

            rect.Width = 95;
            rect.Height = TxtFnt.Height + 2;

            e.Graphics.DrawString("Date : " + String.Format("{0:d}", TknDate), TxtFnt, BlkBrsh, rect, StrTxtFrmtL);
            e.Graphics.DrawRectangle(Pens.Transparent, rect.X, rect.Y, rect.Width, rect.Height);

            rect.X = 116;
            // rect.Y = rect.Y + rect.Height;
            rect.Height = TxtFnt.Height + 2;

            e.Graphics.DrawString("Receipt # " + RefNo, TxtFnt, BlkBrsh, rect, StrTxtFrmtR);
            e.Graphics.DrawRectangle(Pens.Transparent, rect.X, rect.Y, rect.Width, rect.Height);

            rect.X = ClPt;
            rect.Y = rect.Y + rect.Height;

            rect.Width = ClWd;
            rect.Height = DrftFnt.Height;

            e.Graphics.DrawString("", DrftFnt, BlkBrsh, rect, StrTxtFrmtC);
            e.Graphics.DrawRectangle(Pens.Transparent, rect.X, rect.Y, rect.Width, rect.Height);

            rect.X = ClPt + 1;
            rect.Y = rect.Y + rect.Height;
            rect.Width = 80;
            rect.Height = TxtFntSmall.Height;

            e.Graphics.DrawRectangle(Pens.Black, rect.X, rect.Y, rect.Width, rect.Height);
            e.Graphics.FillRectangle(GryBrsh, rect);
            e.Graphics.DrawString("Patient Id ", TxtFntSmall, BlkBrsh, rect, StrTxtFrmtL);

            rect.X = 100 + 1;
            //rect.Y = 172;
            rect.Width = 111;
            rect.Height = TxtFntSmall.Height;

            e.Graphics.DrawString(PNo, TxtFntSmall, BlkBrsh, rect, StrTxtFrmtL);
            e.Graphics.DrawRectangle(Pens.Black, rect.X, rect.Y, rect.Width, rect.Height);

            rect.X = ClPt + 1;
            rect.Y = rect.Y + rect.Height;
            rect.Width = 80;
            rect.Height = TxtFntSmall.Height;

            e.Graphics.DrawRectangle(Pens.Black, rect.X, rect.Y, rect.Width, rect.Height);
            e.Graphics.FillRectangle(GryBrsh, rect);
            e.Graphics.DrawString("Patient Name ", TxtFntSmall, BlkBrsh, rect, StrTxtFrmtL);

            rect.X = 100 + 1;
            //rect.Y = rect.Y + rect.Height;
            rect.Width = 111;
            rect.Height = TxtFntSmall.Height;

            e.Graphics.DrawString(PName, TxtFntSmall, BlkBrsh, rect, StrTxtFrmtL);
            e.Graphics.DrawRectangle(Pens.Black, rect.X, rect.Y, rect.Width, rect.Height);

            rect.X = ClPt + 1;
            rect.Y = rect.Y + rect.Height;
            rect.Width = 80;
            rect.Height = (4 * TxtFntSmall.Height);

            e.Graphics.DrawRectangle(Pens.Black, rect.X, rect.Y, rect.Width, rect.Height);
            e.Graphics.FillRectangle(GryBrsh, rect);
            e.Graphics.DrawString("Address ", TxtFntSmall, BlkBrsh, rect, StrTxtFrmtL);

            rect.X = 100 + 1;
            //rect.Y = rect.Y + rect.Height;
            rect.Width = 111;
            rect.Height = (4 * TxtFntSmall.Height);

            e.Graphics.DrawString(PAdress, TxtFntSmall, BlkBrsh, rect, StrTxtFrmtL);
            e.Graphics.DrawRectangle(Pens.Black, rect.X, rect.Y, rect.Width, rect.Height);

            rect.X = ClPt + 1;
            rect.Y = rect.Y + rect.Height;
            rect.Width = 80;
            rect.Height = TxtFntSmall.Height;

            e.Graphics.DrawRectangle(Pens.Black, rect.X, rect.Y, rect.Width, rect.Height);
            e.Graphics.FillRectangle(GryBrsh, rect);
            e.Graphics.DrawString("Registration Fee ", TxtFntSmall, BlkBrsh, rect, StrTxtFrmtL);

            rect.X = 100 + 1;
            //rect.Y = rect.Y + rect.Height;
            rect.Width = 111;
            rect.Height = TxtFntSmall.Height;

            e.Graphics.DrawString(RegFee, TxtFntSmall, BlkBrsh, rect, StrTxtFrmtL);
            e.Graphics.DrawRectangle(Pens.Black, rect.X, rect.Y, rect.Width, rect.Height);

            rect.X = ClPt + 1;
            rect.Y = rect.Y + rect.Height;
            rect.Width = 80;
            rect.Height = TxtFntSmall.Height;

            e.Graphics.DrawRectangle(Pens.Black, rect.X, rect.Y, rect.Width, rect.Height);
            e.Graphics.FillRectangle(GryBrsh, rect);
            e.Graphics.DrawString("Fee Received ", TxtFntSmall, BlkBrsh, rect, StrTxtFrmtL);

            rect.X = 100 + 1;
            //rect.Y = rect.Y + rect.Height;
            rect.Width = 111;
            rect.Height = TxtFntSmall.Height;

            e.Graphics.DrawString(RecFee, TxtFntSmall, BlkBrsh, rect, StrTxtFrmtL);
            e.Graphics.DrawRectangle(Pens.Black, rect.X, rect.Y, rect.Width, rect.Height);

            rect.X = ClPt;
            rect.Y = rect.Y + rect.Height;
            rect.Width = ClWd;
            rect.Height = TxtFnt.Height;

            e.Graphics.DrawString("", TxtFnt, BlkBrsh, rect, StrTxtFrmtC);
            e.Graphics.DrawRectangle(Pens.Transparent, rect.X, rect.Y, rect.Width, rect.Height);

            rect.X = ClPt + 1;
            rect.Y = rect.Y + rect.Height;
            rect.Width = ClWd;
            rect.Height = TxtFntSmall.Height + 2;

            e.Graphics.DrawString("Printed on " + DateTime.Now, TxtFntSmall, BlkBrsh, rect, StrTxtFrmtC);
            e.Graphics.DrawRectangle(Pens.Transparent, rect.X, rect.Y, rect.Width, rect.Height);
        }
        private void PrePrntPDFReceiptDesgn(System.Drawing.Printing.PrintPageEventArgs e)
        {


            // Construct string format and alignment 

            StringFormat StrTxtFrmtC = new StringFormat();
            StringFormat StrTxtFrmtR = new StringFormat();
            StringFormat StrTxtFrmtL = new StringFormat();

            // Set alignment, line alignment, and trimming properties of a string 

            StrTxtFrmtC.Alignment = StringAlignment.Center;
            StrTxtFrmtC.LineAlignment = StringAlignment.Center;
            StrTxtFrmtC.Trimming = StringTrimming.EllipsisCharacter;

            StrTxtFrmtR.Alignment = StringAlignment.Far;
            StrTxtFrmtR.LineAlignment = StringAlignment.Near;
            StrTxtFrmtR.Trimming = StringTrimming.Character;

            StrTxtFrmtL.Alignment = StringAlignment.Near;
            StrTxtFrmtL.LineAlignment = StringAlignment.Near;
            StrTxtFrmtL.Trimming = StringTrimming.Character;

            // Construct Brushes
            SolidBrush BluBrsh = new SolidBrush(Color.Blue);
            SolidBrush RedBrsh = new SolidBrush(Color.Red);
            SolidBrush GrnBrsh = new SolidBrush(Color.Green);
            SolidBrush WitBrsh = new SolidBrush(Color.White);
            SolidBrush BlkBrsh = new SolidBrush(Color.Black);
            SolidBrush GryBrsh = new SolidBrush(Color.LightGray);
            SolidBrush TrnspBrsh = new SolidBrush(Color.Transparent);

            // Construct Font 
            string RunningPath = AppDomain.CurrentDomain.BaseDirectory;
            string FONT = string.Format("{0}Resources\\CenturyGothic.ttf", Path.GetFullPath(Path.Combine(RunningPath, @"..\..\")));

            Font MainFont = new Font(FONT, 7, FontStyle.Bold);
            Font DrftFnt = new Font(FONT, 11, FontStyle.Regular);
            Font hdrFont = new Font(FONT, 8, FontStyle.Bold);
            Font TxtFnt = new Font(FONT, 7, FontStyle.Regular);
            Font TxtFntSmall = new Font(FONT, 6, FontStyle.Regular);

            // Create a Rectangle
            int RwPt = 0; // Y Point
            int ClPt = 0; // X Point
            int ClWd = 0; // Column Width
            int RwHg = 0; // Row Height

            ClPt = 20;
            ClWd = 191;
            RwHg = 50; //DrftFnt.Height + 2; // 40
            RwPt = 20;

            Rectangle rect = new Rectangle(ClPt, RwPt, ClWd, RwHg);
            {
                rect.Width = 50;
                rect.Height = RwHg;
            }

            // capture company logo
            //e.Graphics.DrawString("Logo", DrftFnt, BlkBrsh, rect, StrTxtFrmtC);
            if (Global.Company.Logo != null)
            {
                MemoryStream logoImg = new MemoryStream(Global.Company.Logo);
                CompanyPictureBoxLogo.Image = System.Drawing.Image.FromStream(logoImg);
                Image logo = Image.FromStream(logoImg);
                e.Graphics.DrawImage(logo, rect.X, rect.Y, rect.Width, rect.Height);
                e.Graphics.DrawRectangle(Pens.Transparent, rect.X, rect.Y, rect.Width, rect.Height);
            }
            else
            {
                e.Graphics.DrawString("", DrftFnt, BlkBrsh, rect, StrTxtFrmtC);
                e.Graphics.DrawRectangle(Pens.Transparent, rect.X, rect.Y, rect.Width, rect.Height);

            }

            rect.X = 71;
            rect.Width = 140;
            rect.Height = MainFont.Height;

            e.Graphics.DrawString(CompanysName, MainFont, BlkBrsh, rect, StrTxtFrmtL);
            e.Graphics.DrawRectangle(Pens.Transparent, rect.X, rect.Y, rect.Width, rect.Height);

            rect.X = 71;
            rect.Y = rect.Y + rect.Height;
            rect.Width = 140;
            rect.Height = RwHg - rect.Height;

            e.Graphics.DrawString(CompnyAddress, TxtFntSmall, BlkBrsh, rect, StrTxtFrmtL);
            e.Graphics.DrawRectangle(Pens.Transparent, rect.X, rect.Y, rect.Width, rect.Height);

            rect.X = ClPt;
            rect.Y = rect.Y + rect.Height;
            rect.Width = ClWd;
            rect.Height = 10;

            e.Graphics.DrawString("", DrftFnt, BlkBrsh, rect, StrTxtFrmtC);
            e.Graphics.DrawRectangle(Pens.Transparent, rect.X, rect.Y, rect.Width, rect.Height);

            rect.X = ClPt;
            rect.Y = rect.Y + rect.Height;
            rect.Width = ClWd;
            rect.Height = hdrFont.Height + 2;

            e.Graphics.DrawString("OP Fee Receipt", hdrFont, BlkBrsh, rect, StrTxtFrmtC);
            e.Graphics.DrawRectangle(Pens.Transparent, rect.X, rect.Y, rect.Width, rect.Height);

            rect.X = ClPt;
            rect.Y = rect.Y + rect.Height;

            rect.Width = ClWd;
            rect.Height = DrftFnt.Height;

            e.Graphics.DrawString("", DrftFnt, BlkBrsh, rect, StrTxtFrmtC);
            e.Graphics.DrawRectangle(Pens.Transparent, rect.X, rect.Y, rect.Width, rect.Height);

            rect.X = ClPt;
            rect.Y = rect.Y + rect.Height;

            rect.Width = 95;
            rect.Height = TxtFnt.Height + 2;

            e.Graphics.DrawString("Date : " + String.Format("{0:d}", TknDate), TxtFnt, BlkBrsh, rect, StrTxtFrmtL);
            e.Graphics.DrawRectangle(Pens.Transparent, rect.X, rect.Y, rect.Width, rect.Height);

            rect.X = 116;
            // rect.Y = rect.Y + rect.Height;
            rect.Height = TxtFnt.Height + 2;

            e.Graphics.DrawString("Receipt # " + RefNo, TxtFnt, BlkBrsh, rect, StrTxtFrmtR);
            e.Graphics.DrawRectangle(Pens.Transparent, rect.X, rect.Y, rect.Width, rect.Height);

            rect.X = ClPt;
            rect.Y = rect.Y + rect.Height;

            rect.Width = ClWd;
            rect.Height = DrftFnt.Height;

            e.Graphics.DrawString("", DrftFnt, BlkBrsh, rect, StrTxtFrmtC);
            e.Graphics.DrawRectangle(Pens.Transparent, rect.X, rect.Y, rect.Width, rect.Height);

            rect.X = ClPt + 1;
            rect.Y = rect.Y + rect.Height;
            rect.Width = 80;
            rect.Height = TxtFnt.Height;

            e.Graphics.DrawRectangle(Pens.Black, rect.X, rect.Y, rect.Width, rect.Height);
            e.Graphics.FillRectangle(GryBrsh, rect);
            e.Graphics.DrawString("Patient Id ", TxtFnt, BlkBrsh, rect, StrTxtFrmtL);

            rect.X = 100 + 1;
            //rect.Y = 172;
            rect.Width = 111;
            rect.Height = TxtFnt.Height;

            e.Graphics.DrawString(PNo, TxtFnt, BlkBrsh, rect, StrTxtFrmtL);
            e.Graphics.DrawRectangle(Pens.Black, rect.X, rect.Y, rect.Width, rect.Height);

            rect.X = ClPt + 1;
            rect.Y = rect.Y + rect.Height;
            rect.Width = 80;
            rect.Height = TxtFnt.Height;

            e.Graphics.DrawRectangle(Pens.Black, rect.X, rect.Y, rect.Width, rect.Height);
            e.Graphics.FillRectangle(GryBrsh, rect);
            e.Graphics.DrawString("Patient Name ", TxtFnt, BlkBrsh, rect, StrTxtFrmtL);

            rect.X = 100 + 1;
            //rect.Y = rect.Y + rect.Height;
            rect.Width = 111;
            rect.Height = TxtFnt.Height;

            e.Graphics.DrawString(PName, TxtFnt, BlkBrsh, rect, StrTxtFrmtL);
            e.Graphics.DrawRectangle(Pens.Black, rect.X, rect.Y, rect.Width, rect.Height);

            rect.X = ClPt + 1;
            rect.Y = rect.Y + rect.Height;
            rect.Width = 80;
            rect.Height = (4 * TxtFnt.Height);

            e.Graphics.DrawRectangle(Pens.Black, rect.X, rect.Y, rect.Width, rect.Height);
            e.Graphics.FillRectangle(GryBrsh, rect);
            e.Graphics.DrawString("Address ", TxtFnt, BlkBrsh, rect, StrTxtFrmtL);

            rect.X = 100 + 1;
            //rect.Y = rect.Y + rect.Height;
            rect.Width = 111;
            rect.Height = (4 * TxtFnt.Height);

            e.Graphics.DrawString(PAdress, TxtFnt, BlkBrsh, rect, StrTxtFrmtL);
            e.Graphics.DrawRectangle(Pens.Black, rect.X, rect.Y, rect.Width, rect.Height);

            rect.X = ClPt + 1;
            rect.Y = rect.Y + rect.Height;
            rect.Width = 80;
            rect.Height = TxtFnt.Height;

            e.Graphics.DrawRectangle(Pens.Black, rect.X, rect.Y, rect.Width, rect.Height);
            e.Graphics.FillRectangle(GryBrsh, rect);
            e.Graphics.DrawString("Registration Fee ", TxtFnt, BlkBrsh, rect, StrTxtFrmtL);

            rect.X = 100 + 1;
            //rect.Y = rect.Y + rect.Height;
            rect.Width = 111;
            rect.Height = TxtFnt.Height;

            e.Graphics.DrawString(RegFee, TxtFnt, BlkBrsh, rect, StrTxtFrmtL);
            e.Graphics.DrawRectangle(Pens.Black, rect.X, rect.Y, rect.Width, rect.Height);

            rect.X = ClPt + 1;
            rect.Y = rect.Y + rect.Height;
            rect.Width = 80;
            rect.Height = TxtFnt.Height;

            e.Graphics.DrawRectangle(Pens.Black, rect.X, rect.Y, rect.Width, rect.Height);
            e.Graphics.FillRectangle(GryBrsh, rect);
            e.Graphics.DrawString("Fee Received ", TxtFnt, BlkBrsh, rect, StrTxtFrmtL);

            rect.X = 100 + 1;
            //rect.Y = rect.Y + rect.Height;
            rect.Width = 111;
            rect.Height = TxtFnt.Height;

            e.Graphics.DrawString(RecFee, TxtFnt, BlkBrsh, rect, StrTxtFrmtL);
            e.Graphics.DrawRectangle(Pens.Black, rect.X, rect.Y, rect.Width, rect.Height);

            rect.X = ClPt;
            rect.Y = rect.Y + rect.Height;
            rect.Width = ClWd;
            rect.Height = TxtFnt.Height;

            e.Graphics.DrawString("", TxtFnt, BlkBrsh, rect, StrTxtFrmtC);
            e.Graphics.DrawRectangle(Pens.Transparent, rect.X, rect.Y, rect.Width, rect.Height);

            rect.X = ClPt + 1;
            rect.Y = rect.Y + rect.Height;
            rect.Width = ClWd;
            rect.Height = TxtFnt.Height + 2;

            e.Graphics.DrawString("Printed on " + DateTime.Now, TxtFntSmall, BlkBrsh, rect, StrTxtFrmtC);
            e.Graphics.DrawRectangle(Pens.Transparent, rect.X, rect.Y, rect.Width, rect.Height);
        }
        private void PrintDocumentDirect_PrintPage(object sender, PrintPageEventArgs e)
        {
            if (CheckBoxFeeReceipt.Checked == true && CheckBoxToken.Checked == true)
            {
                PrePrntTokenReceiptDesgn(e);
            }
            else if (CheckBoxFeeReceipt.Checked == false && CheckBoxToken.Checked == true)
            {
                PrePrntTokenDesgn(e);
            }
            else if (CheckBoxFeeReceipt.Checked == true && CheckBoxToken.Checked == false)
            {
                PrePrntReceiptDesgn(e);
            }

        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F9)
            {
                BtnPrint.PerformClick();
            }
            else if (keyData == Keys.Escape)
            {
                BtnCancel.PerformClick();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
