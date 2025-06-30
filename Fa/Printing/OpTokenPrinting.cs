using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fa.model.Hms;
using fa.api.Hms;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using fa.views.hms.ip;
using fa.views.hms.patient;
using fa.model.Hms.Master;
using fa.api.utils;
using fa.libraries.utils;
using fa.model.Employee;
using fa.views.controls;
using fa.model.Hms.Op;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Diagnostics;
using fa.model.Common;
using fa.views.utils;
using fa.model.Hms.common;
using fa.model.hms.config;
using fa.api.Accounting;
using System.Text.RegularExpressions;
using fa.views.utils.Common;
using fa.model.Accounting.Transactions;
using static fa.views.utils.Common.BlankTablesWithBorder;
using static fa.views.utils.Common.TableCellAlignment;
using System.Drawing.Printing;
using fa.model.Accounting.Masters;

namespace fa.Printing
{
    public class OpTokenPrinting
    {
        public static PdfPTable SubTitleHeading(string SubTitleName)
        {
            int TitleColumns = 1;
            float[] TitleWidths = new float[] { 100f };

            PdfPTable MainSubTitle = new PdfPTable(1);

            MainSubTitle.SetTotalWidth(new float[] { 100 });
            MainSubTitle.DefaultCell.BorderColor = BaseColor.WHITE;
            MainSubTitle.DefaultCell.BorderColorBottom = BaseColor.WHITE;

            PdfPTable SubTitleTable = new PdfPTable(TitleColumns);

            PdfPCell SubTitleCell = new PdfPCell();
            SubTitleTable.SetWidths(TitleWidths);

            SubTitleCell = TableInnerCellAlignment((int)BrushBorder.N, SubTitleName, (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Bold_Italic_10_Black"), false, BaseColor.WHITE, false);
            MainSubTitle.AddCell(SubTitleCell);

            return MainSubTitle;
        }
        public void PrintTokeninPDF(int RegId, bool isPrint)
        {
            Registration Registration = OpManager.Instance.GetRegisterByRegId(RegId);
            if (Registration != null)
            {
                using (System.IO.MemoryStream ToknmemoryStream = new System.IO.MemoryStream())
                {
                    var psize = new iTextSharp.text.Rectangle(100, 300);
                    var PagSize = new iTextSharp.text.Rectangle(210, 298);
                    //Document Documenting = new Document(PagSize, 10, 10, 10, 10);
                    Document Documenting = new Document(new RectangleReadOnly(210, 298), 10, 10, 10, 10); // Pge size Chgnge Using RectangleReadOnly(1500, 1500) You can put on size value.
                    PdfWriter writer = PdfWriter.GetInstance(Documenting, ToknmemoryStream);
                    Documenting.Open();

                    PdfPTable TableCompanyInfo = new PdfPTable(1);
                    TableCompanyInfo.WidthPercentage = 100;
                    float[] widths = new float[] { 100f };
                    TableCompanyInfo.SetWidths(widths);

                    Company lCompany = CompanyManager.GetCompanyForModelForPrint(Registration.CompanyId);

                    PdfPCell CellRef = TableInnerCellAlignment((int)BrushBorder.N, lCompany.DisplayAs, (int)Element.ALIGN_CENTER, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black"), false, BaseColor.WHITE, false);
                    CellRef.NoWrap = false;
                    TableCompanyInfo.AddCell(CellRef);

                    Address address = AddressManager.Instance.GetAddressById((long)lCompany.AddressId);
                    if(address.CityOrTown != null)
                    {
                        CellRef = TableInnerCellAlignment((int)BrushBorder.N, address.CityOrTown, (int)Element.ALIGN_CENTER, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black"), false, BaseColor.WHITE, false);
                        TableCompanyInfo.AddCell(CellRef);
                    }
                    if(lCompany.ContactInfoId != null)
                    {
                        ContactInfo contactInfo = ContactInfoManager.Instance.GetContactInfoId((long)lCompany.ContactInfoId);

                        CellRef = TableInnerCellAlignment((int)BrushBorder.N, "Contact : " + contactInfo.Phone, (int)Element.ALIGN_CENTER, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black"), false, BaseColor.WHITE, false);
                        TableCompanyInfo.AddCell(CellRef);
                    }
                    Documenting.Add(TableCompanyInfo);

                    PdfPTable TableTokenHead = new PdfPTable(1);
                    float[] Widths = new float[] { 100f };
                    TableTokenHead.SetWidths(Widths);
                    

                    CellRef = TableInnerCellAlignment((int)BrushBorder.N, "TOKEN", (int)Element.ALIGN_CENTER, PdfDataAlignment.GetFont("Font_Bold_Italic_20_Black"), false, BaseColor.WHITE, false);
                    TableTokenHead.AddCell(CellRef);

                    CellRef = TableInnerCellAlignment((int)BrushBorder.N, "Date : " + Global.getTransactionDate().ToString(Global.Company.DateFormat), (int)Element.ALIGN_CENTER, PdfDataAlignment.GetFont("Font_Bold_Italic_10_Black"), false, BaseColor.WHITE, false);
                    TableTokenHead.AddCell(CellRef);

                    CellRef = TableInnerCellAlignment((int)BrushBorder.A, "Token No : " + Registration.TockenNo, (int)Element.ALIGN_CENTER, PdfDataAlignment.GetFont("Font_Bold_Italic_16_Black"), false, BaseColor.WHITE, false);
                    TableTokenHead.AddCell(CellRef);
                    Documenting.Add(TableTokenHead);
                    
                    TableTokenHead = new PdfPTable(3);
                    Widths = new float[] { 30f, 10f, 60f };
                    TableTokenHead.SetWidths(Widths);

                    CellRef = TableInnerCellAlignment((int)BrushBorder.N, "Name", (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black"), false, BaseColor.WHITE, false);
                    TableTokenHead.AddCell(CellRef);

                    CellRef = TableInnerCellAlignment((int)BrushBorder.N, " : ", (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black"), false, BaseColor.WHITE, false);
                    TableTokenHead.AddCell(CellRef);

                    CellRef = TableInnerCellAlignment((int)BrushBorder.N, Registration.Patient.Name, (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black"), false, BaseColor.WHITE, false);
                    TableTokenHead.AddCell(CellRef);

                    CellRef = TableInnerCellAlignment((int)BrushBorder.N, "Age", (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black"), false, BaseColor.WHITE, false);
                    TableTokenHead.AddCell(CellRef);

                    CellRef = TableInnerCellAlignment((int)BrushBorder.N, " : ", (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black"), false, BaseColor.WHITE, false);
                    TableTokenHead.AddCell(CellRef);

                    CellRef = TableInnerCellAlignment((int)BrushBorder.N, Registration.Patient.Age.ToString(), (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black"), false, BaseColor.WHITE, false);
                    TableTokenHead.AddCell(CellRef);

                    if(Registration.RequestedDoctorId != null)
                    {
                        CellRef = TableInnerCellAlignment((int)BrushBorder.N, "Doctor", (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black"), false, BaseColor.WHITE, false);
                        TableTokenHead.AddCell(CellRef);

                        CellRef = TableInnerCellAlignment((int)BrushBorder.N, " : ", (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black"), false, BaseColor.WHITE, false);
                        TableTokenHead.AddCell(CellRef);

                        Employee employee = EmployeeManager.Instance.GetEmployeeInfoById((long)Registration.RequestedDoctorId);

                        CellRef = TableInnerCellAlignment((int)BrushBorder.N, employee.Name, (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black"), false, BaseColor.WHITE, false);
                        TableTokenHead.AddCell(CellRef);
                    }
                    Documenting.Add(TableTokenHead);
                    
                    PdfPTable TableBarcode = new PdfPTable(3);
                    PdfPCell BarcodeCell = new PdfPCell();
                    {
                        MemoryStream Barcode = new MemoryStream();
                        var Image = BarCode.GenerateImageBarcode1(Registration.Patient.PatientNumber);
                        Image.Save(Barcode, System.Drawing.Imaging.ImageFormat.Png);
                        iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(Barcode.ToArray());

                        var Temp = image;
                        int fixedHeight = 40;
                        image.ScaleToFit(50f, 25f);
                        var ImgData = image;
                        BarcodeCell = new PdfPCell(image);
                        BarcodeCell.BorderColor = BaseColor.WHITE;
                        BarcodeCell.FixedHeight = fixedHeight;
                        BarcodeCell.Padding = 2;
                        BarcodeCell.HorizontalAlignment = Element.ALIGN_MIDDLE; 
                        BarcodeCell.VerticalAlignment = Element.ALIGN_CENTER;
                        image.ScaleAbsolute(TableTokenHead.TotalWidth, fixedHeight);
                    }
                    BarcodeCell.Rowspan = 2;
                    BarcodeCell.Colspan = 3;
                    TableBarcode.AddCell(BarcodeCell);
                    Documenting.Add(TableBarcode);

                    PdfPTable TableWis = new PdfPTable(3);
                    PdfPCell CellWis = new PdfPCell(new Phrase("GET WELL SOON...", PdfDataAlignment.GetFont("Font_Normal_Italic_10_Black")));
                    CellWis.BorderColor = BaseColor.WHITE;
                    CellWis.HorizontalAlignment = Element.ALIGN_CENTER;
                    CellWis.Colspan = 3;
                    TableWis.SpacingBefore = 5;
                    TableWis.SpacingAfter = 5;
                    TableWis.AddCell(CellWis);
                    Documenting.Add(TableWis);

                    Documenting.Close();

                    ToknmemoryStream.Close();

                    PdfGeneration PdfGeneration = new PdfGeneration();
                    PdfGeneration.IsPrint = isPrint;
                    PdfGeneration.FileName = GetTokenFileName();
                    PdfGeneration.PdfFile = ToknmemoryStream.ToArray();
                    PdfGeneration.SavePdfFile();
                    
                }
            }
        }
       
        private string GetPaymentFileName()
        {
            return string.Format("Receive Payment"/* {0}-{1}-{2} {3}.{4}.{5}", Global.getTransactionDate().Day, Global.getTransactionDate().Month, Global.getTransactionDate().Year, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second*/);
        }
        private string GetReceiptFileName()
        {
            return string.Format("Outpatient Fee Receipt" /*{0}-{1}-{2} {3}.{4}.{5}", Global.getTransactionDate().Day, Global.getTransactionDate().Month, Global.getTransactionDate().Year, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second*/);
        }
        private string GetTokenFileName()
        {
            return string.Format("Outpatient Token"/* {0}-{1}-{2} {3}.{4}.{5}", Global.getTransactionDate().Day, Global.getTransactionDate().Month, Global.getTransactionDate().Year, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second*/);
        }
        public void PrintReceiptFromOp(long RegId, bool isPrint)
        {
            Registration Registration = OpManager.Instance.GetOpRegistrationById(RegId);
            if (Registration != null)
            {
                HospitalConfiguration HospitalConfiguration = HospitalConfigurationManager.Instance.GetSettingsByCompanyId(Global.Company.CompanyId);
                if (HospitalConfiguration != null && HospitalConfiguration.OPRegistrationFeeAccountId != null && HospitalConfiguration.DefaultOPConsultingFee>0)
                {
                    PatientLedger lPatientLedger = PatientLedgerManager.Instance.GetPatientByOpIdType(RegId, TransactionType.PAYMENT);
                    if(lPatientLedger!=null)
                    {
                        using (System.IO.MemoryStream RecptmemoryStream = new System.IO.MemoryStream())
                        {
                            var PagSize = new iTextSharp.text.Rectangle(298, 840);
                            Document document = new Document(PageSize.A6, 0, 0, 35, 25);
                            PdfWriter writer = PdfWriter.GetInstance(document, RecptmemoryStream);
                            document.Open();

                            MiniPdfHeader PdfHeader = new MiniPdfHeader()
                            {
                                Islogo = true,
                                IsAddress = true,
                                IsPhone = false,
                                IsEmail = false,
                                IsWebsite = false,
                                IsLicenceInfo = false,
                            };
                            PdfPTable HTable = PdfHeader.PageHeader();
                            document.Add(HTable);

                            document.Add(BlankRows((int)BrushBorder.N));

                            PdfPTable TableHead = new PdfPTable(1); //3
                            float[] Widths = new float[] { 100f }; //{ 40f, 30f, 30f };
                            TableHead.SetWidths(Widths);

                            PdfPCell HeaderCell = new PdfPCell();

                            HeaderCell = TableInnerCellAlignment((int)BrushBorder.N, "OP Fee Receipt", (int)Element.ALIGN_CENTER, PdfDataAlignment.GetFont("Font_Bold_Italic_10_Black"), false, BaseColor.WHITE, false);
                            TableHead.AddCell(HeaderCell);

                            document.Add(TableHead);

                            document.Add(BlankRows((int)BrushBorder.N));
                            document.Add(BlankRows((int)BrushBorder.N));
                            document.Add(BlankRows((int)BrushBorder.N));

                            TableHead = new PdfPTable(2);
                            Widths = new float[] { 50f, 50f };
                            TableHead.SetWidths(Widths);

                            HeaderCell = TableInnerCellAlignment((int)BrushBorder.N, "Date: " + Registration.DateOfRegistration.ToShortDateString(), (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black"), false, BaseColor.WHITE, false);
                            TableHead.AddCell(HeaderCell);
                            
                            HeaderCell = TableInnerCellAlignment((int)BrushBorder.N, "Receipt #: " + lPatientLedger.RefNumber, (int)Element.ALIGN_RIGHT, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black"), false, BaseColor.WHITE, false);
                            TableHead.AddCell(HeaderCell);

                            document.Add(TableHead);
    
                            document.Add(BlankRows((int)BrushBorder.N));
                            document.Add(BlankRows((int)BrushBorder.N));
                            document.Add(BlankRows((int)BrushBorder.N));

                            TableHead = new PdfPTable(2);
                            Widths = new float[] { 40f, 60f };
                            TableHead.SetWidths(Widths);
                            
                            //body content                    
                            if (Registration.Patient != null)
                            {
                                //number
                                HeaderCell = TableInnerCellAlignment((int)BrushBorder.A, "Patient Id", (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Bold_Italic_8_White"), true, BaseColor.LIGHT_GRAY, false);
                                TableHead.AddCell(HeaderCell);

                                HeaderCell = TableInnerCellAlignment((int)BrushBorder.TBR, Registration.Patient.PatientNumber, (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), true, BaseColor.WHITE, false);
                                TableHead.AddCell(HeaderCell);

                                //name
                                HeaderCell = TableInnerCellAlignment((int)BrushBorder.BRL, "Patient Name", (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Bold_Italic_8_White"), true, BaseColor.LIGHT_GRAY, false);
                                TableHead.AddCell(HeaderCell);

                                HeaderCell = TableInnerCellAlignment((int)BrushBorder.RB, Registration.Patient.Name, (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), true, BaseColor.WHITE, false);
                                TableHead.AddCell(HeaderCell);

                                
                                //address
                                if (Registration.Patient.AddressId != null)
                                {
                                    Address Address = AddressManager.Instance.GetAddressById((long)Registration.Patient.AddressId);
                                    if (Address != null && !string.IsNullOrEmpty(Address.FullAddress))
                                    {
                                        HeaderCell = TableInnerCellAlignment((int)BrushBorder.BRL, "Address", (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Bold_Italic_8_White"), true, BaseColor.LIGHT_GRAY, false);
                                        TableHead.AddCell(HeaderCell);

                                        HeaderCell = TableInnerCellAlignment((int)BrushBorder.RB, Address.FullAddress, (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), true, BaseColor.WHITE, true);
                                        TableHead.AddCell(HeaderCell);                                        
                                    }
                                }
                                //Registration fee

                                HeaderCell = TableInnerCellAlignment((int)BrushBorder.BRL, "Registration Fee", (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Bold_Italic_8_White"), true, BaseColor.LIGHT_GRAY, false);
                                TableHead.AddCell(HeaderCell);

                                HeaderCell = TableInnerCellAlignment((int)BrushBorder.RB, HospitalConfiguration.DefaultOPConsultingFee.ToString(Global.Company.PrimaryCurrency.CurrencyFormat) + " INR", (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), true, BaseColor.WHITE, false);
                                TableHead.AddCell(HeaderCell);
                                                                
                                //amount received

                                HeaderCell = TableInnerCellAlignment((int)BrushBorder.BRL, "Fee Received", (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Bold_Italic_8_White"), true, BaseColor.LIGHT_GRAY, false);
                                TableHead.AddCell(HeaderCell);

                                HeaderCell = TableInnerCellAlignment((int)BrushBorder.RB, Registration.RegistrationFee.ToString(Global.Company.PrimaryCurrency.CurrencyFormat) + " INR", (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), true, BaseColor.WHITE, false);
                                TableHead.AddCell(HeaderCell);
                                                                
                            }
                            document.Add(TableHead);
                            document.Add(BlankRows((int)BrushBorder.N));
                            document.Add(BlankRows((int)BrushBorder.N));
                            document.Add(BlankRows((int)BrushBorder.N));

                            TableHead = new PdfPTable(1);
                            Widths = new float[] { 100f };
                            TableHead.SetWidths(Widths);

                            HeaderCell = TableInnerCellAlignment((int)BrushBorder.N, GetFooterText(), (int)Element.ALIGN_CENTER, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), false, BaseColor.WHITE, false);
                            TableHead.AddCell(HeaderCell);
                                                        
                            document.Add(TableHead);
                            document.Close();
                            RecptmemoryStream.Close();

                            PdfGeneration PdfGeneration = new PdfGeneration();
                            PdfGeneration.IsPrint = isPrint;
                            PdfGeneration.FileName = GetReceiptFileName();
                            PdfGeneration.PdfFile = RecptmemoryStream.ToArray();
                            PdfGeneration.SavePdfFile();
                        }
                    }
                }
            }
        }
        
        private string GetFooterText()
        {
            return string.Format("Printed at {0}-{1}-{2} {3}.{4} {5}", Global.getTransactionDate().Day, Global.getTransactionDate().Month, Global.getTransactionDate().Year, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.ToString("tt"));
        }
        public void PrintReceiptFromLedger(long LedgerId, bool isPrint)
        {                     
            PatientLedger PatientLedger = PatientLedgerManager.Instance.GetPatientLedgerById(LedgerId);
            if (PatientLedger != null)
            {

                using (System.IO.MemoryStream RecptmemoryStream = new System.IO.MemoryStream())
                {
                    var PagSize = new iTextSharp.text.Rectangle(298, 840);
                    Document document = new Document(PageSize.A6, 0, 0, 35, 25);
                    PdfWriter writer = PdfWriter.GetInstance(document, RecptmemoryStream);
                    document.Open();

                    MiniPdfHeader PdfHeader = new MiniPdfHeader()
                    {
                        Islogo = true,
                        IsAddress = true,
                        IsPhone = false,
                        IsEmail = false,
                        IsWebsite = false,
                        IsLicenceInfo = false,
                    };
                    PdfPTable HTable = PdfHeader.PageHeader();
                    document.Add(HTable);

                    PdfPTable TableHead = new PdfPTable(1);
                    float[] Widths = new float[] { 100f };
                    TableHead.SetWidths(Widths);

                    PdfPCell HeaderCell = new PdfPCell();
                    HeaderCell = TableInnerCellAlignment((int)BrushBorder.N, "Payment Receipt", (int)Element.ALIGN_CENTER, PdfDataAlignment.GetFont("Font_Bold_Italic_10_Black"), false, BaseColor.WHITE, false);
                    TableHead.AddCell(HeaderCell);

                    document.Add(TableHead);

                    document.Add(BlankRows((int)BrushBorder.N));
                    document.Add(BlankRows((int)BrushBorder.N));
                    document.Add(BlankRows((int)BrushBorder.N));

                    TableHead = new PdfPTable(2);
                    Widths = new float[] { 50f, 50f };
                    TableHead.SetWidths(Widths);

                    HeaderCell = TableInnerCellAlignment((int)BrushBorder.N, "Date: " + PatientLedger.Date.ToString(Global.Company.DateFormat), (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black"), false, BaseColor.WHITE, false);
                    TableHead.AddCell(HeaderCell);

                    HeaderCell = TableInnerCellAlignment((int)BrushBorder.N, "Payment #: " + PatientLedger.RefNumber, (int)Element.ALIGN_RIGHT, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black"), false, BaseColor.WHITE, false);
                    TableHead.AddCell(HeaderCell);

                    document.Add(TableHead);

                    document.Add(BlankRows((int)BrushBorder.N));
                    document.Add(BlankRows((int)BrushBorder.N));
                    document.Add(BlankRows((int)BrushBorder.N));

                    TableHead = new PdfPTable(2);
                    Widths = new float[] { 40f, 60f };
                    TableHead.SetWidths(Widths);

                    //body content                    
                    if (PatientLedger.Patient != null)
                    {
                        //number
                        HeaderCell = TableInnerCellAlignment((int)BrushBorder.A, "Patient Id", (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Bold_Italic_8_White"), true, BaseColor.LIGHT_GRAY, false);
                        TableHead.AddCell(HeaderCell);

                        HeaderCell = TableInnerCellAlignment((int)BrushBorder.TBR, PatientLedger.Patient.PatientNumber, (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), true, BaseColor.WHITE, false);
                        TableHead.AddCell(HeaderCell);

                        //name
                        HeaderCell = TableInnerCellAlignment((int)BrushBorder.BRL, "Patient Name", (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Bold_Italic_8_White"), true, BaseColor.LIGHT_GRAY, false);
                        TableHead.AddCell(HeaderCell);

                        HeaderCell = TableInnerCellAlignment((int)BrushBorder.RB, PatientLedger.Patient.Name, (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), true, BaseColor.WHITE, false);
                        TableHead.AddCell(HeaderCell);


                        //address
                        if (PatientLedger.Patient.AddressId != null)
                        {
                            Address Address = AddressManager.Instance.GetAddressById((long)PatientLedger.Patient.AddressId);
                            if (Address != null && !string.IsNullOrEmpty(Address.FullAddress))
                            {
                                HeaderCell = TableInnerCellAlignment((int)BrushBorder.BRL, "Address", (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Bold_Italic_8_White"), true, BaseColor.LIGHT_GRAY, false);
                                TableHead.AddCell(HeaderCell);

                                HeaderCell = TableInnerCellAlignment((int)BrushBorder.RB, Address.FullAddress, (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), true, BaseColor.WHITE, true);
                                TableHead.AddCell(HeaderCell);
                            }
                        }

                        //amount received
                        int RoundingPrecision = Global.Company.PrimaryCurrency.RoundingPrecision;
                        string formatSpecifier = $"F{RoundingPrecision}";

                        HeaderCell = TableInnerCellAlignment((int)BrushBorder.BRL, "Amount Received", (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Bold_Italic_8_White"), true, BaseColor.LIGHT_GRAY, false);
                        TableHead.AddCell(HeaderCell);

                        HeaderCell = TableInnerCellAlignment((int)BrushBorder.RB, PatientLedger.Amount.ToString(formatSpecifier) + " INR", (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), true, BaseColor.WHITE, false);
                        TableHead.AddCell(HeaderCell);

                        PatientPaymentDetail PaymentDetail = PatientLedgerManager.Instance.GetPatientPatientPaymentDetailById(PatientLedger.Id);
                        if (PaymentDetail != null)
                        {
                            HeaderCell = TableInnerCellAlignment((int)BrushBorder.BRL, "Payment Details", (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Bold_Italic_8_White"), true, BaseColor.LIGHT_GRAY, false);
                            TableHead.AddCell(HeaderCell);

                            PaymentType PaymentType = PaymentDetail.PaymentType;
                            HeaderCell = TableInnerCellAlignment((int)BrushBorder.BRL, PaymentType == PaymentType.CASH ? "By Cash" : PaymentType == PaymentType.CHECK ? "By Check\n#" + PaymentDetail.DocumentNumber : PaymentType == PaymentType.BANKTRANSFER ? "Through Bank\n#" + PaymentDetail.DocumentNumber : "By Card\n#" + PaymentDetail.DocumentNumber, (int)Element.ALIGN_LEFT, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), true, BaseColor.WHITE, true);
                            TableHead.AddCell(HeaderCell);
                        }

                        document.Add(TableHead);
                        document.Add(BlankRows((int)BrushBorder.N));
                        document.Add(BlankRows((int)BrushBorder.N));
                        document.Add(BlankRows((int)BrushBorder.N));

                        TableHead = new PdfPTable(1);
                        Widths = new float[] { 100f };
                        TableHead.SetWidths(Widths);

                        HeaderCell = TableInnerCellAlignment((int)BrushBorder.N, GetFooterText(), (int)Element.ALIGN_CENTER, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"), false, BaseColor.WHITE, false);
                        TableHead.AddCell(HeaderCell);
                    }
                   
                    document.Add(TableHead);
                    document.Close();
                    RecptmemoryStream.Close();

                    PdfGeneration PdfGeneration = new PdfGeneration();
                    PdfGeneration.IsPrint = isPrint;
                    PdfGeneration.FileName = GetPaymentFileName();
                    PdfGeneration.PdfFile = RecptmemoryStream.ToArray();
                    PdfGeneration.SavePdfFile();
                }
            }
        }
    }    
}
