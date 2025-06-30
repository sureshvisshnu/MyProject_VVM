using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace Fa.views.controls.grid
{
    public class CheckBoxHeaderCell : DataGridViewColumnHeaderCell
    {
        private bool _checked;
        private Rectangle _checkBoxBounds;
        private const int CHECKBOX_SIZE = 15;
        public Rectangle CheckBoxBounds { get; private set; }

        private static Dictionary<int, bool> _checkedStates = new Dictionary<int, bool>();

        public static bool IsChecked(int columnIndex)
        {
            return _checkedStates.ContainsKey(columnIndex) && _checkedStates[columnIndex];
        }

        public bool Checked
        {
            get => _checked;
            set
            {
                if (_checked != value)
                {
                    _checked = value;
                    DataGridView?.InvalidateColumn(ColumnIndex);
                }
            }
        }

        protected override void Paint(
            Graphics graphics,
            Rectangle clipBounds,
            Rectangle cellBounds,
            int rowIndex,
            DataGridViewElementStates dataGridViewElementState,
            object value,
            object formattedValue,
            string errorText,
            DataGridViewCellStyle cellStyle,
            DataGridViewAdvancedBorderStyle advancedBorderStyle,
            DataGridViewPaintParts paintParts)
        {
            base.Paint(graphics, clipBounds, cellBounds, rowIndex, dataGridViewElementState,
                        value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);

            Size checkSize = CheckBoxRenderer.GetGlyphSize(graphics, CheckBoxState.UncheckedNormal);
            int x = cellBounds.X + (cellBounds.Width - checkSize.Width) / 2;
            int y = cellBounds.Y + (cellBounds.Height - checkSize.Height) / 2;

            CheckBoxState state = IsChecked(ColumnIndex)
                ? CheckBoxState.CheckedNormal
                : CheckBoxState.UncheckedNormal;
            CheckBoxRenderer.DrawCheckBox(graphics, new Point(x, y), state);
        }
        protected override void OnMouseClick(DataGridViewCellMouseEventArgs e)
        {
            DataGridView!.ClearSelection();
            DataGridView.CurrentCell = null;

            int columnIndex = e.ColumnIndex;
            if (!_checkedStates.ContainsKey(columnIndex))
                _checkedStates.Add(columnIndex, false);

            _checkedStates[columnIndex] = !_checkedStates[columnIndex];

            DataGridView.InvalidateColumn(columnIndex);
            base.OnMouseClick(e);

        }

        public event ColumnCheckChangedEventHandler? ColumnCheckChanged;
        protected virtual void OnColumnCheckChanged(int columnIndex, bool isChecked)
        {
            ColumnCheckChanged?.Invoke(this, new ColumnCheckChangedEventArgs(columnIndex, isChecked));
        }
    }

    public class ColumnCheckChangedEventArgs : EventArgs
    {
        public int ColumnIndex { get; }
        public bool IsChecked { get; }

        public ColumnCheckChangedEventArgs(int columnIndex, bool isChecked)
        {
            ColumnIndex = columnIndex;
            IsChecked = isChecked;
        }
    }

    public delegate void ColumnCheckChangedEventHandler(object sender, ColumnCheckChangedEventArgs e);
}
