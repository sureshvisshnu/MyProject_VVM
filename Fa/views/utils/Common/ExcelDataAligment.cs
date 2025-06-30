using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fa.views.utils.Common
{
    public class ExcelDataAligment
    {
        public static void SetColour(_Worksheet worksheet, int RowIndex, int ColumnStart, int ColumnEnd, XlRgbColor BColor, XlRgbColor FColor)
        {
            var Range = worksheet.Range[worksheet.Cells[RowIndex, ColumnStart], worksheet.Cells[RowIndex, ColumnEnd]];
            Range.Interior.Color = BColor;
            Range.Font.Color = FColor;
        }
        public static void SetAlignment(_Worksheet worksheet, int RowIndex, int ColumnStart, int ColumnEnd, XlHAlign Alignment, string Format)
        {
            var Range = worksheet.Range[worksheet.Cells[RowIndex, ColumnStart], worksheet.Cells[RowIndex, ColumnEnd]];
            Range.EntireColumn.ColumnWidth = 20;
            Range.HorizontalAlignment = Alignment;
            if (Format == "0.00")
            {
                Range.NumberFormat = "0.00";
            }
        }

        public static void SetAlignmentWidth(_Worksheet worksheet, int RowIndex, int ColumnStart, int ColumnEnd, XlHAlign Alignment, string Format, int widthadjust)
        {
            var Range = worksheet.Range[worksheet.Cells[RowIndex, ColumnStart], worksheet.Cells[RowIndex, ColumnEnd]];
            Range.EntireColumn.ColumnWidth = 5 * widthadjust;
            Range.HorizontalAlignment = Alignment;
            if (Format == "0.00")
            {
                Range.NumberFormat = "0.00";
            }
        }
    }
}
