using fa;
using fa.context;
using fa.views;
using fa.views.catalog;
using fa.views.purchase;
using Fa.reports.Purchase;
using FADataAccessLibrary.Api.OrderManagement;
using FADataAccessLibrary.Model.Purchase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisioForge.MediaFramework.ONVIF;
using Color = System.Drawing.Color;

namespace Fa.views.purchase
{
    public partial class FormProductResolution : FormBase
    {

        private readonly long _invoiceTemplateId;
        public long ProductMappingId { get; private set; }
        public List<ProductResolutionDto> ResolvedItems { get; private set; }
        private bool _hasUnsavedChanges = false;
        public event Action MappingCompleted;
        public bool ProductMapping { get; private set; } = false; // Expose the validation result
        public Dictionary<string, string> UpdatedMappings { get; private set; }

        public static string EnterNameErrorMsg = "Template Name could not be empty, please enter Name";
        public static string TmpltAssignErrorMsg = "Missing : Headers are not assigned properly";
        public static string TmpltSaveSuccessMsg = "Product mappings saved successfully!";

        private bool HasUnsavedChanges() => _hasUnsavedChanges;
        public FormProductResolution(List<ProductResolutionDto> resolutionData, long invoiceTemplateId)
        {
            InitializeComponent();
            ResolvedItems = resolutionData;
            _invoiceTemplateId = invoiceTemplateId;
            this.BtnMappingDone.Click += BtnMappingDone_Click!;
            TextBoxProductMappingId.Text = _invoiceTemplateId.ToString();
            var bindingSource = new BindingSource();
            bindingSource.DataSource = ResolvedItems;
            GridViewProductRectification.DataSource = bindingSource;

            ConfigureGrid();
        }

        private void ConfigureGrid()
        {
            var editColumn = new DataGridViewButtonColumn
            {
                HeaderText = "Action",
                Text = "Select",
                UseColumnTextForButtonValue = true
            };
            GridViewProductRectification.Columns.Add(editColumn);
        }

        private void DisplayGrid()
        {
            GridViewProductRectification.AutoGenerateColumns = false;
            GridViewProductRectification.Columns.Clear();

            // Add columns with explicit names
            GridViewProductRectification.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "SlNo",
                DataPropertyName = "SlNo",
                HeaderText = "SL No",
                Width = 50
            });
            GridViewProductRectification.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ProductName", 
                DataPropertyName = "ProductName",
                HeaderText = "Invoice Product Name",
                Width = 250
            });
            GridViewProductRectification.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "CatalogProductName",
                DataPropertyName = "CatalogProductName",
                HeaderText = "Catalog Product",
                Width = 250
            });
            GridViewProductRectification.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "InvoiceUom",
                DataPropertyName = "InvoiceUom",
                HeaderText = "Invoice UOM",
                Width = 100
            });
            GridViewProductRectification.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "CatalogUom",
                DataPropertyName = "CatalogUom",
                HeaderText = "Catalog UOM",
                Width = 100
            });
            GridViewProductRectification.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "MaterialId",
                DataPropertyName = "MaterialId",
                HeaderText = "Material ID",
                Width = 80
            });
        }
        private void FormProductResolution_Load(object sender, EventArgs e)
        {
            DisplayGrid();
        }

        private void GridViewProductRectification_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (GridViewProductRectification.Columns[e.ColumnIndex].Name == "InvoiceProduct")
            {
                var row = GridViewProductRectification.Rows[e.RowIndex];
                bool isMatch = (bool)row.Cells["IsMatch"].Value;

                if (!isMatch)
                {
                    e.CellStyle.BackColor = Color.LightGray;
                    e.CellStyle.ForeColor = Color.DarkRed;
                }
            }
        }

        private void GridViewProductRectification_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2 && e.RowIndex >= 0)
            {
                var selectedItem = ResolvedItems[e.RowIndex];
                {
                    FormSearchItems searchForm = new FormSearchItems(this);
                    searchForm.SearchText = selectedItem.ProductName!;
                    searchForm.ShowDialog();

                    if (searchForm.SelectedProduct != null)
                    {
                        selectedItem.CatalogProductName = searchForm.SelectedProduct.Name;

                        selectedItem.MaterialId = searchForm.SelectedProduct.MaterialId;

                        selectedItem.IsMatch = true;

                        GridViewProductRectification.Rows[e.RowIndex].Cells[2].Value = searchForm.SelectedProduct.Name;
                        GridViewProductRectification.Rows[e.RowIndex].Cells[4].Value = searchForm.SelectedProduct.UOM;
                        GridViewProductRectification.Rows[e.RowIndex].Cells[5].Value = searchForm.SelectedProduct.MaterialId;

                        GridViewProductRectification.InvalidateRow(e.RowIndex);
                    }
                }
            }
        }

        private void toolStripBtnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateProductMappings()) return;

            var productMappings = GetProductMappingFromForm();
            long parentTemplateId = Convert.ToInt64(TextBoxProductMappingId.Text);

            if (new ProductMappingManager().SaveProductMappings(parentTemplateId, productMappings))
            {
                MessageBox.Show("Product mappings saved successfully!");
                RefreshProductGrid(parentTemplateId);
            }
        }

        // ToolStripStatusLabelErrorProductTemplate
        public List<ProductMappingTemplate> GetProductMappingFromForm()
        {
            var productMappings = new List<ProductMappingTemplate>();
            long parentTemplateId = Convert.ToInt64(TextBoxProductMappingId.Text);

            foreach (DataGridViewRow row in GridViewProductRectification.Rows)
            {
                if (row.IsNewRow) continue;

                var mapping = new ProductMappingTemplate
                {
                    InvoiceProductName = row.Cells["ProductName"].Value?.ToString() ?? "",
                    MaterialId = row.Cells["MaterialId"].Value?.ToString() ?? "",
                    CatalogProductName = row.Cells["CatalogProductName"].Value?.ToString() ?? "",
                    InvoiceUOM = row.Cells["InvoiceUOM"].Value?.ToString() ?? "",
                    CatalogUOM = row.Cells["CatalogUOM"].Value?.ToString() ?? "",
                    InvoiceMappingTemplateId = parentTemplateId
                };

                productMappings.Add(mapping);
            }

            return productMappings;
        }
        private bool ValidateProductMappings()
        {
            foreach (DataGridViewRow row in GridViewProductRectification.Rows)
            {
                if (row.IsNewRow) continue;

                if (string.IsNullOrEmpty(row.Cells["MaterialId"].Value?.ToString()))
                {
                    MessageBox.Show("Material ID is required in row " + (row.Index + 1));
                    return false;
                }
            }
            return true;
        }

        private void RefreshProductGrid(long templateId)
        {
            using (var context = new AccountMasterContext())
            {
                GridViewProductRectification.DataSource = context.ProductMappingTemplates
                    .Where(p => p.InvoiceMappingTemplateId == templateId)
                    .Select(p => new
                    {
                        p.Id,
                        p.InvoiceProductName,
                        p.MaterialId,
                        p.CatalogProductName,
                        p.InvoiceUOM,
                        p.CatalogUOM
                    }).ToList();
            }
        }

        private void BtnMappingSave_Click(object sender, EventArgs e)
        {
            try
            {
                var productMappings = GetProductMappingFromForm();
                long parentTemplateId = Convert.ToInt64(TextBoxProductMappingId.Text);

                if (new ProductMappingManager().SaveProductMappings(parentTemplateId, productMappings))
                {
                    ToolStripStatusLabelErrorProductTemplate.Text = TmpltSaveSuccessMsg;
                    //RefreshProductGrid(parentTemplateId); // Refresh grid after save
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
       
        private void BtnMappingDone_Click(object sender, EventArgs e)
        {
            bool hasValidData = true;

            foreach (DataGridViewRow row in GridViewProductRectification.Rows)
            {
                // Skip the "new row" placeholder
                if (row.IsNewRow) continue;

                // Check if CatalogProductName, CatalogUOM, or MaterialId are null/empty
                bool isCatalogProductEmpty = string.IsNullOrWhiteSpace(row.Cells["CatalogProductName"]?.Value?.ToString());
                bool isCatalogUOMEmpty = string.IsNullOrWhiteSpace(row.Cells["CatalogUOM"]?.Value?.ToString());
                bool isMaterialIdEmpty = string.IsNullOrWhiteSpace(row.Cells["MaterialId"]?.Value?.ToString());

                if (isCatalogProductEmpty || isCatalogUOMEmpty || isMaterialIdEmpty)
                {
                    hasValidData = false;
                    break; // Exit early if any row is invalid
                }
            }

            ProductMapping = hasValidData;

            //if (!ProductMapping)
            //{
            //    MessageBox.Show("CatalogProductName, CatalogUOM, and MaterialId must be filled for all rows.");
            //    return; // Keep the form open
            //}

            GridViewProductRectification.EndEdit();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void GridViewProductRectification_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            _hasUnsavedChanges = true;
        }
    }
}
