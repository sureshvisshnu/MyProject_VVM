namespace Fa.views.hms.Masters
{
    partial class FormQuestion
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormQuestion));
            TextBoxAllergieCategoryId = new TextBox();
            ImageListAllergie = new ImageList(components);
            BtnAllergieExit = new Button();
            BtnAllergieSave = new Button();
            BtnAllergieCancel = new Button();
            TextBoxAllergieId = new TextBox();
            contextMenuStripAllergieCategory = new ContextMenuStrip(components);
            newCategoryToolStripMenuItem = new ToolStripMenuItem();
            newAllergieToolStripMenuItem = new ToolStripMenuItem();
            TabControlAllergie = new TabControl();
            TabPageAllergieDetails = new TabPage();
            ComboBoxAllergieCategory = new fa.views.controls.ComboBoxSwapTextBox();
            TextBoxAllergieDisplayas = new fa.views.controls.text.NameTextBoxAllowSpace(components);
            label4 = new Label();
            TextBoxAllergieName = new fa.views.controls.text.NameTextBoxAllowSpace(components);
            TextBoxAllergieReasonInactive = new TextBox();
            CheckBoxAllergieIsActive = new CheckBox();
            labelReasonforInactive = new Label();
            LabelCostCenterDisplayAs = new Label();
            LabelCostCenterName = new Label();
            statusStrip1 = new StatusStrip();
            AllergieErrorMsg = new ToolStripStatusLabel();
            contextMenuStripAllergie = new ContextMenuStrip(components);
            newAllergieToolStripMenuItem1 = new ToolStripMenuItem();
            contextMenuStripAllergieCategory.SuspendLayout();
            TabControlAllergie.SuspendLayout();
            TabPageAllergieDetails.SuspendLayout();
            statusStrip1.SuspendLayout();
            contextMenuStripAllergie.SuspendLayout();
            SuspendLayout();
            // 
            // TextBoxAllergieCategoryId
            // 
            TextBoxAllergieCategoryId.Location = new Point(20, 402);
            TextBoxAllergieCategoryId.Name = "TextBoxAllergieCategoryId";
            TextBoxAllergieCategoryId.Size = new Size(100, 23);
            TextBoxAllergieCategoryId.TabIndex = 52;
            TextBoxAllergieCategoryId.Visible = false;
            // 
            // ImageListAllergie
            // 
            ImageListAllergie.ColorDepth = ColorDepth.Depth8Bit;
            ImageListAllergie.ImageStream = (ImageListStreamer)resources.GetObject("ImageListAllergie.ImageStream");
            ImageListAllergie.TransparentColor = Color.Transparent;
            ImageListAllergie.Images.SetKeyName(0, "Category.png");
            ImageListAllergie.Images.SetKeyName(1, "allergy.jpg");
            // 
            // BtnAllergieExit
            // 
            BtnAllergieExit.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnAllergieExit.Location = new Point(687, 401);
            BtnAllergieExit.Name = "BtnAllergieExit";
            BtnAllergieExit.Size = new Size(83, 23);
            BtnAllergieExit.TabIndex = 49;
            BtnAllergieExit.Text = "Exit [F10]";
            BtnAllergieExit.UseVisualStyleBackColor = true;
            BtnAllergieExit.Click += BtnAllergieExit_Click;
            // 
            // BtnAllergieSave
            // 
            BtnAllergieSave.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnAllergieSave.Location = new Point(598, 401);
            BtnAllergieSave.Name = "BtnAllergieSave";
            BtnAllergieSave.Size = new Size(83, 23);
            BtnAllergieSave.TabIndex = 45;
            BtnAllergieSave.Text = "Save [F8]";
            BtnAllergieSave.UseVisualStyleBackColor = true;
            BtnAllergieSave.Click += BtnAllergieSave_Click;
            BtnAllergieSave.PreviewKeyDown += BtnAllergieSave_PreviewKeyDown;
            // 
            // BtnAllergieCancel
            // 
            BtnAllergieCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnAllergieCancel.Location = new Point(509, 401);
            BtnAllergieCancel.Name = "BtnAllergieCancel";
            BtnAllergieCancel.Size = new Size(83, 23);
            BtnAllergieCancel.TabIndex = 46;
            BtnAllergieCancel.Text = "Cancel [Esc]";
            BtnAllergieCancel.UseVisualStyleBackColor = true;
            BtnAllergieCancel.Click += BtnAllergieCancel_Click;
            // 
            // TextBoxAllergieId
            // 
            TextBoxAllergieId.Location = new Point(126, 402);
            TextBoxAllergieId.Name = "TextBoxAllergieId";
            TextBoxAllergieId.Size = new Size(100, 23);
            TextBoxAllergieId.TabIndex = 50;
            TextBoxAllergieId.Visible = false;
            // 
            // contextMenuStripAllergieCategory
            // 
            contextMenuStripAllergieCategory.Items.AddRange(new ToolStripItem[] { newCategoryToolStripMenuItem, newAllergieToolStripMenuItem });
            contextMenuStripAllergieCategory.Name = "contextMenuStripCategory";
            contextMenuStripAllergieCategory.Size = new Size(150, 48);
            // 
            // newCategoryToolStripMenuItem
            // 
            newCategoryToolStripMenuItem.Image = (Image)resources.GetObject("newCategoryToolStripMenuItem.Image");
            newCategoryToolStripMenuItem.Name = "newCategoryToolStripMenuItem";
            newCategoryToolStripMenuItem.Size = new Size(149, 22);
            newCategoryToolStripMenuItem.Text = "New Category";
            newCategoryToolStripMenuItem.Click += newCategoryToolStripMenuItem_Click;
            // 
            // newAllergieToolStripMenuItem
            // 
            newAllergieToolStripMenuItem.Image = (Image)resources.GetObject("newAllergieToolStripMenuItem.Image");
            newAllergieToolStripMenuItem.Name = "newAllergieToolStripMenuItem";
            newAllergieToolStripMenuItem.Size = new Size(149, 22);
            newAllergieToolStripMenuItem.Text = "New Allergie";
            newAllergieToolStripMenuItem.Click += newAllergieToolStripMenuItem_Click;
            // 
            // TabControlAllergie
            // 
            TabControlAllergie.Controls.Add(TabPageAllergieDetails);
            TabControlAllergie.Location = new Point(8, 3);
            TabControlAllergie.Name = "TabControlAllergie";
            TabControlAllergie.SelectedIndex = 0;
            TabControlAllergie.Size = new Size(789, 392);
            TabControlAllergie.TabIndex = 44;
            // 
            // TabPageAllergieDetails
            // 
            TabPageAllergieDetails.Controls.Add(ComboBoxAllergieCategory);
            TabPageAllergieDetails.Controls.Add(TextBoxAllergieDisplayas);
            TabPageAllergieDetails.Controls.Add(label4);
            TabPageAllergieDetails.Controls.Add(TextBoxAllergieName);
            TabPageAllergieDetails.Controls.Add(TextBoxAllergieReasonInactive);
            TabPageAllergieDetails.Controls.Add(CheckBoxAllergieIsActive);
            TabPageAllergieDetails.Controls.Add(labelReasonforInactive);
            TabPageAllergieDetails.Controls.Add(LabelCostCenterDisplayAs);
            TabPageAllergieDetails.Controls.Add(LabelCostCenterName);
            TabPageAllergieDetails.Location = new Point(4, 24);
            TabPageAllergieDetails.Name = "TabPageAllergieDetails";
            TabPageAllergieDetails.Padding = new Padding(3);
            TabPageAllergieDetails.Size = new Size(781, 364);
            TabPageAllergieDetails.TabIndex = 0;
            TabPageAllergieDetails.Text = "Question Details";
            TabPageAllergieDetails.UseVisualStyleBackColor = true;
            // 
            // ComboBoxAllergieCategory
            // 
            ComboBoxAllergieCategory.AutoCompleteMode = AutoCompleteMode.Suggest;
            ComboBoxAllergieCategory.AutoCompleteSource = AutoCompleteSource.ListItems;
            ComboBoxAllergieCategory.FormattingEnabled = true;
            ComboBoxAllergieCategory.Location = new Point(18, 120);
            ComboBoxAllergieCategory.Name = "ComboBoxAllergieCategory";
            ComboBoxAllergieCategory.Size = new Size(445, 23);
            ComboBoxAllergieCategory.TabIndex = 7;
            ComboBoxAllergieCategory.TxtVisible = true;
            // 
            // TextBoxAllergieDisplayas
            // 
            TextBoxAllergieDisplayas.BackColor = Color.White;
            TextBoxAllergieDisplayas.Location = new Point(18, 75);
            TextBoxAllergieDisplayas.MaxLength = 50;
            TextBoxAllergieDisplayas.Name = "TextBoxAllergieDisplayas";
            TextBoxAllergieDisplayas.Size = new Size(562, 23);
            TextBoxAllergieDisplayas.TabIndex = 6;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label4.Location = new Point(17, 107);
            label4.Name = "label4";
            label4.Size = new Size(76, 13);
            label4.TabIndex = 56;
            label4.Text = "Group Name";
            // 
            // TextBoxAllergieName
            // 
            TextBoxAllergieName.BackColor = Color.White;
            TextBoxAllergieName.Location = new Point(18, 32);
            TextBoxAllergieName.MaxLength = 50;
            TextBoxAllergieName.Name = "TextBoxAllergieName";
            TextBoxAllergieName.Size = new Size(562, 23);
            TextBoxAllergieName.TabIndex = 5;
            TextBoxAllergieName.KeyPress += TextBoxAllergieName_KeyPress;
            TextBoxAllergieName.PreviewKeyDown += TextBoxAllergieName_PreviewKeyDown;
            // 
            // TextBoxAllergieReasonInactive
            // 
            TextBoxAllergieReasonInactive.BackColor = Color.White;
            TextBoxAllergieReasonInactive.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TextBoxAllergieReasonInactive.Location = new Point(17, 195);
            TextBoxAllergieReasonInactive.MaxLength = 512;
            TextBoxAllergieReasonInactive.Multiline = true;
            TextBoxAllergieReasonInactive.Name = "TextBoxAllergieReasonInactive";
            TextBoxAllergieReasonInactive.Size = new Size(716, 145);
            TextBoxAllergieReasonInactive.TabIndex = 10;
            // 
            // CheckBoxAllergieIsActive
            // 
            CheckBoxAllergieIsActive.AutoSize = true;
            CheckBoxAllergieIsActive.Location = new Point(21, 155);
            CheckBoxAllergieIsActive.Name = "CheckBoxAllergieIsActive";
            CheckBoxAllergieIsActive.Size = new Size(126, 19);
            CheckBoxAllergieIsActive.TabIndex = 9;
            CheckBoxAllergieIsActive.Text = "Is Additional Note?";
            CheckBoxAllergieIsActive.UseVisualStyleBackColor = true;
            CheckBoxAllergieIsActive.CheckedChanged += CheckBoxAllergieIsActive_CheckedChanged;
            CheckBoxAllergieIsActive.PreviewKeyDown += CheckBoxAllergieIsActive_PreviewKeyDown;
            // 
            // labelReasonforInactive
            // 
            labelReasonforInactive.AutoSize = true;
            labelReasonforInactive.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            labelReasonforInactive.Location = new Point(18, 179);
            labelReasonforInactive.Name = "labelReasonforInactive";
            labelReasonforInactive.Size = new Size(80, 13);
            labelReasonforInactive.TabIndex = 19;
            labelReasonforInactive.Text = "Additional Note";
            // 
            // LabelCostCenterDisplayAs
            // 
            LabelCostCenterDisplayAs.AutoSize = true;
            LabelCostCenterDisplayAs.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            LabelCostCenterDisplayAs.Location = new Point(18, 61);
            LabelCostCenterDisplayAs.Name = "LabelCostCenterDisplayAs";
            LabelCostCenterDisplayAs.Size = new Size(56, 13);
            LabelCostCenterDisplayAs.TabIndex = 14;
            LabelCostCenterDisplayAs.Text = "Display As";
            // 
            // LabelCostCenterName
            // 
            LabelCostCenterName.AutoSize = true;
            LabelCostCenterName.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            LabelCostCenterName.Location = new Point(18, 19);
            LabelCostCenterName.Name = "LabelCostCenterName";
            LabelCostCenterName.Size = new Size(39, 13);
            LabelCostCenterName.TabIndex = 11;
            LabelCostCenterName.Text = "Name";
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { AllergieErrorMsg });
            statusStrip1.Location = new Point(0, 455);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(804, 22);
            statusStrip1.TabIndex = 53;
            statusStrip1.Text = "statusStrip1";
            // 
            // AllergieErrorMsg
            // 
            AllergieErrorMsg.Name = "AllergieErrorMsg";
            AllergieErrorMsg.Size = new Size(49, 17);
            AllergieErrorMsg.Text = "              ";
            // 
            // contextMenuStripAllergie
            // 
            contextMenuStripAllergie.Items.AddRange(new ToolStripItem[] { newAllergieToolStripMenuItem1 });
            contextMenuStripAllergie.Name = "contextMenuStripSymptoms";
            contextMenuStripAllergie.Size = new Size(142, 26);
            // 
            // newAllergieToolStripMenuItem1
            // 
            newAllergieToolStripMenuItem1.Name = "newAllergieToolStripMenuItem1";
            newAllergieToolStripMenuItem1.Size = new Size(141, 22);
            newAllergieToolStripMenuItem1.Text = "New Allergie";
            newAllergieToolStripMenuItem1.Click += newCategoryToolStripMenuItem1_Click;
            // 
            // FormAllergie
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(804, 477);
            Controls.Add(statusStrip1);
            Controls.Add(TextBoxAllergieCategoryId);
            Controls.Add(BtnAllergieExit);
            Controls.Add(BtnAllergieSave);
            Controls.Add(BtnAllergieCancel);
            Controls.Add(TextBoxAllergieId);
            Controls.Add(TabControlAllergie);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormAllergie";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Add Medical Question";
            FormClosing += FormAllergie_FormClosing;
            Load += FormAllergie_Load;
            Controls.SetChildIndex(TabControlAllergie, 0);
            Controls.SetChildIndex(TextBoxAllergieId, 0);
            Controls.SetChildIndex(BtnAllergieCancel, 0);
            Controls.SetChildIndex(BtnAllergieSave, 0);
            Controls.SetChildIndex(BtnAllergieExit, 0);
            Controls.SetChildIndex(TextBoxAllergieCategoryId, 0);
            Controls.SetChildIndex(statusStrip1, 0);
            Controls.SetChildIndex(ProductIdTransport, 0);
            Controls.SetChildIndex(ProductBatchIdTransport, 0);
            Controls.SetChildIndex(AccountIdTransport, 0);
            contextMenuStripAllergieCategory.ResumeLayout(false);
            TabControlAllergie.ResumeLayout(false);
            TabPageAllergieDetails.ResumeLayout(false);
            TabPageAllergieDetails.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            contextMenuStripAllergie.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button BtnAllergieDelete;
        private TextBox TextBoxAllergieCategoryId;
        private Dropdown_Button.UserControlButtonWithMenu BtnAllergieNew;
        private fa.views.controls.text.DelayedTextChangeTextBox TextBoxAllergieSearch;
        private Button BtnImport;
        private Button BtnExport;
        private Button BtnAllergieExit;
        private Button BtnAllergieSave;
        private Button BtnAllergieCancel;
        private TextBox TextBoxAllergieId;
        private Button BtnAllergieEdit;
        private TreeView TreeViewAllergie;
        private TabControl TabControlAllergieCategory;
        private TabPage TabPageAllergieCategory;
        private fa.views.controls.ComboBoxSwapTextBox ComboBoxParentAllergieCategory;
        private TextBox TextBoxAllergieCategoryDescription;
        private Label LabelSymptomCategoryDescription;
        private TextBox TextBoxAllergieCategoryDisplayAs;
        private TextBox TextBoxAllergieCategoryName;
        private Label label14;
        private Label label15;
        private Label LabelParentAllergieCategory;
        private TabControl TabControlAllergie;
        private TabPage TabPageAllergieDetails;
        private fa.views.controls.ComboBoxSwapTextBox ComboBoxAllergieCategory;
        private fa.views.controls.text.NameTextBoxAllowSpace TextBoxAllergieDisplayas;
        private Label label4;
        private fa.views.controls.text.NameTextBoxAllowSpace TextBoxAllergieName;
        private TextBox TextBoxAllergieReasonInactive;
        private CheckBox CheckBoxAllergieIsActive;
        private Label labelReasonforInactive;
        private fa.views.controls.DataViewVerticalScroll GridViewKeywords;
        private DataGridViewTextBoxColumn SNO;
        private fa.views.controls.grid.DataGridViewNameColumn Keywords;
        private DataGridViewButtonColumn DeleteKWords;
        private DataGridViewTextBoxColumn id;
        private Label LabelCostCenterDisplayAs;
        private TextBox TextBoxAllergieDescription;
        private Label LabelCostCenterDescription;
        private Label LabelCostCenterName;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel AllergieErrorMsg;
        private ImageList ImageListAllergie;
        private ContextMenuStrip contextMenuStripAllergie;
        private ToolStripMenuItem newAllergieToolStripMenuItem1;
        private ContextMenuStrip contextMenuStripAllergieCategory;
        private ToolStripMenuItem newCategoryToolStripMenuItem;
        private ToolStripMenuItem newAllergieToolStripMenuItem;
    }
}