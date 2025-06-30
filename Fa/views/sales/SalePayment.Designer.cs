namespace fa.views.sales
{
    partial class FormPOSReceivePayment
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
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPOSReceivePayment));
            statusStrip1 = new StatusStrip();
            ErrorMsg = new ToolStripStatusLabel();
            label1 = new Label();
            TabControlInvoiceDetails = new TabControl();
            TabDetails = new TabPage();
            TextBoxRefNo = new TextBox();
            DateTimePickerInvoiceDate = new controls.text.DateWithCalendar();
            label18 = new Label();
            TextBoxAddress = new TextBox();
            TextBoxName = new TextBox();
            label17 = new Label();
            label16 = new Label();
            label5 = new Label();
            BtnReceive = new Button();
            BtnCancel = new Button();
            RbtCash = new RadioButton();
            RbtCheque = new RadioButton();
            RbtBank = new RadioButton();
            RbtCard = new RadioButton();
            GroupBoxCreditCard = new GroupBox();
            ComboBoxCreditCardAccount = new controls.ComboBoxSwapTextBox();
            TextBoxCreditAmountReceived = new controls.text.CurrencyTextBox();
            label24 = new Label();
            TextBoxCreditCardBalance = new controls.text.CurrencyTextBox();
            label22 = new Label();
            TextBoxCreditCardAmount = new controls.text.CurrencyTextBox();
            label21 = new Label();
            TextBoxCreditTransaction = new TextBox();
            DateTimePickerCreditDate = new controls.text.DateWithCalendar();
            label8 = new Label();
            label10 = new Label();
            label9 = new Label();
            GroupBoxCashPayment = new GroupBox();
            TextBoxCashAmountReceived = new controls.text.CurrencyTextBox();
            label23 = new Label();
            TextBoxCashBalance = new controls.text.CurrencyTextBox();
            label4 = new Label();
            TextBoxCashAmount = new controls.text.CurrencyTextBox();
            label3 = new Label();
            GroupBoxCheckInfomation = new GroupBox();
            ComboBoxCheckAccount = new controls.ComboBoxSwapTextBox();
            TextBoxCheckDocument = new TextBox();
            DateTimePickerCheckDate = new controls.text.DateWithCalendar();
            label15 = new Label();
            label7 = new Label();
            label6 = new Label();
            GroupBoxBankTransfer = new GroupBox();
            ComboBoxBankAccount = new controls.ComboBoxSwapTextBox();
            TextBoxBankTransaction = new TextBox();
            label14 = new Label();
            label13 = new Label();
            BtnReceiveDeliver = new Button();
            TextBoxPaymentId = new TextBox();
            GroupBoxPayMethod = new GroupBox();
            RbtUpi = new RadioButton();
            GridViewPendingInvoice = new controls.DataViewVerticalScroll();
            InvoiceDate = new DataGridViewTextBoxColumn();
            InvoiceNumber = new DataGridViewTextBoxColumn();
            InvAmount = new controls.grid.DataGridViewCurrencyColumn();
            Column1 = new DataGridViewTextBoxColumn();
            ab2ToolStrip1 = new controls.Ab2ToolStrip();
            toolStripLabel1 = new ToolStripLabel();
            TextBoxReceivePaymentSearch = new ToolStripTextBox();
            BtnSearchReceivePayment = new ToolStripButton();
            TextBoxSalesNetAmount = new TextBox();
            TextBoxSaleId = new TextBox();
            GroupBoxUpiPayment = new GroupBox();
            ComboBoxUpiAccount = new controls.ComboBoxSwapTextBox();
            TextBoxUpiAmountReceived = new controls.text.CurrencyTextBox();
            label25 = new Label();
            UpiDateTime = new controls.text.DateWithCalendar();
            label20 = new Label();
            label19 = new Label();
            label12 = new Label();
            TextBoxUpiNumber = new TextBox();
            TextBoxUpiBalance = new controls.text.CurrencyTextBox();
            label2 = new Label();
            TextBoxUpiAmount = new controls.text.CurrencyTextBox();
            label11 = new Label();
            label26 = new Label();
            LableTotalAmountReceived = new Label();
            statusStrip1.SuspendLayout();
            TabControlInvoiceDetails.SuspendLayout();
            TabDetails.SuspendLayout();
            GroupBoxCreditCard.SuspendLayout();
            GroupBoxCashPayment.SuspendLayout();
            GroupBoxCheckInfomation.SuspendLayout();
            GroupBoxBankTransfer.SuspendLayout();
            GroupBoxPayMethod.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewPendingInvoice).BeginInit();
            ab2ToolStrip1.SuspendLayout();
            GroupBoxUpiPayment.SuspendLayout();
            SuspendLayout();
            // 
            // ProductIdTransport
            // 
            ProductIdTransport.Size = new Size(100, 21);
            // 
            // ProductBatchIdTransport
            // 
            ProductBatchIdTransport.Size = new Size(100, 21);
            // 
            // AccountIdTransport
            // 
            AccountIdTransport.Size = new Size(100, 21);
            // 
            // checkBoxIsPatient
            // 
            checkBoxIsPatient.Size = new Size(69, 17);
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { ErrorMsg });
            statusStrip1.Location = new Point(0, 513);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(767, 22);
            statusStrip1.TabIndex = 0;
            statusStrip1.Text = "statusStrip1";
            // 
            // ErrorMsg
            // 
            ErrorMsg.Name = "ErrorMsg";
            ErrorMsg.Size = new Size(31, 17);
            ErrorMsg.Text = "        ";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(13, 39);
            label1.Name = "label1";
            label1.Size = new Size(88, 13);
            label1.TabIndex = 3;
            label1.Text = "Pending Invoices";
            // 
            // TabControlInvoiceDetails
            // 
            TabControlInvoiceDetails.Controls.Add(TabDetails);
            TabControlInvoiceDetails.Location = new Point(340, 39);
            TabControlInvoiceDetails.Name = "TabControlInvoiceDetails";
            TabControlInvoiceDetails.SelectedIndex = 0;
            TabControlInvoiceDetails.Size = new Size(420, 199);
            TabControlInvoiceDetails.TabIndex = 0;
            TabControlInvoiceDetails.TabStop = false;
            // 
            // TabDetails
            // 
            TabDetails.Controls.Add(TextBoxRefNo);
            TabDetails.Controls.Add(DateTimePickerInvoiceDate);
            TabDetails.Controls.Add(label18);
            TabDetails.Controls.Add(TextBoxAddress);
            TabDetails.Controls.Add(TextBoxName);
            TabDetails.Controls.Add(label17);
            TabDetails.Controls.Add(label16);
            TabDetails.Controls.Add(label5);
            TabDetails.Location = new Point(4, 22);
            TabDetails.Name = "TabDetails";
            TabDetails.Padding = new Padding(3);
            TabDetails.Size = new Size(412, 173);
            TabDetails.TabIndex = 0;
            TabDetails.Text = "Invoice Details";
            TabDetails.UseVisualStyleBackColor = true;
            // 
            // TextBoxRefNo
            // 
            TextBoxRefNo.BackColor = Color.White;
            TextBoxRefNo.Location = new Point(265, 24);
            TextBoxRefNo.Name = "TextBoxRefNo";
            TextBoxRefNo.ReadOnly = true;
            TextBoxRefNo.Size = new Size(132, 21);
            TextBoxRefNo.TabIndex = 11;
            TextBoxRefNo.TabStop = false;
            // 
            // DateTimePickerInvoiceDate
            // 
            DateTimePickerInvoiceDate.BackColor = Color.White;
            DateTimePickerInvoiceDate.BorderStyle = BorderStyle.FixedSingle;
            DateTimePickerInvoiceDate.Date = null;
            DateTimePickerInvoiceDate.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            DateTimePickerInvoiceDate.Format = "MM/dd/yyyy";
            DateTimePickerInvoiceDate.Location = new Point(265, 65);
            DateTimePickerInvoiceDate.MaxDate = new DateTime(9997, 12, 31, 8, 50, 3, 0);
            DateTimePickerInvoiceDate.MinDate = new DateTime(1900, 1, 1, 22, 13, 47, 0);
            DateTimePickerInvoiceDate.Name = "DateTimePickerInvoiceDate";
            DateTimePickerInvoiceDate.ReadOnly = true;
            DateTimePickerInvoiceDate.Size = new Size(93, 21);
            DateTimePickerInvoiceDate.TabIndex = 10;
            DateTimePickerInvoiceDate.TabStop = false;
            // 
            // label18
            // 
            label18.AutoSize = true;
            label18.Location = new Point(262, 7);
            label18.Name = "label18";
            label18.Size = new Size(53, 13);
            label18.TabIndex = 6;
            label18.Text = "Invoice #";
            // 
            // TextBoxAddress
            // 
            TextBoxAddress.BackColor = Color.White;
            TextBoxAddress.Location = new Point(13, 66);
            TextBoxAddress.Multiline = true;
            TextBoxAddress.Name = "TextBoxAddress";
            TextBoxAddress.ReadOnly = true;
            TextBoxAddress.Size = new Size(241, 88);
            TextBoxAddress.TabIndex = 4;
            TextBoxAddress.TabStop = false;
            // 
            // TextBoxName
            // 
            TextBoxName.BackColor = Color.White;
            TextBoxName.Location = new Point(13, 24);
            TextBoxName.Name = "TextBoxName";
            TextBoxName.ReadOnly = true;
            TextBoxName.Size = new Size(241, 21);
            TextBoxName.TabIndex = 7;
            TextBoxName.TabStop = false;
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.Location = new Point(262, 49);
            label17.Name = "label17";
            label17.Size = new Size(30, 13);
            label17.TabIndex = 2;
            label17.Text = "Date";
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Location = new Point(10, 49);
            label16.Name = "label16";
            label16.Size = new Size(46, 13);
            label16.TabIndex = 1;
            label16.Text = "Address";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(10, 7);
            label5.Name = "label5";
            label5.Size = new Size(34, 13);
            label5.TabIndex = 0;
            label5.Text = "Name";
            // 
            // BtnReceive
            // 
            BtnReceive.Location = new Point(576, 485);
            BtnReceive.Name = "BtnReceive";
            BtnReceive.Size = new Size(86, 23);
            BtnReceive.TabIndex = 13;
            BtnReceive.Text = "Receive [F8]";
            BtnReceive.UseVisualStyleBackColor = true;
            BtnReceive.Click += BtnReceive_Click;
            // 
            // BtnCancel
            // 
            BtnCancel.Location = new Point(495, 485);
            BtnCancel.Name = "BtnCancel";
            BtnCancel.Size = new Size(75, 23);
            BtnCancel.TabIndex = 15;
            BtnCancel.Text = "Cancel [Esc]";
            BtnCancel.UseVisualStyleBackColor = true;
            BtnCancel.Click += BtnCancel_Click;
            // 
            // RbtCash
            // 
            RbtCash.AutoSize = true;
            RbtCash.Checked = true;
            RbtCash.Location = new Point(9, 20);
            RbtCash.Name = "RbtCash";
            RbtCash.Size = new Size(49, 17);
            RbtCash.TabIndex = 1;
            RbtCash.TabStop = true;
            RbtCash.Text = "Cash";
            RbtCash.UseVisualStyleBackColor = true;
            RbtCash.CheckedChanged += RbtCash_CheckedChanged;
            // 
            // RbtCheque
            // 
            RbtCheque.AutoSize = true;
            RbtCheque.Location = new Point(61, 20);
            RbtCheque.Name = "RbtCheque";
            RbtCheque.Size = new Size(62, 17);
            RbtCheque.TabIndex = 1;
            RbtCheque.Text = "Cheque";
            RbtCheque.UseVisualStyleBackColor = true;
            RbtCheque.CheckedChanged += RbtCash_CheckedChanged;
            // 
            // RbtBank
            // 
            RbtBank.AutoSize = true;
            RbtBank.Location = new Point(129, 20);
            RbtBank.Name = "RbtBank";
            RbtBank.Size = new Size(92, 17);
            RbtBank.TabIndex = 1;
            RbtBank.Text = "Bank Transfer";
            RbtBank.UseVisualStyleBackColor = true;
            RbtBank.CheckedChanged += RbtCash_CheckedChanged;
            // 
            // RbtCard
            // 
            RbtCard.AutoSize = true;
            RbtCard.Location = new Point(224, 20);
            RbtCard.Name = "RbtCard";
            RbtCard.Size = new Size(48, 17);
            RbtCard.TabIndex = 1;
            RbtCard.Text = "Card";
            RbtCard.UseVisualStyleBackColor = true;
            RbtCard.CheckedChanged += RbtCash_CheckedChanged;
            // 
            // GroupBoxCreditCard
            // 
            GroupBoxCreditCard.BackColor = SystemColors.Window;
            GroupBoxCreditCard.Controls.Add(ComboBoxCreditCardAccount);
            GroupBoxCreditCard.Controls.Add(TextBoxCreditAmountReceived);
            GroupBoxCreditCard.Controls.Add(label24);
            GroupBoxCreditCard.Controls.Add(TextBoxCreditCardBalance);
            GroupBoxCreditCard.Controls.Add(label22);
            GroupBoxCreditCard.Controls.Add(TextBoxCreditCardAmount);
            GroupBoxCreditCard.Controls.Add(label21);
            GroupBoxCreditCard.Controls.Add(TextBoxCreditTransaction);
            GroupBoxCreditCard.Controls.Add(DateTimePickerCreditDate);
            GroupBoxCreditCard.Controls.Add(label8);
            GroupBoxCreditCard.Controls.Add(label10);
            GroupBoxCreditCard.Controls.Add(label9);
            GroupBoxCreditCard.Location = new Point(821, 44);
            GroupBoxCreditCard.Name = "GroupBoxCreditCard";
            GroupBoxCreditCard.Size = new Size(409, 183);
            GroupBoxCreditCard.TabIndex = 8;
            GroupBoxCreditCard.TabStop = false;
            GroupBoxCreditCard.Text = "Credit/Debit Card Details";
            GroupBoxCreditCard.Visible = false;
            // 
            // ComboBoxCreditCardAccount
            // 
            ComboBoxCreditCardAccount.AutoCompleteMode = AutoCompleteMode.Suggest;
            ComboBoxCreditCardAccount.AutoCompleteSource = AutoCompleteSource.ListItems;
            ComboBoxCreditCardAccount.FormattingEnabled = true;
            ComboBoxCreditCardAccount.Location = new Point(28, 50);
            ComboBoxCreditCardAccount.Name = "ComboBoxCreditCardAccount";
            ComboBoxCreditCardAccount.Size = new Size(223, 21);
            ComboBoxCreditCardAccount.TabIndex = 8;
            ComboBoxCreditCardAccount.TxtVisible = true;
            // 
            // TextBoxCreditAmountReceived
            // 
            TextBoxCreditAmountReceived.BackColor = SystemColors.Window;
            TextBoxCreditAmountReceived.Decimals = 2;
            TextBoxCreditAmountReceived.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TextBoxCreditAmountReceived.Length = 10;
            TextBoxCreditAmountReceived.Location = new Point(267, 140);
            TextBoxCreditAmountReceived.Name = "TextBoxCreditAmountReceived";
            TextBoxCreditAmountReceived.ReadOnly = true;
            TextBoxCreditAmountReceived.Size = new Size(129, 21);
            TextBoxCreditAmountReceived.TabIndex = 40;
            TextBoxCreditAmountReceived.Text = "0.00";
            TextBoxCreditAmountReceived.TextAlign = HorizontalAlignment.Right;
            // 
            // label24
            // 
            label24.AutoSize = true;
            label24.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label24.Location = new Point(264, 123);
            label24.Name = "label24";
            label24.Size = new Size(139, 13);
            label24.TabIndex = 39;
            label24.Text = "Amount received so far";
            // 
            // TextBoxCreditCardBalance
            // 
            TextBoxCreditCardBalance.BackColor = SystemColors.Window;
            TextBoxCreditCardBalance.Decimals = 2;
            TextBoxCreditCardBalance.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TextBoxCreditCardBalance.Length = 10;
            TextBoxCreditCardBalance.Location = new Point(264, 92);
            TextBoxCreditCardBalance.Name = "TextBoxCreditCardBalance";
            TextBoxCreditCardBalance.ReadOnly = true;
            TextBoxCreditCardBalance.Size = new Size(129, 21);
            TextBoxCreditCardBalance.TabIndex = 38;
            TextBoxCreditCardBalance.TabStop = false;
            TextBoxCreditCardBalance.Text = "0.00";
            TextBoxCreditCardBalance.TextAlign = HorizontalAlignment.Right;
            // 
            // label22
            // 
            label22.AutoSize = true;
            label22.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label22.Location = new Point(261, 75);
            label22.Name = "label22";
            label22.Size = new Size(51, 13);
            label22.TabIndex = 37;
            label22.Text = "Balance";
            // 
            // TextBoxCreditCardAmount
            // 
            TextBoxCreditCardAmount.BackColor = SystemColors.Window;
            TextBoxCreditCardAmount.Decimals = 2;
            TextBoxCreditCardAmount.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TextBoxCreditCardAmount.Length = 10;
            TextBoxCreditCardAmount.Location = new Point(261, 49);
            TextBoxCreditCardAmount.Name = "TextBoxCreditCardAmount";
            TextBoxCreditCardAmount.ReadOnly = true;
            TextBoxCreditCardAmount.Size = new Size(129, 21);
            TextBoxCreditCardAmount.TabIndex = 36;
            TextBoxCreditCardAmount.Text = "0.00";
            TextBoxCreditCardAmount.TextAlign = HorizontalAlignment.Right;
            TextBoxCreditCardAmount.TextChanged += TextBoxCreditCardAmount_TextChanged;
            // 
            // label21
            // 
            label21.AutoSize = true;
            label21.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label21.Location = new Point(258, 32);
            label21.Name = "label21";
            label21.Size = new Size(104, 13);
            label21.TabIndex = 35;
            label21.Text = "Amount received";
            // 
            // TextBoxCreditTransaction
            // 
            TextBoxCreditTransaction.Location = new Point(28, 92);
            TextBoxCreditTransaction.MaxLength = 20;
            TextBoxCreditTransaction.Name = "TextBoxCreditTransaction";
            TextBoxCreditTransaction.Size = new Size(223, 21);
            TextBoxCreditTransaction.TabIndex = 9;
            // 
            // DateTimePickerCreditDate
            // 
            DateTimePickerCreditDate.BackColor = Color.White;
            DateTimePickerCreditDate.BorderStyle = BorderStyle.FixedSingle;
            DateTimePickerCreditDate.Date = null;
            DateTimePickerCreditDate.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            DateTimePickerCreditDate.Format = "MM/dd/yyyy";
            DateTimePickerCreditDate.Location = new Point(28, 134);
            DateTimePickerCreditDate.MaxDate = new DateTime(9997, 12, 31, 8, 50, 3, 0);
            DateTimePickerCreditDate.MinDate = new DateTime(1900, 1, 1, 23, 4, 53, 0);
            DateTimePickerCreditDate.Name = "DateTimePickerCreditDate";
            DateTimePickerCreditDate.ReadOnly = false;
            DateTimePickerCreditDate.Size = new Size(93, 21);
            DateTimePickerCreditDate.TabIndex = 10;
            DateTimePickerCreditDate.PreviewKeyDown += DateTimePickerCreditDate_PreviewKeyDown;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label8.Location = new Point(25, 75);
            label8.Name = "label8";
            label8.Size = new Size(86, 13);
            label8.TabIndex = 33;
            label8.Text = "Transaction #";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label10.Location = new Point(25, 117);
            label10.Name = "label10";
            label10.Size = new Size(34, 13);
            label10.TabIndex = 34;
            label10.Text = "Date";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label9.Location = new Point(25, 34);
            label9.Name = "label9";
            label9.Size = new Size(53, 13);
            label9.TabIndex = 32;
            label9.Text = "Account";
            // 
            // GroupBoxCashPayment
            // 
            GroupBoxCashPayment.BackColor = SystemColors.Window;
            GroupBoxCashPayment.Controls.Add(TextBoxCashAmountReceived);
            GroupBoxCashPayment.Controls.Add(label23);
            GroupBoxCashPayment.Controls.Add(TextBoxCashBalance);
            GroupBoxCashPayment.Controls.Add(label4);
            GroupBoxCashPayment.Controls.Add(TextBoxCashAmount);
            GroupBoxCashPayment.Controls.Add(label3);
            GroupBoxCashPayment.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            GroupBoxCashPayment.Location = new Point(340, 296);
            GroupBoxCashPayment.Name = "GroupBoxCashPayment";
            GroupBoxCashPayment.Size = new Size(416, 183);
            GroupBoxCashPayment.TabIndex = 2;
            GroupBoxCashPayment.TabStop = false;
            GroupBoxCashPayment.Text = "Cash";
            GroupBoxCashPayment.Enter += GroupBoxCashPayment_Enter;
            // 
            // TextBoxCashAmountReceived
            // 
            TextBoxCashAmountReceived.BackColor = SystemColors.Window;
            TextBoxCashAmountReceived.Decimals = 2;
            TextBoxCashAmountReceived.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TextBoxCashAmountReceived.Length = 10;
            TextBoxCashAmountReceived.Location = new Point(34, 145);
            TextBoxCashAmountReceived.Name = "TextBoxCashAmountReceived";
            TextBoxCashAmountReceived.ReadOnly = true;
            TextBoxCashAmountReceived.Size = new Size(129, 21);
            TextBoxCashAmountReceived.TabIndex = 24;
            TextBoxCashAmountReceived.Text = "0.00";
            TextBoxCashAmountReceived.TextAlign = HorizontalAlignment.Right;
            // 
            // label23
            // 
            label23.AutoSize = true;
            label23.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label23.Location = new Point(31, 128);
            label23.Name = "label23";
            label23.Size = new Size(139, 13);
            label23.TabIndex = 23;
            label23.Text = "Amount received so far";
            // 
            // TextBoxCashBalance
            // 
            TextBoxCashBalance.BackColor = SystemColors.Window;
            TextBoxCashBalance.Decimals = 2;
            TextBoxCashBalance.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TextBoxCashBalance.Length = 10;
            TextBoxCashBalance.Location = new Point(34, 99);
            TextBoxCashBalance.Name = "TextBoxCashBalance";
            TextBoxCashBalance.ReadOnly = true;
            TextBoxCashBalance.Size = new Size(129, 21);
            TextBoxCashBalance.TabIndex = 22;
            TextBoxCashBalance.TabStop = false;
            TextBoxCashBalance.Text = "0.00";
            TextBoxCashBalance.TextAlign = HorizontalAlignment.Right;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label4.Location = new Point(31, 82);
            label4.Name = "label4";
            label4.Size = new Size(51, 13);
            label4.TabIndex = 2;
            label4.Text = "Balance";
            // 
            // TextBoxCashAmount
            // 
            TextBoxCashAmount.BackColor = SystemColors.Window;
            TextBoxCashAmount.Decimals = 2;
            TextBoxCashAmount.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TextBoxCashAmount.Length = 10;
            TextBoxCashAmount.Location = new Point(34, 52);
            TextBoxCashAmount.Name = "TextBoxCashAmount";
            TextBoxCashAmount.ReadOnly = true;
            TextBoxCashAmount.Size = new Size(129, 21);
            TextBoxCashAmount.TabIndex = 2;
            TextBoxCashAmount.Text = "0.00";
            TextBoxCashAmount.TextAlign = HorizontalAlignment.Right;
            TextBoxCashAmount.TextChanged += TextBoxCashAmount_TextChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label3.Location = new Point(31, 35);
            label3.Name = "label3";
            label3.Size = new Size(104, 13);
            label3.TabIndex = 0;
            label3.Text = "Amount received";
            // 
            // GroupBoxCheckInfomation
            // 
            GroupBoxCheckInfomation.BackColor = SystemColors.Window;
            GroupBoxCheckInfomation.Controls.Add(ComboBoxCheckAccount);
            GroupBoxCheckInfomation.Controls.Add(TextBoxCheckDocument);
            GroupBoxCheckInfomation.Controls.Add(DateTimePickerCheckDate);
            GroupBoxCheckInfomation.Controls.Add(label15);
            GroupBoxCheckInfomation.Controls.Add(label7);
            GroupBoxCheckInfomation.Controls.Add(label6);
            GroupBoxCheckInfomation.Location = new Point(821, 233);
            GroupBoxCheckInfomation.Name = "GroupBoxCheckInfomation";
            GroupBoxCheckInfomation.Size = new Size(409, 183);
            GroupBoxCheckInfomation.TabIndex = 3;
            GroupBoxCheckInfomation.TabStop = false;
            GroupBoxCheckInfomation.Text = "Check/Draft/Cashier Check Details";
            GroupBoxCheckInfomation.Visible = false;
            // 
            // ComboBoxCheckAccount
            // 
            ComboBoxCheckAccount.AutoCompleteMode = AutoCompleteMode.Suggest;
            ComboBoxCheckAccount.AutoCompleteSource = AutoCompleteSource.ListItems;
            ComboBoxCheckAccount.FormattingEnabled = true;
            ComboBoxCheckAccount.Location = new Point(28, 129);
            ComboBoxCheckAccount.Name = "ComboBoxCheckAccount";
            ComboBoxCheckAccount.Size = new Size(223, 21);
            ComboBoxCheckAccount.TabIndex = 5;
            ComboBoxCheckAccount.TxtVisible = true;
            // 
            // TextBoxCheckDocument
            // 
            TextBoxCheckDocument.Location = new Point(28, 47);
            TextBoxCheckDocument.MaxLength = 20;
            TextBoxCheckDocument.Name = "TextBoxCheckDocument";
            TextBoxCheckDocument.Size = new Size(206, 21);
            TextBoxCheckDocument.TabIndex = 3;
            // 
            // DateTimePickerCheckDate
            // 
            DateTimePickerCheckDate.BackColor = Color.White;
            DateTimePickerCheckDate.BorderStyle = BorderStyle.FixedSingle;
            DateTimePickerCheckDate.Date = null;
            DateTimePickerCheckDate.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            DateTimePickerCheckDate.Format = "MM/dd/yyyy";
            DateTimePickerCheckDate.Location = new Point(28, 88);
            DateTimePickerCheckDate.MaxDate = new DateTime(9997, 12, 31, 8, 50, 3, 0);
            DateTimePickerCheckDate.MinDate = new DateTime(1900, 1, 1, 23, 33, 2, 0);
            DateTimePickerCheckDate.Name = "DateTimePickerCheckDate";
            DateTimePickerCheckDate.ReadOnly = false;
            DateTimePickerCheckDate.Size = new Size(93, 21);
            DateTimePickerCheckDate.TabIndex = 4;
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label15.Location = new Point(25, 71);
            label15.Name = "label15";
            label15.Size = new Size(34, 13);
            label15.TabIndex = 4;
            label15.Text = "Date";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label7.Location = new Point(28, 113);
            label7.Name = "label7";
            label7.Size = new Size(35, 13);
            label7.TabIndex = 2;
            label7.Text = "Bank";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label6.Location = new Point(25, 30);
            label6.Name = "label6";
            label6.Size = new Size(77, 13);
            label6.TabIndex = 1;
            label6.Text = "Document #";
            // 
            // GroupBoxBankTransfer
            // 
            GroupBoxBankTransfer.BackColor = SystemColors.Window;
            GroupBoxBankTransfer.Controls.Add(ComboBoxBankAccount);
            GroupBoxBankTransfer.Controls.Add(TextBoxBankTransaction);
            GroupBoxBankTransfer.Controls.Add(label14);
            GroupBoxBankTransfer.Controls.Add(label13);
            GroupBoxBankTransfer.Location = new Point(821, 422);
            GroupBoxBankTransfer.Name = "GroupBoxBankTransfer";
            GroupBoxBankTransfer.Size = new Size(409, 183);
            GroupBoxBankTransfer.TabIndex = 6;
            GroupBoxBankTransfer.TabStop = false;
            GroupBoxBankTransfer.Text = "Bank Transfer Details";
            GroupBoxBankTransfer.Visible = false;
            // 
            // ComboBoxBankAccount
            // 
            ComboBoxBankAccount.AutoCompleteMode = AutoCompleteMode.Suggest;
            ComboBoxBankAccount.AutoCompleteSource = AutoCompleteSource.ListItems;
            ComboBoxBankAccount.FormattingEnabled = true;
            ComboBoxBankAccount.Location = new Point(28, 52);
            ComboBoxBankAccount.Name = "ComboBoxBankAccount";
            ComboBoxBankAccount.Size = new Size(223, 21);
            ComboBoxBankAccount.TabIndex = 6;
            ComboBoxBankAccount.TxtVisible = true;
            // 
            // TextBoxBankTransaction
            // 
            TextBoxBankTransaction.Location = new Point(28, 96);
            TextBoxBankTransaction.MaxLength = 20;
            TextBoxBankTransaction.Name = "TextBoxBankTransaction";
            TextBoxBankTransaction.Size = new Size(223, 21);
            TextBoxBankTransaction.TabIndex = 7;
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label14.Location = new Point(25, 78);
            label14.Name = "label14";
            label14.Size = new Size(86, 13);
            label14.TabIndex = 3;
            label14.Text = "Transaction #";
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label13.Location = new Point(25, 35);
            label13.Name = "label13";
            label13.Size = new Size(53, 13);
            label13.TabIndex = 2;
            label13.Text = "Account";
            // 
            // BtnReceiveDeliver
            // 
            BtnReceiveDeliver.Location = new Point(668, 485);
            BtnReceiveDeliver.Name = "BtnReceiveDeliver";
            BtnReceiveDeliver.Size = new Size(86, 23);
            BtnReceiveDeliver.TabIndex = 14;
            BtnReceiveDeliver.Text = "Deliver [F9]";
            BtnReceiveDeliver.UseVisualStyleBackColor = true;
            BtnReceiveDeliver.Click += BtnReceiveDeliver_Click;
            // 
            // TextBoxPaymentId
            // 
            TextBoxPaymentId.Location = new Point(93, 482);
            TextBoxPaymentId.Name = "TextBoxPaymentId";
            TextBoxPaymentId.Size = new Size(100, 21);
            TextBoxPaymentId.TabIndex = 52;
            TextBoxPaymentId.TabStop = false;
            TextBoxPaymentId.Visible = false;
            // 
            // GroupBoxPayMethod
            // 
            GroupBoxPayMethod.BackColor = SystemColors.Control;
            GroupBoxPayMethod.Controls.Add(RbtUpi);
            GroupBoxPayMethod.Controls.Add(RbtCash);
            GroupBoxPayMethod.Controls.Add(RbtCheque);
            GroupBoxPayMethod.Controls.Add(RbtBank);
            GroupBoxPayMethod.Controls.Add(RbtCard);
            GroupBoxPayMethod.Location = new Point(340, 244);
            GroupBoxPayMethod.Name = "GroupBoxPayMethod";
            GroupBoxPayMethod.Size = new Size(416, 46);
            GroupBoxPayMethod.TabIndex = 1;
            GroupBoxPayMethod.TabStop = false;
            GroupBoxPayMethod.Text = "Pay Method";
            // 
            // RbtUpi
            // 
            RbtUpi.AutoSize = true;
            RbtUpi.Location = new Point(278, 20);
            RbtUpi.Name = "RbtUpi";
            RbtUpi.Size = new Size(42, 17);
            RbtUpi.TabIndex = 2;
            RbtUpi.Text = "UPI";
            RbtUpi.UseVisualStyleBackColor = true;
            // 
            // GridViewPendingInvoice
            // 
            GridViewPendingInvoice.AllowUserToAddRows = false;
            GridViewPendingInvoice.AllowUserToDeleteRows = false;
            GridViewPendingInvoice.AllowUserToResizeColumns = false;
            GridViewPendingInvoice.AllowUserToResizeRows = false;
            GridViewPendingInvoice.BackgroundColor = SystemColors.Control;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = SystemColors.Control;
            dataGridViewCellStyle3.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle3.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.Control;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.WindowText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            GridViewPendingInvoice.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            GridViewPendingInvoice.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            GridViewPendingInvoice.Columns.AddRange(new DataGridViewColumn[] { InvoiceDate, InvoiceNumber, InvAmount, Column1 });
            GridViewPendingInvoice.EnableHeadersVisualStyles = false;
            GridViewPendingInvoice.Location = new Point(13, 56);
            GridViewPendingInvoice.MultiSelect = false;
            GridViewPendingInvoice.Name = "GridViewPendingInvoice";
            GridViewPendingInvoice.ReadOnly = true;
            GridViewPendingInvoice.RowHeadersVisible = false;
            GridViewPendingInvoice.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            GridViewPendingInvoice.ShowCellToolTips = false;
            GridViewPendingInvoice.Size = new Size(321, 423);
            GridViewPendingInvoice.TabIndex = 0;
            GridViewPendingInvoice.TabStop = false;
            GridViewPendingInvoice.RowEnter += GridViewPendingInvoice_RowEnter;
            GridViewPendingInvoice.Enter += GridViewPendingInvoice_Enter;
            GridViewPendingInvoice.Leave += GridViewPendingInvoice_Leave;
            // 
            // InvoiceDate
            // 
            InvoiceDate.HeaderText = "Date";
            InvoiceDate.Name = "InvoiceDate";
            InvoiceDate.ReadOnly = true;
            InvoiceDate.Resizable = DataGridViewTriState.False;
            InvoiceDate.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // InvoiceNumber
            // 
            InvoiceNumber.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            InvoiceNumber.HeaderText = "Invoice #";
            InvoiceNumber.Name = "InvoiceNumber";
            InvoiceNumber.ReadOnly = true;
            InvoiceNumber.Resizable = DataGridViewTriState.False;
            InvoiceNumber.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // InvAmount
            // 
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleRight;
            InvAmount.DefaultCellStyle = dataGridViewCellStyle4;
            InvAmount.HeaderText = "Amount";
            InvAmount.Name = "InvAmount";
            InvAmount.ReadOnly = true;
            InvAmount.Resizable = DataGridViewTriState.False;
            // 
            // Column1
            // 
            Column1.HeaderText = "Id";
            Column1.Name = "Column1";
            Column1.ReadOnly = true;
            Column1.Resizable = DataGridViewTriState.False;
            Column1.SortMode = DataGridViewColumnSortMode.NotSortable;
            Column1.Visible = false;
            // 
            // ab2ToolStrip1
            // 
            ab2ToolStrip1.BackColor = SystemColors.ControlLight;
            ab2ToolStrip1.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point);
            ab2ToolStrip1.GripStyle = ToolStripGripStyle.Hidden;
            ab2ToolStrip1.Items.AddRange(new ToolStripItem[] { toolStripLabel1, TextBoxReceivePaymentSearch, BtnSearchReceivePayment });
            ab2ToolStrip1.Location = new Point(0, 0);
            ab2ToolStrip1.Name = "ab2ToolStrip1";
            ab2ToolStrip1.Padding = new Padding(5);
            ab2ToolStrip1.Size = new Size(767, 32);
            ab2ToolStrip1.TabIndex = 1;
            ab2ToolStrip1.Text = "ab2ToolStrip1";
            // 
            // toolStripLabel1
            // 
            toolStripLabel1.Name = "toolStripLabel1";
            toolStripLabel1.Size = new Size(44, 19);
            toolStripLabel1.Text = "Search";
            // 
            // TextBoxReceivePaymentSearch
            // 
            TextBoxReceivePaymentSearch.BorderStyle = BorderStyle.FixedSingle;
            TextBoxReceivePaymentSearch.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point);
            TextBoxReceivePaymentSearch.MaxLength = 30;
            TextBoxReceivePaymentSearch.Name = "TextBoxReceivePaymentSearch";
            TextBoxReceivePaymentSearch.Size = new Size(250, 22);
            TextBoxReceivePaymentSearch.KeyDown += TextBoxReceivePaymentSearch_KeyDown;
            // 
            // BtnSearchReceivePayment
            // 
            BtnSearchReceivePayment.DisplayStyle = ToolStripItemDisplayStyle.Text;
            BtnSearchReceivePayment.Image = (Image)resources.GetObject("BtnSearchReceivePayment.Image");
            BtnSearchReceivePayment.ImageTransparentColor = Color.Magenta;
            BtnSearchReceivePayment.Name = "BtnSearchReceivePayment";
            BtnSearchReceivePayment.Size = new Size(26, 19);
            BtnSearchReceivePayment.Text = "Go";
            BtnSearchReceivePayment.Click += BtnSearchReceivePayment_Click;
            // 
            // TextBoxSalesNetAmount
            // 
            TextBoxSalesNetAmount.BackColor = SystemColors.Window;
            TextBoxSalesNetAmount.BorderStyle = BorderStyle.FixedSingle;
            TextBoxSalesNetAmount.Font = new Font("Tahoma", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            TextBoxSalesNetAmount.Location = new Point(122, 488);
            TextBoxSalesNetAmount.Name = "TextBoxSalesNetAmount";
            TextBoxSalesNetAmount.ReadOnly = true;
            TextBoxSalesNetAmount.Size = new Size(100, 23);
            TextBoxSalesNetAmount.TabIndex = 53;
            TextBoxSalesNetAmount.TabStop = false;
            TextBoxSalesNetAmount.Visible = false;
            // 
            // TextBoxSaleId
            // 
            TextBoxSaleId.Location = new Point(16, 488);
            TextBoxSaleId.Name = "TextBoxSaleId";
            TextBoxSaleId.Size = new Size(100, 21);
            TextBoxSaleId.TabIndex = 54;
            TextBoxSaleId.TabStop = false;
            TextBoxSaleId.Visible = false;
            // 
            // GroupBoxUpiPayment
            // 
            GroupBoxUpiPayment.BackColor = SystemColors.Window;
            GroupBoxUpiPayment.Controls.Add(ComboBoxUpiAccount);
            GroupBoxUpiPayment.Controls.Add(TextBoxUpiAmountReceived);
            GroupBoxUpiPayment.Controls.Add(label25);
            GroupBoxUpiPayment.Controls.Add(UpiDateTime);
            GroupBoxUpiPayment.Controls.Add(label20);
            GroupBoxUpiPayment.Controls.Add(label19);
            GroupBoxUpiPayment.Controls.Add(label12);
            GroupBoxUpiPayment.Controls.Add(TextBoxUpiNumber);
            GroupBoxUpiPayment.Controls.Add(TextBoxUpiBalance);
            GroupBoxUpiPayment.Controls.Add(label2);
            GroupBoxUpiPayment.Controls.Add(TextBoxUpiAmount);
            GroupBoxUpiPayment.Controls.Add(label11);
            GroupBoxUpiPayment.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            GroupBoxUpiPayment.Location = new Point(821, 611);
            GroupBoxUpiPayment.Name = "GroupBoxUpiPayment";
            GroupBoxUpiPayment.Size = new Size(416, 183);
            GroupBoxUpiPayment.TabIndex = 55;
            GroupBoxUpiPayment.TabStop = false;
            GroupBoxUpiPayment.Text = "UPI";
            GroupBoxUpiPayment.Visible = false;
            // 
            // ComboBoxUpiAccount
            // 
            ComboBoxUpiAccount.AutoCompleteMode = AutoCompleteMode.Suggest;
            ComboBoxUpiAccount.AutoCompleteSource = AutoCompleteSource.ListItems;
            ComboBoxUpiAccount.FormattingEnabled = true;
            ComboBoxUpiAccount.Location = new Point(186, 52);
            ComboBoxUpiAccount.Name = "ComboBoxUpiAccount";
            ComboBoxUpiAccount.Size = new Size(223, 21);
            ComboBoxUpiAccount.TabIndex = 25;
            ComboBoxUpiAccount.TxtVisible = true;
            // 
            // TextBoxUpiAmountReceived
            // 
            TextBoxUpiAmountReceived.BackColor = SystemColors.Window;
            TextBoxUpiAmountReceived.Decimals = 2;
            TextBoxUpiAmountReceived.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TextBoxUpiAmountReceived.Length = 10;
            TextBoxUpiAmountReceived.Location = new Point(189, 147);
            TextBoxUpiAmountReceived.Name = "TextBoxUpiAmountReceived";
            TextBoxUpiAmountReceived.ReadOnly = true;
            TextBoxUpiAmountReceived.Size = new Size(129, 21);
            TextBoxUpiAmountReceived.TabIndex = 31;
            TextBoxUpiAmountReceived.Text = "0.00";
            TextBoxUpiAmountReceived.TextAlign = HorizontalAlignment.Right;
            // 
            // label25
            // 
            label25.AutoSize = true;
            label25.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label25.Location = new Point(186, 130);
            label25.Name = "label25";
            label25.Size = new Size(139, 13);
            label25.TabIndex = 30;
            label25.Text = "Amount received so far";
            // 
            // UpiDateTime
            // 
            UpiDateTime.BackColor = Color.White;
            UpiDateTime.BorderStyle = BorderStyle.FixedSingle;
            UpiDateTime.Date = null;
            UpiDateTime.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            UpiDateTime.Format = "MM/dd/yyyy";
            UpiDateTime.Location = new Point(193, 94);
            UpiDateTime.MaxDate = new DateTime(9997, 12, 31, 8, 50, 3, 0);
            UpiDateTime.MinDate = new DateTime(1900, 1, 1, 23, 33, 2, 0);
            UpiDateTime.Name = "UpiDateTime";
            UpiDateTime.ReadOnly = false;
            UpiDateTime.Size = new Size(93, 21);
            UpiDateTime.TabIndex = 28;
            // 
            // label20
            // 
            label20.AutoSize = true;
            label20.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label20.Location = new Point(190, 77);
            label20.Name = "label20";
            label20.Size = new Size(34, 13);
            label20.TabIndex = 29;
            label20.Text = "Date";
            // 
            // label19
            // 
            label19.AutoSize = true;
            label19.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label19.Location = new Point(186, 36);
            label19.Name = "label19";
            label19.Size = new Size(53, 13);
            label19.TabIndex = 27;
            label19.Text = "Account";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            label12.Location = new Point(34, 130);
            label12.Name = "label12";
            label12.Size = new Size(124, 13);
            label12.TabIndex = 23;
            label12.Text = "UPI ID or Phone Number";
            // 
            // TextBoxUpiNumber
            // 
            TextBoxUpiNumber.BackColor = Color.White;
            TextBoxUpiNumber.Location = new Point(34, 147);
            TextBoxUpiNumber.MaxLength = 20;
            TextBoxUpiNumber.Name = "TextBoxUpiNumber";
            TextBoxUpiNumber.Size = new Size(129, 21);
            TextBoxUpiNumber.TabIndex = 24;
            TextBoxUpiNumber.TabStop = false;
            // 
            // TextBoxUpiBalance
            // 
            TextBoxUpiBalance.BackColor = SystemColors.Window;
            TextBoxUpiBalance.Decimals = 2;
            TextBoxUpiBalance.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TextBoxUpiBalance.Length = 10;
            TextBoxUpiBalance.Location = new Point(34, 99);
            TextBoxUpiBalance.Name = "TextBoxUpiBalance";
            TextBoxUpiBalance.ReadOnly = true;
            TextBoxUpiBalance.Size = new Size(129, 21);
            TextBoxUpiBalance.TabIndex = 22;
            TextBoxUpiBalance.TabStop = false;
            TextBoxUpiBalance.Text = "0.00";
            TextBoxUpiBalance.TextAlign = HorizontalAlignment.Right;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label2.Location = new Point(31, 82);
            label2.Name = "label2";
            label2.Size = new Size(51, 13);
            label2.TabIndex = 2;
            label2.Text = "Balance";
            // 
            // TextBoxUpiAmount
            // 
            TextBoxUpiAmount.BackColor = SystemColors.Window;
            TextBoxUpiAmount.Decimals = 2;
            TextBoxUpiAmount.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            TextBoxUpiAmount.Length = 10;
            TextBoxUpiAmount.Location = new Point(34, 52);
            TextBoxUpiAmount.Name = "TextBoxUpiAmount";
            TextBoxUpiAmount.ReadOnly = true;
            TextBoxUpiAmount.Size = new Size(129, 21);
            TextBoxUpiAmount.TabIndex = 2;
            TextBoxUpiAmount.Text = "0.00";
            TextBoxUpiAmount.TextAlign = HorizontalAlignment.Right;
            TextBoxUpiAmount.TextChanged += TextBoxUpiAmount_TextChanged;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label11.Location = new Point(31, 35);
            label11.Name = "label11";
            label11.Size = new Size(104, 13);
            label11.TabIndex = 0;
            label11.Text = "Amount received";
            // 
            // label26
            // 
            label26.AutoSize = true;
            label26.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            label26.Location = new Point(273, 490);
            label26.Name = "label26";
            label26.Size = new Size(91, 13);
            label26.TabIndex = 56;
            label26.Text = "Total Received";
            // 
            // LableTotalAmountReceived
            // 
            LableTotalAmountReceived.AutoSize = true;
            LableTotalAmountReceived.Font = new Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point);
            LableTotalAmountReceived.Location = new Point(370, 490);
            LableTotalAmountReceived.Name = "LableTotalAmountReceived";
            LableTotalAmountReceived.Size = new Size(31, 13);
            LableTotalAmountReceived.TabIndex = 57;
            LableTotalAmountReceived.Text = "0.00";
            // 
            // FormPOSReceivePayment
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(767, 535);
            Controls.Add(LableTotalAmountReceived);
            Controls.Add(label26);
            Controls.Add(GroupBoxUpiPayment);
            Controls.Add(GridViewPendingInvoice);
            Controls.Add(label1);
            Controls.Add(TextBoxSaleId);
            Controls.Add(TextBoxSalesNetAmount);
            Controls.Add(GroupBoxPayMethod);
            Controls.Add(TextBoxPaymentId);
            Controls.Add(BtnReceiveDeliver);
            Controls.Add(GroupBoxBankTransfer);
            Controls.Add(GroupBoxCheckInfomation);
            Controls.Add(BtnCancel);
            Controls.Add(BtnReceive);
            Controls.Add(ab2ToolStrip1);
            Controls.Add(statusStrip1);
            Controls.Add(GroupBoxCreditCard);
            Controls.Add(TabControlInvoiceDetails);
            Controls.Add(GroupBoxCashPayment);
            Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormPOSReceivePayment";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Receive Payment [POS]";
            Load += FormPOSReceivePayment_Load;
            Controls.SetChildIndex(checkBoxIsPatient, 0);
            Controls.SetChildIndex(GroupBoxCashPayment, 0);
            Controls.SetChildIndex(TabControlInvoiceDetails, 0);
            Controls.SetChildIndex(GroupBoxCreditCard, 0);
            Controls.SetChildIndex(statusStrip1, 0);
            Controls.SetChildIndex(ab2ToolStrip1, 0);
            Controls.SetChildIndex(BtnReceive, 0);
            Controls.SetChildIndex(BtnCancel, 0);
            Controls.SetChildIndex(GroupBoxCheckInfomation, 0);
            Controls.SetChildIndex(GroupBoxBankTransfer, 0);
            Controls.SetChildIndex(BtnReceiveDeliver, 0);
            Controls.SetChildIndex(TextBoxPaymentId, 0);
            Controls.SetChildIndex(GroupBoxPayMethod, 0);
            Controls.SetChildIndex(TextBoxSalesNetAmount, 0);
            Controls.SetChildIndex(TextBoxSaleId, 0);
            Controls.SetChildIndex(label1, 0);
            Controls.SetChildIndex(GridViewPendingInvoice, 0);
            Controls.SetChildIndex(AccountIdTransport, 0);
            Controls.SetChildIndex(ProductIdTransport, 0);
            Controls.SetChildIndex(ProductBatchIdTransport, 0);
            Controls.SetChildIndex(GroupBoxUpiPayment, 0);
            Controls.SetChildIndex(label26, 0);
            Controls.SetChildIndex(LableTotalAmountReceived, 0);
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            TabControlInvoiceDetails.ResumeLayout(false);
            TabDetails.ResumeLayout(false);
            TabDetails.PerformLayout();
            GroupBoxCreditCard.ResumeLayout(false);
            GroupBoxCreditCard.PerformLayout();
            GroupBoxCashPayment.ResumeLayout(false);
            GroupBoxCashPayment.PerformLayout();
            GroupBoxCheckInfomation.ResumeLayout(false);
            GroupBoxCheckInfomation.PerformLayout();
            GroupBoxBankTransfer.ResumeLayout(false);
            GroupBoxBankTransfer.PerformLayout();
            GroupBoxPayMethod.ResumeLayout(false);
            GroupBoxPayMethod.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)GridViewPendingInvoice).EndInit();
            ab2ToolStrip1.ResumeLayout(false);
            ab2ToolStrip1.PerformLayout();
            GroupBoxUpiPayment.ResumeLayout(false);
            GroupBoxUpiPayment.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private controls.Ab2ToolStrip ab2ToolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripTextBox TextBoxReceivePaymentSearch;
        private System.Windows.Forms.ToolStripButton BtnSearchReceivePayment;
        private System.Windows.Forms.Label label1;
        private controls.DataViewVerticalScroll GridViewPendingInvoice;
        private System.Windows.Forms.TabControl TabControlInvoiceDetails;
        private System.Windows.Forms.TabPage TabDetails;
        private System.Windows.Forms.Button BtnReceive;
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.RadioButton RbtCash;
        private System.Windows.Forms.RadioButton RbtCheque;
        private System.Windows.Forms.RadioButton RbtBank;
        private System.Windows.Forms.RadioButton RbtCard;
        private System.Windows.Forms.GroupBox GroupBoxCreditCard;
        private System.Windows.Forms.GroupBox GroupBoxCashPayment;
        private System.Windows.Forms.Label label3;
        private controls.text.CurrencyTextBox TextBoxCashAmount;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox TextBoxName;
        private System.Windows.Forms.TextBox TextBoxAddress;
        private System.Windows.Forms.Label label18;
        private controls.text.DateWithCalendar DateTimePickerInvoiceDate;
        private System.Windows.Forms.TextBox TextBoxRefNo;
        private System.Windows.Forms.GroupBox GroupBoxCheckInfomation;
        private controls.text.DateWithCalendar DateTimePickerCheckDate;
        private controls.ComboBoxSwapTextBox ComboBoxCheckAccount;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox TextBoxCheckDocument;
        private System.Windows.Forms.GroupBox GroupBoxBankTransfer;
        private System.Windows.Forms.TextBox TextBoxBankTransaction;
        private controls.ComboBoxSwapTextBox ComboBoxBankAccount;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button BtnReceiveDeliver;
        private controls.text.CurrencyTextBox TextBoxCashBalance;
        private System.Windows.Forms.ToolStripStatusLabel ErrorMsg;
        private System.Windows.Forms.TextBox TextBoxPaymentId;
        private System.Windows.Forms.DataGridViewTextBoxColumn InvoiceDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn InvoiceNumber;
        private controls.grid.DataGridViewCurrencyColumn InvAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.GroupBox GroupBoxPayMethod;
        private System.Windows.Forms.TextBox TextBoxSalesNetAmount;
        private System.Windows.Forms.TextBox TextBoxSaleId;
        private controls.ComboBoxSwapTextBox ComboBoxCreditCardAccount;
        private System.Windows.Forms.TextBox TextBoxCreditTransaction;
        private controls.text.DateWithCalendar DateTimePickerCreditDate;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private RadioButton RbtUpi;
        private TextBox TextBoxUpiNumber;
        private TextBox TextBoxUpiNum;
        private GroupBox GroupBoxUpiPayment;
        private Label label12;
        private controls.text.CurrencyTextBox TextBoxUpiBalance;
        private Label label2;
        private controls.text.CurrencyTextBox TextBoxUpiAmount;
        private Label label11;
        private controls.text.DateWithCalendar UpiDateTime;
        private Label label20;
        private Label label19;
        private controls.ComboBoxSwapTextBox ComboBoxUpiAccount;
        private controls.text.CurrencyTextBox TextBoxCreditCardAmount;
        private Label label21;
        private controls.text.CurrencyTextBox TextBoxCreditCardBalance;
        private Label label22;
        private controls.text.CurrencyTextBox TextBoxCashAmountReceived;
        private Label label23;
        private controls.text.CurrencyTextBox TextBoxCreditAmountReceived;
        private Label label24;
        private controls.text.CurrencyTextBox TextBoxUpiAmountReceived;
        private Label label25;
        private Label label26;
        private Label LableTotalAmountReceived;
    }
}