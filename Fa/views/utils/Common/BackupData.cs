using fa.Data;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fa.views.utils.Common
{
    public class BackupData
    {
        public void BackupMyData()
        {
            Cursor.Current = Cursors.WaitCursor;
            string server = !string.IsNullOrEmpty(Global.DefaultHostName) ? Global.DefaultHostName : "localhost";
            string database = "vv-matrix";
            string user = "admin";
            string password = "adminpass";
            string backupFilePath = string.Empty;
            try
            {
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "SQL files (*.sql)|*.sql";
                    saveFileDialog.Title = "Save Backup File";
                    saveFileDialog.FileName = "Dump" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".sql";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        backupFilePath = saveFileDialog.FileName;
                    }
                    else
                    {
                        return;
                    }
                }
                Cursor.Current = Cursors.WaitCursor;
                string mysqldumpPath = @"C:\Program Files\MySQL\MySQL Server 8.0\bin\mysqldump";
                string args = $"-h {server} -u {user} -p{password} {database} --hex-blob --routines --triggers --single-transaction --databases {database}";

                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = mysqldumpPath,
                    Arguments = args,
                    RedirectStandardInput = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                using (Process process = Process.Start(psi)!)
                {
                    using (StreamReader reader = process.StandardOutput)
                    {
                        string output = reader.ReadToEnd();
                        File.WriteAllText(backupFilePath, output);
                    }
                    process.WaitForExit();

                    if (process.ExitCode == 0)
                    {
                        MessageBox.Show($"Backup created successfully at {backupFilePath}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        string error = process.StandardError.ReadToEnd();
                        MessageBox.Show($"Backup failed. Error: {error}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
