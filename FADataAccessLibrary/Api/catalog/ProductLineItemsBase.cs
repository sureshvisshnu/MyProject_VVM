namespace fa.api.catalog
{
    public class ProductLineItemsBase
    {
        public bool? isInventoryAtBatch;
        public bool? isUseHSNCode;
        public String CategoryName { get; set; }
        public float CostPrice { get; set; }
        public double CurrentQty { get; set; }
        public float? DefaultDiscountLocal { get; set; }
        public string Description { get; set; }
        public string HSNCode { get; set; }
        public long Id { get; set; }
        public string MaterialId { get; set; }
        public float MSRP { get; set; }

        public String Name { get; set; }
        public double OpenStock { get; set; }
        public String ParentName { get; set; }
        public float PurchasePrice { get; set; }
        public double PurchaseQty { get; set; }
        public float RetailPrice { get; set; }
        public String RetailUOM { get; set; }
        public int RetailXfactor { get; set; }
        public double SalesQty { get; set; }
        public String SalesTaxCGST { get; set; }
        public String SalesTaxIGST { get; set; }
        public float SalesTaxPresntageCGST { get; set; }
        public float SalesTaxPresntageIGST { get; set; }
        public float SalesTaxPresntageSGST { get; set; }
        public String SalesTaxSGST { get; set; }

        public String Uom { get; set; }
        public float WholeSalePrice { get; set; }
        public String WholesaleUOM { get; set; }
        public int WholeSaleXfactor { get; set; }
    }
}