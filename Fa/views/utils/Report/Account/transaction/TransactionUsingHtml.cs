using fa.report.accounting.transcation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace fa.views.utils.Report.Account.transaction
{
    public class TransactionUsingHtml
    {
        Transaction Transaction = new Transaction();

        public void LaserPrint(RbtTransaction RbtTransaction,bool isPrint)
        {
            string[] PageNumberToPage = new string[1000];
            DataTable dataTable = Transaction.TransactionAlignment(RbtTransaction);


            string jj = "";

            //var Renderer = new IronPdf.HtmlToPdf();
            //Renderer.PrintOptions.Header = new SimpleHeaderFooter()
            //{
            //    LeftText = "<h5>Abc Consulting Inc</h5>\n123/12, East cross street, Nagercoil",              
            //    RightText = "<h5>Transaction</h5>\nFrom 12.12.2000 To 12.04.2000",
            //    FontSize = 14

            //};
            
            //Renderer.PrintOptions.Footer = new SimpleHeaderFooter()
            //{

            //    CenterText = "Thank you",
            //    LeftText = "printed on 12.12.2000",
            //    RightText = "1/1",
            //    DrawDividerLine = true,
            //    FontSize = 10
            //};
            //var PDF = Renderer.RenderHtmlAsPdf("Hello IronPdf");



            var sfDlg = new SaveFileDialog();
            sfDlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments);
            sfDlg.Filter = string.Format("{1} files|*.{0}", "pdf", "pdf");
            sfDlg.RestoreDirectory = true;
            sfDlg.FileName = "test.pdf";
            if (sfDlg.ShowDialog() == DialogResult.OK)
            {
                //PDF.SaveAs(sfDlg.FileName);
            };

            


            //for (int i = 0; i < Rows; i++)
            //{                  
            //  for (int j = 0; j < Cols; j++)
            //  {
            //      var Temp = dataTable.Rows[i][j].ToString();                    
            //  }
            //}

        }
    }
}
