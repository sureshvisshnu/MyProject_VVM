using fa.api.Accounting;
using fa.model.Accounting.Masters;
using fa.views.utils;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace fa.reports.common.headers
{
    public class PdfHeader:Header
    {
        /*
        public PdfPTable Render()
        {
            var CompanyName = Company.DisplayAs;
            var Address = Company.Address.FullAddressInSingleLine;
            int HeadColumns = 3;
            int MinimumHeight = 10;
            float[] HeadWidths = new float[] { 65f, 10f, 25f };
            if (Company.Logo != null)
            {
                HeadColumns = 4;
                HeadWidths = new float[] { 15f, 50f, 10f, 25f };
            }
            PdfPTable HeadTable = new PdfPTable(HeadColumns);
            PdfPCell HeadCell = new PdfPCell();
            HeadTable.SetWidths(HeadWidths);
            if (Company.Logo != null)
            {
                Image image = Image.GetInstance(Company.Logo);
                image.ScaleToFit(200f, 20f);
                image.ScaleAbsolute(50, 50);
                var ImgData = image;
                HeadCell = new PdfPCell(image);
                HeadCell.BorderColor = BaseColor.WHITE;
                HeadCell.MinimumHeight = 25;
                HeadCell.Padding = 4;
                HeadCell.Rowspan = 2;
                HeadCell.HorizontalAlignment = Element.ALIGN_TOP;
                HeadTable.AddCell(HeadCell);
            }
            var AddressData = CompanyName;
            
            HeadCell = new PdfPCell(new Phrase(Company.DisplayAs, PdfDataAlignment.GetFont("Font_Bold_Italic_10_Black")));
            HeadCell.BorderColor = BaseColor.WHITE;
            HeadCell.MinimumHeight = MinimumHeight;
            HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
            HeadTable.AddCell(HeadCell);

            Account lAccount = AccountManager.Instance.GetAccountById(AccountId);

            var AccountHeadingData = "A/C: ";
            HeadCell = new PdfPCell(new Phrase(AccountHeadingData, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            HeadCell.BorderColor = BaseColor.WHITE;
            HeadCell.MinimumHeight = MinimumHeight;
            HeadCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            HeadTable.AddCell(HeadCell);

            var AccountData = lAccount.Name;
            HeadCell = new PdfPCell(new Phrase(AccountData, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            HeadCell.BorderColor = BaseColor.WHITE;
            HeadCell.MinimumHeight = MinimumHeight;
            HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
            HeadTable.AddCell(HeadCell);



            AddressData = (string.IsNullOrEmpty(Address) ? "" : Address + "\n") + heading + "\n ";
            HeadCell = new PdfPCell(new Phrase(AddressData, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            HeadCell.BorderColor = BaseColor.WHITE;
            HeadCell.MinimumHeight = 40;
            HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
            HeadTable.AddCell(HeadCell);


            AccountHeadingData = "Group: ";
            HeadCell = new PdfPCell(new Phrase(AccountHeadingData, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            HeadCell.BorderColor = BaseColor.WHITE;
            HeadCell.MinimumHeight = MinimumHeight;
            HeadCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            HeadTable.AddCell(HeadCell);

            AccountData = lAccount.AccountGroup.Name;
            HeadCell = new PdfPCell(new Phrase(AccountData, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            HeadCell.BorderColor = BaseColor.WHITE;
            HeadCell.MinimumHeight = MinimumHeight;
            HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
            HeadTable.AddCell(HeadCell);
          
            return HeadTable;

            return null;            
        }*/
    }
}
