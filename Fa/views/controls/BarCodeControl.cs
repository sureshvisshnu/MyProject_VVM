using System;
using System.Drawing;
using System.Windows.Forms;
using fa.api.utils;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using fa.views.utils;

namespace fa.views.controls
{

    public partial class BarCodeControl : UserControl
    {

        protected string _NamePrice { get; set; }
        public string NamePrice
        {
            get
            {
                return _NamePrice;
            }
            set
            {
                _NamePrice = value;
            }
        }
        
        protected int _Quantity { get; set; }
        public int Quantity
        {
            get
            {
                return _Quantity;
            }
            set
            {
                _Quantity = value;
            }
        }
        protected Point _StartLocation {get;set;}
        public Point StartLocation
        {
            get
            {
                return _StartLocation;
            }
            set
            {
                _StartLocation = value;
            }
        }
        protected string _label { get; set; }
        public string Caption
        {
            get
            {
                return _label;
            }
            set
            {
                _label = value;
                label5.Text = value;
            }
        }
        protected string _text { get; set; }
        public override string Text 
        {
            get
            {
                return _text;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _text = value;
                    PrintBarCode();

                }
            }
        }
        private void PrintBarCode()
        {
            if (!string.IsNullOrEmpty(_text))
            {
                pictureBoxPatientNumber.BackgroundImage = BarCode.GenerateImageBarcode(_text);
            }
            else
            {
                pictureBoxPatientNumber.ResetText();
            }
        }

        public BarCodeControl()
        {
            InitializeComponent();
        }

        private void BarCode_Load(object sender, EventArgs e)
        {
            Text = "";
        }

        //public void printLabel()
        //{
        //    System.Drawing.Printing.PrintDocument myPrintDocument = new System.Drawing.Printing.PrintDocument();
        //    PrintDialog myPrinDialog = new PrintDialog();
        //    myPrintDocument.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(printLabel_PrintPage);
        //    myPrinDialog.Document = myPrintDocument;
        //    myPrintDocument.PrinterSettings.Copies =(short)_Quantity;
        //    if (myPrinDialog.ShowDialog() == DialogResult.OK)
        //    {
        //        myPrintDocument.Print();
        //    }
        //}
        //public void printLabel_PrintPage(System.Object sender, System.Drawing.Printing.PrintPageEventArgs e)
        //{
        //    //e.Graphics.DrawImage(BarCode.GenerateImageBarcode(Text), 0, 0);

        //    DataGridView DataGridView = new DataGridView();
        //    DataGridView.DataError += DataGridView_DataError;
        //    System.Drawing.Image Image = BarCode.GenerateImageBarcode(Text);
        //    DataGridView.DefaultCellStyle.SelectionBackColor = Color.White;
        //    DataGridView.DefaultCellStyle.SelectionForeColor = Color.Black;
        //    DataGridView.BackgroundColor = Color.White;
        //    DataGridView.BorderStyle = BorderStyle.None;
        //    DataGridView.CellBorderStyle = DataGridViewCellBorderStyle.None;
        //    DataGridView.RowHeadersVisible = false;
        //    DataGridView.ColumnHeadersVisible = false;
        //    DataGridView.AllowUserToAddRows = false;
        //    DataGridView.Width = 200;
        //    DataGridView.Height = 100;
            
        //    DataGridViewImageColumn imageCol = new DataGridViewImageColumn();
        //    imageCol.ImageLayout = DataGridViewImageCellLayout.Zoom;
        //    DataGridView.Columns.Add(imageCol);
        //    DataGridView.Columns[0].Width = 200;

        //    DataGridView.AllowUserToResizeColumns = false;
        //    DataGridView.Rows.Add(3);
        //    DataGridView.AllowUserToResizeRows = true;

        //    DataGridView.Rows[0].Height = 40;
        //    DataGridView.Rows[0].DefaultCellStyle.WrapMode=DataGridViewTriState.True;
        //    DataGridView.Rows[0].Cells[0] = new DataGridViewTextBoxCell();
        //    DataGridView.Rows[0].Cells[0].Value = "\n" + Global.Company.Name + "\n" + NamePrice;
        //    DataGridView.Rows[0].Cells[0].Style.Alignment = DataGridViewContentAlignment.MiddleLeft;

        //    DataGridView.Rows[1].Height = 30;
        //    DataGridView.Rows[1].Cells[0] = new DataGridViewImageCell();
        //    DataGridView.Rows[1].Cells[0].Style.WrapMode = DataGridViewTriState.True;
        //    DataGridView.Rows[1].Cells[0].Value = Image;
        //    (DataGridView.Rows[1].Cells[0] as DataGridViewImageCell).ImageLayout = DataGridViewImageCellLayout.Stretch;
        //    DataGridView.Rows[1].Cells[0].Style.Alignment = DataGridViewContentAlignment.MiddleLeft;

        //    DataGridView.Rows[2].Height = 30;
        //    DataGridView.Rows[2].Cells[0] = new DataGridViewTextBoxCell();
        //    DataGridView.Rows[2].Cells[0].Style.WrapMode = DataGridViewTriState.True;
        //    DataGridView.Rows[2].Cells[0].Value = Text;
        //    DataGridView.Rows[2].Cells[0].Style.Alignment = DataGridViewContentAlignment.TopCenter;

        //    Bitmap bm = new Bitmap(DataGridView.Width, DataGridView.Height);
        //    DataGridView.DrawToBitmap(bm, new System.Drawing.Rectangle(0, 0, DataGridView.Width, DataGridView.Height));
        //    e.Graphics.DrawImage(bm, 0, 0);

        //}
      
        //public void Printcheck()
        //{
            
        //    System.Drawing.Printing.PrintDocument myPrintDocument = new System.Drawing.Printing.PrintDocument();
        //    PrintDialog myPrinDialog = new PrintDialog();           
        //    myPrintDocument.DefaultPageSettings.PaperSource.SourceName= "A4";
        //    myPrintDocument.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(printA4_Check);         
        //    myPrinDialog.Document = myPrintDocument;
        //    if (myPrinDialog.ShowDialog() == DialogResult.OK)
        //    {
        //        myPrintDocument.Print();
        //    }
        //}
        private void DataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }
        private DataGridView GenerateGride(int lQuantity)
        {
            DataGridView DataGridView = new DataGridView();
            DataGridView.DataError += DataGridView_DataError;            
            System.Drawing.Image Image = BarCode.GenerateImageBarcode(Text);
            DataGridView.DefaultCellStyle.SelectionBackColor = Color.White;
            DataGridView.DefaultCellStyle.SelectionForeColor = Color.Black;
            DataGridView.BackgroundColor = Color.White;
            DataGridView.BorderStyle = BorderStyle.None;
            DataGridView.CellBorderStyle = DataGridViewCellBorderStyle.None;
            DataGridView.RowHeadersVisible = false;
            DataGridView.ColumnHeadersVisible = false;
            DataGridView.AllowUserToAddRows = false;
            DataGridView.Width= 900;
            DataGridView.Height = 1200;
            for (int i = 0; i < 4; i++)
            {
                DataGridViewImageColumn imageCol = new DataGridViewImageColumn();
                imageCol.ImageLayout = DataGridViewImageCellLayout.Zoom;
                DataGridView.Columns.Add(imageCol);
                DataGridView.Columns[i].Width = 200;

            }
            DataGridView.AllowUserToResizeColumns = false;
            DataGridView.Rows.Add((((lQuantity / 4) + ((lQuantity % 4) != 0 ? 1 : 0))*3));
            
            int x = 0;
            int y = 0;
            int z = 2;

            foreach (DataGridViewRow row in DataGridView.Rows)
            {
                DataGridView.AllowUserToResizeRows = true;
                row.Height = 30;

                if (y==row.Index)
                {
                   
                    row.Height = 40;
                    for (int j = 0; j < 4; j++)
                    {
                        row.Cells[j] = new DataGridViewTextBoxCell();
                        row.Cells[j].Style.WrapMode=DataGridViewTriState.True;
                        row.Cells[j].Value = "\n"+Global.Company.Name + "\n" + NamePrice;
                        row.Cells[j].Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                    }
                    y = y + 3;
                    continue;
                }
                if (z == row.Index)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        row.Cells[j] = new DataGridViewTextBoxCell();
                        row.Cells[j].Style.WrapMode = DataGridViewTriState.True;
                        row.Cells[j].Value = Text;
                        row.Cells[j].Style.Alignment = DataGridViewContentAlignment.TopCenter;
                    }
                    z = z + 3;
                    continue;
                }
                for (int j = 0; j < 4; j++)
                {
                    if ((lQuantity) <= x)
                    {
                        row.Cells[j] = new DataGridViewTextBoxCell();
                        continue;
                    }
                    row.Cells[j] = new DataGridViewImageCell();
                    row.Cells[j].Style.WrapMode = DataGridViewTriState.True;
                    row.Cells[j].Value = Image;
                    (row.Cells[j] as DataGridViewImageCell).ImageLayout = DataGridViewImageCellLayout.Stretch;
                    row.Cells[j].Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                    x++;
                }
            }
            return DataGridView;
        }
        public void printA4_Check(System.Object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            if (Quantity <= 56)
            {
                DataGridView DataGridViewPrint = GenerateGride(Quantity);
                Bitmap bm = new Bitmap(DataGridViewPrint.Width, DataGridViewPrint.Height);
                DataGridViewPrint.DrawToBitmap(bm, new System.Drawing.Rectangle(0, 0, DataGridViewPrint.Width, DataGridViewPrint.Height));
                e.Graphics.DrawImage(bm, 0, 0);
            }
            else
            {
                DataGridView DataGridViewPrint = GenerateGride(56);
                Bitmap bm = new Bitmap(DataGridViewPrint.Width, DataGridViewPrint.Height);
                DataGridViewPrint.DrawToBitmap(bm, new System.Drawing.Rectangle(0, 0, DataGridViewPrint.Width, DataGridViewPrint.Height));
                e.Graphics.DrawImage(bm, 0, 0);
                Quantity = Quantity - 56;
                e.HasMorePages = true;
            }
        }


        public void PrintcheckByItext()
        {
            GeneratePDF("Barcode", "pdf", true);
        }

        public void GeneratePDF(string fileName, string fileExtension, bool isPrint)
        {
            var path = System.AppDomain.CurrentDomain.BaseDirectory;
            using (MemoryStream myMemoryStream = new MemoryStream())
            {
                Document pdfDoc = new Document(PageSize.A4, -60, -60, 19, 20);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, myMemoryStream);
                pdfDoc.Open();
                
                PdfPTable ReportMainTable = MainTable(fileName);

                pdfDoc.Add(ReportMainTable);
                pdfDoc.Close();

                PdfGeneration.SaveMemoryStream(myMemoryStream, fileName, fileExtension, isPrint, PaperTypes.A4_PORTRAIT);
            }
        }
        private PdfPTable MainTable(String TypeOfReport)
        {
            MemoryStream Barcode = new MemoryStream();

            var Image = BarCode.GenerateImageBarcode1(Text);
            Image.Save(Barcode, System.Drawing.Imaging.ImageFormat.Png);

            var AboveBarcodeName = " "+Global.Company.Name + "\n" + " "+NamePrice;

            var BelowBarcodeName = Text;
            if (BelowBarcodeName.Length < 45)
            {
                int Space = (45 - BelowBarcodeName.Length);
                int Spacelength = ((Space / 2) + (Space % 2));
                string FrontSpace = "";
                for (int s = 0; s < Spacelength; s++)
                {
                    FrontSpace += " ";
                }
                BelowBarcodeName = FrontSpace + BelowBarcodeName;
            }

            string RunningPath = AppDomain.CurrentDomain.BaseDirectory;
            string FONT = string.Format("{0}Resources\\CenturyGothic.ttf", Path.GetFullPath(Path.Combine(RunningPath, @"..\..\")));
            iTextSharp.text.Font Font_Bold_Italic_10_Black = FontFactory.GetFont(FONT, 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
            iTextSharp.text.Font Font_Bold_Italic_9_White = FontFactory.GetFont(FONT, 10, iTextSharp.text.Font.BOLD, BaseColor.WHITE);
            iTextSharp.text.Font Font_Normal_Italic_8_Black = FontFactory.GetFont(FONT, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

            int Cols = 4;
            int Rows = ((Quantity / 4) + ((Quantity % 4) != 0 ? 1 : 0));
            int AddColumn = (Quantity % 4);
            PdfPTable ReportMainTable = new PdfPTable(Cols);

            float[] widths = null;
            
            widths = new float[] { 25f, 25f, 25f, 25f };
            ReportMainTable.SetWidths(widths);


            iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(Barcode.ToArray());
            image.ScaleAbsoluteHeight(28);
            image.ScaleAbsoluteWidth(130);
            var Temp = image;

            for (int i = 0; i < Rows; i++)
            {
                PdfPCell RowCell = new PdfPCell();

                for (int j = 0; j < Cols; j++)
                {
                    if (AddColumn != 0 && AddColumn <= j && i == (Rows - 1))
                    {
                        RowCell = new PdfPCell();
                        RowCell.BorderColor = BaseColor.WHITE;
                        ReportMainTable.AddCell(RowCell);
                        continue;
                    }
                    RowCell = new PdfPCell(new Phrase(AboveBarcodeName, Font_Normal_Italic_8_Black));

                    RowCell.MinimumHeight = 20;
                    RowCell.PaddingTop = -3;
                    RowCell.UseVariableBorders = true;
                    RowCell.BorderColorLeft = BaseColor.GRAY;
                    RowCell.BorderColorTop = BaseColor.GRAY;
                    RowCell.BorderColorRight = BaseColor.GRAY;
                    RowCell.BorderColorBottom = BaseColor.WHITE;
                    RowCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    RowCell.AddElement(new Phrase(AboveBarcodeName, Font_Normal_Italic_8_Black));

                    RowCell.MinimumHeight = 20;
                    RowCell.PaddingTop = -3;
                    RowCell.UseVariableBorders = true;
                    RowCell.BorderColorLeft = BaseColor.GRAY;
                    RowCell.BorderColorTop = BaseColor.WHITE;
                    RowCell.BorderColorRight = BaseColor.GRAY;
                    RowCell.BorderColorBottom = BaseColor.WHITE;
                    RowCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    RowCell.AddElement(Temp);

                    RowCell.MinimumHeight = 10;
                    RowCell.PaddingTop = -4;
                    RowCell.UseVariableBorders = true;
                    RowCell.BorderColorLeft = BaseColor.GRAY;
                    RowCell.BorderColorTop = BaseColor.GRAY;
                    RowCell.BorderColorRight = BaseColor.GRAY;
                    RowCell.BorderColorBottom = BaseColor.GRAY;
                    RowCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    RowCell.VerticalAlignment = Element.ALIGN_TOP;
                    RowCell.AddElement(new Phrase(BelowBarcodeName, Font_Normal_Italic_8_Black));
                    ReportMainTable.AddCell(RowCell);
                }
            }

            return ReportMainTable;
        }        
    }
}
