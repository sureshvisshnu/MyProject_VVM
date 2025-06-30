using System;
using System.Data;
using System.Diagnostics;
using System.Runtime.Intrinsics.Arm;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using fa.api.utils;
using fa.views.hms.masters.upload;
using VisioForge.Libs.MediaFoundation.OPM;

namespace fa.views.controls.fileupload
{
    //public delegate void OnClickUpload(object sender, EventArgs e);
    //public delegate void OnClickExit(object sender, EventArgs e);
    public partial class FileUpload : UserControl
    {
        public bool StopProcessing = false;
        public bool PauseWaitCursor = false;
        public bool WorkFlow = false;
        string InsertFile;
        public string FileUploadMismatchColoumn = "Selected File for uploading is mismatched with coloumn header";
        public string FileUploadMismatchNofColoumn = "Selected File for uploading is mismatched with No of coloumn";
        public string FileUploadHasHeader = "Selected File for uploading is found with header";
        public string FileUploadNoHeader = "Selected File for uploading is not found with header";
        public string FileUploadProcessing = "Processing....";
        public string FileUploaContinuing = "Continuing....";
        public string FileUploaPaused = "Paused....";

        public CancellationTokenSource loopCanceller = new CancellationTokenSource();
        public UploadProcessor Processor { get; set; }
        public FileUpload()
        {
            InitializeComponent();
        }
        public event EventHandler OnClickUpload;
        public event EventHandler OnClickExit;
        public event EventHandler ShowDialog;

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
            InsertFile = string.Empty;
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
                        Invoke((Action)(() => ChkBoxHasHeader.Enabled = false));
                        Invoke((Action)(() => checkBoxRefreshData.Enabled = false));
                        this.loopCanceller.Token.ThrowIfCancellationRequested(); // exit, if cancelled
                        Invoke((Action)(() => TxtAccessLog.Clear()));
                        Invoke((Action)(() => TxtErrorLog.Clear()));
                        EventHandler handler = OnClickUpload;
                        handler?.Invoke(this, e);
                        Invoke((Action)(() => BtnChoose.Enabled = true));
                        Invoke((Action)(() => BtnUpload.Enabled = true));
                        Invoke((Action)(() => ChkBoxHasHeader.Enabled = true));
                        Invoke((Action)(() => checkBoxRefreshData.Enabled = true));
                        WorkFlow = false;
                    }
                    catch (OperationCanceledException ex) //catch (Exception ex)
                    {
                        Invoke((Action)(() => BtnChoose.Enabled = true));
                        Invoke((Action)(() => BtnUpload.Enabled = true));
                        Invoke((Action)(() => ChkBoxHasHeader.Enabled = true));
                        Invoke((Action)(() => checkBoxRefreshData.Enabled = true));
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
                            Invoke((Action)(() => BtnExit.Enabled = true));
                            Invoke((Action)(() => ChkBoxHasHeader.Enabled = true));
                            Invoke((Action)(() => checkBoxRefreshData.Enabled = true));
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
        public string FileName()
        {
            return TextBoxFileName.Text;
        }
        public bool HasHeader()
        {
            return ChkBoxHasHeader.Checked;
        }
        public bool RefreshAllData()
        {
            return checkBoxRefreshData.Checked;
        }
        public void ChangeCheckeState()
        {
            checkBoxRefreshData.Checked = false;
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

        public void RefreshallDataVisibility()
        {
            checkBoxRefreshData.Enabled = true;
            checkBoxRefreshData.Visible = true;
        }

        private void checkBoxRefreshData_CheckedChanged(object sender, EventArgs e)
        {
            EventHandler handler = ShowDialog;
            handler?.Invoke(this, e);
        }
    }
}
