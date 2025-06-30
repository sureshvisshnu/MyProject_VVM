namespace Fa.views.hms.helper
{
    partial class FormSelectLabTestElement
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSelectLabTestElement));
            BtnLabTestElementNew = new Button();
            TextBoxLabTestElementSelected = new fa.views.controls.text.NameTextBoxAllowSpace(components);
            BtnLabTestElementRemove = new Button();
            BtnLabTestElementSelect = new Button();
            BtnLabTestElementCancel = new Button();
            BtnLabTestElementDone = new Button();
            TreeViewSelectedLabTestElement = new TreeView();
            TextBoxLabTestElementSearch = new fa.views.controls.text.NameTextBoxAllowSpace(components);
            TreeViewLabTestElement = new TreeView();
            SuspendLayout();
            // 
            // BtnLabTestElementNew
            // 
            BtnLabTestElementNew.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnLabTestElementNew.Location = new Point(12, 408);
            BtnLabTestElementNew.Name = "BtnLabTestElementNew";
            BtnLabTestElementNew.Size = new Size(93, 23);
            BtnLabTestElementNew.TabIndex = 76;
            BtnLabTestElementNew.Text = "New [F3]";
            BtnLabTestElementNew.UseVisualStyleBackColor = true;
            // 
            // TextBoxLabTestElementSelected
            // 
            TextBoxLabTestElementSelected.Location = new Point(350, 25);
            TextBoxLabTestElementSelected.MaxLength = 50;
            TextBoxLabTestElementSelected.Name = "TextBoxLabTestElementSelected";
            TextBoxLabTestElementSelected.Size = new Size(239, 23);
            TextBoxLabTestElementSelected.TabIndex = 75;
            // 
            // BtnLabTestElementRemove
            // 
            BtnLabTestElementRemove.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnLabTestElementRemove.Location = new Point(257, 271);
            BtnLabTestElementRemove.Name = "BtnLabTestElementRemove";
            BtnLabTestElementRemove.Size = new Size(84, 23);
            BtnLabTestElementRemove.TabIndex = 74;
            BtnLabTestElementRemove.Text = "<-- Remove";
            BtnLabTestElementRemove.UseVisualStyleBackColor = true;
            // 
            // BtnLabTestElementSelect
            // 
            BtnLabTestElementSelect.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnLabTestElementSelect.Location = new Point(257, 144);
            BtnLabTestElementSelect.Name = "BtnLabTestElementSelect";
            BtnLabTestElementSelect.Size = new Size(84, 23);
            BtnLabTestElementSelect.TabIndex = 73;
            BtnLabTestElementSelect.Text = "Select -->";
            BtnLabTestElementSelect.UseVisualStyleBackColor = true;
            // 
            // BtnLabTestElementCancel
            // 
            BtnLabTestElementCancel.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnLabTestElementCancel.Location = new Point(397, 408);
            BtnLabTestElementCancel.Name = "BtnLabTestElementCancel";
            BtnLabTestElementCancel.Size = new Size(93, 23);
            BtnLabTestElementCancel.TabIndex = 72;
            BtnLabTestElementCancel.Text = "Reset [Esc]";
            BtnLabTestElementCancel.UseVisualStyleBackColor = true;
            // 
            // BtnLabTestElementDone
            // 
            BtnLabTestElementDone.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            BtnLabTestElementDone.Location = new Point(496, 408);
            BtnLabTestElementDone.Name = "BtnLabTestElementDone";
            BtnLabTestElementDone.Size = new Size(93, 23);
            BtnLabTestElementDone.TabIndex = 71;
            BtnLabTestElementDone.Text = "Done [F8]";
            BtnLabTestElementDone.UseVisualStyleBackColor = true;
            // 
            // TreeViewSelectedLabTestElement
            // 
            TreeViewSelectedLabTestElement.CheckBoxes = true;
            TreeViewSelectedLabTestElement.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TreeViewSelectedLabTestElement.HideSelection = false;
            TreeViewSelectedLabTestElement.Location = new Point(350, 52);
            TreeViewSelectedLabTestElement.Name = "TreeViewSelectedLabTestElement";
            TreeViewSelectedLabTestElement.Size = new Size(239, 341);
            TreeViewSelectedLabTestElement.TabIndex = 70;
            // 
            // TextBoxLabTestElementSearch
            // 
            TextBoxLabTestElementSearch.Location = new Point(12, 25);
            TextBoxLabTestElementSearch.MaxLength = 50;
            TextBoxLabTestElementSearch.Name = "TextBoxLabTestElementSearch";
            TextBoxLabTestElementSearch.Size = new Size(239, 23);
            TextBoxLabTestElementSearch.TabIndex = 68;
            // 
            // TreeViewLabTestElement
            // 
            TreeViewLabTestElement.CheckBoxes = true;
            TreeViewLabTestElement.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TreeViewLabTestElement.HideSelection = false;
            TreeViewLabTestElement.Location = new Point(12, 52);
            TreeViewLabTestElement.Name = "TreeViewLabTestElement";
            TreeViewLabTestElement.Size = new Size(239, 341);
            TreeViewLabTestElement.TabIndex = 69;
            // 
            // FormSelectLabTestElement
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(600, 456);
            Controls.Add(BtnLabTestElementNew);
            Controls.Add(TextBoxLabTestElementSelected);
            Controls.Add(BtnLabTestElementRemove);
            Controls.Add(BtnLabTestElementSelect);
            Controls.Add(BtnLabTestElementCancel);
            Controls.Add(BtnLabTestElementDone);
            Controls.Add(TreeViewSelectedLabTestElement);
            Controls.Add(TextBoxLabTestElementSearch);
            Controls.Add(TreeViewLabTestElement);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormSelectLabTestElement";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Select LabTest Element";
            Load += FormSelectLabTestElement_Load;
            Controls.SetChildIndex(PatientIdTransport, 0);
            Controls.SetChildIndex(ProductIdTransport, 0);
            Controls.SetChildIndex(ProductBatchIdTransport, 0);
            Controls.SetChildIndex(AccountIdTransport, 0);
            Controls.SetChildIndex(TreeViewLabTestElement, 0);
            Controls.SetChildIndex(TextBoxLabTestElementSearch, 0);
            Controls.SetChildIndex(TreeViewSelectedLabTestElement, 0);
            Controls.SetChildIndex(BtnLabTestElementDone, 0);
            Controls.SetChildIndex(BtnLabTestElementCancel, 0);
            Controls.SetChildIndex(BtnLabTestElementSelect, 0);
            Controls.SetChildIndex(BtnLabTestElementRemove, 0);
            Controls.SetChildIndex(TextBoxLabTestElementSelected, 0);
            Controls.SetChildIndex(BtnLabTestElementNew, 0);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button BtnLabTestElementNew;
        private fa.views.controls.text.NameTextBoxAllowSpace TextBoxLabTestElementSelected;
        private Button BtnLabTestElementRemove;
        private Button BtnLabTestElementSelect;
        private Button BtnLabTestElementCancel;
        private Button BtnLabTestElementDone;
        public TreeView TreeViewSelectedLabTestElement;
        private fa.views.controls.text.NameTextBoxAllowSpace TextBoxLabTestElementSearch;
        public TreeView TreeViewLabTestElement;
    }
}