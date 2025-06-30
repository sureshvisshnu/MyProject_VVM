namespace Fa.views.controls.Photo
{
    partial class CameraControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CameraControl));
            HeaderName = new Label();
            pictureBoxPhoto = new PictureBox();
            btnPatientPhotoUpload = new Button();
            BtnTake = new Button();
            BtnStop = new Button();
            BtnStart = new Button();
            BtnRemove = new Label();
            ComboBoxChooseCamara = new ComboBox();
            ((System.ComponentModel.ISupportInitialize)pictureBoxPhoto).BeginInit();
            SuspendLayout();
            // 
            // HeaderName
            // 
            HeaderName.AutoSize = true;
            HeaderName.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            HeaderName.Location = new Point(4, 2);
            HeaderName.Margin = new Padding(4, 0, 4, 0);
            HeaderName.Name = "HeaderName";
            HeaderName.Size = new Size(72, 13);
            HeaderName.TabIndex = 44;
            HeaderName.Text = "Header Name";
            // 
            // pictureBoxPhoto
            // 
            pictureBoxPhoto.BackgroundImageLayout = ImageLayout.None;
            pictureBoxPhoto.Image = (Image)resources.GetObject("pictureBoxPhoto.Image");
            pictureBoxPhoto.Location = new Point(2, 18);
            pictureBoxPhoto.Margin = new Padding(4, 3, 4, 3);
            pictureBoxPhoto.Name = "pictureBoxPhoto";
            pictureBoxPhoto.Size = new Size(150, 150);
            pictureBoxPhoto.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBoxPhoto.TabIndex = 50;
            pictureBoxPhoto.TabStop = false;
            // 
            // btnPatientPhotoUpload
            // 
            btnPatientPhotoUpload.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            btnPatientPhotoUpload.Location = new Point(2, 198);
            btnPatientPhotoUpload.Margin = new Padding(4, 3, 4, 3);
            btnPatientPhotoUpload.Name = "btnPatientPhotoUpload";
            btnPatientPhotoUpload.Size = new Size(150, 25);
            btnPatientPhotoUpload.TabIndex = 56;
            btnPatientPhotoUpload.Text = "Upload Photo";
            btnPatientPhotoUpload.UseVisualStyleBackColor = true;
            btnPatientPhotoUpload.Click += btnPatientPhotoUpload_Click;
            // 
            // BtnTake
            // 
            BtnTake.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            BtnTake.Location = new Point(2, 171);
            BtnTake.Margin = new Padding(4, 3, 4, 3);
            BtnTake.Name = "BtnTake";
            BtnTake.Size = new Size(150, 25);
            BtnTake.TabIndex = 55;
            BtnTake.TabStop = false;
            BtnTake.Text = "Take Photo";
            BtnTake.UseVisualStyleBackColor = true;
            BtnTake.Visible = false;
            BtnTake.Click += BtnTake_Click;
            // 
            // BtnStop
            // 
            BtnStop.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            BtnStop.Location = new Point(2, 198);
            BtnStop.Margin = new Padding(4, 3, 4, 3);
            BtnStop.Name = "BtnStop";
            BtnStop.Size = new Size(150, 25);
            BtnStop.TabIndex = 57;
            BtnStop.TabStop = false;
            BtnStop.Text = "Stop";
            BtnStop.UseVisualStyleBackColor = true;
            BtnStop.Visible = false;
            BtnStop.Click += BtnStop_Click;
            // 
            // BtnStart
            // 
            BtnStart.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            BtnStart.Location = new Point(2, 171);
            BtnStart.Margin = new Padding(4, 3, 4, 3);
            BtnStart.Name = "BtnStart";
            BtnStart.Size = new Size(150, 25);
            BtnStart.TabIndex = 58;
            BtnStart.Text = "Start";
            BtnStart.UseVisualStyleBackColor = true;
            BtnStart.Click += BtnStart_Click;
            // 
            // BtnRemove
            // 
            BtnRemove.Image = (Image)resources.GetObject("BtnRemove.Image");
            BtnRemove.Location = new Point(132, 19);
            BtnRemove.Margin = new Padding(4, 0, 4, 0);
            BtnRemove.Name = "BtnRemove";
            BtnRemove.Size = new Size(20, 20);
            BtnRemove.TabIndex = 59;
            BtnRemove.Click += BtnRemove_Click;
            // 
            // ComboBoxChooseCamara
            // 
            ComboBoxChooseCamara.AutoCompleteMode = AutoCompleteMode.Suggest;
            ComboBoxChooseCamara.AutoCompleteSource = AutoCompleteSource.ListItems;
            ComboBoxChooseCamara.FormattingEnabled = true;
            ComboBoxChooseCamara.Location = new Point(2, 171);
            ComboBoxChooseCamara.Margin = new Padding(4, 3, 4, 3);
            ComboBoxChooseCamara.Name = "ComboBoxChooseCamara";
            ComboBoxChooseCamara.Size = new Size(150, 23);
            ComboBoxChooseCamara.TabIndex = 60;
            ComboBoxChooseCamara.Visible = false;
            ComboBoxChooseCamara.SelectedIndexChanged += ComboBoxChooseCamara_SelectedIndexChanged;
            // 
            // CameraControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(BtnRemove);
            Controls.Add(BtnStop);
            Controls.Add(BtnStart);
            Controls.Add(btnPatientPhotoUpload);
            Controls.Add(BtnTake);
            Controls.Add(pictureBoxPhoto);
            Controls.Add(HeaderName);
            Controls.Add(ComboBoxChooseCamara);
            Name = "CameraControl";
            Size = new Size(155, 225);
            TabIndexChanged += CameraControl_TabIndexChanged;
            Enter += CameraControl_Enter;
            ((System.ComponentModel.ISupportInitialize)pictureBoxPhoto).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label HeaderName;
        private PictureBox pictureBoxPhoto;
        private Button btnPatientPhotoUpload;
        private Button BtnTake;
        private Button BtnStop;
        private Button BtnStart;
        private Label BtnRemove;
        private ComboBox ComboBoxChooseCamara;
    }
}
