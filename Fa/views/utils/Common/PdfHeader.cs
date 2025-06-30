using fa.api.Accounting;
using fa.api.System;
using fa.model.Accounting.Masters;
using fa.model.Common;
using fa.model.System;
using fa.reports.Inventory;
using FADataAccessLibrary.report.Inventory;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisioForge.Libs.TagLib.Ogg;
using Font = iTextSharp.text.Font;

namespace fa.views.utils.Common
{
    public class PdfPageHeader
    {
        public bool Islogo { get; set; }
        public bool IsAddress { get; set; }
        public bool IsPhone { get; set; }
        public bool IsEmail { get; set; }
        public bool IsWebsite { get; set; }
        public bool IsLicenceInfo { get; set; }
        public string ReportLine1 { get; set; }
        public string ReportLine2 { get; set; }
        public bool IsMainHeader { get; set; }
        public PaperTypes PaperTypes { get; set; } 
        public int RIndex { get; set; }
        PdfPTable HeadTable = null;

        private static readonly Font FNIB8Font = new Font(PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"));

        public PdfPTable PageHeader()
        {
            int HeadColumns = IsMainHeader ? 3 : 2;
            HeadTable = new PdfPTable(HeadColumns);
            float[] HeadWidths = IsMainHeader ? new float[] { 30f, 20f, 30f } : new float[] { 50f, 50f };
            HeadTable.SetWidths(HeadWidths);
            PdfPCell HeadCell = new PdfPCell();

            var CompanyName = Global.Company.DisplayAs+(string.IsNullOrEmpty(Global.Company.Slogan) ? "" : "\n" + Global.Company.Slogan);
            var Logo = Islogo && Global.getLogoAsBytes() != null ? Global.getLogoAsBytes() : null;
            Address lAddress = Global.Company.AddressId != null ? AddressManager.Instance.GetAddress(Global.Company) : null;
            var Address = IsAddress && lAddress != null ? AddressManager.Instance.GetAddress(Global.Company).FullAddressInSingleLine.Trim() : string.Empty;
            ContactInfo ContactInfo = Global.Company.ContactInfoId != null ? ContactInfoManager.Instance.GetContactInfoId((long)Global.Company.ContactInfoId) : null;
            var Phone = IsPhone && ContactInfo != null ? ContactInfo.Phone.Trim().Replace("-", ""): string.Empty;
            var Email = IsEmail && ContactInfo != null ? Global.Company.ContactInfo.Email.Trim() : string.Empty;
            var WebSite = IsWebsite && ContactInfo != null ? Global.Company.ContactInfo.WebSite.Trim() : string.Empty;
            var LicenseInfo = string.Empty;
            if (Global.Company.CompanyLicence.Count > 0 && IsLicenceInfo)
            {
                foreach (CompanyLicence licence in Global.Company.CompanyLicence)
                {
                   if(licence.Value != null && licence.Value != "0" && licence.Value != "%")
                    {
                        LicenseInfo += (licence != null && licence.IncludeInReport) ? (string.IsNullOrEmpty(LicenseInfo) ? (licence.DisplayName + ":" + licence.Value) : ("\n" + licence.DisplayName + ":" + licence.Value)) : string.Empty;
                    }
                }
            }

            if (Logo != null)
            {
                HeadColumns = IsMainHeader ? 4 : 3;
                HeadWidths = IsMainHeader ?PaperTypes==PaperTypes.A4_LANDSCAPE? new float[] { 6f, 35f, 10f, 35f }: new float[] { 8f, 35f, 10f, 35f } : PaperTypes == PaperTypes.A4_LANDSCAPE ? new float[] { 8f, 40f, 40f }: new float[] { 8f, 40f, 40f };
                HeadTable = new PdfPTable(HeadColumns);
                HeadTable.SetWidths(HeadWidths);

                iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(Global.getLogoAsBytes());
                if (PaperTypes == PaperTypes.A4_LANDSCAPE)
                {
                    image.ScaleToFit(200f, 20f);
                    image.ScaleAbsolute(50, 50);
                }
                else
                {
                    image.ScaleToFit(200f, 20f);
                    image.ScaleAbsolute(45, 45);
                }
                var ImgData = image;
                HeadCell = new PdfPCell(image);
                HeadCell.AddElement(image);
                HeadCell.BorderColor = BaseColor.WHITE;
                HeadCell.Rowspan = 2;
                HeadCell.HorizontalAlignment = Element.ALIGN_CENTER;
                HeadCell.VerticalAlignment = Element.ALIGN_CENTER;
                HeadTable.AddCell(HeadCell);
            }
            var AddressData = CompanyName;
            HeadCell = new PdfPCell(new Phrase(AddressData, PdfDataAlignment.GetFont("Font_Bold_Italic_10_Black")));
            HeadCell.BorderColor = BaseColor.WHITE;
            HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
            HeadCell.VerticalAlignment = Element.ALIGN_CENTER;
            HeadCell.MinimumHeight = 5;
            HeadTable.AddCell(HeadCell);

            if (IsMainHeader)
            {
                HeadCell = new PdfPCell(new Phrase(" ", PdfDataAlignment.GetFont("Font_Bold_Italic_10_Black")));
                HeadCell.BorderColor = BaseColor.WHITE;
                HeadCell.HorizontalAlignment = Element.ALIGN_CENTER;
                HeadCell.VerticalAlignment = Element.ALIGN_CENTER;
                HeadCell.Rowspan = 2;
                HeadTable.AddCell(HeadCell);
            }
            AddressData = ReportLine1;
            var RptLine2 = new Chunk(ReportLine2, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"));
            RptLine2.SetTextRise(-6);
            RptLine2.setLineHeight(4);
            Paragraph AddressDataParagraph = new Paragraph(AddressData, PdfDataAlignment.GetFont("Font_Bold_Italic_12_LightGray"));
            AddressDataParagraph.Alignment = Element.ALIGN_RIGHT;
            Paragraph ReportLine2paragraph = new Paragraph(RptLine2);
            ReportLine2paragraph.Alignment = Element.ALIGN_RIGHT;

            HeadCell = new PdfPCell();
            HeadCell.BorderColor = BaseColor.WHITE;
            HeadCell.VerticalAlignment = Element.ALIGN_CENTER;
            HeadCell.Rowspan = 2;
            HeadCell.PaddingTop = -1;
            HeadCell.AddElement(AddressDataParagraph);
            HeadCell.AddElement(ReportLine2paragraph);
            HeadTable.AddCell(HeadCell);

            if (!string.IsNullOrEmpty(Phone)) { Address+= "\nPhone: " + Phone.Trim('_'); }
            if (!string.IsNullOrEmpty(Email)) { Address += "\nEmail: " + Email; }
            if (!string.IsNullOrEmpty(WebSite)) {Address += "\nWebSite: " + WebSite; }
            if (!string.IsNullOrEmpty(LicenseInfo)) { Address += ("\n"+LicenseInfo);}
            HeadCell = new PdfPCell(new Phrase(Address, FNIB8Font));
            HeadCell.BorderColor = BaseColor.WHITE;
            HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
            HeadCell.VerticalAlignment = Element.ALIGN_CENTER;
            HeadCell.PaddingTop = -1;
            HeadTable.AddCell(HeadCell);

            HeadCell = new PdfPCell(new Phrase(" ", FNIB8Font));
            HeadCell.BorderColor = BaseColor.WHITE;
            HeadCell.MinimumHeight = 10;
            HeadCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            HeadCell.Colspan = HeadColumns;
            HeadTable.AddCell(HeadCell);

            return HeadTable;
        }

    }
    public class PdfTableHeader
    {
        public PdfPTable ReportTableHeader(DataTable DataTable, string Type)
        {
            int Cols = DataTable.Columns.Count;
            int Rows = DataTable.Rows.Count;
            float MinHeight = 20;
            PdfPTable ReportMainTable = new PdfPTable(Cols);
            float[] widths = new float[] { 25f, 25f, 35f, 45f, 25f };
            if (Type == "TrialBalanceReport")
            {
                widths = new float[] { 50f, 25f, 25f };
            }
            if (Type == "Transaction")
            {
                widths = new float[] { 25f, 25f, 35f, 45f, 25f };
            } 
            if (Type == "Stock Request By Item")
            {
                widths = new float[] { 10f, 30f, 50f, 30f, 30f, 20f };
                MinHeight = 18;
            }
            if (Type == "Stock Request By Date")
            {
                widths = new float[] { 10f, 30f, 20f, 40f, 40f, 20f, 30f };
                MinHeight = 18;
            }
            if (Type == "Stock Request By Location")
            {
                widths = new float[] { 10f, 30f, 20f, 40f, 20f, 30f };               
                MinHeight = 18;
            }
            if (Type == "Stock Adjustment By Item")
            {
                widths = new float[] { 10f, 30f, 50f, 30f, 30f, 20f };
                MinHeight = 18;
            }
            if (Type == "Stock Adjustment By Date")
            {
                widths = new float[] { 10f, 30f, 20f, 40f, 20f, 30f };
                MinHeight = 18;
            }
            if (Type == "Stock Adjustment By Location")
            {
                widths = new float[] { 10f, 30f, 20f, 20f, 30f };
                MinHeight = 18;
            }
            if (Type == "Stock Receive By Item")
            {
                widths = new float[] { 10f, 30f, 50f, 30f, 30f, 20f, 30f };
                MinHeight = 18;
            }
            if (Type == "Stock Receive By Date")
            {
                widths = new float[] { 10f, 30f, 20f, 40f, 40f, 20f, 30f , 30f };
                MinHeight = 18;
            }
            if (Type == "Stock Receive By Location")
            {
                widths = new float[] { 10f, 30f, 20f, 40f, 20f, 30f, 30f };
                MinHeight = 18;
            }
            if (Type == "Damage Entry By Item")
            {
                widths = new float[] { 10f, 40f, 70f, 30f, 30f, 20f, 20f };
                MinHeight = 18;
            }
            if (Type == "Damage Entry By Date")
            {
                widths = new float[] { 10f, 40f, 30f, 40f, 30f, 30f, 40f };
                MinHeight = 18;
            }
            if (Type == "Damage Entry By Location")
            {
                widths = new float[] { 10f, 40f, 30f, 30f, 30f, 40f };
                MinHeight = 18;
            }
            if (Type == "Stock Movement By Item")
            {
                widths = new float[] { 10f, 40f, 70f, 30f, 30f, 20f, 20f };
                MinHeight = 18;
            }
            if (Type == "Stock Movement By Date")
            {
                widths = new float[] { 10f, 40f, 30f, 40f, 40f, 30f, 30f, 40f };
                MinHeight = 18;
            }
            if (Type == "Stock Movement By Location")
            {
                widths = new float[] { 20f, 40f, 30f, 50f, 30f, 30f, 40f };
                MinHeight = 18;
            }
            if(Type == "Patient Visit Counting By Date" || Type == "Patient Visit Counting By Diagnosis" || Type == "Patient Visit Counting By Department") 
            {
                widths = new float[] { 10f, 30f, 30f, 30f, 30f, 30f, 30f, 30f };
                MinHeight = 18;
            }
            if (Type == "OpReport")
            {
                widths = new float[] { 15f, 30f, 65f, 30f, 30f, 30f, 25f, 40f, 20f };
            }
            if (Type == "IpReport")
            {

                widths = new float[] { 20f, 15f, 30f, 53f, 20f, 10f, 20f, 20f, 20f, 22f };
                if (Cols == 9)
                {
                    widths = new float[] { 20f, 20f, 30f, 20f, 10f, 20f, 20f, 20f, 22f };
                }
            }
            if (Type == "CareTakerReport")
            {

                widths = new float[] { 28f, 28f, 22f, 22f, 22f, 22f, 22f, 25f, 22f };
                if (Cols == 8)
                {
                    widths = new float[] { 28f, 28f, 22f, 22f, 22f, 22f, 25f, 22f };
                }
            }
            if (Type == "UnAssignCareTakerReport")
            {
                widths = new float[] { 5f, 25f, 5f, 30f, 25f, 15f };
                if (Cols == 5)
                {
                    widths = new float[] { 5f, 5f, 30f, 25f, 15f };
                }
            }
            if(Type == "Care Taker UnAssign Report By Date")
            {
                widths = new float[] { 10f, 40f, 15f, 60f, 30f, 40f, 30f };
            }
            if(Type == "Care Taker UnAssign Report By Doctor" || Type == "Care Taker UnAssign Report By Nurse")
            {
                widths = new float[] { 10f, 30f, 15f, 60f, 40f, 30f };
            }
            if(Type == "Care Taker UnAssign Report By Department")
            {
                widths = new float[] { 10f, 30f, 40f, 15f, 60f, 30f, 30f };
            }
            if (Type == "UnAssignCareTakerReportDept")
            {
                widths = new float[] { 5f, 25f, 5f, 25f, 15f };
            }
            if (Type == "ProductFamilyWiseSalesReport" || Type == "AreaWiseSalesReport")
            {
                widths = new float[] { 10f, 20f, 35f, 20f, 15f, 20f, 20f, 20f, 20f, 20f };
            }
            if (Type == "CustomerwiseSalesReport")
            {
                widths = new float[] { 15f, 25f, 40f, 25f, 25f, 25f, 27f };
            }
            if (Type == "CatagorywiseSalesReport")
            {
                widths = new float[] { 10f, 30f, 45f, 13f, 12f, 20f, 20f, 20f, 15f, 20f };
            }
            if (Type == "SupplierWisePurchaseReport")
            {
                widths = new float[] { 10f, 25f, 40f, 25f, 25f, 25f, 27f };
            }
            if (Type == "CatagoryWisePurchaseReport")
            {
                widths = new float[] { 10f, 25f, 40f, 15f, 15f, 20f, 20f, 20f, 20f, 20f };
            }
            if (Type == "RptWardAndBed")
            {
                widths = new float[] { 30f, 25f, 20f, 30f, 20f, 40f, 30f, 40f, 20f };
            }
            if (Type == "Transfer Patient By Date")
            {
                widths = new float[] { 10f, 20f, 30f, 30f, 30f, 20f, 20f, 30f, 30f };
            }
            if (Type == "Transfer Patient By Ward")
            {
                widths = new float[] { 10f, 30f, 30f, 30f, 20f, 20f, 20f, 30f, 30f };
            }
            if (Type == "Transfer Patient By Consultant")
            {
                widths = new float[] { 10f, 30f, 30f, 30f, 40f, 20f, 20f, 20f, 30f };
            }
            if (Type == "Receive Amount By Date")
            {
                widths = new float[] { 10f, 20f, 20f, 40f, 20f, 40f, 40f, 20f, 30f };
            }
            if (Type == "Receive Amount By Patient")
            {
                widths = new float[] { 10f, 20f, 20f, 20f, 40f, 40f, 20f, 30f };
            }
            if (Type == "Receive Amount By Amount")
            {
                widths = new float[] { 10f, 20f, 40f, 20f, 20f, 40f, 40f, 20f, 30f };
            }
            if (Type == "Receive Amount By Consultant")
            {
                widths = new float[] { 10f, 40f, 40f, 20f, 20f, 20f, 40f, 20f, 30f };
            }
            if (Type == "ItemReport")
            {
                widths = new float[] { 5f, 10f, 20f, 10f, 10f, 10f, 10f, 10f, 10f, 10f, 10f, 10f, 10f, 10f, 10f, 10f, 10f };
            }
            if(Type == "Sales GSTR By HSN")
            {
                widths = new float[] { 10f, 30f, 30f, 30f, 30f, 30f, 30f, 30f, 30f, 30f };
            }
            if(Type == "Quote Report By Invoice")
            {
                widths = new float[] { 10f, 20f, 25f, 60f, 25f, 30f, 25f };
                MinHeight = 18;
            }
            if(Type == "Quote Report By Customer")
            {
                widths = new float[] { 10f, 25f, 40f, 25f, 25f, 25f, 27f };
                MinHeight = 18;
            }
            if(Type =="Quote Report By Category" || Type == "Quote Report By Product Family" || Type == "Quote Report By Item")
            {
                widths = new float[] { 10f, 25f, 50f, 18f, 18f, 18f, 18f, 18f };
                MinHeight = 18;
            }
            if (Type == "ItemLedger")
            {
                Cols = DataTable.Columns.Count-1;
                Rows = DataTable.Rows.Count - 1;
                ReportMainTable = new PdfPTable(Cols);
                widths = new float[] { 20f, 37f, 35f, 35f, 40f, 20f, 20f, 22f };
                if (Global.Company.MaintainRackNumber)
                {
                    widths = new float[] { 20f, 37f, 35f, 35f, 30f, 25f, 20f, 20f, 22f };
                }
                if ((Global.Company.MaintainRackNumber && Cols == 11) || (!Global.Company.MaintainRackNumber && Cols == 10))
                {
                    widths = new float[] { 25f, 37f, 35f, 35f, 40f, 17f, 22f, 18f, 20f, 20f };
                    if (Global.Company.MaintainRackNumber)
                    {
                        widths = new float[] { 25f, 37f, 30f, 30f, 30f, 25f, 25f, 22f, 15f, 20f, 20f };
                    }
                }
                else if ((Global.Company.MaintainRackNumber && Cols == 13) || (!Global.Company.MaintainRackNumber && Cols == 12))
                {
                    widths = new float[] { 24f, 37f, 30f, 30f, 37f, 30f, 24f, 17f, 22f, 20f, 23f, 20f };
                    if (Global.Company.MaintainRackNumber)
                    {
                        widths = new float[] { 26f, 35f, 32f, 32f, 25f, 30f, 20f, 26f, 15f, 22f, 20f, 26f, 20f };
                    }
                }
            }
            if (Type == "Patient Ledger" || Type == "Ledger")
            {
                Cols = DataTable.Columns.Count - 1;
                Rows = DataTable.Rows.Count - 1;
                ReportMainTable = new PdfPTable(Cols);
                widths = new float[] { 25f, 60f, 25f, 30f, 25f };
                MinHeight = 16;
            }
            if (Type == "StockReport")
            {
                Cols = DataTable.Columns.Count; // !Global.Company.MaintainRackNumber ? DataTable.Columns.Count - 1 : DataTable.Columns.Count;
                widths = new float[] { 10f, 25f, 50f, 20f, 20f, 20f, 20f, 20f, 20f, 20f, 20f, 20f, 20f, 20f, 20f, 20f, 20f, 20f, 20f };
                if (Global.Company.MaintainRackNumber)
                {
                    widths = new float[] { 10f, 25f, 50f, 20f, 18f, 22f, 20f, 20f, 20f, 20f, 20f, 20f, 20f, 20f, 20f, 20f, 20f, 20f, 20f, 20f };
                }
                ReportMainTable = new PdfPTable(Cols);
            }
            if (Type == "StockReportRackWise")
            {
                widths = new float[] { 10f, 25f, 50f, 20f, 20f, 20f, 20f, 20f, 20f, 20f, 20f, 20f, 20f, 20f, 20f, 20f, 20f, 20f, 20f };
            }
            if (Type == "UserWiseSalesReport")
            {
                widths = new float[] { 10f, 25f, 25f, 40f, 20f, 25f, 25f, 25f, 25f };
            }
            if (Type == "ExpiryReport")
            {
                widths = new float[] { 10f, 25f, 50f, 20f, 20f, 20f, 20f };
            }
            if (Type == "PriceList")
            {
                Rows = DataTable.Rows.Count - 1;
                ReportMainTable = new PdfPTable(Cols);
                widths = new float[] { 10f, 25f, 55f, 20f, 20f, 20f };
                if (Cols == 8)
                {
                    widths = new float[] { 10f, 25f, 55f, 20f, 20f, 20f, 20f, 20f };
                }
            }
            if (Type == "Prescription")
            {               
                widths = new float[] { 155f, 40f, 40f, 40f, 40f, 40f, 40f, 40f, 60f };
            }
            if (Type == "PrescriptionNew")
            {
                widths = new float[] { 130f, 45f, 50f, 45f, 50f, 60f, 50f, 50f, 60f, 160f }; // new float[] { 130f, 60f, 40f, 75f, 65f, 65f, 65f, 65f, 65f, 125f };
            }
            if (Type == "Invoice")
            {
                widths = new float[] { 5f, 72f, 23f };
            }
            if (Type == "InvoiceDetails")
            {
                widths = new float[] { 5f, 10f, 65f, 20f };
            }
            if (Type == "InvoiceReference")
            {
                widths = new float[] { 5f, 10f, 10f, 55f, 20f };
            }
            if (Type == "LabTest")
            {
                widths = new float[] { 135f, 40f, 40f, 80f };
                MinHeight = 10;
            }
            if (Type == "Patient Due List")
            {
                widths = new float[] { 15f, 60f, 15f, 40f, 40f, 60f, 30f };
            }
            if (Type == "DeliveryReport")
            {
                Rows = DataTable.Rows.Count - 1;
                ReportMainTable = new PdfPTable(Cols);
                widths = new float[] { 10f, 20f, 55f, 25f, 20f, 20f };                
            }
            if (Type == "LabTestReport")
            {
                widths = new float[] { 18f, 20f, 30f, 30f, 30f, 18f, 15f, 15f, 18f, 18f, 18f, 18f };
                if (Cols == 11)
                {
                    widths = new float[] { 20f, 25f, 35f, 35f, 20f, 20f, 15f, 20f, 20f, 20f, 20f };
                }
            }
            if (Type == "FeeChargeReport")
            {
                MinHeight = 18;
                widths = new float[] { 7f, 10f, 34f, 7f, 40f, 15f };
                if (Cols == 7)
                {
                    widths = new float[] { 7f, 10f, 26f, 7f, 20, 28f, 15f };
                }
            }
            if (Type == "PatientDetailsReport")
            {
                widths = new float[] { 13f, 48f, 22f, 20f, 18f, 10f, 28f, 50f, 25f, 50f, 50f };
            }
            if (Type == "PurchaseOrderBYINVOICE")
            {
                widths = new float[] { 10f, 15f, 15f, 40f, 15f, 15f, 15f };
                MinHeight = 18;
            }
            if (Type == "PurchaseOrderBYPFMAILY" || Type == "PurchaseOrderBYCATEGORY" || Type == "PurchaseOrderBYITEM")
            {
                widths = new float[] { 10f, 30f, 40f, 15f, 15f, 15f, 15f };
                MinHeight = 18;
            }
            if (Type == "PurchaseOrderBYVENDOR")
            {
                widths = new float[] { 10f, 15f, 15f, 45f, 15f, 15f, 15f, 15f };
                MinHeight = 18;
            }
            ReportMainTable.SetWidths(widths);
            PdfPCell HeaderCell = new PdfPCell();
            int Colms = DataTable.Columns.Count;

            for (int i = 0; i < Colms; i++)
            {
                DataColumn column = DataTable.Columns[i];

                if (column.Caption == "Id" || (!Global.Company.MaintainRackNumber && column.Caption == "Rack Number"))
                {
                    continue;
                }

                HeaderCell = new PdfPCell(new Phrase(column.Caption, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
                HeaderCell.BackgroundColor = new BaseColor(230, 230, 230);
                if (Type == "Ledger")
                {
                    HeaderCell.BackgroundColor = new BaseColor(200, 200, 200);
                }
                HeaderCell.BorderColor = new BaseColor(160, 160, 160);
                if (Type == "LabTest")
                {
                    HeaderCell = new PdfPCell(new Phrase(column.Caption, PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black")));
                    HeaderCell.BackgroundColor = BaseColor.WHITE;
                    HeaderCell.BorderColor = BaseColor.BLACK;
                }
                HeaderCell.MinimumHeight = MinHeight;

                if (column.Caption == "Stock" || column.Caption == "Quantity" || column.Caption == "Amount" || column.Caption == "Fees" || column.Caption == "Retail Price" || column.Caption == "WholeSale Price"
                    || column.Caption == "MSRP" || column.Caption == "Opening Stock" || column.Caption == "Closing Stock" || column.Caption == "Purchase" || column.Caption == "Purchase Return" || column.Caption == "Salse"
                    || column.Caption == "Salse Return" || column.Caption == "Stock In" || column.Caption == "Stock Out" || column.Caption == "To Patient" || column.Caption == "Damage" || column.Caption == "Adjust"
                    || column.Caption == "Cost" || column.Caption == "Value" || column.Caption == "Last Month Sale" || column.Caption == "Free" || column.Caption == "Total Quantity" || column.Caption == "Total Value"
                    || column.Caption == "Central Tax Amount" || column.Caption == "State Tax Amount" || column.Caption == "Tax Amount" || column.Caption == "Taxable Value" || column.Caption == "Received Amount"
                    || column.Caption == "Credit" || column.Caption == "Debit" || column.Caption == "Sub Total" || column.Caption == "Tax" || column.Caption == "Total" || column.Caption == "Net Amount" || column.Caption == "Discount" 
                    || column.Caption == "Cash Amount" || column.Caption == "Credit Amount" || column.Caption == "Age" || column.Caption == "Rent" || column.Caption == "Qty" || column.Caption == "Charge Amount"
                    || column.Caption == "Fee Charged" || column.Caption == "Amount Received" || column.Caption == "Balance" || column.Caption == "Token No" || column.Caption == "Male Adult" || column.Caption == "Female Adult" 
                    || column.Caption == "Male Child" || column.Caption == "Female Child" || column.Caption == "Others" || column.Caption == "Age" || column.Caption == "Price")
                {
                    HeaderCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                }
                else
                {
                    HeaderCell.HorizontalAlignment = Element.ALIGN_LEFT;
                }
                if(Type == "PrescriptionNew")
                {
                    HeaderCell.HorizontalAlignment = Element.ALIGN_LEFT;
                }

                if (Type == "Quote Report By Invoice" || Type == "Quote Report By Customer" || Type == "Quote Report By Category" || Type == "Quote Report By Product Family" || Type == "Quote Report By Item")
                {
                    if (i == 0)
                    {
                        HeaderCell.UseVariableBorders = true;
                        HeaderCell.BorderWidthBottom = (float)BorderStyle.None;
                    }
                    else
                    {
                        HeaderCell.UseVariableBorders = true;
                        HeaderCell.BorderWidthLeft = (float)BorderStyle.None;
                        HeaderCell.BorderWidthBottom = (float)BorderStyle.None;
                    }
                }
                else if (Type == "LabTest")
                {
                    HeaderCell.BorderWidthLeft = (float)BorderStyle.None;
                    HeaderCell.BorderWidthRight = (float)BorderStyle.None;
                    HeaderCell.BorderWidthTop = (float)BorderStyle.None;
                    HeaderCell.PaddingBottom = 5;
                    if (column.Caption == "Value")
                    {
                        HeaderCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    }
                    if (i == 0)
                    {
                        HeaderCell.UseVariableBorders = true;
                        HeaderCell.BorderColorBottom = BaseColor.WHITE;
                    }
                    else
                    {
                        HeaderCell.UseVariableBorders = true;
                        HeaderCell.BorderColorLeft = BaseColor.WHITE;
                        HeaderCell.BorderColorBottom = BaseColor.WHITE;
                    }
                }
                if (Type == "Patient Due List" || Type == "CustomerwiseSalesReport" || Type == "CatagorywiseSalesReport")
                {
                    if (i == 0)
                    {
                        HeaderCell.UseVariableBorders = true;
                        HeaderCell.BorderWidthBottom = (float)BorderStyle.None;
                    }
                    else
                    {
                        HeaderCell.UseVariableBorders = true;
                        HeaderCell.BorderWidthLeft = (float)BorderStyle.None;
                        HeaderCell.BorderWidthBottom = (float)BorderStyle.None;
                    }
                }
                ReportMainTable.AddCell(HeaderCell);
            }
            return ReportMainTable;
        }
    }
    public class PdfFooter
    {
        public bool IsDate { get; set; }
        public bool IsPageNumber { get; set; }
        public string Text { get; set; }
        public byte[] PdfFile { get; set; }
        public long PaperFormatId { get; set; }
        public bool IsReport { get; set; }
        public bool IsLandScape { get; set; }
        public bool IsA5LandScape { get; set; }

        private float[] A2 = { 126f, 425f, 550f, 1050f };
        private float[] A3 = { 86f, 260f, 390f, 730f };
        private float[] A4_PORTRAIT = { 35f, 0f, 0f, 540f };
        private float[] A4_LANDSCAPE = { 40f, 270f, 450f, 800f };
        private float[] A5_LANDSCAPE = { 30f, 270f, 350f, 570f };
        private float[] FooterX;

        //PdfDataAlignment.GetFont("Font_Normal_Italic_6_Black")
        private static readonly Font FNIB1Font = new Font(PdfDataAlignment.GetFont("Font_Normal_Italic_6_Black"));

        public byte[] GetPdfFileWithFooter()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                if (IsReport)
                {
                    FooterX = A4_PORTRAIT;
                    if (IsLandScape)
                    {
                        FooterX = A4_LANDSCAPE;
                    }
                    if (IsA5LandScape)
                    {
                        FooterX = A5_LANDSCAPE;
                    }
                }
                else
                {
                    PrintPaperFormat PrintPaperFormat = PaperFormatManager.Instance.GetPrintPaperFormatById(PaperFormatId);
                    if (PrintPaperFormat != null)
                    {
                        FooterX = (PrintPaperFormat.Name == "A2") ? A2 :
                                  (PrintPaperFormat.Name == "A3") ? A3 :
                                  (PrintPaperFormat.Name == "A4 PORTRAIT") ? A4_PORTRAIT :
                                  (PrintPaperFormat.Name == "A4 LANDSCAPE") ? A4_LANDSCAPE :
                                  (PrintPaperFormat.Name == "A5 LANDSCAPE") ? A4_PORTRAIT : null!;
                    }
                }

                PdfReader reader = new PdfReader(PdfFile);
                using (PdfStamper stamper = new PdfStamper(reader, stream))
                {
                    int pages = reader.NumberOfPages;
                    if (IsA5LandScape)
                    {
                        for (int i = 1; i <= pages; i++)
                        {
                            ColumnText.ShowTextAligned(stamper.GetUnderContent(i),
                                @Element.ALIGN_LEFT, new Phrase((IsDate ? ("Printed On: " + DateTime.Now.ToString(Global.Company.DateFormat) + " " + DateTime.Now.ToShortTimeString()) : " "), FNIB1Font), FooterX[0], 20f, 0);

                            ColumnText.ShowTextAligned(stamper.GetUnderContent(i),
                                @Element.ALIGN_LEFT, new Phrase(Text, FNIB1Font), FooterX[1], 20f, 0);

                            ColumnText.ShowTextAligned(stamper.GetUnderContent(i),
                                @Element.ALIGN_LEFT, new Phrase("", FNIB1Font), FooterX[2], 20f, 0);

                            float pageWidth = reader.GetPageSize(i).Width;
                            ColumnText.ShowTextAligned(stamper.GetUnderContent(i),
                                @Element.ALIGN_RIGHT, new Phrase(IsPageNumber ? ((i.ToString() + "/" + pages)) : "", FNIB1Font), FooterX[3], 20f, 0);

                        }
                    }
                    else if (!IsLandScape)
                    {
                        for (int i = 1; i <= pages; i++)
                        {
                            ColumnText.ShowTextAligned(stamper.GetUnderContent(i),
                                @Element.ALIGN_LEFT, new Phrase((IsDate ? ("Printed On: " + DateTime.Now.ToString(Global.Company.DateFormat) + " " + DateTime.Now.ToShortTimeString()) : " "), FNIB1Font), 23f, 10f, 0);

                            ColumnText.ShowTextAligned(stamper.GetUnderContent(i),
                                @Element.ALIGN_LEFT, new Phrase(Text, FNIB1Font), FooterX[1], 30f, 0);

                            ColumnText.ShowTextAligned(stamper.GetUnderContent(i),
                                @Element.ALIGN_LEFT, new Phrase("", FNIB1Font), FooterX[2], 30f, 0);

                            float pageWidth = reader.GetPageSize(i).Width;
                            ColumnText.ShowTextAligned(stamper.GetUnderContent(i),
                                @Element.ALIGN_RIGHT, new Phrase(IsPageNumber ? ((i.ToString() + "/" + pages)) : "", FNIB1Font), pageWidth - 23f, 10f, 0);

                        }
                    }
                    else
                    {
                        for (int i = 1; i <= pages; i++)
                        {
                            ColumnText.ShowTextAligned(stamper.GetUnderContent(i),
                                @Element.ALIGN_LEFT, new Phrase((IsDate ? ("Printed On: " + DateTime.Now.ToString(Global.Company.DateFormat) + " " + DateTime.Now.ToShortTimeString()) : " "), FNIB1Font), FooterX[0], 30f, 0);

                            ColumnText.ShowTextAligned(stamper.GetUnderContent(i),
                                @Element.ALIGN_LEFT, new Phrase(Text, FNIB1Font), FooterX[1], 30f, 0);

                            ColumnText.ShowTextAligned(stamper.GetUnderContent(i),
                                @Element.ALIGN_LEFT, new Phrase("", FNIB1Font), FooterX[2], 30f, 0);

                            float pageWidth = reader.GetPageSize(i).Width;
                            ColumnText.ShowTextAligned(stamper.GetUnderContent(i),
                                @Element.ALIGN_RIGHT, new Phrase(IsPageNumber ? ((i.ToString() + "/" + pages)) : "", FNIB1Font), FooterX[3], 30f, 0);

                        }
                    }
                }
                PdfFile = stream.ToArray();
            }

            return PdfFile;
        }
    }
}
