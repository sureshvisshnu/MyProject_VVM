using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static fa.views.utils.Common.BlankTablesWithBorder;

namespace fa.views.utils.Common
{
    public class TableAlignments
    {
        public static PdfPTable TableAlignment()
        {

            PdfTableHeader TableHeader = new PdfTableHeader();

            PdfPTable TableAlignment = new PdfPTable(1);
            return TableAlignment;

        }
        public string WriteTable(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(@"<table>
                    <tr>
                    <th>No</th>
                    <th>Code</th>
                    <th>Name</th>
                    </tr>
                    ");

            foreach (DataRow dr in dt.Rows)
            {
                sb.Append("<tr>");
                sb.AppendFormat("<td>{0}</td>", dr["no"]);
                sb.AppendFormat("<td>{0}</td>", dr["code"]);
                sb.AppendFormat("<td>{0}</td>", dr["name"]);
                sb.Append("</tr>");
            }


            sb.Append("</table>");

            return sb.ToString();
        }
        public void HtmlFile()
        {
            //FileStream stream = new FileStream((Path.GetTempPath() + Filename + GetExtension(PatientDocument.FileType)), FileMode.CreateNew);
            var docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            var dirName = $@"{docPath}\HtmlFiles";
            DirectoryInfo di = Directory.CreateDirectory(dirName);

            using (FileStream fs = new FileStream(di + "HtmlTest" + ".html", FileMode.Create)) //Path.GetTempPath()
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    w.WriteLine("<html>");
                    w.WriteLine("</head>");
                    w.WriteLine("<Title>");
                    w.WriteLine("Test HTML File");
                    w.WriteLine("</Title>");
                    w.WriteLine("<body>");
                    w.WriteLine("This is First HTML");
                    w.WriteLine("This is First Line");
                    w.WriteLine("</body>");
                    w.WriteLine("</html>");
                }
            }
        }
        public StringBuilder HtmlExport(DataGridView GridView)
        {
            StringBuilder strB = new StringBuilder();

            //StringWriter sw = new StringWriter();
            //HtmlTextWriter htw = new HtmlTextWriter(sw);

            //create html & table
            var docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            var dirName = $@"{docPath}\HtmlFiles";
            DirectoryInfo di = Directory.CreateDirectory(dirName);

            strB.AppendLine("<html><body><center><" +
                          "table border='1' cellpadding='0' cellspacing='0'>");
            strB.AppendLine("<tr>");
            //cteate table header
            for (int i = 0; i < GridView.Columns.Count; i++)
            {
                strB.AppendLine("<td align='center' valign='middle'>" +
                               GridView.Columns[i].HeaderText + "</td>");
            }

            //create table body
            strB.AppendLine("<tr>");
            for (int i = 0; i < GridView.Rows.Count; i++)
            {
                strB.AppendLine("<tr>");
                foreach (DataGridViewCell dgvc in GridView.Rows[i].Cells)
                {
                    strB.AppendLine("<td align='center' valign='middle'>" +
                                    dgvc.Value.ToString() + "</td>");
                }
                strB.AppendLine("</tr>");

            }
            //table footer & end of html file
            strB.AppendLine("</table></center></body></html>");
            //File.WriteAllText(@"E:\Files\DataGridView.html", strB);
            return strB;

            /*
            //Table start.
            string html = "<table cellpadding='5' cellspacing='0' style='border: 1px solid #ccc;font-size: 9pt;font-family:arial'>";

            //Adding HeaderRow.
            html += "<tr>";
            foreach (DataGridViewColumn column in GridView.Columns)
            {
                html += "<th style='background-color: #B8DBFD;border: 1px solid #ccc'>" + column.HeaderText + "</th>";
            }
            html += "</tr>";

            //Adding DataRow.
            foreach (DataGridViewRow row in GridView.Rows)
            {
                html += "<tr>";
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.Value != null)
                    {
                        html += "<td style='width:120px;border: 1px solid #ccc'>" + cell.Value != null ? cell.Value.ToString() : string.Empty + "</td>";
                    }
                }
                html += "</tr>";
            }

            //Table end.
            html += "</table>";
             File.WriteAllText(@"E:\Files\DataGridView.html", html);
            */


        }
    }
}
