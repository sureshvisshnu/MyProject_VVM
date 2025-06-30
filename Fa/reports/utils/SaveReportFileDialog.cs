using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web;


namespace fa.reports.utils
{
    class SaveReportFileDialog
    {

        // print start--------------------------------------------------------------------------------------------

        PrintDocument PrintDocument = new  PrintDocument();
        StringFormat strFormat;  
        ArrayList arrColumnLefts = new ArrayList(); 
        ArrayList arrColumnWidths = new ArrayList(); 
        int iCellHeight = 0;  
        int iTotalWidth = 0;  
        int iRow = 0; 
        bool bFirstPage = false; 
        bool bNewPage = false; 
        int iHeaderHeight = 0;
        string printHeading;
        DataGridView dataGridView = new DataGridView();
        public bool PrintDataGridView(DataGridView dataGridView,string heading)
        {
            if (dataGridView.Rows.Count != 0)
            {
                this.dataGridView = dataGridView;
                printHeading = heading;
                 PrintDialog printDialog = new PrintDialog();
                printDialog.Document = PrintDocument;
                printDialog.UseEXDialog = true;
                PrintDocument.BeginPrint += On_BeginPrint;
                PrintDocument.PrintPage += On_PrintPage;
                 if (DialogResult.OK == printDialog.ShowDialog())
                {
                    PrintDocument.DocumentName = heading;
                    PrintDocument.Print();
                }
            }
            
            return false;
        }

        void On_BeginPrint(object sender, PrintEventArgs e)
        {
            try
            {
                strFormat = new StringFormat();
                strFormat.Alignment = StringAlignment.Near;
                strFormat.LineAlignment = StringAlignment.Center;
                strFormat.Trimming = StringTrimming.EllipsisCharacter;

                arrColumnLefts.Clear();
                arrColumnWidths.Clear();
                iCellHeight = 0;
                iRow = 0;
                bFirstPage = true;
                bNewPage = true;

                iTotalWidth = 0;
                foreach (DataGridViewColumn dgvGridCol in dataGridView.Columns)
                {
                    if (!dgvGridCol.Visible) continue;
                    if (dgvGridCol.Name == string.Empty || dgvGridCol.GetType() == typeof(DataGridViewButtonColumn)) continue;
                    iTotalWidth += dgvGridCol.Width;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        void On_PrintPage(object sender, PrintPageEventArgs e)
        {

            try
            {
                //Set the left margin
                int iLeftMargin = e.MarginBounds.Left;
                //Set the top margin
                int iTopMargin = e.MarginBounds.Top;
                //Whether more pages have to print or not
                bool bMorePagesToPrint = false;
                int iTmpWidth = 0;

                //For the first page to print set the cell width and header height
                if (bFirstPage)
                {
                    foreach (DataGridViewColumn GridCol in dataGridView.Columns)
                    {

                        iTmpWidth = (int)(Math.Floor((double)((double)GridCol.Width /
                                       (double)iTotalWidth * (double)iTotalWidth *
                                       ((double)e.MarginBounds.Width / (double)iTotalWidth))));

                        iHeaderHeight = (int)(e.Graphics.MeasureString(GridCol.HeaderText,
                                    GridCol.InheritedStyle.Font, iTmpWidth).Height) + 11;

                        // Save width and height of headres
                        arrColumnLefts.Add(iLeftMargin);
                        arrColumnWidths.Add(iTmpWidth);
                        iLeftMargin += iTmpWidth;
                    }
                }
                //Loop till all the grid rows not get printed
                while (iRow <= dataGridView.Rows.Count - 1)
                {
                    DataGridViewRow GridRow = dataGridView.Rows[iRow];
                    //Set the cell height
                    iCellHeight = GridRow.Height + 5;
                    int iCount = 0;
                    //Check whether the current page settings allo more rows to print
                    if (iTopMargin + iCellHeight >= e.MarginBounds.Height + e.MarginBounds.Top)
                    {
                        bNewPage = true;
                        bFirstPage = false;
                        bMorePagesToPrint = true;
                        break;
                    }
                    else
                    {
                        DataTable dt = new DataTable();

                        if (bNewPage)
                        {
                            //Draw Header
                            e.Graphics.DrawString(printHeading, new System.Drawing.Font(dataGridView.Font, FontStyle.Bold),
                                    Brushes.Black, e.MarginBounds.Left, e.MarginBounds.Top -
                                    e.Graphics.MeasureString(printHeading, new System.Drawing.Font(dataGridView.Font,
                                    FontStyle.Bold), e.MarginBounds.Width).Height - 13);

                            String strDate = Global.getTransactionDate() + " " + DateTime.Now.ToShortTimeString();
                            //Draw Date
                            e.Graphics.DrawString(strDate, new System.Drawing.Font(dataGridView.Font, FontStyle.Bold),
                                    Brushes.Black, e.MarginBounds.Left + (e.MarginBounds.Width -
                                    e.Graphics.MeasureString(strDate, new System.Drawing.Font(dataGridView.Font,
                                    FontStyle.Bold), e.MarginBounds.Width).Width), e.MarginBounds.Top -
                                    e.Graphics.MeasureString(printHeading, new System.Drawing.Font(new System.Drawing.Font(dataGridView.Font,
                                    FontStyle.Bold), FontStyle.Bold), e.MarginBounds.Width).Height - 13);

                            //Draw Columns                 
                            iTopMargin = e.MarginBounds.Top;

                            foreach (DataGridViewColumn GridCol in dataGridView.Columns)
                            {
                                if (!GridCol.Visible) continue;
                                if (GridCol.Name == string.Empty || GridCol.GetType() == typeof(DataGridViewButtonColumn)) continue;
                                dt.Columns.Add(GridCol.Name, typeof(string));
                                dt.Columns[GridCol.Name].Caption = GridCol.HeaderText;

                                e.Graphics.FillRectangle(new SolidBrush(Color.LightGray),
                                    new System.Drawing.Rectangle((int)arrColumnLefts[iCount], iTopMargin,
                                    (int)arrColumnWidths[iCount], iHeaderHeight));

                                e.Graphics.DrawRectangle(Pens.Black,
                                    new System.Drawing.Rectangle((int)arrColumnLefts[iCount], iTopMargin,
                                    (int)arrColumnWidths[iCount], iHeaderHeight));

                                e.Graphics.DrawString(GridCol.HeaderText, GridCol.InheritedStyle.Font,
                                    new SolidBrush(GridCol.InheritedStyle.ForeColor),
                                    new RectangleF((int)arrColumnLefts[iCount], iTopMargin,
                                    (int)arrColumnWidths[iCount], iHeaderHeight), strFormat);
                                iCount++;
                            }
                            bNewPage = false;
                            iTopMargin += iHeaderHeight;
                        }
                        iCount = 0;
                        //Draw Columns Contents                
                        foreach (DataGridViewCell Cel in GridRow.Cells)
                        {
                            if (Cel.Value != null && Cel.Visible ==true && Cel.GetType() != typeof(DataGridViewButtonColumn))
                            {

                                e.Graphics.DrawString(Cel.Value.ToString(), Cel.InheritedStyle.Font,
                                            new SolidBrush(Cel.InheritedStyle.ForeColor),
                                            new RectangleF((int)arrColumnLefts[iCount], (float)iTopMargin,
                                            (int)arrColumnWidths[iCount], (float)iCellHeight), strFormat);

                                e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle((int)arrColumnLefts[iCount],
                                    iTopMargin, (int)arrColumnWidths[iCount], iCellHeight));
                            }
                            //Drawing Cells Borders 
                            

                            iCount++;
                        }
                        //By Datatable
                        //foreach (DataGridViewRow row in dataGridView.Rows)
                        //{
                        //    DataRow drNewRow = dt.NewRow();
                        //    foreach (DataColumn col in dt.Columns)
                        //    {
                        //        drNewRow[col.ColumnName] = row.Cells[col.ColumnName].Value == null ? "---" : row.Cells[col.ColumnName].Value;
 
                        //            e.Graphics.DrawString(row.Cells[col.ColumnName].Value.ToString(), Cel.InheritedStyle.Font,
                        //                        new SolidBrush(Cel.InheritedStyle.ForeColor),
                        //                        new RectangleF((int)arrColumnLefts[iCount], (float)iTopMargin,
                        //                        (int)arrColumnWidths[iCount], (float)iCellHeight), strFormat);
                        //         //Drawing Cells Borders 
                        //        e.Graphics.DrawRectangle(Pens.Black, new System.Drawing.Rectangle((int)arrColumnLefts[iCount],
                        //                iTopMargin, (int)arrColumnWidths[iCount], iCellHeight));

                        //        iCount++;


                        //    }
                        //    dt.Rows.Add(drNewRow);
                        //}

                    }
                    iRow++;
                    iTopMargin += iCellHeight;
                }

                //If more lines exist, print another page.
                if (bMorePagesToPrint)
                    e.HasMorePages = true;
                else
                    e.HasMorePages = false;
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public bool ExportToFile(DataGridView dataGridView, string heading, string fileName, string fileExtension)
        {
            if (dataGridView.Rows.Count != 0)
            {

               try
                {
                    if (dataGridView.ColumnCount == 0) return false;
                    DataTable dt = new DataTable();
                    foreach (DataGridViewColumn col in dataGridView.Columns)
                    {
                        if (!col.Visible) continue;
                        if (col.Name == string.Empty || col.GetType() == typeof(DataGridViewButtonColumn)) continue;
                        dt.Columns.Add(col.Name, typeof(string));
                        dt.Columns[col.Name].Caption = col.HeaderText;
                    }
                    if (dt.Columns.Count == 0) return false;
                    foreach (DataGridViewRow row in dataGridView.Rows)
                    {
                        DataRow drNewRow = dt.NewRow();
                        foreach (DataColumn col in dt.Columns)
                        {
                            drNewRow[col.ColumnName] = row.Cells[col.ColumnName].Value == null ? "---" : row.Cells[col.ColumnName].Value;
                        }
                        dt.Rows.Add(drNewRow);
                    }

                    switch (fileExtension.ToLower())
                    {
                        case "xls":
 
                            break;
                        case "pdf":
                            GeneratePDF(dt, heading, fileName, fileExtension);
                            break;
                          
                        default:
                            break;
                    }

                }
                catch(Exception e)
                {
                    MessageBox.Show("File Error Please Contact System Admin");
                    Console.WriteLine(e.ToString());
                }
            }
            return true;
        }
        //public static string GetAppLocation()
        //{
        //    return AppDomain.CurrentDomain.BaseDirectory;
        //}
        public void GeneratePDF(DataTable dataTable, string heading, string fileName, string fileExtension)
        {
            using (MemoryStream myMemoryStream = new MemoryStream())
            {
                iTextSharp.text.Font fontBoldItalic = FontFactory.GetFont("Arial", 15, iTextSharp.text.Font.BOLDITALIC, BaseColor.WHITE);
                iTextSharp.text.Font fontTiny = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.ITALIC, BaseColor.BLACK);
                
                //Create water Mark Text Inside the pdf content.
                //string watermarkText = "Elango";
                //float fontSize = 50;
                //float xPosition = 450;
                //float yPosition = 900;
                //float angle = 45;

                Document pdfDoc = new Document(PageSize.A3, 0, 0, 40, 25);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, myMemoryStream);
                int cols = dataTable.Columns.Count;
                int rows = dataTable.Rows.Count;
                pdfDoc.Open();

                //Water Mark Creation. 

                //PdfContentByte under = writer.DirectContentUnder;
                //BaseFont baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.WINANSI, BaseFont.EMBEDDED);
                //under.BeginText();
                //under.SetColorFill(BaseColor.LIGHT_GRAY);
                //under.SetFontAndSize(baseFont, fontSize);
                //under.ShowTextAligned(PdfContentByte.ALIGN_CENTER, watermarkText, xPosition, yPosition, angle);
                //under.EndText();
 

                iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(Global.getLogoAsBytes());
                image.ScaleToFit(300f, 30f);
                Paragraph pageHeading = new Paragraph(Global.getTransactionDate().ToShortDateString() + " - " + heading + "                            ");
                pageHeading.IndentationLeft = 90f;
                pageHeading.Alignment = Element.ALIGN_RIGHT;

                pageHeading.Add(image);
                pdfDoc.Add(pageHeading);
                



                pdfDoc.Add(new Paragraph("\n"));

                PdfPTable table = new PdfPTable(cols);
                PdfPCell headerCell = new PdfPCell();
 
                foreach (DataColumn column in dataTable.Columns)
                {
                    table.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    headerCell = new PdfPCell(new Phrase(column.ColumnName, fontBoldItalic));
                    headerCell.BackgroundColor = new BaseColor(44, 193, 133);
                    headerCell.MinimumHeight = 30;
                    headerCell.Padding = 5;

                    table.AddCell(headerCell);
                }
                for (int i = 0; i < rows; i++)
                {
                    PdfPCell rowCell = new PdfPCell();

                    for (int j = 0; j < cols; j++)
                    {
                        var temp = dataTable.Rows[i][j].ToString();
                        rowCell = new PdfPCell(new Phrase(temp, fontTiny));
                        table.AddCell(rowCell);
                    }
                }
                pdfDoc.Add(table);
                pdfDoc.Close();

                SaveReportFileDialog.SaveMemoryStream(myMemoryStream, fileName, fileExtension);
            }
        }
        public static bool SaveMemoryStream(MemoryStream ms, string defaultFileName, string extension)
        {
            bool wasFileSaved = false;
            try
            {
                SaveFileDialog sfDlg = new SaveFileDialog();
                try
                {
                    sfDlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                    switch (extension.ToLower())
                    {
                        case "xls":
                            sfDlg.Filter = "Microsoft Office Excel Workbook (*.xls)|*.xls";
                            break;
                        case "pdf":
                            sfDlg.Filter = "Adobe Portable Document Format (*.pdf)|*.pdf";
                            break;
                        default:
                            sfDlg.Filter = string.Format("{1} files|*.{0}", extension, extension.ToUpper());
                            break;
                    }

                    sfDlg.RestoreDirectory = true;
                    sfDlg.FileName = defaultFileName;
                    if (sfDlg.ShowDialog() == DialogResult.OK)
                    {
                        File.WriteAllBytes(sfDlg.FileName, ms.ToArray());

                        wasFileSaved = true;

                        if (DialogResult.Yes == MessageBox.Show(
                                        "Do you want to open file?",
                                        "Confirmation",
                                        MessageBoxButtons.YesNo,
                                        MessageBoxIcon.Question))
                        {
                             System.Diagnostics.Process.Start(sfDlg.FileName);
                        }
                    }
                }
                finally
                {
                    sfDlg.Dispose();
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(
                        exc.Message,
                        "Aborted",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
            }

            return wasFileSaved;
        }


    }
}
