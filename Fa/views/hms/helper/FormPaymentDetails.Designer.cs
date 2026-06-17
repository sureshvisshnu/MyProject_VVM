namespace Fa.views.hms.helper
{
    partial class FormPaymentDetails
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPaymentDetails));
            GridViewPayments = new fa.views.controls.DataViewVerticalScroll();
            sno = new DataGridViewTextBoxColumn();
            PayDate = new DataGridViewTextBoxColumn();
            InvPayNo = new DataGridViewTextBoxColumn();
            particular = new DataGridViewTextBoxColumn();
            amount = new DataGridViewTextBoxColumn();
            bal = new DataGridViewTextBoxColumn();
            Column1 = new DataGridViewTextBoxColumn();
            PaymentStatusStrip = new StatusStrip();
            ErrorMsgItemLedger = new ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)GridViewPayments).BeginInit();
            PaymentStatusStrip.SuspendLayout();
            SuspendLayout();
            // 
            // GridViewPayments
            // 
            GridViewPayments.AllowUserToAddRows = false;
            GridViewPayments.AllowUserToDeleteRows = false;
            GridViewPayments.AllowUserToResizeColumns = false;
            GridViewPayments.AllowUserToResizeRows = false;
            GridViewPayments.BackgroundColor = SystemColors.Control;
            GridViewPayments.BorderStyle = BorderStyle.Fixed3D;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            GridViewPayments.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            GridViewPayments.ColumnHeadersHeight = 20;
            GridViewPayments.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            GridViewPayments.Columns.AddRange(new DataGridViewColumn[] { sno, PayDate, InvPayNo, particular, amount, bal, Column1 });
            GridViewPayments.EditMode = DataGridViewEditMode.EditProgrammatically;
            GridViewPayments.EnableHeadersVisualStyles = false;
            GridViewPayments.Location = new Point(12, 12);
            GridViewPayments.MultiSelect = false;
            GridViewPayments.Name = "GridViewPayments";
            GridViewPayments.ReadOnly = true;
            GridViewPayments.RowHeadersVisible = false;
            GridViewPayments.RowTemplate.Height = 20;
            GridViewPayments.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            GridViewPayments.ShowCellToolTips = false;
            GridViewPayments.ShowEditingIcon = false;
            GridViewPayments.Size = new Size(460, 168);
            GridViewPayments.TabIndex = 39;
            // 
            // sno
            // 
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            sno.DefaultCellStyle = dataGridViewCellStyle2;
            sno.HeaderText = "Sl. No";
            sno.Name = "sno";
            sno.ReadOnly = true;
            sno.Resizable = DataGridViewTriState.False;
            sno.SortMode = DataGridViewColumnSortMode.NotSortable;
            sno.Width = 30;
            // 
            // PayDate
            // 
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            PayDate.DefaultCellStyle = dataGridViewCellStyle3;
            PayDate.HeaderText = "Date Inv/Pay";
            PayDate.Name = "PayDate";
            PayDate.ReadOnly = true;
            PayDate.Resizable = DataGridViewTriState.False;
            PayDate.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // InvPayNo
            // 
            InvPayNo.HeaderText = "Inv/Pay No";
            InvPayNo.Name = "InvPayNo";
            InvPayNo.ReadOnly = true;
            InvPayNo.Width = 70;
            // 
            // particular
            // 
            particular.HeaderText = "Particulars";
            particular.Name = "particular";
            particular.ReadOnly = true;
            particular.Width = 80;
            // 
            // amount
            // 
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.True;
            amount.DefaultCellStyle = dataGridViewCellStyle4;
            amount.HeaderText = "Amount";
            amount.Name = "amount";
            amount.ReadOnly = true;
            amount.SortMode = DataGridViewColumnSortMode.NotSortable;
            amount.Width = 80;
            // 
            // bal
            // 
            bal.HeaderText = "Balance";
            bal.Name = "bal";
            bal.ReadOnly = true;
            bal.Width = 80;
            // 
            // Column1
            // 
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle5.WrapMode = DataGridViewTriState.True;
            Column1.DefaultCellStyle = dataGridViewCellStyle5;
            Column1.HeaderText = "Id";
            Column1.Name = "Column1";
            Column1.ReadOnly = true;
            Column1.Resizable = DataGridViewTriState.False;
            Column1.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column1.Visible = false;
            // 
            // PaymentStatusStrip
            // 
            PaymentStatusStrip.ImageScalingSize = new Size(20, 20);
            PaymentStatusStrip.Items.AddRange(new ToolStripItem[] { ErrorMsgItemLedger });
            PaymentStatusStrip.Location = new Point(0, 189);
            PaymentStatusStrip.Name = "PaymentStatusStrip";
            PaymentStatusStrip.Padding = new Padding(1, 0, 10, 0);
            PaymentStatusStrip.Size = new Size(484, 22);
            PaymentStatusStrip.TabIndex = 40;
            PaymentStatusStrip.Text = "Status";
            // 
            // ErrorMsgItemLedger
            // 
            ErrorMsgItemLedger.Name = "ErrorMsgItemLedger";
            ErrorMsgItemLedger.Size = new Size(34, 17);
            ErrorMsgItemLedger.Text = "         ";
            // 
            // FormPaymentDetails
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(484, 211);
            Controls.Add(PaymentStatusStrip);
            Controls.Add(GridViewPayments);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormPaymentDetails";
            Text = "Payment Details";
            Load += FormPaymentDetails_Load;
            ((System.ComponentModel.ISupportInitialize)GridViewPayments).EndInit();
            PaymentStatusStrip.ResumeLayout(false);
            PaymentStatusStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private fa.views.controls.DataViewVerticalScroll GridViewPayments;
        private DataGridViewTextBoxColumn sno;
        private DataGridViewTextBoxColumn PayDate;
        private DataGridViewTextBoxColumn InvPayNo;
        private DataGridViewTextBoxColumn particular;
        private DataGridViewTextBoxColumn amount;
        private DataGridViewTextBoxColumn bal;
        private DataGridViewTextBoxColumn Column1;
        private StatusStrip PaymentStatusStrip;
        private ToolStripStatusLabel ErrorMsgItemLedger;
    }
}