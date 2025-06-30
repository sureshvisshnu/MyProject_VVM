using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fa.views.utils;
using iTextSharp.text.pdf;
using iTextSharp.text;
using VisioForge.Libs.MediaFoundation.OPM;
using Font = iTextSharp.text.Font;
using fa;
using fa.api.utils;

namespace Fa.views.utils.Report.Catalog
{
    class SavePrintTaxCodeReport
    {
        public bool ExportToFileOrPrint(DataGridView ReportGridView, string ReportHeading, string ReportName, string fileExtension, bool isPrint)
        {
            if(ReportGridView.Rows.Count != 0) 
            {
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    switch (fileExtension.ToLower())
                    {
                        case "xls":
                            break;
                        case "pdf":
                            var DataTable = DataGridViewAsDataTable(ReportGridView);
                            if (DataTable != null)
                            {
                                GeneratePDF(DataTable, ReportHeading, ReportName, fileExtension, isPrint);
                                Cursor.Current = Cursors.Default;
                                break;
                            }
                            else
                                break;
                        default: 
                        break;
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("File Error Please Contact System Admin");
                    Console.WriteLine(e.ToString());
                }
            }
            return true;
        }
        public static DataTable DataGridViewAsDataTable(DataGridView ReportGridView)
        {
            DataTable dt = new DataTable();
            if (ReportGridView.Rows.Count != 0)
            {
                try
                {
                    if (ReportGridView.ColumnCount == 0) return null;
                    foreach (DataGridViewColumn col in ReportGridView.Columns)
                    {
                        if (!col.Visible) continue;
                        if (col.Name == string.Empty || col.GetType() == typeof(DataGridViewButtonColumn)) continue;
                        dt.Columns.Add(col.Name, typeof(string));
                        dt.Columns[col.Name].Caption = col.HeaderText;
                    }
                    if (dt.Columns.Count == 0) return null;
                    foreach (DataGridViewRow row in ReportGridView.Rows)
                    {
                        int i = 0;
                        DataRow drNewRow = dt.NewRow();
                        foreach (DataColumn col in dt.Columns)
                        {
                            if (i > 4)
                            {
                                string taxvalue;
                                taxvalue = row.Cells[col.ColumnName].Value == null ? "" : string.Format("{0:F2}", Decimal.Parse(row.Cells[col.ColumnName].Value.ToString()));
                                drNewRow[col.ColumnName] = taxvalue;
                            }
                            else
                            {
                                drNewRow[col.ColumnName] = row.Cells[col.ColumnName].Value == null ? " " : row.Cells[col.ColumnName].Value;
                            }
                            i++;
                        }
                        dt.Rows.Add(drNewRow);
                    }
                }
                catch(Exception ex) 
                {
                    MessageBox.Show("File Error Please Contact System Admin");
                    Console.WriteLine(ex.ToString());
                    return null;
                }
            }
            return dt;
        }
        private static readonly Font FBI9W = new Font(PdfDataAlignment.GetFont("Font_Bold_Italic_9_White"));
        private static readonly Font FBI8B = new Font(PdfDataAlignment.GetFont("Font_Bold_Italic_8_Black"));
        private static readonly Font FNI8B = new Font(PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black"));
        private static readonly Font FNI12B = new Font(PdfDataAlignment.GetFont("Font_Bold_Italic_12_LightGray"));
        public void GeneratePDF(DataTable dataTable, string heading, string fileName, string fileExtension, bool isPrint)
        {
            var path = System.AppDomain.CurrentDomain.BaseDirectory;
            using (MemoryStream myMemoryStream = new MemoryStream())
            {
                Document pdfDoc;
                if (dataTable.Columns.Count > 13)
                {
                    pdfDoc = new Document(PageSize.A2, -45, -45, 20, 20);
                }
                else if (dataTable.Columns.Count > 9)
                {
                    pdfDoc = new Document(PageSize.A3, -45, -45, 20, 20);
                }
                else
                {
                    pdfDoc = new Document(PageSize.A4, -45, -45, 20, 38);
                }
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, myMemoryStream);

                pdfDoc.Open();
                var CompanyName = Global.Company.DisplayAs + "\n";
                var Address = Global.Company.Address.FullAddressInSingleLine + "\n";
                var Phone = string.Empty;
                var Email = string.Empty;
                var Web = string.Empty;

                if (Global.Company.ContactInfo.Phone != "" && Global.Company.ContactInfo.Phone != "-")
                {
                    var fax = Global.Company.ContactInfo.Fax != "" ? " / " + Global.Company.ContactInfo.Fax : "";
                    Phone = Global.Company.ContactInfo.Phone + fax + "\n";
                }
                if (!string.IsNullOrEmpty(Global.Company.ContactInfo.Email))
                {
                    Email = Global.Company.ContactInfo.Email == "" ? "" : "Email: " + (Global.Company.ContactInfo).Email + "\n";
                }
                if (!string.IsNullOrEmpty(Global.Company.ContactInfo.WebSite))
                {
                    Web = Global.Company.ContactInfo.WebSite == "" ? "" : "Web: " + (Global.Company.ContactInfo).WebSite + "\n\n";
                }
                int HeadColumns = 2;
                float[] HeadWidths = new float[] { 70f, 30f };
                if (Global.getLogoAsBytes() != null)
                {
                    HeadColumns = 3;
                    HeadWidths = new float[] { 15f, 55f, 30f };
                }

                PdfPTable HeadTable = new PdfPTable(HeadColumns);
                PdfPCell HeadCell = new PdfPCell();

                HeadTable.SetWidths(HeadWidths);
                if (Global.getLogoAsBytes() != null)
                {
                    iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(Global.getLogoAsBytes());
                    image.ScaleToFit(200f, 20f);
                    image.ScaleAbsolute(80, 80);
                    HeadCell = new PdfPCell(image);
                    HeadCell.BorderColor = BaseColor.WHITE;
                    HeadCell.MinimumHeight = 25;
                    HeadCell.Padding = 4;
                    HeadCell.HorizontalAlignment = Element.ALIGN_MIDDLE;
                    HeadTable.AddCell(HeadCell);
                }
                var AddressData = CompanyName + Address + Phone + Email + Web;
                HeadCell = new PdfPCell(new Phrase(AddressData, FBI8B));
                HeadCell.BorderColor = BaseColor.WHITE;
                HeadCell.MinimumHeight = 25;
                HeadCell.PaddingLeft = 14;
                HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
                HeadTable.AddCell(HeadCell);

                var HeadingData = heading;
                HeadCell = new PdfPCell(new Phrase(HeadingData, FNI12B));
                HeadCell.BorderColor = BaseColor.WHITE;
                HeadCell.MinimumHeight = 80;
                HeadCell.Padding = 4;
                HeadCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                HeadTable.AddCell(HeadCell);

                pdfDoc.Add(HeadTable);

                PdfPTable ReportMainTable = MainTable(dataTable, fileName);
                pdfDoc.Add(ReportMainTable);
                pdfDoc.Close();

                if (dataTable.Columns.Count > 13)
                {
                    PdfGeneration.SaveMemoryStream(myMemoryStream, fileName, fileExtension, isPrint, PaperTypes.A2);
                }
                else if (dataTable.Columns.Count > 9)
                {
                    PdfGeneration.SaveMemoryStream(myMemoryStream, fileName, fileExtension, isPrint, PaperTypes.A3);
                }
                else
                {
                    PdfGeneration.SaveMemoryStream(myMemoryStream, fileName, fileExtension, isPrint, PaperTypes.A4_PORTRAIT);
                }
            }
        }
        private PdfPTable MainTable(DataTable DataTable, String TypeOfReport)
        {
            int Cols = DataTable.Columns.Count ;
            int Rows = DataTable.Rows.Count;
            PdfPTable ReportMainTable = new PdfPTable(Cols);

            float[] Defaultwidths = new float[] { 5f, 10f, 40f, 15f, 15f, 10f, 10f, 10f, 10f };
            float[] widths = new float[Cols];
            for (int i = 0; i < Cols; i++)
            {
                widths[i] = Defaultwidths[i];
            }
            ReportMainTable.SetWidths(widths);

            //Main table Head
            PdfPCell HeaderCell = new PdfPCell();
            foreach (DataColumn column in DataTable.Columns)
            {
                HeaderCell = new PdfPCell(new Phrase(column.Caption, FBI9W));
                HeaderCell.BackgroundColor = new BaseColor(160, 160, 160);
                HeaderCell.BorderColor = BaseColor.BLACK;
                HeaderCell.MinimumHeight = 25;
                HeaderCell.Padding = 4;
                if (column.Caption == "") { continue; }
                if (column.Caption == "#" || column.Caption == "Code" || column.Caption == "Description" || column.Caption == "Effective From Date"
                    || column.Caption == "Effective To Date")
                {
                    HeaderCell.HorizontalAlignment = Element.ALIGN_LEFT;
                }
                else
                {
                    HeaderCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                }

                ReportMainTable.AddCell(HeaderCell);
            }

            //Main Table body
            for (int i = 0; i < Rows; i++)
            {
                PdfPCell RowCell = new PdfPCell();
                for (int j = 0; j < Cols; j++)
                {
                    var Temp = DataTable.Rows[i][j].ToString();
                    RowCell = new PdfPCell(new Phrase(Temp, FNI8B));
                    RowCell.BorderColor = BaseColor.BLACK;
                    RowCell.MinimumHeight = 20;
                    RowCell.Padding = 2;

                    if (j == 0 || j == 1 || j == 2 || j == 3 || j == 4)
                    {
                        RowCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    }
                    else
                    {
                        RowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    }

                    ReportMainTable.AddCell(RowCell);
                }
            }

            for (int j = 0; j < Cols; j++)
            {
                PdfPCell rowCell = new PdfPCell();
                rowCell = new PdfPCell(new Phrase("", FNI8B));
                rowCell.BorderColor = BaseColor.BLACK;

                rowCell.MinimumHeight = 1f;
                ReportMainTable.AddCell(rowCell);
            }
            return ReportMainTable;
        }
    }
}
