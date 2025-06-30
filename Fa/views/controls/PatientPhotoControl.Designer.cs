namespace fa.views.controls
{
    partial class PatientPhotoControl
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PatientPhotoControl));
            label1 = new Label();
            BtnStart = new Button();
            pictureBoxPhoto = new PictureBox();
            ComboBoxChooseCamara = new ComboBox();
            BtnStop = new Button();
            BtnTake = new Button();
            imageList1 = new ImageList(components);
            BtnRemove = new Label();
            toolTip1 = new ToolTip(components);
            btnPatientPhotoUpload = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBoxPhoto).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(4, 2);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(72, 13);
            label1.TabIndex = 43;
            label1.Text = "Patient Photo";
            // 
            // BtnStart
            // 
            BtnStart.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            BtnStart.Location = new Point(0, 226);
            BtnStart.Margin = new Padding(4, 3, 4, 3);
            BtnStart.Name = "BtnStart";
            BtnStart.Size = new Size(201, 28);
            BtnStart.TabIndex = 52;
            BtnStart.Text = "Start";
            BtnStart.UseVisualStyleBackColor = true;
            BtnStart.Click += BtnStart_Click;
            // 
            // pictureBoxPhoto
            // 
            pictureBoxPhoto.BackgroundImageLayout = ImageLayout.None;
            pictureBoxPhoto.Image = (Image)resources.GetObject("pictureBoxPhoto.Image");
            pictureBoxPhoto.Location = new Point(0, 23);
            pictureBoxPhoto.Margin = new Padding(4, 3, 4, 3);
            pictureBoxPhoto.Name = "pictureBoxPhoto";
            pictureBoxPhoto.Size = new Size(201, 204);
            pictureBoxPhoto.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBoxPhoto.TabIndex = 49;
            pictureBoxPhoto.TabStop = false;
            // 
            // ComboBoxChooseCamara
            // 
            ComboBoxChooseCamara.AutoCompleteMode = AutoCompleteMode.Suggest;
            ComboBoxChooseCamara.AutoCompleteSource = AutoCompleteSource.ListItems;
            ComboBoxChooseCamara.FormattingEnabled = true;
            ComboBoxChooseCamara.Location = new Point(0, 230);
            ComboBoxChooseCamara.Margin = new Padding(4, 3, 4, 3);
            ComboBoxChooseCamara.Name = "ComboBoxChooseCamara";
            ComboBoxChooseCamara.Size = new Size(200, 23);
            ComboBoxChooseCamara.TabIndex = 50;
            ComboBoxChooseCamara.Visible = false;
            ComboBoxChooseCamara.SelectedIndexChanged += ComboBoxChooseCamara_SelectedIndexChanged;
            // 
            // BtnStop
            // 
            BtnStop.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            BtnStop.Location = new Point(0, 255);
            BtnStop.Margin = new Padding(4, 3, 4, 3);
            BtnStop.Name = "BtnStop";
            BtnStop.Size = new Size(201, 28);
            BtnStop.TabIndex = 52;
            BtnStop.TabStop = false;
            BtnStop.Text = "Stop";
            BtnStop.UseVisualStyleBackColor = true;
            BtnStop.Visible = false;
            BtnStop.Click += BtnStop_Click;
            // 
            // BtnTake
            // 
            BtnTake.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            BtnTake.Location = new Point(0, 228);
            BtnTake.Margin = new Padding(4, 3, 4, 3);
            BtnTake.Name = "BtnTake";
            BtnTake.Size = new Size(201, 25);
            BtnTake.TabIndex = 52;
            BtnTake.TabStop = false;
            BtnTake.Text = "Take Photo";
            BtnTake.UseVisualStyleBackColor = true;
            BtnTake.Visible = false;
            BtnTake.Click += BtnTake_Click;
            BtnTake.PreviewKeyDown += button3_PreviewKeyDown;
            // 
            // imageList1
            // 
            imageList1.ColorDepth = ColorDepth.Depth8Bit;
            imageList1.ImageStream = (ImageListStreamer)resources.GetObject("imageList1.ImageStream");
            imageList1.TransparentColor = Color.Transparent;
            imageList1.Images.SetKeyName(0, "pp.PNG");
            // 
            // BtnRemove
            // 
            BtnRemove.Image = (Image)resources.GetObject("BtnRemove.Image");
            BtnRemove.Location = new Point(174, 23);
            BtnRemove.Margin = new Padding(4, 0, 4, 0);
            BtnRemove.Name = "BtnRemove";
            BtnRemove.Size = new Size(27, 24);
            BtnRemove.TabIndex = 53;
            toolTip1.SetToolTip(BtnRemove, "Remove Photo");
            BtnRemove.Click += BtnRemove_Click;
            // 
            // btnPatientPhotoUpload
            // 
            btnPatientPhotoUpload.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            btnPatientPhotoUpload.Location = new Point(0, 256);
            btnPatientPhotoUpload.Margin = new Padding(4, 3, 4, 3);
            btnPatientPhotoUpload.Name = "btnPatientPhotoUpload";
            btnPatientPhotoUpload.Size = new Size(201, 25);
            btnPatientPhotoUpload.TabIndex = 54;
            btnPatientPhotoUpload.Text = "Upload Photo";
            btnPatientPhotoUpload.UseVisualStyleBackColor = true;
            btnPatientPhotoUpload.Click += btnPatientPhotoUpload_Click;
            // 
            // PatientPhotoControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(BtnRemove);
            Controls.Add(label1);
            Controls.Add(BtnStop);
            Controls.Add(pictureBoxPhoto);
            Controls.Add(btnPatientPhotoUpload);
            Controls.Add(BtnStart);
            Controls.Add(BtnTake);
            Controls.Add(ComboBoxChooseCamara);
            Margin = new Padding(4, 3, 4, 3);
            Name = "PatientPhotoControl";
            Size = new Size(201, 282);
            Load += PatientPhotoControl_Load;
            TabIndexChanged += PatientPhotoControl_TabIndexChanged;
            Enter += PatientPhotoControl_Enter;
            ((System.ComponentModel.ISupportInitialize)pictureBoxPhoto).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Button BtnStart;
        private PictureBox pictureBoxPhoto;
        private ComboBox ComboBoxChooseCamara;
        private Button BtnStop;
        private Button BtnTake;
        private ImageList imageList1;
        private Label BtnRemove;
        private ToolTip toolTip1;
        private Button btnPatientPhotoUpload;
    }
}
