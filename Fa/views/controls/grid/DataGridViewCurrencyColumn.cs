
using fa.api.utils;
using fa.libraries.Validation;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace fa.views.controls.grid
{
    //---------------------------------------------------------------------------------------------------------
    //Name column
    //---------------------------------------------------------------------------------------------------------
    public partial class DataGridViewNameColumn : DataGridViewColumn
    {
        public DataGridViewNameColumn() : base(new NameCell())
        {
        }
        private NameCell NameCellTemplate
        {
            get
            {
                return (NameCell)this.CellTemplate;
            }
        }
        [Category("Behavior"), DefaultValue(NameCell.DefaultNameLength), Description("The minimum text length 0 maximum text length is 999999999")]
        public int NameLength
        {
            get
            {
                if (this.NameCellTemplate == null)
                {
                    throw new InvalidOperationException("Operation cannot be completed because this DataGridViewColumn does not have a CellTemplate.");
                }
                return this.NameCellTemplate.NameLength;
            }
            set
            {
                if (this.NameCellTemplate == null)
                {
                    throw new InvalidOperationException("Operation cannot be completed because this DataGridViewColumn does not have a CellTemplate.");
                }
                this.NameCellTemplate.NameLength = value;
                if (this.DataGridView != null)
                {
                    DataGridViewRowCollection dataGridViewRows = this.DataGridView.Rows;
                    int rowCount = dataGridViewRows.Count;
                    for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
                    {
                        DataGridViewRow dataGridViewRow = dataGridViewRows.SharedRow(rowIndex);
                        NameCell? dataGridViewCell = dataGridViewRow.Cells[this.Index] as NameCell;
                        if (dataGridViewCell != null)
                        {
                            dataGridViewCell.SetNameLength(rowIndex, value);
                        }
                    }
                    this.DataGridView.InvalidateColumn(this.Index);
                }
            }
        }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {
                NameCell? NameCell = value as NameCell;
                if (value != null && NameCell == null)
                {
                    throw new InvalidCastException("Must be a Name Cell");
                }
                base.CellTemplate = value;
            }
        }
    }
    public class NameCell : DataGridViewTextBoxCell
    {
        public NameCell()
                : base()
        {
            this.Namelength = DefaultNameLength;
        }
        private static Type defaultEditType = typeof(NameEditingControl);

        internal const int DefaultNameLength = 50;

        private int Namelength;
        [DefaultValue(DefaultNameLength)]
        public int NameLength
        {
            get
            {
                return this.Namelength;
            }
            set
            {
                if (value < 0 && value> 999999999)
                {
                    throw new ArgumentOutOfRangeException("The Name length property cannot be smaller than 0.");
                }
                if (this.Namelength != value)
                {
                    SetNameLength(this.RowIndex, value);
                    OnCommonChange();
                }
            }
        }
        private void OnCommonChange()
        {
            if (this.DataGridView != null && !this.DataGridView.IsDisposed && !this.DataGridView.Disposing)
            {
                if (this.RowIndex == -1)
                {
                    this.DataGridView.InvalidateColumn(this.ColumnIndex);
                }
                else
                {
                    this.DataGridView.UpdateCellValue(this.ColumnIndex, this.RowIndex);
                }
            }
        }
        internal void SetNameLength(int rowIndex, int value)
        {
            Debug.Assert(value >= 0 && value<= 999999999);
            this.Namelength = value;
            if (OwnsEditingNumericUpDown(rowIndex))
            {
                this.EditingNumeric.NameLength = value;
            }
        }
        private NameEditingControl EditingNumeric
        {
            get
            {
                return this.DataGridView?.EditingControl as NameEditingControl ?? throw new InvalidOperationException("Editing control is null."); // return this.DataGridView.EditingControl as NameEditingControl;
            }
        }
        private bool OwnsEditingNumericUpDown(int rowIndex)
        {
            if (rowIndex == -1 || this.DataGridView == null)
            {
                return false;
            }
            CurrencyEditingControl? numericUpDownEditingControl = this.DataGridView.EditingControl as CurrencyEditingControl;
            return numericUpDownEditingControl != null && rowIndex == ((IDataGridViewEditingControl)numericUpDownEditingControl).EditingControlRowIndex;
        }
        private NameEditingControl NameEditingControl
        {
            get
            {
                return this.DataGridView?.EditingControl as NameEditingControl ?? throw new InvalidOperationException("Editing control is null.");  // return this.DataGridView.EditingControl as NameEditingControl;
            }
        }
        public override object Clone()
        {
            NameCell? dataGridViewCell = base.Clone() as NameCell;
            if (dataGridViewCell != null)
            {
                dataGridViewCell.NameLength = this.NameLength;
            }
            return dataGridViewCell!;
        }
        public override void InitializeEditingControl(int rowIndex, object
            initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            base.InitializeEditingControl(rowIndex, initialFormattedValue,
                dataGridViewCellStyle);
            NameEditingControl? ctl =
                DataGridView?.EditingControl as NameEditingControl;
            ctl!.NameLength = NameLength;
            try
            {
                if (this.Value == null)
                {
                    ctl.Text = string.Empty;
                }
                else
                {
                    ctl.Text = this.Value.ToString()!;
                }
            }
            catch
            {

            }

        }

        public override Type EditType
        {
            get
            {
                return defaultEditType;
            }
        }

        public override Type ValueType
        {
            get
            {
                Type valueType = base.ValueType;
                if (valueType != null)
                {
                    return valueType;
                }
                return typeof(System.String);
            }
        }
        public override object DefaultNewRowValue
        {
            get
            {
                return string.Empty;
            }
        }

    }

    class NameEditingControl : TextBox, IDataGridViewEditingControl
    {
        DataGridView dataGridView;
        private bool valueChanged = false;
        int rowIndex;
        private int _length;
        public int NameLength
        {
            get
            {
                return _length;
            }
            set
            {
                _length = value;
            }
        }
        public override string Text
        {
            get
            {
               
                return base.Text;
            }

            set
            {
                base.Text = value;
            }
        }

        public NameEditingControl()
        {
            this.MaxLength = NameLength;
            this.TabStop = false;
        }
        public virtual DataGridView EditingControlDataGridView
        {
            get
            {
                return this.dataGridView;
            }
            set
            {
                this.dataGridView = value;
            }
        }
        public object EditingControlFormattedValue
        {
            get
            {              
                return this.Text;
            }
            set
            {
                this.Text = (string)value;
            }
        }

        public object GetEditingControlFormattedValue(
            DataGridViewDataErrorContexts context)
        {

            return EditingControlFormattedValue;
        }

        public void ApplyCellStyleToEditingControl(
            DataGridViewCellStyle dataGridViewCellStyle)
        {
            this.Font = dataGridViewCellStyle.Font;
            this.ForeColor = dataGridViewCellStyle.ForeColor;
            this.BackColor = dataGridViewCellStyle.BackColor;
            this.TextAlign = HorizontalAlignment.Left;
        }

        public int EditingControlRowIndex
        {
            get
            {
                return rowIndex;
            }
            set
            {
                rowIndex = value;
            }
        }

        public bool EditingControlWantsInputKey(
            Keys key, bool dataGridViewWantsInputKey)
        {

            switch (key & Keys.KeyCode)
            {
                case Keys.Tab:
                case Keys.Up:
                case Keys.Left:
                case Keys.Right:
                case Keys.Down:
                case Keys.Control:
                case Keys.Z:
                case Keys.ControlKey:
                    return true;
                default:
                    return dataGridViewWantsInputKey;
            }
        }

        public void PrepareEditingControlForEdit(bool selectAll)
        {
        }

        public bool RepositionEditingControlOnValueChange
        {
            get
            {
                return false;
            }
        }

        public bool EditingControlValueChanged
        {
            get
            {
                return valueChanged;
            }
            set
            {
                valueChanged = value;
            }
        }

        protected override void OnTextChanged(EventArgs eventargs)
        {
            //base.Text = this.Text;
            valueChanged = true;
            this.EditingControlDataGridView.NotifyCurrentCellDirty(true);
            base.OnTextChanged(eventargs);
        }
        public Cursor EditingPanelCursor
        {
            get
            {
                return base.Cursor;
            }
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyValue != 8 && e.KeyValue != Convert.ToChar(Keys.Enter) && e.KeyValue != Convert.ToChar(Keys.Escape) && !char.IsControl((char)e.KeyValue))
            {
                if (this.Text.Length >= NameLength && this.SelectedText.Length == 0)
                {
                    e.SuppressKeyPress = true;
                }
            }
            else if ((e.KeyCode == Keys.V && e.Control) && Clipboard.ContainsText())
            {
                this.MaxLength = NameLength;
                KeypressValidation.Instance.Keypress_PasteCheckingProduct(this, e, "NameChecking");
            }
            base.OnKeyDown(e);
        }
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && e.KeyChar != Convert.ToChar(Keys.Enter) && e.KeyChar != Convert.ToChar(Keys.Escape) && !char.IsControl(e.KeyChar))
            {
                Regex Regex = new Regex("^[a-zA-Z0-9!@$%^&*()_+.,-/]*$");
                if ((Char.IsLetterOrDigit(e.KeyChar) || Regex.IsMatch(e.KeyChar.ToString()) || e.KeyChar == '\\' || char.IsWhiteSpace(e.KeyChar)))
                {
                    e.Handled = false;
                }
                else
                {
                    e.Handled = true;
                }
            }
            base.OnKeyPress(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.MaxLength = NameLength;
                KeypressValidation.Instance.AddContextMenu(this, "NameChecking");
            }
            base.OnMouseDown(e);
        }
    }
    //---------------------------------------------------------------------------------------------------------
    //Number colum
    //---------------------------------------------------------------------------------------------------------
    public partial class DataGridViewNumberColumn : DataGridViewColumn
    {
        public DataGridViewNumberColumn() : base(new NumberCell())
        {
        }
        private NumberCell NumberCellTemplate
        {
            get
            {
                return (NumberCell)this.CellTemplate;
            }
        }
        [Category("Behavior"), DefaultValue(CurrencyCell.defaultDecimalPlaces), Description("The minimum decimal point 1 maximum decimal place is 5")]
        public int NumberLength
        {
            get
            {
                if (this.NumberCellTemplate == null)
                {
                    throw new InvalidOperationException("Operation cannot be completed because this DataGridViewColumn does not have a CellTemplate.");
                }
                return this.NumberCellTemplate.NumberLength;
            }
            set
            {
                if (this.NumberCellTemplate == null)
                {
                    throw new InvalidOperationException("Operation cannot be completed because this DataGridViewColumn does not have a CellTemplate.");
                }
                this.NumberCellTemplate.NumberLength = value;
                if (this.DataGridView != null)
                {
                    DataGridViewRowCollection dataGridViewRows = this.DataGridView.Rows;
                    int rowCount = dataGridViewRows.Count;
                    for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
                    {
                        DataGridViewRow dataGridViewRow = dataGridViewRows.SharedRow(rowIndex);
                        NumberCell? dataGridViewCell = dataGridViewRow.Cells[this.Index] as NumberCell;
                        if (dataGridViewCell != null)
                        {
                            dataGridViewCell.SetNumberLength(rowIndex, value);
                        }
                    }
                    this.DataGridView.InvalidateColumn(this.Index);
                }
            }
        }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {
                NumberCell? NumberCell = value as NumberCell;
                if (value != null && NumberCell == null)
                {
                    throw new InvalidCastException("Must be a NumericCell");
                }
                base.CellTemplate = value;
            }
        }
    }
    public class NumberCell : DataGridViewTextBoxCell
    {
        public NumberCell()
                : base()
        {
            this.numberlength = DefaultNumberLength;
        }
        private static Type defaultEditType = typeof(NumberEditingControl);

        internal const int DefaultNumberLength = 1;

        private int numberlength;
        [DefaultValue(DefaultNumberLength)]
        public int NumberLength
        {
            get
            {
                return this.numberlength;
            }
            set
            {
                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException("The Number length property cannot be smaller than 1.");
                }
                if (this.numberlength != value)
                {
                    SetNumberLength(this.RowIndex, value);
                    OnCommonChange();
                }
            }
        }
        private void OnCommonChange()
        {
            if (this.DataGridView != null && !this.DataGridView.IsDisposed && !this.DataGridView.Disposing)
            {
                if (this.RowIndex == -1)
                {
                    this.DataGridView.InvalidateColumn(this.ColumnIndex);
                }
                else
                {
                    this.DataGridView.UpdateCellValue(this.ColumnIndex, this.RowIndex);
                }
            }
        }
        internal void SetNumberLength(int rowIndex, int value)
        {
            Debug.Assert(value >= 1);
            this.numberlength = value;
            if (OwnsEditingNumericUpDown(rowIndex))
            {
                this.EditingNumeric.NumberLength = value;
            }
        }
        private NumberEditingControl EditingNumeric
        {
            get
            {
                return this.DataGridView?.EditingControl as NumberEditingControl ?? throw new InvalidOperationException("Editing control is null."); // return this.DataGridView.EditingControl as NumberEditingControl;
            }
        }
        private bool OwnsEditingNumericUpDown(int rowIndex)
        {
            if (rowIndex == -1 || this.DataGridView == null)
            {
                return false;
            }
            CurrencyEditingControl numericUpDownEditingControl = this.DataGridView.EditingControl as CurrencyEditingControl;
            return numericUpDownEditingControl != null && rowIndex == ((IDataGridViewEditingControl)numericUpDownEditingControl).EditingControlRowIndex;
        }
        private NumberEditingControl NumberEditingControl
        {
            get
            {
                return this.DataGridView?.EditingControl as NumberEditingControl ?? throw new InvalidOperationException("Editing control is null."); // return this.DataGridView.EditingControl as NumberEditingControl;
            }
        }
        public override object Clone()
        {
            NumberCell? dataGridViewCell = base.Clone() as NumberCell;
            if (dataGridViewCell != null)
            {
                dataGridViewCell.NumberLength = this.NumberLength;
            }
            return dataGridViewCell!;
        }
        public override void InitializeEditingControl(int rowIndex, object
            initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            base.InitializeEditingControl(rowIndex, initialFormattedValue,
                dataGridViewCellStyle);
            NumberEditingControl? ctl =
                DataGridView?.EditingControl as NumberEditingControl;
            ctl!.NumberLength = NumberLength;
            if (this.Value == null||this.Value.ToString()==string.Empty)
            {
                this.Value = "0";
                ctl.Text ="0";
            }
            else
            {
                
                ctl.Text = this.Value.ToString()!;
            }
        }

        public override Type EditType
        {
            get
            {
                return defaultEditType;
            }
        }

        public override Type ValueType
        {
            get
            {
                Type valueType = base.ValueType;
                if (valueType != null)
                {
                    return valueType;
                }
                return typeof(System.String);
            }
        }    
        public override object DefaultNewRowValue
        {
            get
            {
                return "0";
            }
        }
        protected override object GetFormattedValue(object value,
                                                     int rowIndex,
                                                     ref DataGridViewCellStyle cellStyle,
                                                     TypeConverter valueTypeConverter,
                                                     TypeConverter formattedValueTypeConverter,
                                                     DataGridViewDataErrorContexts context)
        {
            object formattedValue = base.GetFormattedValue(value, rowIndex, ref cellStyle, valueTypeConverter, formattedValueTypeConverter, context);
            string? formattedNumber = formattedValue as string;
            
            return formattedValue;
        }
    }

    class NumberEditingControl : TextBox, IDataGridViewEditingControl
    {
        DataGridView dataGridView;
        private bool valueChanged = false;
        int rowIndex;
        private int _length;
        public int NumberLength
        {
            get
            {
                return _length;
            }
            set
            {
                _length = value;
            }
        }
        public override string Text
        {
            get
            {
                //if (string.IsNullOrEmpty(base.Text))
                //{
                //    base.Text= "0";
                //}
                return base.Text;
            }

            set
            {
                this.TextAlign = HorizontalAlignment.Right;
                base.Text = value;
            }
        }

        public NumberEditingControl()
        {
            this.MaxLength = NumberLength;
            this.TabStop = false;
        }
        public virtual DataGridView EditingControlDataGridView
        {
            get
            {
                return this.dataGridView;
            }
            set
            {
                this.dataGridView = value;
            }
        }
        public object EditingControlFormattedValue
        {
            get
            {
                if (string.IsNullOrEmpty(this.Text))
                {
                    return "0";
                }
                return this.Text;
            }
            set
            {
                this.Text = (string)value;
            }
        }

        public object GetEditingControlFormattedValue(
            DataGridViewDataErrorContexts context)
        {

            return EditingControlFormattedValue;
        }

        public void ApplyCellStyleToEditingControl(
            DataGridViewCellStyle dataGridViewCellStyle)
        {
            this.Font = dataGridViewCellStyle.Font;
            this.ForeColor = dataGridViewCellStyle.ForeColor;
            this.BackColor = dataGridViewCellStyle.BackColor;

        }

        public int EditingControlRowIndex
        {
            get
            {
                return rowIndex;
            }
            set
            {
                rowIndex = value;
            }
        }

        public bool EditingControlWantsInputKey(
            Keys key, bool dataGridViewWantsInputKey)
        {

            switch (key & Keys.KeyCode)
            {
                case Keys.Tab:
                case Keys.Up:
                case Keys.Down:
                case Keys.Control:
                case Keys.Z:
                case Keys.ControlKey:
                    return true;
                default:
                    return !dataGridViewWantsInputKey;
            }
        }

        public void PrepareEditingControlForEdit(bool selectAll)
        {
        }

        public bool RepositionEditingControlOnValueChange
        {
            get
            {
                return false;
            }
        }

        public bool EditingControlValueChanged
        {
            get
            {
                return valueChanged;
            }
            set
            {
                valueChanged = value;
            }
        }

        protected override void OnTextChanged(EventArgs eventargs)
        {
            //base.Text = this.Text;
            valueChanged = true;
            this.EditingControlDataGridView.NotifyCurrentCellDirty(true);
            base.OnTextChanged(eventargs);
        }
        public Cursor EditingPanelCursor
        {
            get
            {
                return base.Cursor;
            }
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.D0
          || e.KeyCode == Keys.D1
          || e.KeyCode == Keys.D2
          || e.KeyCode == Keys.D3
          || e.KeyCode == Keys.D4
          || e.KeyCode == Keys.D5
          || e.KeyCode == Keys.D6
          || e.KeyCode == Keys.D7
          || e.KeyCode == Keys.D8
          || e.KeyCode == Keys.D9
          || e.KeyCode == Keys.NumPad0
          || e.KeyCode == Keys.NumPad1
          || e.KeyCode == Keys.NumPad2
          || e.KeyCode == Keys.NumPad3
          || e.KeyCode == Keys.NumPad4
          || e.KeyCode == Keys.NumPad5
          || e.KeyCode == Keys.NumPad6
          || e.KeyCode == Keys.NumPad7
          || e.KeyCode == Keys.NumPad8
          || e.KeyCode == Keys.NumPad9)
            {
                if (this.Text.Length >= NumberLength && this.SelectedText.Length == 0)
                {
                    e.SuppressKeyPress = true;
                }
            }
            else if ((e.KeyCode == Keys.V && e.Control) && Clipboard.ContainsText())
            {
                this.MaxLength = NumberLength;
                KeypressValidation.Instance.Keypress_PasteChecking(this, e, "Number");
            }
            base.OnKeyDown(e);
        }
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && e.KeyChar != Convert.ToChar(Keys.Enter) && e.KeyChar != Convert.ToChar(Keys.Escape) && !char.IsControl(e.KeyChar))
            {
                if (!(Char.IsDigit(e.KeyChar) || (e.KeyChar == (char)Keys.Back)))
                {
                    e.Handled = true;
                }               
            }
            base.OnKeyPress(e);
        }
             
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.MaxLength = NumberLength;
                KeypressValidation.Instance.AddContextMenu(this, "Number");
            }
            base.OnMouseDown(e);
        }
    }

    //--------------------------------------------------------------------------------------------------------------------------------
    //Currency column
    //--------------------------------------------------------------------------------------------------------------------------------
    public partial class DataGridViewCurrencyColumn : DataGridViewColumn
    {
        public DataGridViewCurrencyColumn() : base(new CurrencyCell())
        {
        }

        private CurrencyCell CurrencyCellTemplate
        {
            get
            {
                return (CurrencyCell)this.CellTemplate;
            }
        }

        [Category("Behavior"), DefaultValue(CurrencyCell.defaultDecimalPlaces), Description("The minimum decimal point 2 maximum decimal place is 5")]
        public int DecimalPlaces
        {
            get
            {
                if (this.CurrencyCellTemplate == null)
                {
                    throw new InvalidOperationException("Operation cannot be completed because this DataGridViewColumn does not have a CellTemplate.");
                }
                return this.CurrencyCellTemplate.DecimalPlaces;
            }
            set
            {
                if (this.CurrencyCellTemplate == null)
                {
                    throw new InvalidOperationException("Operation cannot be completed because this DataGridViewColumn does not have a CellTemplate.");
                }
                this.CurrencyCellTemplate.DecimalPlaces = value;
                if (this.DataGridView != null)
                {
                    DataGridViewRowCollection dataGridViewRows = this.DataGridView.Rows;
                    int rowCount = dataGridViewRows.Count;
                    for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
                    {
                        DataGridViewRow dataGridViewRow = dataGridViewRows.SharedRow(rowIndex);
                        CurrencyCell? dataGridViewCell = dataGridViewRow.Cells[this.Index] as CurrencyCell;
                        if (dataGridViewCell != null)
                        {
                            dataGridViewCell.SetDecimalPlaces(rowIndex, value);
                        }
                    }
                    this.DataGridView.InvalidateColumn(this.Index);
                }
            }
        }
        [Category("Behavior"), DefaultValue(CurrencyCell.defaultCurrencyLength), Description("The maximum currency length is 15")]
        public int Currencylength
        {
            get
            {
                if (this.CurrencyCellTemplate == null)
                {
                    throw new InvalidOperationException("Operation cannot be completed because this DataGridViewColumn does not have a CellTemplate.");
                }
                return this.CurrencyCellTemplate.Currencylength;
            }
            set
            {
                if (this.CurrencyCellTemplate == null)
                {
                    throw new InvalidOperationException("Operation cannot be completed because this DataGridViewColumn does not have a CellTemplate.");
                }
                this.CurrencyCellTemplate.Currencylength = value;
                if (this.DataGridView != null)
                {
                    DataGridViewRowCollection dataGridViewRows = this.DataGridView.Rows;
                    int rowCount = dataGridViewRows.Count;
                    for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
                    {
                        DataGridViewRow dataGridViewRow = dataGridViewRows.SharedRow(rowIndex);
                        CurrencyCell? dataGridViewCell = dataGridViewRow.Cells[this.Index] as CurrencyCell;
                        if (dataGridViewCell != null)
                        {
                            dataGridViewCell.SetCurrencylength(rowIndex, value);
                        }
                    }
                    this.DataGridView.InvalidateColumn(this.Index);
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {
                CurrencyCell? currencyCell = value as CurrencyCell;
                if (value != null && currencyCell == null)
                {
                    throw new InvalidCastException("Must be a CurrencyCell");
                }
                base.CellTemplate = value;
            }
        }
    }

    public class CurrencyCell : DataGridViewTextBoxCell
    {
        public CurrencyCell()
                : base()
        {
            this.decimalPlaces = defaultDecimalPlaces;
            this.currencylength = defaultCurrencyLength;
        }
        private static Type defaultEditType = typeof(CurrencyEditingControl);


        internal const int defaultDecimalPlaces = 2;
        internal const int defaultCurrencyLength = 10;

        private int decimalPlaces;
        private int currencylength;
        [DefaultValue(defaultDecimalPlaces)]
        public int DecimalPlaces
        {
            get
            {
                return this.decimalPlaces;
            }
            set
            {
                if (value < 2 || value > 5)
                {
                    throw new ArgumentOutOfRangeException("The DecimalPlaces property cannot be smaller than 2 or larger than 5.");
                }
                if (this.decimalPlaces != value)
                {
                    SetDecimalPlaces(this.RowIndex, value);
                    OnCommonChange();
                }
            }
        }
        [DefaultValue(defaultCurrencyLength)]
        public int Currencylength
        {
            get
            {
                return this.currencylength;
            }
            set
            {
                if (value < 4 || value > 15)
                {
                    throw new ArgumentOutOfRangeException("The Currencylength property cannot be smaller than 4 or larger than 15.");
                }
                if (this.decimalPlaces != value)
                {
                    SetCurrencylength(this.RowIndex, value);
                    OnCommonChange();
                }
            }
        }
        private void OnCommonChange()
        {
            if (this.DataGridView != null && !this.DataGridView.IsDisposed && !this.DataGridView.Disposing)
            {
                if (this.RowIndex == -1)
                {
                    this.DataGridView.InvalidateColumn(this.ColumnIndex);
                }
                else
                {
                    this.DataGridView.UpdateCellValue(this.ColumnIndex, this.RowIndex);
                }
            }
        }
        internal void SetDecimalPlaces(int rowIndex, int value)
        {
            Debug.Assert(value >= 2 && value <= 5);
            this.decimalPlaces = value;
            if (OwnsEditingNumericUpDown(rowIndex))
            {
                this.EditingNumericCurrency.DecimalPlace = value;
            }
        }
        internal void SetCurrencylength(int rowIndex, int value)
        {
            Debug.Assert(value >= 4 && value <= 15);
            this.currencylength = value;
            if (OwnsEditingNumericUpDown(rowIndex))
            {
                this.EditingNumericCurrency.CurrencyLength = value;
            }
        }
        private bool OwnsEditingNumericUpDown(int rowIndex)
        {
            if (rowIndex == -1 || this.DataGridView == null)
            {
                return false;
            }
            CurrencyEditingControl numericUpDownEditingControl = this.DataGridView.EditingControl as CurrencyEditingControl;
            return numericUpDownEditingControl != null && rowIndex == ((IDataGridViewEditingControl)numericUpDownEditingControl).EditingControlRowIndex;
        }
        private CurrencyEditingControl EditingNumericCurrency
        {
            get
            {
                return this.DataGridView?.EditingControl as CurrencyEditingControl ?? throw new InvalidOperationException("Editing control is null."); // return this.DataGridView.EditingControl as CurrencyEditingControl;
            }
        }

        

        public override object Clone()
        {
            CurrencyCell? dataGridViewCell = base.Clone() as CurrencyCell;
            if (dataGridViewCell != null)
            {
                dataGridViewCell.DecimalPlaces = this.DecimalPlaces;
                dataGridViewCell.Currencylength = this.Currencylength;
            }
            return dataGridViewCell!;
        }

        public override void InitializeEditingControl(int rowIndex, object
            initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            base.InitializeEditingControl(rowIndex, initialFormattedValue,
                dataGridViewCellStyle);
            CurrencyEditingControl? ctl =
                DataGridView?.EditingControl as CurrencyEditingControl;
            ctl!.DecimalPlace = this.DecimalPlaces;
            ctl.CurrencyLength = this.Currencylength;
            if (this.Value == null)
            {
                this.Value = DefaultValue();          
                ctl.Text = DefaultValue();
            }
            else
            {
                ctl.Text = (Convert.ToDecimal(this.Value)).ToString(DefaultValue());
            }
        }

        public override Type EditType
        {
            get
            {
                return defaultEditType; 
            }
        }

        public override Type ValueType
        {
            get
            {
                Type valueType = base.ValueType;
                if (valueType != null)
                {
                    return valueType;
                }
                return typeof(System.String);
            }
        }

        
        public String DefaultValue()
        {
            string Result = "0.";
            for (int i = 0; i < this.decimalPlaces; i++)
            {
                Result = Result + "0";
            }
            return Result;
        }
        public override object DefaultNewRowValue
        {
            get
            {
                return DefaultValue();
            }
        }
        
        protected override object GetFormattedValue(object value,
                                                    int rowIndex,
                                                    ref DataGridViewCellStyle cellStyle,
                                                    TypeConverter valueTypeConverter,
                                                    TypeConverter formattedValueTypeConverter,
                                                    DataGridViewDataErrorContexts context)
        {
            try
            {
                object formattedValue = base.GetFormattedValue(value, rowIndex, ref cellStyle, valueTypeConverter, formattedValueTypeConverter, context);

                string formattedNumber = formattedValue?.ToString() == "NaN" ? "0.00" : formattedValue as string ?? "0";

                if (!string.IsNullOrEmpty(formattedNumber) && formattedNumber != "NaN")
                {
                    if (decimal.TryParse(formattedNumber, System.Globalization.NumberStyles.Float, null, out decimal formattedDecimal))
                    {
                        return formattedDecimal.ToString(DefaultValue());
                    }
                }
                return formattedValue!;
            }
            finally
            {
            }
            /*
            try
            {
                object formattedValue = base.GetFormattedValue(value, rowIndex, ref cellStyle, valueTypeConverter, formattedValueTypeConverter, context);
                string formattedNumber = formattedValue?.ToString() == "NaN" ? "0.00" : formattedValue as string ?? string.Empty;
                if (!string.IsNullOrEmpty(formattedNumber) && value != null && formattedNumber != "NaN")
                {
                    Decimal formattedDecimal =Decimal.Parse(formattedNumber, System.Globalization.NumberStyles.Float);

                    return formattedDecimal.ToString(DefaultValue());
                }
                return formattedValue!;
            }
            finally
            {

            }
            */
        }

       
    }

    class CurrencyEditingControl : TextBox, IDataGridViewEditingControl
    {
        DataGridView dataGridView;
        private bool valueChanged = false;
        int rowIndex;
        
        public override string Text
        {
            get
            {
                return base.Text;
            }

            set
            {
                this.TextAlign = HorizontalAlignment.Right;
                base.Text = value;
            }
        }

        public CurrencyEditingControl()
        {
            this.MaxLength = CurrencyLength;
            this.TabStop = false;
        }
        public virtual DataGridView EditingControlDataGridView
        {
            get
            {
                return this.dataGridView;
            }
            set
            {
                this.dataGridView = value;
            }
        }
        public object EditingControlFormattedValue
        {
            get
            {
                if(string.IsNullOrEmpty(this.Text))
                {
                    return DefaultValue();
                }
                return this.Text;
            }
            set
            {
                this.Text = (string)value;
            }
        }

        // Implements the 
        // IDataGridViewEditingControl.GetEditingControlFormattedValue method.
        public object GetEditingControlFormattedValue(
            DataGridViewDataErrorContexts context)
        {
            
            return EditingControlFormattedValue;
        }

        // Implements the 
        // IDataGridViewEditingControl.ApplyCellStyleToEditingControl method.
        public void ApplyCellStyleToEditingControl(
            DataGridViewCellStyle dataGridViewCellStyle)
        {
            this.Font = dataGridViewCellStyle.Font;
            this.ForeColor = dataGridViewCellStyle.ForeColor;
            this.BackColor = dataGridViewCellStyle.BackColor;

        }

        public int EditingControlRowIndex
        {
            get
            {
                return rowIndex;
            }
            set
            {
                rowIndex = value;
            }
        }

        public bool EditingControlWantsInputKey(
            Keys key, bool dataGridViewWantsInputKey)
        {

            switch (key & Keys.KeyCode)
            {
                case Keys.Tab:
                case Keys.Up:
                case Keys.Down:
                case Keys.Left:
                case Keys.Right:
                case Keys.Control:
                    return true;
                default:
                    return !dataGridViewWantsInputKey;
            }
        }

        public void PrepareEditingControlForEdit(bool selectAll)
        {
        }

        public bool RepositionEditingControlOnValueChange
        {
            get
            {
                return false;
            }
        }

       
        private int _DecimalPlace;
        public int DecimalPlace
        {
            get
            {
                return _DecimalPlace;
            }
            set
            {
                _DecimalPlace = value;
            }
        }
        private int _currencylength;
        public int CurrencyLength
        {
            get
            {
                return _currencylength;
            }
            set
            {
                _currencylength = value;
            }
        }

        public bool EditingControlValueChanged
        {
            get
            {
                return valueChanged;
            }
            set
            {
                valueChanged = value;
            }
        }
        
        protected override void OnTextChanged(EventArgs eventargs)
        {
            valueChanged = true;
            this.EditingControlDataGridView.NotifyCurrentCellDirty(true);
            base.OnTextChanged(eventargs);
        }
        public Cursor EditingPanelCursor
        {
            get
            {
                return base.Cursor;
            }
        }
        private string Seperator = ".";


        private void HighlisghtDecimals()
        {
            int decimalPlace = this.Text.IndexOf(Seperator);
            this.SelectionStart = decimalPlace + 1;
            this.SelectionLength = this.CurrencyLength;
            this.Select();
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.D0
           || e.KeyCode == Keys.D1
           || e.KeyCode == Keys.D2
           || e.KeyCode == Keys.D3
           || e.KeyCode == Keys.D4
           || e.KeyCode == Keys.D5
           || e.KeyCode == Keys.D6
           || e.KeyCode == Keys.D7
           || e.KeyCode == Keys.D8
           || e.KeyCode == Keys.D9
           || e.KeyCode == Keys.NumPad0
           || e.KeyCode == Keys.NumPad1
           || e.KeyCode == Keys.NumPad2
           || e.KeyCode == Keys.NumPad3
           || e.KeyCode == Keys.NumPad4
           || e.KeyCode == Keys.NumPad5
           || e.KeyCode == Keys.NumPad6
           || e.KeyCode == Keys.NumPad7
           || e.KeyCode == Keys.NumPad8
           || e.KeyCode == Keys.NumPad9
           || e.KeyCode == Keys.Decimal
           || e.KeyCode == Keys.OemPeriod
           || e.KeyCode == Keys.OemMinus
           || e.KeyCode == Keys.Subtract
           )
            {
                this.MaxLength = CurrencyLength;
                int intCursorPos = this.SelectionStart;
                int decimalPlace = this.Text.IndexOf(Seperator);
                int tmpLength = (decimalPlace == -1 && !(e.KeyCode == Keys.Decimal || e.KeyCode == Keys.OemPeriod) ? this.CurrencyLength - this.DecimalPlace - 1 : this.CurrencyLength);
                if (this.Text.Length >= tmpLength && this.SelectedText.Length==0 && !(e.KeyCode == Keys.Decimal || e.KeyCode == Keys.OemPeriod))
                {
                    e.SuppressKeyPress = true;
                }

                //allowed
                else
                if (e.KeyCode == Keys.OemMinus || e.KeyCode == Keys.Subtract)
                {
                    if (this.SelectionStart != 0 || this.Text.Contains("-"))
                    {
                        e.SuppressKeyPress = true;
                    }
                }
                if ((e.KeyCode == Keys.Decimal || e.KeyCode == Keys.OemPeriod) && this.Text.Contains(Seperator))

                {
                    HighlisghtDecimals();
                    e.SuppressKeyPress = true;
                }


                string[] dec;
                if (this.Text.Contains(Seperator) && intCursorPos > decimalPlace/* + this.DecimalPlace*/)
                {
                    int dselectionlen = this.SelectionLength;
                    dec = this.Text.Split('.');
                    if (dec.Length == 2 && dec[1].Length==(2 - dselectionlen))
                    {
                        e.SuppressKeyPress = true;
                    }
                }
            }
            else if (e.KeyCode == Keys.Back
                || e.KeyCode == Keys.Delete
                || e.KeyCode == Keys.Left
                || e.KeyCode == Keys.Right
                || e.KeyCode == Keys.Up
                || e.KeyCode == Keys.Down)
            {
                if (e.KeyCode == Keys.Up)
                {
                    int decimalPlace = this.Text.IndexOf(Seperator);
                    this.SelectionStart = 0;
                    this.SelectionLength = (decimalPlace > -1 ? decimalPlace : this.Text.Length) + 1;
                    this.Select();
                }
                else if (e.KeyCode == Keys.Down)
                {
                    int decimalPlace = this.Text.IndexOf(Seperator);
                    if (decimalPlace > -1)
                    {
                        HighlisghtDecimals();
                        e.SuppressKeyPress = true;
                    }

                }

            }
            else if ((e.KeyCode == Keys.V && e.Control) && Clipboard.ContainsText())
            {
                if (Clipboard.GetText().Contains(".") && Clipboard.GetText().Length<= CurrencyLength)
                {
                    this.MaxLength = CurrencyLength;
                }
                else
                {
                    if(Clipboard.GetText().Length <= (CurrencyLength- DecimalPlace))
                    {
                        this.MaxLength = CurrencyLength;
                    }
                    else
                    {
                        this.MaxLength = (CurrencyLength - (DecimalPlace + 1));
                    }
                }
                KeypressValidation.Instance.Keypress_PasteChecking(this, e, "NumberDot");
            }
            else if (((e.KeyCode == Keys.X || e.KeyCode == Keys.C || e.KeyCode == Keys.Z || e.KeyCode == Keys.Y) && e.Control))
            {
            }
            else
            {
                e.SuppressKeyPress = true;
            }
            

            base.OnKeyDown(e);
        }


        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && e.KeyChar != Convert.ToChar(Keys.Enter) && e.KeyChar != Convert.ToChar(Keys.Escape) && !char.IsControl(e.KeyChar))
            {
                if (!char.IsDigit(e.KeyChar) && e.KeyChar != '-' && e.KeyChar != '.' && !char.IsControl(e.KeyChar))
                {
                    e.Handled = true;
                }
                else if (e.KeyChar == '-' && (this.Text.Contains("-") || this.SelectionStart != 0))
                {
                    e.Handled = true;
                }
                else if (e.KeyChar == '.' && this.Text.Contains("."))
                {
                    e.Handled = true;
                }
            }
            base.OnKeyPress(e);
        }
        public String DefaultValue()
        {
            string Result = "0.";
            for (int i = 0; i < this.DecimalPlace; i++)
            {
                Result = Result + "0";
            }
            return Result;
        }
        protected override void OnLeave(EventArgs e)
        {
            if(string.IsNullOrEmpty(this.Text))
            {
                this.Text =DefaultValue();
            }
            base.OnLeave(e);
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (Clipboard.GetText().Contains(".") && Clipboard.GetText().Length <= CurrencyLength)
                {
                    this.MaxLength = CurrencyLength;
                }
                else if (!Clipboard.GetText().Contains(".") && !this.SelectedText.Contains(".") && Clipboard.GetText().Length <= CurrencyLength)
                {
                    //if (this.Text.Contains(".") && this.Text.IndexOf(Seperator) < this.SelectionStart && Clipboard.GetText().Length<= DecimalPlace)
                    //{
                    //    this.MaxLength = CurrencyLength;
                    //}
                    //else
                    //{
                    //    this.MaxLength = (CurrencyLength - (DecimalPlace + 1));
                    //}
                }
                else
                {
                    this.MaxLength = (CurrencyLength - (DecimalPlace + 1));
                }
                this.ContextMenuStrip = null;
               AddContextMenu(this, "NumberDot");                
            }
            base.OnMouseDown(e);
        }
        public void AddContextMenu(TextBox txtBox, string validation)
        {
            if (txtBox.ContextMenuStrip == null)
            {
                ContextMenuStrip cms = new ContextMenuStrip();
                ToolStripMenuItem tsmiCut = new ToolStripMenuItem("Cut");
                tsmiCut.Click += (sender, e) => txtBox.Cut();
                cms.Items.Add(tsmiCut);
                ToolStripMenuItem tsmiCopy = new ToolStripMenuItem("Copy");
                tsmiCopy.Click += (sender, e) => txtBox.Copy();
                cms.Items.Add(tsmiCopy);
                ToolStripMenuItem tsmiPaste = new ToolStripMenuItem("Paste");
                tsmiPaste.Click += new EventHandler((s, e) => Mousepress_PasteAction(s, e, txtBox, validation));
                cms.Items.Add(tsmiPaste);
                txtBox.ContextMenuStrip = cms;
            }
        }
        void Mousepress_PasteAction(object sender, EventArgs e, TextBox txtBox, string validation)
        {
            if (!this.SelectedText.Contains("."))
            {
                if (Clipboard.GetText().Contains("."))
                {
                    return;
                }
            }
            if (Clipboard.ContainsText())
            {
                Regex? Regex = validation == "NameChecking" ? new Regex("^[a-zA-Z0-9!@#$%^&*()_+.,-/ ]*$") : validation == "NameCheckingNonAllowSpace" ? new Regex("^[a-zA-Z0-9!@#$%^&*()_+.,-/]*$") : validation == "EmailChecking" ? new Regex("^[a-zA-Z0-9@.-_]*$") : validation == "WebsiteNameChecking" ? new Regex(@"^[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(/\S*)?$") : validation == "NumberDot" ? new Regex("^\\$?(\\d{1,3},?(\\d{3},?)*\\d{3}(.\\d{0,3})?|\\d{1,3}(.\\d{2})?)$") : validation == "TaxDetailsNumberChecking" ? new Regex("^[a-zA-Z0-9-]*$") : validation == "LoginChecking" ? new Regex("^[a-zA-Z0-9,.@]*$") : validation == "Number" ? new Regex("^[0-9]*$") : null;
                var clipData = Clipboard.GetDataObject().GetData(typeof(string));
                if (Regex != null && Regex.Match(clipData.ToString()!).Success)
                {
                    txtBox.Paste();
                }
            }
        }
    }



    //--------------------------------------------------------------------------------------------------------------------------------
    //Quantity column
    //--------------------------------------------------------------------------------------------------------------------------------
    public partial class DataGridViewQuantityColumn : DataGridViewColumn
    {
        public DataGridViewQuantityColumn() : base(new QuantityCell())
        {
        }

        private QuantityCell QuantityCellTemplate
        {
            get
            {
                return (QuantityCell)this.CellTemplate;
            }
        }

        [Category("Behavior"), DefaultValue(QuantityCell.defaultDecimalPlaces), Description("The minimum decimal point 2 maximum decimal place is 5")]
        public int DecimalPlaces
        {
            get
            {
                if (this.QuantityCellTemplate == null)
                {
                    throw new InvalidOperationException("Operation cannot be completed because this DataGridViewColumn does not have a CellTemplate.");
                }
                return this.QuantityCellTemplate.DecimalPlaces;
            }
            set
            {
                if (this.QuantityCellTemplate == null)
                {
                    throw new InvalidOperationException("Operation cannot be completed because this DataGridViewColumn does not have a CellTemplate.");
                }
                this.QuantityCellTemplate.DecimalPlaces = value;
                if (this.DataGridView != null)
                {
                    DataGridViewRowCollection dataGridViewRows = this.DataGridView.Rows;
                    int rowCount = dataGridViewRows.Count;
                    for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
                    {
                        DataGridViewRow dataGridViewRow = dataGridViewRows.SharedRow(rowIndex);
                        QuantityCell? dataGridViewCell = dataGridViewRow.Cells[this.Index] as QuantityCell;
                        if (dataGridViewCell != null)
                        {
                            dataGridViewCell.SetDecimalPlaces(rowIndex, value);
                        }
                    }
                    this.DataGridView.InvalidateColumn(this.Index);
                }
            }
        }
        [Category("Behavior"), DefaultValue(QuantityCell.defaultQuantityLength), Description("The maximum Quantity length is 15")]
        public int Quantitylength
        {
            get
            {
                if (this.QuantityCellTemplate == null)
                {
                    throw new InvalidOperationException("Operation cannot be completed because this DataGridViewColumn does not have a CellTemplate.");
                }
                return this.QuantityCellTemplate.Quantitylength;
            }
            set
            {
                if (this.QuantityCellTemplate == null)
                {
                    throw new InvalidOperationException("Operation cannot be completed because this DataGridViewColumn does not have a CellTemplate.");
                }
                this.QuantityCellTemplate.Quantitylength = value;
                if (this.DataGridView != null)
                {
                    DataGridViewRowCollection dataGridViewRows = this.DataGridView.Rows;
                    int rowCount = dataGridViewRows.Count;
                    for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
                    {
                        DataGridViewRow dataGridViewRow = dataGridViewRows.SharedRow(rowIndex);
                        QuantityCell? dataGridViewCell = dataGridViewRow.Cells[this.Index] as QuantityCell;
                        if (dataGridViewCell != null)
                        {
                            dataGridViewCell.SetQuantitylength(rowIndex, value);
                        }
                    }
                    this.DataGridView.InvalidateColumn(this.Index);
                }
            }
        }
        [ Category("Behavior"), DefaultValue(QuantityCell.defaultNegativeSign), Description("")]
        public bool NegativeSign
        {
            get
            {
                if (this.QuantityCellTemplate == null)
                {
                    throw new InvalidOperationException("Operation cannot be completed because this DataGridViewColumn does not have a CellTemplate.");
                }
                return this.QuantityCellTemplate.NegativeSign;
            }
            set
            {
                if (this.QuantityCellTemplate == null)
                {
                    throw new InvalidOperationException("Operation cannot be completed because this DataGridViewColumn does not have a CellTemplate.");
                }
                this.QuantityCellTemplate.NegativeSign = value;
                if (this.DataGridView != null)
                {
                    DataGridViewRowCollection dataGridViewRows = this.DataGridView.Rows;
                    int rowCount = dataGridViewRows.Count;
                    for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
                    {
                        DataGridViewRow dataGridViewRow = dataGridViewRows.SharedRow(rowIndex);
                        QuantityCell dataGridViewCell = dataGridViewRow.Cells[this.Index] as QuantityCell;
                        if (dataGridViewCell != null)
                        {
                            dataGridViewCell.SetNegativeSign(rowIndex, value);
                        }
                    }
                    this.DataGridView.InvalidateColumn(this.Index);
                }
            }
        }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {
                QuantityCell? QuantityCell = value as QuantityCell;
                if (value != null && QuantityCell == null)
                {
                    throw new InvalidCastException("Must be a QuantityCell");
                }
                base.CellTemplate = value;
            }
        }
    }

    public class QuantityCell : DataGridViewTextBoxCell
    {
        public QuantityCell()
                : base()
        {
            this.decimalPlaces = Global.Company != null ? Global.Company.QuantityPricision : 2;
            this.quantitylength = 8 + (Global.Company != null ? Global.Company.QuantityPricision : 2);
            this.negativesign = false;
        }
        private static Type defaultEditType = typeof(QuantityEditingControl);


        internal const int defaultDecimalPlaces = 2;
        internal const int defaultQuantityLength = 10;
        internal const bool defaultNegativeSign = false;
        private bool negativesign;
        private int decimalPlaces;
        private int quantitylength;
        [DefaultValue(defaultDecimalPlaces)]
        public int DecimalPlaces
        {
            get
            {
                return this.decimalPlaces;
            }
            set
            {
                if (value < 0 || value > 5)
                {
                    throw new ArgumentOutOfRangeException("The DecimalPlaces property cannot be smaller than 0 or larger than 5.");
                }
                if (this.decimalPlaces != value)
                {
                    SetDecimalPlaces(this.RowIndex, value);
                    OnCommonChange();
                }
            }
        }
        [DefaultValue(defaultQuantityLength)]
        public int Quantitylength
        {
            get
            {
                return this.quantitylength;
            }
            set
            {
                if (value < 5 || value > 15)
                {
                    throw new ArgumentOutOfRangeException("The Quantitylength property cannot be smaller than 5 or larger than 15.");
                }
                if (this.decimalPlaces != value)
                {
                    SetQuantitylength(this.RowIndex, value);
                    OnCommonChange();
                }
            }
        }
        [DefaultValue(defaultNegativeSign)]
        public bool NegativeSign
        {
            get
            {
                return this.negativesign;
            }
            set
            {
                if (this.negativesign != value)
                {
                    SetNegativeSign(this.RowIndex, value);
                    OnCommonChange();
                }
            }
        }
        private void OnCommonChange()
        {
            if (this.DataGridView != null && !this.DataGridView.IsDisposed && !this.DataGridView.Disposing)
            {
                if (this.RowIndex == -1)
                {
                    this.DataGridView.InvalidateColumn(this.ColumnIndex);
                }
                else
                {
                    this.DataGridView.UpdateCellValue(this.ColumnIndex, this.RowIndex);
                }
            }
        }
        internal void SetDecimalPlaces(int rowIndex, int value)
        {
            Debug.Assert(value >= 0 && value <= 5);
            this.decimalPlaces = value;
            if (OwnsEditingNumericUpDown(rowIndex))
            {
                this.EditingNumericQuantity.DecimalPlace = value;
            }
        }
        internal void SetQuantitylength(int rowIndex, int value)
        {
            Debug.Assert(value >= 5 && value <= 15);
            this.quantitylength = value;
            if (OwnsEditingNumericUpDown(rowIndex))
            {
                this.EditingNumericQuantity.QuantityLength = value;
            }
        }
        internal void SetNegativeSign(int rowIndex, bool value)
        {
            this.negativesign = value;
            if (OwnsEditingNumericUpDown(rowIndex))
            {
                this.EditingNumericQuantity.NegativeSign = value;
            }
        }
        private bool OwnsEditingNumericUpDown(int rowIndex)
        {
            if (rowIndex == -1 || this.DataGridView == null)
            {
                return false;
            }
            QuantityEditingControl numericUpDownEditingControl = this.DataGridView.EditingControl as QuantityEditingControl;
            return numericUpDownEditingControl != null && rowIndex == ((IDataGridViewEditingControl)numericUpDownEditingControl).EditingControlRowIndex;
        }
        private QuantityEditingControl EditingNumericQuantity
        {
            get
            {
                return this.DataGridView?.EditingControl as QuantityEditingControl ?? throw new InvalidOperationException("Editing control is null."); // return this.DataGridView.EditingControl as QuantityEditingControl;
            }
        }



        public override object Clone()
        {
            QuantityCell? dataGridViewCell = base.Clone() as QuantityCell;
            if (dataGridViewCell != null)
            {
                dataGridViewCell.DecimalPlaces = this.DecimalPlaces;
                dataGridViewCell.Quantitylength = this.Quantitylength;
                dataGridViewCell.NegativeSign = this.NegativeSign;
            }
            return dataGridViewCell!;
        }

        public override void InitializeEditingControl(int rowIndex, object
            initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            base.InitializeEditingControl(rowIndex, initialFormattedValue,
                dataGridViewCellStyle);
            QuantityEditingControl? ctl =
                DataGridView?.EditingControl as QuantityEditingControl;
            ctl!.DecimalPlace = this.DecimalPlaces;
            ctl.QuantityLength = this.Quantitylength;
            ctl.NegativeSign = this.NegativeSign;
            if (this.Value == null)
            {
                this.Value = DefaultValue();
                ctl.Text = DefaultValue();
            }
            else
            {
                ctl.Text = (Convert.ToDecimal(this.Value)).ToString(DefaultValue());
            }
        }

        public override Type EditType
        {
            get
            {
                return defaultEditType;
            }
        }

        public override Type ValueType
        {
            get
            {
                Type valueType = base.ValueType;
                if (valueType != null)
                {
                    return valueType;
                }
                return typeof(System.String);
            }
        }


        public String DefaultValue()
        {
            string Result = "0.";
            for (int i = 0; i < this.decimalPlaces; i++)
            {
                Result = Result + "0";
            }
            return Result;
        }
        public override object DefaultNewRowValue
        {
            get
            {
                return DefaultValue();
            }
        }
       
        protected override object GetFormattedValue(object value,
                                                    int rowIndex,
                                                    ref DataGridViewCellStyle cellStyle,
                                                    TypeConverter valueTypeConverter,
                                                    TypeConverter formattedValueTypeConverter,
                                                    DataGridViewDataErrorContexts context)
        {
            try
            {
                object formattedValue = base.GetFormattedValue(value, rowIndex, ref cellStyle, valueTypeConverter, formattedValueTypeConverter, context);
                string? formattedNumber = formattedValue as string;
                if (!string.IsNullOrEmpty(formattedNumber) && value != null)
                {
                    Decimal formattedDecimal = Decimal.Parse(formattedNumber, System.Globalization.NumberStyles.Float);

                    return formattedDecimal.ToString(DefaultValue());
                }
                return formattedValue;
            }
            finally
            {

            }
        }


    }

    class QuantityEditingControl : TextBox, IDataGridViewEditingControl
    {
        DataGridView dataGridView;
        private bool valueChanged = false;
        int rowIndex;

        public override string Text
        {
            get
            {
                return base.Text;
            }

            set
            {
                this.TextAlign = HorizontalAlignment.Right;
                base.Text = value;
            }
        }

        public QuantityEditingControl()
        {
            this.MaxLength = QuantityLength;
            this.TabStop = false;
        }
        public virtual DataGridView EditingControlDataGridView
        {
            get
            {
                return this.dataGridView;
            }
            set
            {
                this.dataGridView = value;
            }
        }
        public object EditingControlFormattedValue
        {
            get
            {
                if (string.IsNullOrEmpty(this.Text))
                {
                    return DefaultValue();
                }
                return this.Text;
            }
            set
            {
                this.Text = (string)value;
            }
        }

        // Implements the 
        // IDataGridViewEditingControl.GetEditingControlFormattedValue method.
        public object GetEditingControlFormattedValue(
            DataGridViewDataErrorContexts context)
        {

            return EditingControlFormattedValue;
        }

        // Implements the 
        // IDataGridViewEditingControl.ApplyCellStyleToEditingControl method.
        public void ApplyCellStyleToEditingControl(
            DataGridViewCellStyle dataGridViewCellStyle)
        {
            this.Font = dataGridViewCellStyle.Font;
            this.ForeColor = dataGridViewCellStyle.ForeColor;
            this.BackColor = dataGridViewCellStyle.BackColor;

        }

        public int EditingControlRowIndex
        {
            get
            {
                return rowIndex;
            }
            set
            {
                rowIndex = value;
            }
        }

        public bool EditingControlWantsInputKey(
            Keys key, bool dataGridViewWantsInputKey)
        {

            switch (key & Keys.KeyCode)
            {
                case Keys.Tab:
                case Keys.Up:
                case Keys.Down:
                case Keys.Control:
                    return true;
                default:
                    return !dataGridViewWantsInputKey;
            }
        }

        public void PrepareEditingControlForEdit(bool selectAll)
        {
        }

        public bool RepositionEditingControlOnValueChange
        {
            get
            {
                return false;
            }
        }


        private int _DecimalPlace;
        public int DecimalPlace
        {
            get
            {
                return _DecimalPlace;
            }
            set
            {
                _DecimalPlace = value;
            }
        }
        private int _Quantitylength;
        public int QuantityLength
        {
            get
            {
                return _Quantitylength;
            }
            set
            {
                _Quantitylength = value;
            }
        }
        private bool _NegativeSign;
        public bool NegativeSign
        {
            get
            {
                return _NegativeSign;
            }
            set
            {
                _NegativeSign = value;
            }
        }
        public bool EditingControlValueChanged
        {
            get
            {
                return valueChanged;
            }
            set
            {
                valueChanged = value;
            }
        }

        protected override void OnTextChanged(EventArgs eventargs)
        {
            valueChanged = true;
            this.EditingControlDataGridView.NotifyCurrentCellDirty(true);
            base.OnTextChanged(eventargs);
        }
        public Cursor EditingPanelCursor
        {
            get
            {
                return base.Cursor;
            }
        }
        private string Seperator = ".";


        private void HighlisghtDecimals()
        {
            int decimalPlace = this.Text.IndexOf(Seperator);
            this.SelectionStart = decimalPlace + 1;
            this.SelectionLength = this.QuantityLength;
            this.Select();
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.D0
           || e.KeyCode == Keys.D1
           || e.KeyCode == Keys.D2
           || e.KeyCode == Keys.D3
           || e.KeyCode == Keys.D4
           || e.KeyCode == Keys.D5
           || e.KeyCode == Keys.D6
           || e.KeyCode == Keys.D7
           || e.KeyCode == Keys.D8
           || e.KeyCode == Keys.D9
           || e.KeyCode == Keys.NumPad0
           || e.KeyCode == Keys.NumPad1
           || e.KeyCode == Keys.NumPad2
           || e.KeyCode == Keys.NumPad3
           || e.KeyCode == Keys.NumPad4
           || e.KeyCode == Keys.NumPad5
           || e.KeyCode == Keys.NumPad6
           || e.KeyCode == Keys.NumPad7
           || e.KeyCode == Keys.NumPad8
           || e.KeyCode == Keys.NumPad9
           || e.KeyCode == Keys.Decimal
           || e.KeyCode == Keys.OemPeriod
            || e.KeyCode == Keys.OemMinus
           )
            {
                this.MaxLength = QuantityLength;
                int intCursorPos = this.SelectionStart;
                int decimalPlace = this.Text.IndexOf(Seperator);
                int tmpLength = (decimalPlace == -1 && !(e.KeyCode == Keys.Decimal || e.KeyCode == Keys.OemPeriod || e.KeyCode == Keys.OemMinus) ? this.QuantityLength - this.DecimalPlace - 1 : this.QuantityLength);
                if (this.Text.Length >= tmpLength && this.SelectedText.Length == 0 && !(e.KeyCode == Keys.Decimal || e.KeyCode == Keys.OemPeriod || e.KeyCode == Keys.OemMinus))
                {
                    e.SuppressKeyPress = true;
                }
                else if (((e.KeyCode == Keys.Decimal || e.KeyCode == Keys.OemPeriod ) && this.Text.Contains(Seperator)))
                {
                    HighlisghtDecimals();
                    e.SuppressKeyPress = true;
                }
                else if ((!NegativeSign && e.KeyCode == Keys.OemMinus) || (e.KeyCode == Keys.OemMinus && this.Text.Contains("-")))
                {
                    e.SuppressKeyPress = true;
                }

                string[] dec;
                if (this.Text.Contains(Seperator) && intCursorPos > decimalPlace/* + this.DecimalPlace*/)
                {
                    int dselectionlen = this.SelectionLength;
                    dec = this.Text.Split('.');
                    if (Global.Company.QuantityPricision == Global.Company.QuantityPricision && dec[1].Length == (Global.Company.QuantityPricision - dselectionlen))
                    {
                        e.SuppressKeyPress = true;
                    }
                }
            }
            else if (e.KeyCode == Keys.Back
                || e.KeyCode == Keys.Delete
                || e.KeyCode == Keys.Left
                || e.KeyCode == Keys.Right
                || e.KeyCode == Keys.Up
                || e.KeyCode == Keys.Down)
            {
                if (e.KeyCode == Keys.Up)
                {
                    int decimalPlace = this.Text.IndexOf(Seperator);
                    this.SelectionStart = 0;
                    this.SelectionLength = (decimalPlace > -1 ? decimalPlace : this.Text.Length) + 1;
                    this.Select();
                }
                else if (e.KeyCode == Keys.Down)
                {
                    int decimalPlace = this.Text.IndexOf(Seperator);
                    if (decimalPlace > -1)
                    {
                        HighlisghtDecimals();
                        e.SuppressKeyPress = true;
                    }

                }

            }
            else if ((e.KeyCode == Keys.V && e.Control) && Clipboard.ContainsText())
            {
                if (Clipboard.GetText().Contains(".") && Clipboard.GetText().Length <= QuantityLength)
                {
                    this.MaxLength = QuantityLength;
                }
                else
                {
                    if (Clipboard.GetText().Length <= (QuantityLength - DecimalPlace))
                    {
                        this.MaxLength = QuantityLength;
                    }
                    else
                    {
                        this.MaxLength = (QuantityLength - (DecimalPlace + 1));
                    }
                }
                KeypressValidation.Instance.Keypress_PasteChecking(this, e, (NegativeSign? "NumberDotNegativeSign" : "NumberDot"));
            }
            else if (((e.KeyCode == Keys.X || e.KeyCode == Keys.C || e.KeyCode == Keys.Z || e.KeyCode == Keys.Y) && e.Control))
            {
            }
            else
            {
                e.SuppressKeyPress = true;
            }


            base.OnKeyDown(e);
        }


        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && e.KeyChar != Convert.ToChar(Keys.Enter) && e.KeyChar != Convert.ToChar(Keys.Escape) && !char.IsControl(e.KeyChar))
            {
                if (!(Char.IsDigit(e.KeyChar) || (e.KeyChar == (char)Keys.Back) || (e.KeyChar == '.') || (e.KeyChar == '-')))
                {
                    e.Handled = true;
                }
                else if (e.KeyChar == '.' && this.Text.IndexOf('.') > -1)
                {
                    e.Handled = true;
                }
                else if (NegativeSign && e.KeyChar == '-' && this.Text.IndexOf('-') > -1)
                {
                    e.Handled = true;
                }
            }
            base.OnKeyPress(e);
        }
        public String DefaultValue()
        {
            string Result = "0.";
            for (int i = 0; i < this.DecimalPlace; i++)
            {
                Result = Result + "0";
            }
            return Result;
        }
        protected override void OnLeave(EventArgs e)
        {
            if (string.IsNullOrEmpty(this.Text))
            {
                this.Text = DefaultValue();
            }
            base.OnLeave(e);
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (Clipboard.GetText().Contains(".") && Clipboard.GetText().Length <= QuantityLength)
                {
                    this.MaxLength = QuantityLength;
                }
                else if (!Clipboard.GetText().Contains(".") && !this.SelectedText.Contains(".") && Clipboard.GetText().Length <= QuantityLength)
                {
                    //if (this.Text.Contains(".") && this.Text.IndexOf(Seperator) < this.SelectionStart && Clipboard.GetText().Length<= DecimalPlace)
                    //{
                    //    this.MaxLength = QuantityLength;
                    //}
                    //else
                    //{
                    //    this.MaxLength = (QuantityLength - (DecimalPlace + 1));
                    //}
                }
                else
                {
                    this.MaxLength = (QuantityLength - (DecimalPlace + 1));
                }
                this.ContextMenuStrip = null;
                AddContextMenu(this, (NegativeSign? "NumberDotNegativeSign" : "NumberDot"));
            }
            base.OnMouseDown(e);
        }
        public void AddContextMenu(TextBox txtBox, string validation)
        {
            if (txtBox.ContextMenuStrip == null)
            {
                ContextMenuStrip cms = new ContextMenuStrip();
                ToolStripMenuItem tsmiCut = new ToolStripMenuItem("Cut");
                tsmiCut.Click += (sender, e) => txtBox.Cut();
                cms.Items.Add(tsmiCut);
                ToolStripMenuItem tsmiCopy = new ToolStripMenuItem("Copy");
                tsmiCopy.Click += (sender, e) => txtBox.Copy();
                cms.Items.Add(tsmiCopy);
                ToolStripMenuItem tsmiPaste = new ToolStripMenuItem("Paste");
                tsmiPaste.Click += new EventHandler((s, e) => Mousepress_PasteAction(s, e, txtBox, validation));
                cms.Items.Add(tsmiPaste);
                txtBox.ContextMenuStrip = cms;
            }
        }
        void Mousepress_PasteAction(object sender, EventArgs e, TextBox txtBox, string validation)
        {
            if (!this.SelectedText.Contains("."))
            {
                if (Clipboard.GetText().Contains("."))
                {
                    return;
                }
            }
            if (Clipboard.ContainsText())
            {
                Regex? Regex = validation == "NameChecking" ? new Regex("^[a-zA-Z0-9!@#$%^&*()_+.,-/ ]*$") : validation == "NameCheckingNonAllowSpace" ? new Regex("^[a-zA-Z0-9!@#$%^&*()_+.,-/]*$") : validation == "EmailChecking" ? new Regex("^[a-zA-Z0-9@.-_]*$") : validation == "WebsiteNameChecking" ? new Regex(@"^[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(/\S*)?$") : validation == "NumberDot" ? new Regex("^\\$?(\\d{1,3},?(\\d{3},?)*\\d{3}(.\\d{0,3})?|\\d{1,3}(.\\d{2})?)$") : validation == "NumberDotNegativeSign" ? new Regex("^\\$[-]?(\\d{1,3},?(\\d{3},?)*\\d{3}(.\\d{0,3})?|\\d{1,3}(.\\d{2})?)$") : validation == "TaxDetailsNumberChecking" ? new Regex("^[a-zA-Z0-9-]*$") : validation == "LoginChecking" ? new Regex("^[a-zA-Z0-9,.@]*$") : validation == "Number" ? new Regex("^[0-9]*$") : null;
                var clipData = Clipboard.GetDataObject().GetData(typeof(string));
                if (Regex != null && Regex.Match(clipData.ToString()!).Success)
                {
                    txtBox.Paste();
                }
            }
        }
    }


    //----------------------------------------------------------------------------------------------------------------------------------------

    ///calender coloumn

    //------------------------------------------------------------------------------------------------------------------------------------

    public class DataGridViewCalendarColumn : DataGridViewColumn
    {
        public DataGridViewCalendarColumn() : base(new CalendarCell())
        {
        }

        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {
                // Ensure that the cell used for the template is a CalendarCell.
                if (value != null &&
                    !value.GetType().IsAssignableFrom(typeof(CalendarCell)))
                {
                    throw new InvalidCastException("Must be a CalendarCell");
                }
                base.CellTemplate = value;
            }
        }

    }

    public class CalendarCell : DataGridViewTextBoxCell
    {

        public CalendarCell()
            : base()
        {
            // Use the short date format.
            if (Global.Company == null)
            {
                this.Style.Format = "d";
            }
            else
            {
                this.Style.Format = Global.Company.DateFormat;
            }

        }

        public override void InitializeEditingControl(int rowIndex, object
            initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            // Set the value of the editing control to the current cell value.
            base.InitializeEditingControl(rowIndex, initialFormattedValue,
                dataGridViewCellStyle);
            CalendarEditingControl? ctl =
                DataGridView?.EditingControl as CalendarEditingControl;
            // Use the default row value when Value property is null.
            if (this.Value == null)
            {
                ctl!.Value = (DateTime)this.DefaultNewRowValue;
            }
            else
            {
                if (Global.Company == null)
                {
                    ctl!.Value =(DateTime)this.Value;
                }
                else
                {
                    try
                    {
                        ctl!.Value = (DateTime)DateUtils.ToDate(((DateTime)this.Value).ToString(Global.Company.DateFormat)!, Global.Company.DateFormat)!;
                    }
                    #pragma warning disable 0168 // variable declared but not used.
                    catch (Exception ex)
                    {
                        //SLogger.
                    }
                    #pragma warning restore 0168
                }
            }
            if(MinDate!=null && MinDate.Date>=Global.getCurrentFiscalYearStartDate())
            {
                ctl!.MinDate = MinDate;
            }
        }

        public override Type EditType
        {
            get
            {
                // Return the type of the editing control that CalendarCell uses.
                return typeof(CalendarEditingControl);
            }
        }

        public override Type ValueType
        {
            get
            {
                // Return the type of the value that CalendarCell contains.

                return typeof(DateTime);
            }
        }

        public override object DefaultNewRowValue
        {
            get
            {
                // Use the current date and time as the default value.
                return /*DateTime.Now*/Global.getTransactionDate();
            }
        }
        private DateTime _MinDate;
        public DateTime MinDate
        {
            get
            {
                return _MinDate;
            }
            set
            { 
                _MinDate = value;
            }
        }
    }

    class CalendarEditingControl : DateTimePicker, IDataGridViewEditingControl
    {
        DataGridView dataGridView;
        private bool valueChanged = false;
        int rowIndex;

        public CalendarEditingControl()
        {
            // this.Format = DateTimePickerFormat.Short;
            this.Format = DateTimePickerFormat.Custom;
            this.CustomFormat = Global.Company.DateFormat;
            //this.MinDate = Global.getTransactionDate();
        }

        // Implements the IDataGridViewEditingControl.EditingControlFormattedValue 
        // property.
        public object EditingControlFormattedValue
        {
            get
            {
                return this.Value.ToShortDateString();
            }
            set
            {
                if (value is String)
                {
                    try
                    {
                        // This will throw an exception of the string is 
                        // null, empty, or not in the format of a date.
                        this.Value = DateTime.Parse((String)value);
                    }
                    catch
                    {
                        // In the case of an exception, just use the 
                        // default value so we're not left with a null
                        // value.
                        this.Value = DateTime.Now;
                    }
                }
            }
        }

        // Implements the 
        // IDataGridViewEditingControl.GetEditingControlFormattedValue method.
        public object GetEditingControlFormattedValue(
            DataGridViewDataErrorContexts context)
        {
            return EditingControlFormattedValue;
        }

        // Implements the 
        // IDataGridViewEditingControl.ApplyCellStyleToEditingControl method.
        public void ApplyCellStyleToEditingControl(
            DataGridViewCellStyle dataGridViewCellStyle)
        {
            this.Font = dataGridViewCellStyle.Font;
            this.CalendarForeColor = dataGridViewCellStyle.ForeColor;
            this.CalendarMonthBackground = dataGridViewCellStyle.BackColor;
        }

        // Implements the IDataGridViewEditingControl.EditingControlRowIndex 
        // property.
        public int EditingControlRowIndex
        {
            get
            {
                return rowIndex;
            }
            set
            {
                rowIndex = value;
            }
        }

        // Implements the IDataGridViewEditingControl.EditingControlWantsInputKey 
        // method.
        public bool EditingControlWantsInputKey(
            Keys key, bool dataGridViewWantsInputKey)
        {
            // Let the DateTimePicker handle the keys listed.
            switch (key & Keys.KeyCode)
            {
                case Keys.Tab:
                case Keys.Left:
                case Keys.Up:
                case Keys.Down:
                case Keys.Right:
                case Keys.Home:
                case Keys.End:
                case Keys.PageDown:
                case Keys.PageUp:
                    return true;
                default:
                    return !dataGridViewWantsInputKey;
            }
        }

        // Implements the IDataGridViewEditingControl.PrepareEditingControlForEdit 
        // method.
        public void PrepareEditingControlForEdit(bool selectAll)
        {
            // No preparation needs to be done.
        }

        // Implements the IDataGridViewEditingControl
        // .RepositionEditingControlOnValueChange property.
        public bool RepositionEditingControlOnValueChange
        {
            get
            {
                return false;
            }
        }

        // Implements the IDataGridViewEditingControl
        // .EditingControlDataGridView property.
        public DataGridView EditingControlDataGridView
        {
            get
            {
                return dataGridView;
            }
            set
            {
                dataGridView = value;
            }
        }

        // Implements the IDataGridViewEditingControl
        // .EditingControlValueChanged property.
        public bool EditingControlValueChanged
        {
            get
            {
                return valueChanged;
            }
            set
            {
                valueChanged = value;
            }
        }

        // Implements the IDataGridViewEditingControl
        // .EditingPanelCursor property.
        public Cursor EditingPanelCursor
        {
            get
            {
                return base.Cursor;
            }
        }

        protected override void OnValueChanged(EventArgs eventargs)
        {
            // Notify the DataGridView that the contents of the cell
            // have changed.
            valueChanged = true;
            this.EditingControlDataGridView.NotifyCurrentCellDirty(true);
            base.OnValueChanged(eventargs);
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
          base.OnKeyDown(e);

        }
        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);
            this.CustomFormat = Global.Company.DateFormat;
            this.Text = DateUtils.ToDate(((DateTime)this.Value.Date).ToString(Global.Company.DateFormat), Global.Company.DateFormat).ToString();
        }
        

    }
}
