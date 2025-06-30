using fa.api.utils;
using fa.model.Common;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Font = iTextSharp.text.Font;
using Image = iTextSharp.text.Image;

namespace fa.reports.common
{
    public class PdfCell:PdfPCell
    {
        public static Font DefaultFont = null; //new Font(iTextSharp.text.Font.);
        public Font Font { get; set; }
        public bool PrintBorder { get; set; }
        public int RowsToSpan { get; set; }
        public PdfCell(Font font, int width, bool printBorder )
        {
            Font = font;
            Width = width;
            PrintBorder = printBorder;
        }

        private void SetCellParams(PdfPCell cell)
        {
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            if (PrintBorder)
                cell.BorderColor = BaseColor.BLACK;
            cell.Rowspan = RowsToSpan;
        }

        public PdfPCell RenderImage(Image img)
        {
            PdfPCell cell = new PdfPCell(img);
            SetCellParams(cell);
            return cell;
        }

        public PdfPCell RenderText(String content)
        {
            PdfPCell cell = new PdfPCell(new Phrase(content));
            SetCellParams(cell);
            return cell;
        }

        public PdfPCell RenderCurrency(String content, Currency currency)
        {
            PdfPCell cell = new PdfPCell(new Phrase(String.Format(currency.CurrencyFormat,content)));
            SetCellParams(cell);
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            return cell;
        }

        public PdfPCell RenderDate(DateTime Date, String DateFormat)
        {
            PdfPCell cell = new PdfPCell(new Phrase(DateUtils.FormatDate(Date, DateFormat)));
            SetCellParams(cell);
            return cell;
        }

    }

    public class PdfFont 
    {
        private string RunningPath = AppDomain.CurrentDomain.BaseDirectory;
        private Font DefaultFont { get; set; }     
        public Font Font { get; set; }
        private Font DerivedFont
        {
            get
            {
                return (Font ?? DefaultFont);
            }
        }
        public Font Bold
        {
            get
            {
                return FontFactory.GetFont(DerivedFont.Familyname, 10, Font.BOLD, BaseColor.BLACK);
            }
        }
        public Font Italic
        {
            get
            {
                return FontFactory.GetFont(DerivedFont.Familyname, 10, iTextSharp.text.Font.BOLD, BaseColor.WHITE);
            }
        }

        public Font Title
        {
            get
            {
                return FontFactory.GetFont(DerivedFont.Familyname, 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
            }
        }
    }


}
