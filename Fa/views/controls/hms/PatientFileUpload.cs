using fa.views.hms.masters.upload;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fa.views.controls.hms
{
    public delegate void OnClickUpload(object sender, EventArgs e);
    public delegate void OnClickDownload(object sender, EventArgs e);
    public delegate void OnClickExit(object sender, EventArgs e);
    public partial class PatientFileUpload : UserControl
    {
        public bool StopProcessing = false;
        public bool PauseWaitCursor = false;
        public bool WorkFlow = false;
        public string FileUploadMismatchColoumn = "Selected File for uploading is mismatched with coloumn header";
        public string CheckBoxUncheckMessage = "Please click the has header checkBox";
        public string SoftwareMisMatchMessage = "Please check the software type";
        public string selectedSoftware { get; set; }
        public bool OverWritePatient { get; set; }
        public CancellationTokenSource loopCanceller = new CancellationTokenSource();
        public UploadProcessor? Processor { get; set; }

        public PatientFileUpload()
        {
            InitializeComponent();
        }
        private void PatientFileUpload_Load(object sender, EventArgs e)
        {
            comboBoxSoftwareType.SelectedIndex = 0;
            selectedSoftware = comboBoxSoftwareType.Text;
            OverWritePatient = checkBoxPatientDublicate.Checked;
        }
        public void SetProgressMax(int max)
        {
            progressBar1.Invoke(new Action(() => progressBar1.Value = 0));
            progressBar1.Invoke(new Action(() => progressBar1.Minimum = 0));
            progressBar1.Invoke(new Action(() => progressBar1.Maximum = max));
        }
        public void IncrementProgress()
        {
            progressBar1.Invoke(new Action(() => progressBar1.Increment(1)));
        }
        public void CompleteIncrementProgress(int maxval)
        {
            progressBar1.Invoke(new Action(() => progressBar1.Increment(maxval)));
        }
        public void EnableExitButton(bool ButtonStatus)
        {
            BtnExit.Invoke(new Action(() => BtnExit.Enabled = ButtonStatus));
        }

        private void BtnChoose_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            TextBoxFileName.Text = "";
            SetProgressMax(0);
            Invoke((Action)(() => TxtAccessLog.Clear()));
            Invoke((Action)(() => TxtErrorLog.Clear()));
            string FilePath = string.Empty;
            string FileExt = string.Empty;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowHelp = true;
            openFileDialog.AddExtension = true;
            openFileDialog.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (!string.IsNullOrEmpty(openFileDialog.FileName))
                {
                    FilePath = openFileDialog.FileName;
                    FileExt = Path.GetExtension(FilePath);
                    if (FileExt.CompareTo(".xls") == 0 || FileExt.CompareTo(".xlsx") == 0 || FileExt.CompareTo(".xlsm") == 0)
                    {
                        TextBoxFileName.Text = FilePath;
                    }
                }
            }
            this.UseWaitCursor = false;
        }
        private void BtnUpload_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBoxFileName.Text))
            {
                MessageBox.Show("Please choose a file before upload.");
                BtnChoose.Select();
            }
            else
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        // UI update, Invoke needed because we are in another thread
                        WorkFlow = true;
                        Invoke((Action)(() => BtnChoose.Enabled = false));
                        Invoke((Action)(() => BtnUpload.Enabled = false));
                        Invoke((Action)(() => BtnDownload.Enabled = false));
                        Invoke((Action)(() => ChkBoxHasHeader.Enabled = false));
                        this.loopCanceller.Token.ThrowIfCancellationRequested(); // exit, if cancelled
                        Invoke((Action)(() => TxtAccessLog.Clear()));
                        Invoke((Action)(() => TxtErrorLog.Clear()));
                        EventHandler handler = OnClickUpload;
                        handler?.Invoke(this, e);
                        Invoke((Action)(() => BtnChoose.Enabled = true));
                        Invoke((Action)(() => BtnUpload.Enabled = true));
                        Invoke((Action)(() => BtnDownload.Enabled = true));
                        Invoke((Action)(() => ChkBoxHasHeader.Enabled = true));
                        WorkFlow = false;
                    }
                    catch (OperationCanceledException ex) //catch (Exception ex)
                    {
                        Invoke((Action)(() => BtnChoose.Enabled = true));
                        Invoke((Action)(() => BtnUpload.Enabled = true));
                        Invoke((Action)(() => BtnDownload.Enabled = true));
                        Invoke((Action)(() => ChkBoxHasHeader.Enabled = true));
                        loopCanceller = new CancellationTokenSource(); // resetting the canceller
                        Invoke((Action)(() => this.Text = ex.HResult.ToString()));
                    }
                    catch (Exception ex)
                    {
                        if (ex.HResult.ToString() == "-2146233080")
                        {
                            WorkFlow = false;
                            Invoke((Action)(() => BtnExit.Enabled = false));
                            this.loopCanceller.Token.ThrowIfCancellationRequested(); // exit, if file error
                            MessageBox.Show("Please choose valid file for upload.");
                            Invoke((Action)(() => BtnChoose.Enabled = true));
                            Invoke((Action)(() => BtnUpload.Enabled = true));
                            Invoke((Action)(() => BtnDownload.Enabled = true));
                            Invoke((Action)(() => BtnExit.Enabled = true));
                            Invoke((Action)(() => ChkBoxHasHeader.Enabled = true));
                            Invoke((Action)(() => progressBar1.Value = 0));
                            Invoke((Action)(() => BtnChoose.Select()));
                            loopCanceller = new CancellationTokenSource(); // resetting the canceller
                        }
                        else
                        {
                            MessageBox.Show(Name + " : " + ex.InnerException!.Message);
                        }
                    }
                    finally
                    {
                        StopProcessing = false;
                    }
                }, loopCanceller.Token);
            }
        }
        public event EventHandler OnClickUpload;
        public string FileName()
        {
            return TextBoxFileName.Text;
        }
        public bool HasHeader()
        {
            return ChkBoxHasHeader.Checked;
        }
        public bool IsOverwrite()
        {
            return checkBoxPatientDublicate.Checked;
        }
        public string SoftwareType()
        {
            return selectedSoftware;
        }
        public bool overWritePatient()
        {
            return OverWritePatient;
        }
        public void ExitFileUpload()
        {
            Invoke((Action)(() => BtnExit.PerformClick()));
        }
        public void AddTimingLog(string TimeLog)
        {
            TimerTextBox.Invoke(new Action(() => TimerTextBox.Text = (TimeLog)));
            TimerTextBox.Invoke(new Action(() => TimerTextBox.Update()));
        }
        public void AddAccessLog(string Log)
        {
            TxtAccessLog.Invoke(new Action(() => TxtAccessLog.AppendText(Log)));
            TxtAccessLog.Invoke(new Action(() => TxtAccessLog.AppendText(Environment.NewLine)));
        }
        public void AddErrorLog(string Log)
        {
            TxtErrorLog.Invoke(new Action(() => TxtErrorLog.AppendText(Log)));
            TxtErrorLog.Invoke(new Action(() => TxtErrorLog.AppendText(Environment.NewLine)));
        }
        public event EventHandler OnClickExit;
        private void BtnExit_Click(object sender, EventArgs e)
        {
            StopProcessing = true;
            EventHandler handler = OnClickExit;
            handler?.Invoke(this, e);
        }
        private void BtnExit_MouseHover(object sender, EventArgs e)
        {
            if (WorkFlow == true)
            {
                PauseWaitCursor = true;
            }
        }
        private void BtnExit_MouseLeave(object sender, EventArgs e)
        {
            if (WorkFlow == true)
            {
                PauseWaitCursor = false;
            }
        }

        private void comboBoxSoftwareType_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedSoftware = comboBoxSoftwareType.Text;
        }
        public event EventHandler OnClickDownload;
        private void BtnDownload_Click(object sender, EventArgs e)
        {
            StopProcessing = true;
            EventHandler handler = OnClickDownload;
            try
            {
                handler?.Invoke(this, e);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during event handling: {ex.Message}");
            }
            finally
            {
                StopProcessing = false;
            }
        }

        private void checkBoxPatientDublicate_CheckedChanged(object sender, EventArgs e)
        {
            OverWritePatient = checkBoxPatientDublicate.Checked;
        }
    }
}
