using System;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace fa.views.controls.text
{
    public partial class DelayedTextChangeTextBox : TextBox
    {
        private int _threshold = 1000;
        private int _SearchFrom;
        private bool _Delay;

        public DelayedTextChangeTextBox()
        {
            InitializeComponent();
        }
        public bool Delay
        {
            get
            {
                return _Delay;
            }
            set { _Delay = value; }
        }
        public int DelayTime
        {
            get
            {
                return _threshold;
            }
            set { _threshold = value; }
        }
        public int Searchstartfrom
        {
            get
            {
                return _SearchFrom;
            }
            set { _SearchFrom = value!=0?value:1; }
        }
        public System.Windows.Forms.Timer Timer;
        protected override void OnTextChanged(EventArgs e)
        {
            if (Delay)
            {
                if (Timer == null)
                {
                    Timer = new Timer();
                    Timer.Interval = DelayTime;
                    Timer.Tick += new EventHandler(this.handleTypingTimerTimeout);
                }
                Timer.Stop();
                Timer.Start();
            }
            else
            {
                if (this.Text.Length==0 || this.Text.Length >= Searchstartfrom)
                {
                    base.OnTextChanged(e);
                }
            }
        }
        public void Clear()
        {
            bool Temp = Delay;
            Delay = false;
            base.Text=string.Empty;
            Delay = Temp;
        }
        private void handleTypingTimerTimeout(object sender, EventArgs e)
        {
            var timer = sender as Timer;
            if (timer == null)
            {
                return;
            }
            timer.Stop();
            if (this.Text.Length == 0 || this.Text.Length >= Searchstartfrom)
            {
                base.OnTextChanged(e);
            }
        }

        private void DelayedTextChangeTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (Timer != null)
                {
                    Timer.Stop();
                }
                if (this.Text.Length == 0 || this.Text.Length >= Searchstartfrom)
                {
                    base.OnTextChanged(e);
                }
            }
        }
        
    }
}
