using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using fa.api.utils;

namespace fa.views.controls
{
    public partial class QRCodeControl : UserControl
    {
        public QRCodeControl()
        {
            InitializeComponent();
        }
        protected string _label { get; set; }
        public string Caption
        {
            get
            {
                return _label;
            }
            set
            {
                _label = value;
                LabelQRCode.Text = value;
            }
        }
        protected string _text { get; set; }
        public override string Text
        {
            get
            {
                return _text;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _text = value;
                    PrintBarCode();

                }
            }
        }
        private void PrintBarCode()
        {
            if (!string.IsNullOrEmpty(_text))
            {
                //PictureBoxQRCode.BackgroundImage = QRCode.GenerateQRCode(_text);
            }
            else
            {
                PictureBoxQRCode.ResetText();
            }
        }

        private void QRCodeControl_Load(object sender, EventArgs e)
        {
            Text = "";
        }
    }
}
