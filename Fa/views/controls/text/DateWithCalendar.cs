using System;
using System.Drawing;
using System.Windows.Forms;
using fa.api.utils;


namespace fa.views.controls.text
{

    public partial class DateWithCalendar : UserControl
    {
        string[] ValidMask = new string[] { "00/00/0000", "00/00/00", "00/00/0000", "00/00/00", "00/00/0000", "00/00/00", "00/00/00", "0000/00/00", "00/LLL/00" };
        string[] ValidFormat = new string[] { "MM/dd/yyyy", "MM/dd/yy", "dd/MM/yyyy", "dd/MM/yy","M/d/yyyy","M/d/yy","yy/MM/dd","yyyy/MM/dd","dd/MMM/yy"};
        DateTime DefaultMinDate = (DateTime) DateUtils.ToDate("01/01/1900", "MM/dd/yyyy")!;
        DateTime DefaultMaxDate = (DateTime)DateUtils.ToDate("12/31/9997", "MM/dd/yyyy")!;

        public event EventHandler ValueChanged;

        DateTime _MinDate;
        public DateTime MinDate
        {
            set
            {
                if(value == null || value < DefaultMinDate || value > DefaultMaxDate)
                {
                    _MinDate = DefaultMinDate;
                }
                else
                {
                    _MinDate = value;
                }
                dateTimePicker.MinDate = _MinDate;
            }
            get
            {
                return _MinDate;
            }
        }

        DateTime _MaxDate;
        public DateTime MaxDate
        {
            set
            {
                if (value == null || value < DefaultMinDate || value > DefaultMaxDate)
                {
                    _MaxDate = DefaultMaxDate;
                }
                else
                {                    
                    _MaxDate = value;
                }
                dateTimePicker.MaxDate = _MaxDate;
            }
            get
            {
                return _MaxDate;
            }
        }

        DateTime? _Date;
        public DateTime? Date
        {
            get
            {
                //DateTime DateTime =new DateTime(0001, 01, 01);
                //if (Format == "dd/MMM/yy")
                //{
                //    DateTime = new DateTime(01, 01, 01);
                //}
                if (!string.IsNullOrEmpty(DateStr))
                {
                    return (DateTime)DateUtils.ToDate(DateStr, this.Format)!;
                }
                return null/*(DateTime)DateUtils.ToDate(DateTime.ToString(), this.Format)*/;
            }
            set
            {
                if (value!=null && DateUtils.ValidDate(((DateTime)value).Date.ToString(Format), Format))
                {
                    DateTime date = DateTime.Parse(value.ToString()!);
                    ResetTexBoxMask(date);
                    maskedTextBox.Text = ((DateTime)value).ToString(this.Format);
                    if (((DateTime)value).Date >= dateTimePicker.MinDate)
                    {
                        //dateTimePicker.Value = (DateTime)DateUtils.ToDate(value.ToString(Format), Format);
                    }
                }
                _Date = value;
            }
        }

        String _DateStr;
        private String DateStr
        {
            get
            {
                if(!isValidDate())
                {
                    return string.Empty;
                }
                return _DateStr;
            }
            set
            {
                _DateStr = value;
            }
        }

        string _Format = null!;
        public string Format
        {
            get
            {
                return _Format;
            }
            set
            {
                bool isValidFormat = false;
                foreach(string m in ValidFormat)
                {
                    if(value.Equals(value))
                    {
                        _Format = value;
                        isValidFormat = true;
                        maskedTextBox.Mask = ValidMask[Array.IndexOf(ValidFormat, _Format)];
                    }
                }
                if(!isValidFormat)
                {
                    throw new System.ArgumentException("Invalid date format:", value);
                }
            }
        }

        bool _ReadOnly = false;
        public bool ReadOnly
        {
            get{
                return _ReadOnly;
            }
            set
            {
                _ReadOnly = value;
                maskedTextBox.ReadOnly = _ReadOnly;
                dateTimePicker.Enabled = !_ReadOnly;
            }
        }
        
        public DateWithCalendar()
        {
            InitializeComponent();
            if (this._Format == null)
            {
                this.Format = ValidFormat[0];
            }
            this.dateTimePicker.MinDate = DefaultMinDate;
            this.dateTimePicker.MaxDate = DefaultMaxDate;
        }

        public void Reset()
        {
            dateTimePicker.ResetText();
            maskedTextBox.ResetText();
        }

        private void MaskedTextBox_DateChange()
        {
            if (isValidDate())
            {
                dateTimePicker.Value = (DateTime)DateUtils.ToDate(maskedTextBox.Text, Format)!;
                DateStr = maskedTextBox.Text;
            }
            else
            {
                maskedTextBox.ResetText();       
            }
        }
        private void OnValueChanged()
        {
            ValueChanged?.Invoke(this, EventArgs.Empty);
        }
        private void DateWithCalendar_Load(object sender, EventArgs e)
        {
            maskedTextBox.Focus();
        }

        private void maskedTextBox_Leave(object sender, EventArgs e)
        {
            if(isValidDate())
            {
                DateStr = dateTimePicker.Value.ToString(this.Format);
            }
            OnLeave(e);
        }
        private void dateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            DateTime date = DateTime.Parse(dateTimePicker.Value.ToString()!);
            ResetTexBoxMask(date);
            maskedTextBox.Text = dateTimePicker.Value.ToString(this.Format);
            DateStr = dateTimePicker.Value.ToString(this.Format);
            OnValueChanged();
        }

        private void maskedTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                dateTimePicker.Select();
                SendKeys.Send("%{DOWN}");
            }
        }

        public void ResetTexBoxMask(DateTime date)
        {
            if (Format == "M/d/yyyy")
            {
                if (date.Day < 10 && date.Month < 10)
                {
                    maskedTextBox.Mask = "0/0/0000";
                }
                else if (date.Day < 10)
                {
                    maskedTextBox.Mask = "00/0/0000";
                }
                else if (date.Month < 10)
                {
                    maskedTextBox.Mask = "0/00/0000";
                }
                else
                {
                    maskedTextBox.Mask = "00/00/0000";
                }
            }
            else if (Format == "M/d/yy")
            {
                if (date.Day < 10 && date.Month < 10)
                {
                    maskedTextBox.Mask = "0/0/00";
                }
                else if (date.Day < 10)
                {
                    maskedTextBox.Mask = "00/0/00";
                }
                else if (date.Month < 10)
                {
                    maskedTextBox.Mask = "0/00/00";
                }
                else
                {
                    maskedTextBox.Mask = "00/00/00";
                }
            }
        }

        private void dateTimePicker_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                //SendKeys.Send("{F4}");
            }
        }

        private void dateTimePicker_MouseDown(object sender, MouseEventArgs e)
        {
            //testing
            //maskedTextBox.Focus();
        }

        private void dateTimePicker_CloseUp(object sender, EventArgs e)
        {
            maskedTextBox.Text = dateTimePicker.Value.ToString(this.Format);
            maskedTextBox.Select();
            maskedTextBox.Focus();
        }

        private bool isValidDate()
        {
            try
            {
                DateTime dd = (DateTime) DateUtils.ToDate(maskedTextBox.Text, this.Format)!;
                if(dd!=null && dd.Ticks >= MinDate.Ticks && dd.Date <= MaxDate.Date)
                {
                    return true;
                }
            }
            #pragma warning disable 0168 // variable declared but not used.
            catch (Exception e)
            {
                //do nothing returns false;
            }
            #pragma warning restore 0168
            return false;
        }

        private void maskedTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if(this.Format=="dd/MMM/yy")
                {

                }
                DateTime dd = (DateTime)DateUtils.ToDate(maskedTextBox.Text, this.Format)!;
                if (dd != null && dd.Ticks >= MinDate.Ticks && dd.Ticks <= MaxDate.Ticks)
                {
                    dateTimePicker.Value = dd;
                }
                OnValueChanged();
            }
            #pragma warning disable 0168
            catch (Exception exp)
            {
                //do nothing returns false;
            }
            #pragma warning restore 0168
        }

        private void dateTimePicker_Leave(object sender, EventArgs e)
        {
            OnLeave(e);
        }

        private void maskedTextBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            this.OnPreviewKeyDown(e);
            //for tool strip
            KeyEventArgs KeyEventArgs = new KeyEventArgs(e.KeyData);
            base.OnKeyDown(KeyEventArgs);
        }

        private void maskedTextBox_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }
        public bool Isempty()
        {
            bool Status = false;
            if(string.IsNullOrEmpty(maskedTextBox.Text.Replace("-","").Trim()))
            {
                Status = true;
            }
            return Status;
        }

        private void DateWithCalendar_Resize(object sender, EventArgs e)
        {
            //this.Height = 20;
            this.Size = new Size(93, 21);
        }
    }
}
