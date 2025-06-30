using fa.api.utils;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace fa.views.utils.Report.Account
{
    class ChartOfAccount
    {
        public bool ExportToFileOrPrint(DataGridView dataGridView, List<string> heading, string fileName, string fileExtension, bool isPrint)
        {
            if (dataGridView.Rows.Count != 0)
            {
                try
                {
                    switch (fileExtension.ToLower())
                    {
                        case "xls":
                            break;
                        case "pdf":
                            var dataTable = DataGridViewAsDataTable(dataGridView);
                            if (dataTable != null)
                            {
                                GeneratePDF(dataTable, heading, fileName, fileExtension, isPrint);
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
        public static DataTable DataGridViewAsDataTable(DataGridView dataGridView)
        {
            DataTable dt = new DataTable();

            if (dataGridView.Rows.Count != 0)
            {
                try
                {
                    if (dataGridView.ColumnCount == 0) return null;
                    foreach (DataGridViewColumn col in dataGridView.Columns)
                    {
                        if (!col.Visible) continue;
                        if (col.Name == string.Empty || col.GetType() == typeof(DataGridViewButtonColumn)) continue;
                        dt.Columns.Add(col.Name, typeof(string));
                        dt.Columns[col.Name].Caption = col.HeaderText;
                    }
                    if (dt.Columns.Count == 0) return null;
                    foreach (DataGridViewRow row in dataGridView.Rows)
                    {
                        DataRow drNewRow = dt.NewRow();
                        foreach (DataColumn col in dt.Columns)
                        {
                            if (col.ColumnName == "Balance")
                            {
                                double temp = double.Parse(row.Cells[col.ColumnName].Value.ToString());
                                drNewRow[col.ColumnName] = row.Cells[col.ColumnName].Value == null ? "---" : temp.ToString(TextUtils.DecimalPlace(Global.Company.PrimaryCurrency.RoundingPrecision));

                            }
                            else
                                drNewRow[col.ColumnName] = row.Cells[col.ColumnName].Value == null ? "---" : row.Cells[col.ColumnName].Value;
                        }
                        dt.Rows.Add(drNewRow);
                    }


                }
                catch (Exception e)
                {
                    MessageBox.Show("File Error Please Contact System Admin");
                    Console.WriteLine(e.ToString());
                    return null;
                }
            }
            return dt;

        }

        public void GeneratePDF(DataTable dataTable, List<string> heading, string fileName, string fileExtension, bool isPrint)
        {
            using (MemoryStream myMemoryStream = new MemoryStream())
            {

                var pageSize = PageSize.A4;
                Document pdfDoc = new Document(pageSize, -30, -30, 30, 32);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, myMemoryStream);
                int cols = dataTable.Columns.Count;
                int rows = dataTable.Rows.Count;
                pdfDoc.Open();

                float a4Size = pdfDoc.PageSize.Top;
                //-----------------------------------------------
                string CompanyName = Global.Company.DisplayAs + "\n";
                string Address = Global.Company.Address.FullAddressInSingleLine;
                string Phone = string.Empty;
                string Email = string.Empty;
                string Web = string.Empty;
                if (Global.Company.ContactInfo != null)
                {
                    if (Global.Company.ContactInfo.Mobile != "" && Global.Company.ContactInfo.Phone != "-")
                    {
                        var fax = Global.Company.ContactInfo.Fax != "" ? "\nFax: " + Global.Company.ContactInfo.Fax : "";
                        Phone = "Phone: " + Global.Company.ContactInfo.Phone + fax + "\n";
                    }
                    if (!string.IsNullOrEmpty(Global.Company.ContactInfo.Email))
                    {
                        Email = Global.Company.ContactInfo.Email == "" ? "" : "Email: " + (Global.Company.ContactInfo).Email + "\n";
                    }
                    if (!string.IsNullOrEmpty(Global.Company.ContactInfo.WebSite))
                    {
                        Web = Global.Company.ContactInfo.WebSite == "" ? "" : "Web: " + (Global.Company.ContactInfo).WebSite + "\n\n";
                    }
                }

                int HeadColumns = 2;
                float[] HeadWidths = new float[] { 50f, 50f };
                if (Global.getLogoAsBytes() != null)
                {
                    HeadColumns = 3;
                    HeadWidths = new float[] { 15f, 40f, 45f};
                }

                PdfPTable HeadTable = new PdfPTable(HeadColumns);
                PdfPCell HeadCell = new PdfPCell();

                HeadTable.SetWidths(HeadWidths);

                

                if (Global.getLogoAsBytes() != null)
                {
                    iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(Global.getLogoAsBytes());
                    image.ScaleToFit(200f, 20f);
                    image.ScaleAbsolute(60, 60);
                    var ImgData = image;
                    HeadCell = new PdfPCell(image);
                    HeadCell.BorderColor = BaseColor.WHITE;
                    HeadCell.MinimumHeight = 30;
                    HeadCell.Padding = 4;
                    HeadCell.HorizontalAlignment = Element.ALIGN_MIDDLE;
                    HeadTable.AddCell(HeadCell);
                }
                var AddressData = CompanyName;
                HeadCell = new PdfPCell(new Phrase(AddressData, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
                HeadCell.AddElement(new Phrase(AddressData, PdfDataAlignment.GetFont("Font_Bold_Italic_10_Black")));

                AddressData = Address + Phone + Email + Web;
                HeadCell.AddElement(new Phrase(AddressData, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));

                HeadCell.BorderColor = BaseColor.WHITE;
                HeadCell.HorizontalAlignment = Element.ALIGN_LEFT;
                HeadTable.AddCell(HeadCell);

                HeadCell = new PdfPCell(new Phrase(heading[0], PdfDataAlignment.GetFont("Font_Bold_Italic_15_LightGray")));
                HeadCell.BorderColor = BaseColor.WHITE;
                HeadCell.Colspan = HeadColumns;
                HeadCell.Padding = 4;
                HeadCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                HeadTable.AddCell(HeadCell);

                HeadCell = new PdfPCell(new Phrase("", PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
                HeadCell.BorderColor = BaseColor.WHITE;
                HeadCell.Colspan = HeadColumns;
                HeadCell.MinimumHeight = 2;
                HeadCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                HeadTable.AddCell(HeadCell);

                HeadCell = new PdfPCell(new Phrase("", PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
                HeadCell.BorderColor = BaseColor.WHITE;
                HeadCell.Colspan = HeadColumns;
                HeadCell.MinimumHeight = 5;
                HeadCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                HeadTable.AddCell(HeadCell);
                pdfDoc.Add(HeadTable);


                PdfPTable table = new PdfPTable(cols);
                PdfPCell headerCell = new PdfPCell();
                float[] widths = new float[] { 75, 135f, 75f, 75f, 45f,40f };

                table.SetWidths(widths);

                foreach (DataColumn column in dataTable.Columns)
                {
                    table.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    headerCell = new PdfPCell(new Phrase(column.ColumnName=="CRDR"?"CR/DR":column.ColumnName, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
                    headerCell.BackgroundColor = new BaseColor(160, 160, 160);
                    headerCell.MinimumHeight = 30;
                    headerCell.Padding = 5;
                    if (column.Caption == "Balance")
                    {
                        headerCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    }
                    
                    table.AddCell(headerCell);
                }
                for (int i = 0; i < rows; i++)
                {
                    PdfPCell rowCell = new PdfPCell();

                    for (int j = 0; j < cols; j++)
                    {
                        var temp = dataTable.Rows[i][j].ToString();
                        rowCell = new PdfPCell(new Phrase(temp, PdfDataAlignment.GetFont("Font_Normal_Italic_8_Black")));
                        if (j == 4)
                        {
                            rowCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        }
                        table.AddCell(rowCell);
                    }
                }

                
                pdfDoc.Add(table);

                pdfDoc.Close();

                PdfGeneration.SaveMemoryStream(myMemoryStream, fileName, fileExtension, isPrint, PaperTypes.A4_PORTRAIT);
            }
        }
    }
}
