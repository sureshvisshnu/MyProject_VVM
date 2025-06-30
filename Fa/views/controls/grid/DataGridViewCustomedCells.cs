using fa.api.utils;
using fa.libraries.Validation;
using Fa.views.controls.ComboBoxSearchable;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Fa.views.controls.grid
{
    //---------------------------------------------------------------------------------------------------------
    //ComBox Filtering Cells
    //---------------------------------------------------------------------------------------------------------
    public class DataGridViewFilteredComboBoxCell : DataGridViewComboBoxCell
    {
        public override Type EditType => typeof(DataGridViewFilteredComboBoxEditingControl);

        public override void InitializeEditingControl(int rowIndex, object initialFormattedValue,
            DataGridViewCellStyle dataGridViewCellStyle)
        {
            base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);

            if (DataGridView.EditingControl is DataGridViewFilteredComboBoxEditingControl ctrl)
            {
                ctrl.Text = initialFormattedValue?.ToString() ?? string.Empty;

                // Copy items from cell to editing control
                if (this.Items.Count > 0)
                {
                    ctrl.Items.Clear();
                    ctrl.Items.AddRange(this.Items.Cast<object>().ToArray());
                }
            }
        }
    }

    public class DataGridViewFilteredComboBoxColumn : DataGridViewColumn
    {
        public DataGridViewFilteredComboBoxColumn() : base(new DataGridViewFilteredComboBoxCell())
        {
        }

        public override object Clone()
        {
            var col = base.Clone() as DataGridViewFilteredComboBoxColumn;
            // Clone any additional properties here
            return col;
        }
    }

    public class DataGridViewFilteredComboBoxEditingControl : FilteredComboBox, IDataGridViewEditingControl
    {
        private DataGridView dataGridView;
        private bool valueChanged;
        private int rowIndex;

        public DataGridView EditingControlDataGridView
        {
            get => dataGridView;
            set => dataGridView = value;
        }

        public object EditingControlFormattedValue
        {
            get => this.Text;
            set => this.Text = value?.ToString() ?? string.Empty;
        }

        public int EditingControlRowIndex
        {
            get => rowIndex;
            set => rowIndex = value;
        }

        public bool EditingControlValueChanged
        {
            get => valueChanged;
            set => valueChanged = value;
        }

        public Cursor EditingPanelCursor => base.Cursor;

        public bool RepositionEditingControlOnValueChange => false;

        public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle)
        {
            this.Font = dataGridViewCellStyle.Font;
            this.ForeColor = dataGridViewCellStyle.ForeColor;
            this.BackColor = dataGridViewCellStyle.BackColor;
        }

        public bool EditingControlWantsInputKey(Keys keyData, bool dataGridViewWantsInputKey)
        {
            switch (keyData & Keys.KeyCode)
            {
                case Keys.Up:
                case Keys.Down:
                case Keys.PageUp:
                case Keys.PageDown:
                    return true;
                default:
                    return !dataGridViewWantsInputKey;
            }
        }

        public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
        {
            return EditingControlFormattedValue;
        }

        public void PrepareEditingControlForEdit(bool selectAll)
        {
            if (selectAll)
            {
                this.SelectAll();
            }
            else
            {
                this.SelectionStart = this.Text.Length;
            }
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            valueChanged = true;
            EditingControlDataGridView.NotifyCurrentCellDirty(true);
        }

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            base.OnSelectedIndexChanged(e);
            valueChanged = true;
            EditingControlDataGridView.NotifyCurrentCellDirty(true);
        }
    }

}
