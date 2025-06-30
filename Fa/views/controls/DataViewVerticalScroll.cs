using System;
using System.Drawing;
using System.Windows.Forms;
using fa.api.Log;

namespace fa.views.controls
{
    public partial class DataViewVerticalScroll : DataGridView
    {
        EventHandler HandlerObj;
        public DataViewVerticalScroll():base()
        {
            if (HandlerObj != null)
            {
                HandlerObj = new EventHandler(VerticalScrollBar_VisibleChanged);
            }
            VerticalScrollBar.Visible = true;
            VerticalScrollBar.VisibleChanged += new EventHandler(VerticalScrollBar_VisibleChanged); ;
        }

        void VerticalScrollBar_VisibleChanged(object sender, EventArgs e)
        {
            try
            {
                int width = VerticalScrollBar.Width;
                this.VerticalScrollBar.Location =
                    new Point(ClientRectangle.Width - (width + 2), 1);
                this.VerticalScrollBar.Size =
                    new Size(width, ClientRectangle.Height - 2);
                //this.VerticalScrollBar.Visible = true;
                //this.VerticalScrollBar.Show();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
            //catch (StackOverflowException ex)
            //{
            //    Logger.LogError(ex);
            //}
        }
        
        private void InitializeComponent()
        {
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // DataViewVerticalScroll
            // 
            this.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.DataViewVerticalScroll_DataError);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.VerticalScrollBar.Visible = true;
            this.VerticalScrollBar.Show();
        }

        private void DataViewVerticalScroll_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }
    }
}
