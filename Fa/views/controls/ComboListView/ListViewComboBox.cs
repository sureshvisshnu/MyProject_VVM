using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fa.views.controls.ComboListView
{
    public class ListViewComboBox : ComboBox
    {
        private ListView listView;
        private Form popupForm;

        public ListViewComboBox()
        {
            this.DropDownHeight = 1; // Hide default dropdown
            this.DropDownWidth = 1;

            // Create ListView
            listView = new ListView();
            listView.View = View.Details;
            listView.FullRowSelect = true;

            // Handle selection
            listView.Click += (s, e) => {
                if (listView.SelectedItems.Count > 0)
                {
                    this.Text = listView.SelectedItems[0].Text;
                    popupForm.Hide();
                }
            };
        }

        protected override void OnDropDown(EventArgs e)
        {
            base.OnDropDown(e);

            // Show ListView in popup
            popupForm = new Form();
            popupForm.FormBorderStyle = FormBorderStyle.None;
            popupForm.ShowInTaskbar = false;
            popupForm.Controls.Add(listView);

            // Position below the combobox
            Point screenPos = this.PointToScreen(new Point(0, this.Height));
            popupForm.Location = screenPos;
            popupForm.Size = new Size(this.Width, 200);

            listView.Dock = DockStyle.Fill;
            popupForm.Show();
        }
    }
}
