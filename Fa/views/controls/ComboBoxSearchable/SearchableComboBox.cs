using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

public class SearchableComboBox : ComboBox, IDataGridViewEditingControl
{
    private List<string> originalItems = new List<string>();
    private DataGridView dataGridView;
    private bool valueChanged = false;
    private int rowIndex;

    public SearchableComboBox()
    {
        this.DropDownStyle = ComboBoxStyle.DropDown;
        this.TextChanged += SearchableComboBox_TextChanged;
    }

    public void SetItems(IEnumerable<string> items)
    {
        originalItems = items.ToList(); // Store original items

        this.DataSource = null;  // Reset DataSource before modifying
        this.DataSource = originalItems; // Set new DataSource
    }

    private void SearchableComboBox_TextChanged(object sender, EventArgs e)
    {
        if (this.Focused)
        {
            string searchText = this.Text.ToLower();
            this.Items.Clear();

            if (string.IsNullOrEmpty(searchText))
            {
                this.Items.AddRange(originalItems.ToArray());
            }
            else
            {
                var filteredItems = originalItems.Where(item => item.ToLower().Contains(searchText)).ToArray();
                this.Items.AddRange(filteredItems);
                this.DroppedDown = true; // Keep dropdown open
            }

            this.SelectionStart = this.Text.Length;
            this.SelectionLength = 0;

            valueChanged = true;
            this.EditingControlDataGridView?.NotifyCurrentCellDirty(true);
        }
    }

    // Implement IDataGridViewEditingControl methods
    public object EditingControlFormattedValue
    {
        get => this.Text;
        set => this.Text = value?.ToString();
    }

    public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context) => this.Text;

    public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle)
    {
        this.Font = dataGridViewCellStyle.Font;
        this.ForeColor = dataGridViewCellStyle.ForeColor;
        this.BackColor = dataGridViewCellStyle.BackColor;
    }

    public int EditingControlRowIndex
    {
        get => rowIndex;
        set => rowIndex = value; // ✅ Now includes a setter
    }

    public bool EditingControlWantsInputKey(Keys keyData, bool dataGridViewWantsInputKey) => true;

    public void PrepareEditingControlForEdit(bool selectAll) { }

    public bool RepositionEditingControlOnValueChange => false;

    public DataGridView EditingControlDataGridView
    {
        get => dataGridView;
        set => dataGridView = value;
    }

    public bool EditingControlValueChanged
    {
        get => valueChanged;
        set => valueChanged = value;
    }

    public Cursor EditingPanelCursor => this.Cursor;
}
