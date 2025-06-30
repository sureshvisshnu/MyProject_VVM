using fa.api.utils;
using fa.views.controls.text;
using System;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace fa.views.controls
{
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.MenuStrip |
                                   ToolStripItemDesignerAvailability.ToolStrip)]
    public class ToolStripCalendar : ToolStripControlHost
    {

        public event EventHandler ValueChanged;
        private DateWithCalendar DateWithCalendar = new DateWithCalendar();

        public ToolStripCalendar() : base(new DateWithCalendar())
        {
            DateTime DefaultMinDate = (DateTime)DateUtils.ToDate("01/01/1900", "MM/dd/yyyy");
            DateTime DefaultMaxDate = (DateTime)DateUtils.ToDate("12/31/9997", "MM/dd/yyyy");

            this.DateWithCalendar = this.Control as DateWithCalendar;
            this.DateWithCalendar.MinDate = DefaultMinDate;
            this.DateWithCalendar.MaxDate = DefaultMaxDate;
        }

        public string Format
        {
            get
            {
                return DateWithCalendar.Format;
            }
            set
            {
                DateWithCalendar.Format = value;
            }
        }
        public DateTime MinDate
        {
            get
            {
                return DateWithCalendar.MinDate;
            }
            set
            {
                DateWithCalendar.MinDate = value;
            }
        }
        public DateTime MaxDate
        {
            get
            {
                return DateWithCalendar.MaxDate;
            }
            set
            {
                DateWithCalendar.MaxDate = value;
            }
        }


        public DateTime? Date
        {
            get
            {
                return DateWithCalendar.Date;
            }
            set
            {
                DateWithCalendar.Date = value;
            }
        }      
        protected override void OnKeyDown(KeyEventArgs e)
        {        
            base.OnKeyDown(e);
        }
        protected override void OnSubscribeControlEvents(Control c)
        {
            base.OnSubscribeControlEvents(c);
            if (c is DateWithCalendar calendar)
            {
                calendar.ValueChanged += Calendar_ValueChanged;
            }
        }

        protected override void OnUnsubscribeControlEvents(Control c)
        {
            base.OnUnsubscribeControlEvents(c);
            if (c is DateWithCalendar calendar)
            {
                calendar.ValueChanged -= Calendar_ValueChanged;
            }
        }
        private void Calendar_ValueChanged(object? sender, EventArgs e)
        {
            ValueChanged?.Invoke(this, e);
        }
    }
}
