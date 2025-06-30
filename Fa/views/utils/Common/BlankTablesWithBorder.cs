using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fa.views.utils.Common
{
    public class BlankTablesWithBorder
    {
        public enum BrushBorder
        {
            A = 0, T = 1, B = 2, L = 3, R = 4, TB = 5, TL = 6, TR = 7, RB = 8, RL = 9, LB = 10, TRL = 11, TBL = 12, TBR = 13, BRL = 14, N = 99
        }
        // Example call this function like this -  PdfPTable BlankHeadersWB = BlankRows((int)BrushBorder.B); | Here (int)BrushBorder.B says bottom line is bordered with gray
        public static PdfPTable BlankRows(int BorderAll)
        {
            PdfTableHeader BlankHeader = new PdfTableHeader();

            PdfPTable BlankHeadTable = new PdfPTable(1);
            PdfPCell blankRow = new PdfPCell(new Phrase("", PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
            blankRow.UseVariableBorders = true;
            blankRow.MinimumHeight = 4;

            blankRow.BorderColorLeft = BaseColor.WHITE;
            blankRow.BorderColorTop = BaseColor.WHITE;
            blankRow.BorderColorRight = BaseColor.WHITE;
            blankRow.BorderColorBottom = BaseColor.WHITE;

            if ((int)BrushBorder.A == BorderAll)
            {
                blankRow.BorderColorLeft = BaseColor.GRAY;
                blankRow.BorderColorTop = BaseColor.GRAY;
                blankRow.BorderColorRight = BaseColor.GRAY;
                blankRow.BorderColorBottom = BaseColor.GRAY;
            }
            else if ((int)BrushBorder.T == BorderAll)
            {
                blankRow.BorderColorTop = BaseColor.GRAY;
            }
            else if ((int)BrushBorder.B == BorderAll)
            {
                blankRow.BorderColorBottom = BaseColor.GRAY;
            }
            else if ((int)BrushBorder.L == BorderAll)
            {
                blankRow.BorderColorLeft = BaseColor.GRAY;
            }
            else if ((int)BrushBorder.R == BorderAll)
            {
                blankRow.BorderColorRight = BaseColor.GRAY;
            }
            else if ((int)BrushBorder.TB == BorderAll)
            {
                blankRow.BorderColorTop = BaseColor.GRAY;
                blankRow.BorderColorBottom = BaseColor.GRAY;
            }
            else if ((int)BrushBorder.TL == BorderAll)
            {
                blankRow.BorderColorLeft = BaseColor.GRAY;
                blankRow.BorderColorTop = BaseColor.GRAY;
            }
            else if ((int)BrushBorder.TR == BorderAll)
            {
                blankRow.BorderColorTop = BaseColor.GRAY;
                blankRow.BorderColorRight = BaseColor.GRAY;
            }
            else if ((int)BrushBorder.RB == BorderAll)
            {
                blankRow.BorderColorRight = BaseColor.GRAY;
                blankRow.BorderColorBottom = BaseColor.GRAY;
            }
            else if ((int)BrushBorder.RL == BorderAll)
            {
                blankRow.BorderColorLeft = BaseColor.GRAY;
                blankRow.BorderColorRight = BaseColor.GRAY;
            }
            else if ((int)BrushBorder.LB == BorderAll)
            {
                blankRow.BorderColorLeft = BaseColor.GRAY;
                blankRow.BorderColorBottom = BaseColor.GRAY;
            }
            else if ((int)BrushBorder.TRL == BorderAll)
            {
                blankRow.BorderColorLeft = BaseColor.GRAY;
                blankRow.BorderColorTop = BaseColor.GRAY;
                blankRow.BorderColorRight = BaseColor.GRAY;
            }
            else if ((int)BrushBorder.TBL == BorderAll)
            {
                blankRow.BorderColorLeft = BaseColor.GRAY;
                blankRow.BorderColorTop = BaseColor.GRAY;
                blankRow.BorderColorBottom = BaseColor.GRAY;
            }
            else if ((int)BrushBorder.TBR == BorderAll)
            {
                blankRow.BorderColorTop = BaseColor.GRAY;
                blankRow.BorderColorRight = BaseColor.GRAY;
                blankRow.BorderColorBottom = BaseColor.GRAY;
            }
            else if ((int)BrushBorder.BRL == BorderAll)
            {
                blankRow.BorderColorLeft = BaseColor.GRAY;
                blankRow.BorderColorRight = BaseColor.GRAY;
                blankRow.BorderColorBottom = BaseColor.GRAY;
            }
            else if ((int)BrushBorder.N == BorderAll)
            {
                blankRow.BorderColorLeft = BaseColor.WHITE;
                blankRow.BorderColorTop = BaseColor.WHITE;
                blankRow.BorderColorRight = BaseColor.WHITE;
                blankRow.BorderColorBottom = BaseColor.WHITE;
            }
            BlankHeadTable.AddCell(blankRow);

            return BlankHeadTable;
        }
    }
}
