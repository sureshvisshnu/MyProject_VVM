using DocumentFormat.OpenXml.Drawing.ChartDrawing;
using fa.api.Accounting;
using fa.model.Common;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fa.views.utils.Common
{
    public class MiniPdfHeader
    {
        public bool Islogo { get; set; }
        public bool IsAddress { get; set; }
        public bool IsPhone { get; set; }
        public bool IsEmail { get; set; }
        public bool IsWebsite { get; set; }
        public bool IsLicenceInfo { get; set; }
        PdfPTable HeadTable = null;

        public PdfPTable PageHeader()
        {
            int HeadColumns = 1;
            PdfPTable HeadTable = new PdfPTable(HeadColumns);
            float[] HeadWidths = new float[] { 100f };
            HeadTable.SetWidths(HeadWidths);
            PdfPCell HeadCell = new PdfPCell();

            var CompanyName = Global.Company.DisplayAs;
            var Logo = Islogo && Global.getLogoAsBytes() != null ? Global.getLogoAsBytes() : null;
            Address lAddress = Global.Company.AddressId != null ? AddressManager.Instance.GetAddress(Global.Company) : null;
            var Address = IsAddress && lAddress != null ? AddressManager.Instance.GetAddress(Global.Company).FullAddressInSingleLine.Trim() : string.Empty;
            ContactInfo ContactInfo = Global.Company.ContactInfoId != null ? ContactInfoManager.Instance.GetContactInfoId((long)Global.Company.ContactInfoId) : null;
            var Phone = IsPhone && ContactInfo != null ? ContactInfo.Phone.Trim() : string.Empty;
            var Email = IsEmail && ContactInfo != null ? Global.Company.ContactInfo.Email.Trim() : string.Empty;
            var WebSite = IsWebsite && ContactInfo != null ? Global.Company.ContactInfo.WebSite.Trim() : string.Empty;
            var LicenseInfo = IsLicenceInfo && Global.Company.CompanyLicence != null && Global.Company.CompanyLicence.Count > 0 && Global.Company.CompanyLicence.First().IncludeInReport ?
                (Global.Company.CompanyLicence.First().DisplayName + " " + Global.Company.CompanyLicence.First().Value) : string.Empty;


            if (Logo != null)
            {
                HeadColumns = 1;
                HeadWidths = new float[] { 100f };
                HeadTable = new PdfPTable(HeadColumns);
                HeadTable.SetWidths(HeadWidths);

                iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(Global.getLogoAsBytes());
                image.ScaleToFit(200f, 20f);
                image.ScaleAbsolute(35, 35);
                image.Alignment = Element.ALIGN_CENTER;
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
            HeadCell.HorizontalAlignment = Element.ALIGN_CENTER;
            HeadCell.VerticalAlignment = Element.ALIGN_CENTER;
            HeadTable.AddCell(HeadCell);


            if (!string.IsNullOrEmpty(Phone)) { Address += "\nPhone " + Phone; }
            if (!string.IsNullOrEmpty(Email)) { Address += "\nEmail " + Email; }
            if (!string.IsNullOrEmpty(WebSite)) { Address += "\nWebSite " + WebSite; }
            if (!string.IsNullOrEmpty(LicenseInfo)) { Address += ("\n" + LicenseInfo); }
            HeadCell = new PdfPCell(new Phrase(Address, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));

            HeadCell.BorderColor = BaseColor.WHITE;
            HeadCell.HorizontalAlignment = Element.ALIGN_CENTER;
            HeadCell.VerticalAlignment = Element.ALIGN_CENTER;
            HeadTable.AddCell(HeadCell);

            HeadCell = new PdfPCell(new Phrase(" ", PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            HeadCell.BorderColor = BaseColor.WHITE;
            HeadCell.MinimumHeight = 10;
            HeadCell.HorizontalAlignment = Element.ALIGN_CENTER;
            HeadCell.Colspan = HeadColumns;
            HeadTable.AddCell(HeadCell);

            return HeadTable;
        }
    }

    public class MiniPdfFooter
    {
        public bool IsDate { get; set; }
        public bool IsPageNumber { get; set; }
        public string Text { get; set; }
        public byte[] PdfFile { get; set; }
        public long PaperFormatId { get; set; }
        public bool IsReport { get; set; }


        private float[] A2 = { 126f, 425f, 550f, 1050f };
        private float[] A3 = { 86f, 260f, 390f, 730f };
        float[] A4_PORTRAIT = { 35f, 0f, 0f, 540f };
        private float[] A4_LANDSCAPE = { 38f, 270f, 400f, 780f };
        private float[] FooterX;

        public byte[] GetPdfFileWithFooter()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                //if (IsReport)
                //{
                    FooterX = A4_PORTRAIT;
                //}
                //else
                //{
                //    PrintPaperFormat PrintPaperFormat = PaperFormatManager.Instance.GetPrintPaperFormatById(PaperFormatId);
                //    if (PrintPaperFormat != null)
                //    {
                //        FooterX = (PrintPaperFormat.Name == "A2") ? A2 :
                //                  (PrintPaperFormat.Name == "A3") ? A3 :
                //                  (PrintPaperFormat.Name == "A4 PORTRAIT") ? A4_PORTRAIT :
                //                  (PrintPaperFormat.Name == "A4 LANDSCAPE") ? A4_LANDSCAPE :
                //                  (PrintPaperFormat.Name == "A5 LANDSCAPE") ? A4_PORTRAIT : null;
                //    }
                //}

                PdfReader reader = new PdfReader(PdfFile);
                using (PdfStamper stamper = new PdfStamper(reader, stream))
                {
                    int pages = reader.NumberOfPages;
                    for (int i = 1; i <= pages; i++)
                    {
                        ColumnText.ShowTextAligned(stamper.GetUnderContent(i),
                        @Element.ALIGN_LEFT, new Phrase((IsDate ? ("Printed On: " + DateTime.Now.ToString(Global.Company.DateFormat) + " " + DateTime.Now.ToShortTimeString()) : " "), PdfDataAlignment.GetFont("Font_Normal_Italic_6_Black")), FooterX[0], 30f, 0);

                        ColumnText.ShowTextAligned(stamper.GetUnderContent(i),
                        @Element.ALIGN_LEFT, new Phrase(Text, PdfDataAlignment.GetFont("Font_Normal_Italic_6_Black")), FooterX[1], 30f, 0);

                        ColumnText.ShowTextAligned(stamper.GetUnderContent(i),
                        @Element.ALIGN_LEFT, new Phrase("", PdfDataAlignment.GetFont("Font_Normal_Italic_6_Black")), FooterX[2], 30f, 0);

                        ColumnText.ShowTextAligned(stamper.GetUnderContent(i),
                        @Element.ALIGN_MIDDLE, new Phrase(IsPageNumber ? ((i.ToString() + "/" + pages)) : "", PdfDataAlignment.GetFont("Font_Normal_Italic_6_Black")), FooterX[3], 30f, 0);
                    }
                }
                PdfFile = stream.ToArray();
            }

            return PdfFile;
        }
    }
}
