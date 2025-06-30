using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fa.views.utils.Common
{
    public class DataGridViewColoumnAdjustment
    {
        public static void AdjustColumnWidthsAfterHiding(DataGridView dataGridView)
        {
            int visibleColumnCount = 0;
            int totalWidthOfVisibleColumns = 0;

            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                if (column.Visible)
                {
                    visibleColumnCount++;
                    totalWidthOfVisibleColumns += column.Width;
                }
            }

            if (visibleColumnCount > 0)
            {
                // Adjust the total width by subtracting the width of the vertical scrollbar
                int adjustedWidth = dataGridView.Width - 12; // SystemInformation.VerticalScrollBarWidth;

                // Calculate the adjustment factor using the adjusted width
                double adjustmentFactor = (double)adjustedWidth / totalWidthOfVisibleColumns;

                foreach (DataGridViewColumn column in dataGridView.Columns)
                {
                    if (column.Visible)
                    {
                        // Apply the adjusted width to each visible column
                        column.Width = (int)(column.Width * adjustmentFactor);
                    }
                }
            }
        }

        public static void AdjustColumnWidthsDataGrid(DataGridView dataGridView)
        {
            int visibleColumnCount = 0;
            int totalWidthOfVisibleColumns = 0;

            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                if (column.Visible)
                {
                    visibleColumnCount++;
                    totalWidthOfVisibleColumns += column.Width;
                }
            }

            if (visibleColumnCount > 0)
            {
                double adjustmentFactor = (double)(dataGridView.Width - SystemInformation.VerticalScrollBarWidth) / totalWidthOfVisibleColumns;

                foreach (DataGridViewColumn column in dataGridView.Columns)
                {
                    if (column.Visible)
                    {
                        column.Width = (int)(column.Width * adjustmentFactor);
                    }
                }
            }
        }
        public static void AdjustHiddenColumnWidths(DataGridView dataGridView)
        {
            int totalWidth = dataGridView.Width; // Total width of the DataGridView control
            int visibleColumnWidth = 0;

            // Calculate the width occupied by visible columns
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                if (column.Visible)
                {
                    visibleColumnWidth += column.Width;
                }
            }

            // Adjust for scrollbar width
            if (dataGridView.Controls.OfType<VScrollBar>().FirstOrDefault()?.Visible == true)
            {
                totalWidth -= SystemInformation.VerticalScrollBarWidth;
            }

            // Calculate the available width for hidden columns
            int availableWidth = totalWidth - visibleColumnWidth;

            // Distribute the available width equally among hidden columns
            int hiddenColumnCount = 0;
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                if (!column.Visible)
                {
                    hiddenColumnCount++;
                }
            }

            int hiddenColumnWidth = hiddenColumnCount > 0 ? availableWidth / hiddenColumnCount : 0;

            // Set the width of each hidden column
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                if (!column.Visible)
                {
                    column.Width = hiddenColumnWidth;
                }
            }
        }

    }
}
