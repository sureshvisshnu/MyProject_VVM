using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static fa.views.utils.Common.BlankTablesWithBorder;

namespace fa.views.utils.Common
{
    public class TableCellAlignment
    {
        public enum CellTextAlignment
        {
            ALIGN_RIGHT = 1, ALIGN_LEFT = 2
        }
        public static PdfPCell TableInnerCellAlignment(int BorderAll, string TableDiscription, int Align, iTextSharp.text.Font SetFont, bool MainBorders, BaseColor BckGrndClr, bool FixCellHeight, int Cspan,int Rspan)
        {
            PdfPCell FilledTableRow = TableInnerCellAlignment(BorderAll, TableDiscription, Align, SetFont, MainBorders, BckGrndClr, FixCellHeight);
            FilledTableRow.Colspan = Cspan;
            FilledTableRow.Rowspan = Rspan;
            return FilledTableRow;
        }
        public static PdfPCell TableInnerCellAlignment(int BorderAll, string TableDiscription, int Align, iTextSharp.text.Font SetFont, bool MainBorders, BaseColor BckGrndClr, bool FixCellHeight)
        {
            PdfPCell FilledTableRow = new PdfPCell(new Phrase(TableDiscription, SetFont));
            FilledTableRow.NoWrap = true;
            if (MainBorders == true)
            {
                FilledTableRow.UseVariableBorders = false;
            }
            else
            {
                FilledTableRow.UseVariableBorders = true;
            }
            float CellCurrentHeight;
            double CellHeigh = 0;
            int TxtLngth = 0;
            TxtLngth = TableDiscription.Length;     
            if (TxtLngth < 30)
            {
                if(FixCellHeight == true)
                {
                    FilledTableRow.NoWrap = false;
                }
                else
                {
                    CellCurrentHeight = SetFont.Size;
                    CellCurrentHeight += 6f;
                    CellHeigh = (0.3514 * SetFont.Size);
                    FilledTableRow.FixedHeight = CellCurrentHeight;
                }                
            }
            else 
            {
                if(FixCellHeight == true)
                {
                    FilledTableRow.NoWrap = false;

                    if (TxtLngth < 45)
                    {
                        FilledTableRow.FixedHeight = 28;
                    }
                    else if (TxtLngth < 60)
                    {
                        FilledTableRow.FixedHeight = 35;
                    }
                    else
                    {
                        FilledTableRow.FixedHeight = 42;
                    }

                }
                else
                {
                    CellCurrentHeight = SetFont.Size;
                    CellCurrentHeight += 6f;
                    CellHeigh = (0.3514 * SetFont.Size);
                    FilledTableRow.FixedHeight = CellCurrentHeight;
                }                
            }
            FilledTableRow.BorderColorLeft = BaseColor.WHITE;
            FilledTableRow.BorderColorTop = BaseColor.WHITE;
            FilledTableRow.BorderColorRight = BaseColor.WHITE;
            FilledTableRow.BorderColorBottom = BaseColor.WHITE;
            if ((int)BrushBorder.A == BorderAll)
            {
                FilledTableRow.BorderColorLeft = BaseColor.GRAY;
                FilledTableRow.BorderColorTop = BaseColor.GRAY;
                FilledTableRow.BorderColorRight = BaseColor.GRAY;
                FilledTableRow.BorderColorBottom = BaseColor.GRAY;
            }
            else if ((int)BrushBorder.T == BorderAll)
            {
                FilledTableRow.BorderColorTop = BaseColor.GRAY;
            }
            else if ((int)BrushBorder.B == BorderAll)
            {
                FilledTableRow.BorderColorBottom = BaseColor.GRAY;
            }
            else if ((int)BrushBorder.L == BorderAll)
            {
                FilledTableRow.BorderColorLeft = BaseColor.GRAY;
            }
            else if ((int)BrushBorder.R == BorderAll)
            {
                FilledTableRow.BorderColorRight = BaseColor.GRAY;
            }
            else if ((int)BrushBorder.TB == BorderAll)
            {
                FilledTableRow.BorderColorTop = BaseColor.GRAY;
                FilledTableRow.BorderColorBottom = BaseColor.GRAY;
            }
            else if ((int)BrushBorder.TL == BorderAll)
            {
                FilledTableRow.BorderColorLeft = BaseColor.GRAY;
                FilledTableRow.BorderColorTop = BaseColor.GRAY;
            }
            else if ((int)BrushBorder.TR == BorderAll)
            {
                FilledTableRow.BorderColorTop = BaseColor.GRAY;
                FilledTableRow.BorderColorRight = BaseColor.GRAY;
            }
            else if ((int)BrushBorder.RB == BorderAll)
            {
                FilledTableRow.BorderColorRight = BaseColor.GRAY;
                FilledTableRow.BorderColorBottom = BaseColor.GRAY;
            }
            else if ((int)BrushBorder.RL == BorderAll)
            {
                FilledTableRow.BorderColorLeft = BaseColor.GRAY;
                FilledTableRow.BorderColorRight = BaseColor.GRAY;
            }
            else if ((int)BrushBorder.LB == BorderAll)
            {
                FilledTableRow.BorderColorLeft = BaseColor.GRAY;
                FilledTableRow.BorderColorBottom = BaseColor.GRAY;
            }
            else if ((int)BrushBorder.TRL == BorderAll)
            {
                FilledTableRow.BorderColorLeft = BaseColor.GRAY;
                FilledTableRow.BorderColorTop = BaseColor.GRAY;
                FilledTableRow.BorderColorRight = BaseColor.GRAY;
            }
            else if ((int)BrushBorder.TBL == BorderAll)
            {
                FilledTableRow.BorderColorLeft = BaseColor.GRAY;
                FilledTableRow.BorderColorTop = BaseColor.GRAY;
                FilledTableRow.BorderColorBottom = BaseColor.GRAY;
            }
            else if ((int)BrushBorder.TBR == BorderAll)
            {
                FilledTableRow.BorderColorTop = BaseColor.GRAY;
                FilledTableRow.BorderColorRight = BaseColor.GRAY;
                FilledTableRow.BorderColorBottom = BaseColor.GRAY;
            }
            else if ((int)BrushBorder.BRL == BorderAll)
            {
                FilledTableRow.BorderColorLeft = BaseColor.GRAY;
                FilledTableRow.BorderColorRight = BaseColor.GRAY;
                FilledTableRow.BorderColorBottom = BaseColor.GRAY;
            }
            else if ((int)BrushBorder.N == BorderAll)
            {
                FilledTableRow.BorderColorLeft = BaseColor.WHITE;
                FilledTableRow.BorderColorTop = BaseColor.WHITE;
                FilledTableRow.BorderColorRight = BaseColor.WHITE;
                FilledTableRow.BorderColorBottom = BaseColor.WHITE;
            }            
            FilledTableRow.BackgroundColor = BckGrndClr;
            FilledTableRow.HorizontalAlignment = Align;
            return FilledTableRow;
        }
    }
}
