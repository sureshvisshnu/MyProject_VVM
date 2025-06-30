namespace fa.views.controls
{
    partial class FileUploader
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            label1 = new Label();
            DataGridViewFileUpload = new DataViewVerticalScroll();
            SeqNumber = new DataGridViewTextBoxColumn();
            FileName = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
            DeleteBtn = new DataGridViewButtonColumn();
            UploadBtn = new DataGridViewButtonColumn();
            filebytes = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)DataGridViewFileUpload).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(-4, 6);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(30, 15);
            label1.TabIndex = 1;
            label1.Text = "Files";
            // 
            // DataGridViewFileUpload
            // 
            DataGridViewFileUpload.AllowUserToAddRows = false;
            DataGridViewFileUpload.AllowUserToDeleteRows = false;
            DataGridViewFileUpload.AllowUserToResizeColumns = false;
            DataGridViewFileUpload.AllowUserToResizeRows = false;
            DataGridViewFileUpload.BackgroundColor = SystemColors.Control;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            DataGridViewFileUpload.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            DataGridViewFileUpload.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            DataGridViewFileUpload.Columns.AddRange(new DataGridViewColumn[] { dataGridViewTextBoxColumn1, dataGridViewTextBoxColumn2, DeleteBtn, UploadBtn, filebytes });
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = SystemColors.Window;
            dataGridViewCellStyle4.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle4.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = Color.White;
            dataGridViewCellStyle4.SelectionForeColor = Color.Black;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.False;
            DataGridViewFileUpload.DefaultCellStyle = dataGridViewCellStyle4;
            DataGridViewFileUpload.EditMode = DataGridViewEditMode.EditOnEnter;
            DataGridViewFileUpload.EnableHeadersVisualStyles = false;
            DataGridViewFileUpload.Location = new Point(0, 24);
            DataGridViewFileUpload.Margin = new Padding(4, 3, 4, 3);
            DataGridViewFileUpload.MultiSelect = false;
            DataGridViewFileUpload.Name = "DataGridViewFileUpload";
            DataGridViewFileUpload.RowHeadersVisible = false;
            DataGridViewFileUpload.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            DataGridViewFileUpload.Size = new Size(346, 163);
            DataGridViewFileUpload.TabIndex = 0;
            DataGridViewFileUpload.CellClick += DataGridViewFileUpload_CellClick;
            DataGridViewFileUpload.CellEnter += DataGridViewFileUpload_CellEnter;
            DataGridViewFileUpload.RowsAdded += DataGridViewFileUpload_RowsAdded;
            // 
            // SeqNumber
            // 
            SeqNumber.HeaderText = "#";
            SeqNumber.Name = "SeqNumber";
            SeqNumber.ReadOnly = true;
            SeqNumber.Width = 25;
            // 
            // FileName
            // 
            FileName.HeaderText = "File";
            FileName.Name = "FileName";
            FileName.ReadOnly = true;
            FileName.Width = 200;
            // 
            // dataGridViewTextBoxColumn1
            // 
            dataGridViewTextBoxColumn1.FillWeight = 34.01361F;
            dataGridViewTextBoxColumn1.HeaderText = "#";
            dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            dataGridViewTextBoxColumn1.ReadOnly = true;
            dataGridViewTextBoxColumn1.Resizable = DataGridViewTriState.False;
            dataGridViewTextBoxColumn1.SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridViewTextBoxColumn1.Width = 25;
            // 
            // dataGridViewTextBoxColumn2
            // 
            dataGridViewTextBoxColumn2.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewTextBoxColumn2.FillWeight = 316.9565F;
            dataGridViewTextBoxColumn2.HeaderText = "File";
            dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            dataGridViewTextBoxColumn2.ReadOnly = true;
            dataGridViewTextBoxColumn2.Resizable = DataGridViewTriState.False;
            dataGridViewTextBoxColumn2.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // DeleteBtn
            // 
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.NullValue = "+";
            DeleteBtn.DefaultCellStyle = dataGridViewCellStyle2;
            DeleteBtn.FillWeight = 21.16574F;
            DeleteBtn.HeaderText = " ";
            DeleteBtn.Name = "DeleteBtn";
            DeleteBtn.Resizable = DataGridViewTriState.False;
            DeleteBtn.ToolTipText = "Add File";
            DeleteBtn.Width = 25;
            // 
            // UploadBtn
            // 
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.NullValue = "X";
            UploadBtn.DefaultCellStyle = dataGridViewCellStyle3;
            UploadBtn.FillWeight = 27.8642F;
            UploadBtn.HeaderText = " ";
            UploadBtn.Name = "UploadBtn";
            UploadBtn.Resizable = DataGridViewTriState.False;
            UploadBtn.Width = 25;
            // 
            // filebytes
            // 
            filebytes.HeaderText = "hidden Bytes";
            filebytes.Name = "filebytes";
            filebytes.Resizable = DataGridViewTriState.False;
            filebytes.SortMode = DataGridViewColumnSortMode.NotSortable;
            filebytes.Visible = false;
            // 
            // FileUploader
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(DataGridViewFileUpload);
            Controls.Add(label1);
            Margin = new Padding(4, 3, 4, 3);
            Name = "FileUploader";
            Size = new Size(346, 190);
            Load += FileUploader_Load;
            SizeChanged += FileUploader_SizeChanged;
            Resize += FileUploader_Resize;
            ((System.ComponentModel.ISupportInitialize)DataGridViewFileUpload).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataViewVerticalScroll DataGridViewFileUpload;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn SeqNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewButtonColumn DeleteBtn;
        private System.Windows.Forms.DataGridViewButtonColumn UploadBtn;
        private System.Windows.Forms.DataGridViewTextBoxColumn filebytes;
    }
}
