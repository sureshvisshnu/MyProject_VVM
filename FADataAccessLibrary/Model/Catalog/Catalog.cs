using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using fa.model.Accounting.Masters;
using fa.api.catalog;
using fa.model.OrderManagement;
using fa.Data;
using fa.api.Accounting;
using FADataAccessLibrary.Model.Common;

namespace fa.model.Catalog
{
    public class CatalogItem : AuditableEntityForCompany
    {
        [Key]
        public long Id { get; set; }
        public CatalogItemType Type { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        private string _displayAs;
        [MaxLength(50)]
        public string DisplayAs
        {
            get
            {
                if (string.IsNullOrEmpty(_displayAs))
                {
                    return this.Name;
                }
                else
                    return _displayAs;
            }
            set
            {
                if (!value.Equals(this.Name))
                {
                    _displayAs = value;
                }
                else
                {
                    _displayAs = this.Name;
                }
            }
        }
        [MaxLength(250)]
        public string Description { get; set; }
        public long? ParentId { get; set; }
        [ForeignKey("ParentId")]
        public virtual CatalogItem Parent { get; set; }        
        public bool? _shouldMaintainInventory { get; set; }
        [NotMapped]
        public bool? ShouldMaintainInventory
        {
            get
            {
                if (_shouldMaintainInventory == null)
                {
                    if (Parent != null)
                    {
                        Parent = CategoryManager.Instance.SAc((long)ParentId);
                        return Parent.ShouldMaintainInventory;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return _shouldMaintainInventory;
                }
            }
            set
            {
                if (value == true)
                {
                    if (ParentId != null)
                    {
                        Parent = CategoryManager.Instance.SAc((long)ParentId);
                        if (Parent._shouldMaintainInventory != true)
                        {
                            Parent.ShouldMaintainInventory = value;
                        }
                    }
                    else
                    {
                        _shouldMaintainInventory = value;
                    }
                }
            }
        }
        public bool? _isInventoryAtBatch { get; set; }
        [NotMapped]
        public bool? isInventoryAtBatch
        {
            get
            {
                if (_isInventoryAtBatch == null)
                {
                    if (Parent != null)
                    {
                        Parent = CategoryManager.Instance.SAc((long)ParentId);
                        return Parent.isInventoryAtBatch;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return _isInventoryAtBatch;
                }
            }
            set
            {
                if(value==true)
                {
                    if (ParentId != null)
                    {
                        Parent = CategoryManager.Instance.SAc((long)ParentId);
                        if (Parent._isInventoryAtBatch!=true)
                        {
                            Parent.isInventoryAtBatch = value;
                        }                       
                    }
                    else
                    {
                        _isInventoryAtBatch = value;
                    }
                }
            }           
        }
        public float? DefaultDiscountLocal { get; set; }
        [NotMapped]
        public float? DefaultDiscount
        {
            get
            {
                if (DefaultDiscountLocal != null)
                {
                    return DefaultDiscountLocal;
                }
                else if (this.Parent != null)
                {
                    Parent = CategoryManager.Instance.SAc((long)ParentId);
                    return Parent.DefaultDiscount;
                }
                else
                    return 0;
            }
            set
            {
                if (value != null)
                {
                    if (ParentId != null)
                    {
                        Parent = CategoryManager.Instance.SAc((long)ParentId);
                        if (Parent.DefaultDiscountLocal != null)
                        {
                            if (Parent.DefaultDiscountLocal == value)
                            {
                                value = null;
                            }
                        }
                        else
                        {
                            Parent.DefaultDiscount = value;
                            if (Parent.DefaultDiscountLocal == null)
                            {
                                value = null;
                            }
                        }
                    }
                    if (value != null)
                    {
                        DefaultDiscountLocal = value;
                        DefaultDiscount = null;
                    }
                }
            }
        }
        public long? SalesAccountLocalId { get; set; }
        [ForeignKey("SalesAccountLocalId")]
        public virtual Account SalesAccountLocal { get; set; }
        [NotMapped]
        public Account SalesAccount
        {
            get
            {
                if (SalesAccountLocal != null)
                {
                    return SalesAccountLocal;
                }
                else if (this.Parent != null)
                {
                    Parent = CategoryManager.Instance.SAc((long)ParentId);
                    return Parent.SalesAccount;
                }
                else
                    return null;
            }
            set
            {
                if (value != null)
                {
                    if (ParentId != null)
                    {
                        Parent = CategoryManager.Instance.SAc((long)ParentId);
                        if (Parent.SalesAccountLocal != null)
                        {
                            if (Parent.SalesAccountLocalId == value.Id)
                            {
                                value = null;
                            }
                        }
                        else
                        {
                            Parent.SalesAccount = value;
                            if (Parent.SalesAccountLocalId == null)
                            {
                                value = null;
                            }
                        }
                    }
                    if (value !=null)
                    { 
                        SalesAccountLocalId = value.Id;
                        SalesAccountLocal = null;
                    }
                }
            }
        }
        public long? PurchaseAccountLocalId { get; set; }
        [ForeignKey("PurchaseAccountLocalId")]
        public virtual Account PurchaseAccountLocal { get; set; }
        [NotMapped]
        public Account PurchaseAccount
        {
            get
            {
                if (PurchaseAccountLocal != null)
                {
                    return PurchaseAccountLocal;
                }
                else if (Parent != null)
                { 
                Parent = CategoryManager.Instance.PAc((long)ParentId);
                return Parent.PurchaseAccount;
                }
                else
                    return null;
            }
            set
            {
                if (value != null)
                {
                    if (ParentId != null)
                    {


                        Parent = CategoryManager.Instance.PAc((long)ParentId);
                        if (Parent.PurchaseAccountLocal != null)
                        {
                            if (Parent.PurchaseAccountLocalId == value.Id)
                            {
                                value = null;
                            }
                        }
                        else
                        {
                            Parent.PurchaseAccount = value;
                            if (Parent.PurchaseAccountLocalId == null)
                            {
                                value = null;
                            }
                        }
                                               
                    }
                    if (value != null)
                    {
                        PurchaseAccountLocalId = value.Id;
                        PurchaseAccountLocal = null;
                    }
                     
                }
            }
        }
        public long? DiscountAccountLocalId { get; set; }
        [ForeignKey("DiscountAccountLocalId")]
        public virtual Account DiscountAccountLocal { get; set; }
        [NotMapped]
        public Account DiscountAccount
        {
            get
            {
                if (DiscountAccountLocal != null)
                {
                    return DiscountAccountLocal;
                }
                else if (Parent != null)
                {
                    Parent = CategoryManager.Instance.DAc((long)ParentId);
                    return Parent.DiscountAccount;
                }
                else
                    return null;
            }
            set
            {
                if (value != null)
                {
                    if (ParentId != null)
                    {


                        Parent = CategoryManager.Instance.DAc((long)ParentId);
                        if (Parent.DiscountAccountLocal != null)
                        {
                            if (Parent.DiscountAccountLocalId == value.Id)
                            {
                                value = null;
                            }
                        }
                        else
                        {
                            Parent.DiscountAccount = value;
                            if (Parent.DiscountAccountLocalId == null)
                            {
                                value = null;
                            }
                        }

                    }
                    if (value != null)
                    {
                        DiscountAccountLocalId = value.Id;
                        DiscountAccountLocal = null;
                    }

                }
            }
        }

        public long? InventoryAccountLocalId { get; set; }
        [ForeignKey("InventoryAccountLocalId")]
        public virtual Account InventoryAccountLocal { get; set; }
        [NotMapped]
        public Account InventoryAccount
        {
            get
            {
                if (InventoryAccountLocal != null)
                {
                    return InventoryAccountLocal;
                }
                else if (Parent != null)
                {
                    Parent = CategoryManager.Instance.IAc((long)ParentId);
                    return Parent.InventoryAccount;
                }
                else
                    return null;
            }
            set
            {
                if(value!=null)
                {
                    if (ParentId != null)
                    {
                        Parent = CategoryManager.Instance.IAc((long)ParentId);
                        if (Parent.InventoryAccountLocal != null)
                        {
                            if (Parent.InventoryAccountLocalId == value.Id)
                            {
                                value = null;
                            }
                        }
                        else
                        {
                            Parent.InventoryAccount = value;
                            if (Parent.InventoryAccountLocalId == null)
                            {
                                value = null;
                            }
                        }
                    }
                    if (value != null)
                    {
                        InventoryAccountLocalId = value.Id;
                        InventoryAccountLocal = null;
                    }
                }
            }
        }
        ICollection<CatalogItemSalesTaxMap> TempData = new List<CatalogItemSalesTaxMap>();
        ICollection<CatalogItemSalesTaxMap> TempData1 = new List<CatalogItemSalesTaxMap>();

        public virtual ICollection<CatalogItemSalesTaxMap> SalesTaxMapLocal { get; set; }
        [NotMapped]
        public ICollection<CatalogItemSalesTaxMap> SalesTax
        {
            get 
            {
                List<CompanySalesTaxAccountMap> CompanySalesTaxAccountMap = (List<CompanySalesTaxAccountMap>)this.Company.SalesTaxAccountMaps;
                if (CompanySalesTaxAccountMap.Count > 0)
                {
                    ICollection<CatalogItemSalesTaxMap> SalesTaxMaps = new List<CatalogItemSalesTaxMap>();
                    foreach (CompanySalesTaxAccountMap SalesTaxMapFromCompany in CompanySalesTaxAccountMap)
                    {
                        CatalogItemSalesTaxMap SalesTaxLocalEntry = this.SalesTaxMapLocal.FirstOrDefault(t => t.SalesTaxMapId == SalesTaxMapFromCompany.MapId);
                        if (SalesTaxLocalEntry == null)
                        {
                            if (Parent != null)
                            {
                                Parent = CategoryManager.Instance.TaxMap((long)ParentId);
                                Parent.Company.SalesTaxAccountMaps = new List<CompanySalesTaxAccountMap>();
                                Parent.Company.SalesTaxAccountMaps.Add(SalesTaxMapFromCompany);
                                ICollection<CatalogItemSalesTaxMap> lCatalogItemSalesTaxMapsa = Parent.SalesTax;
                                if (Parent.TempData1.Count > 0)
                                {
                                    TempData1.Add(Parent.TempData1.First());
                                }
                            }
                            if (SalesTaxLocalEntry == null && Parent == null)
                            {
                                CountrySaleTax countrySaleTax = CountryManager.Instance.CountryTaxById((long)SalesTaxMapFromCompany.CountrySaleTaxId);
                                if (countrySaleTax != null && countrySaleTax.EffectiveFrom <= Global.TransactionDate && countrySaleTax.EffectiveTo >= Global.TransactionDate)
                                {
                                    CatalogItemSalesTaxMap SalesTaxMapNew = new CatalogItemSalesTaxMap();
                                    SalesTaxMapNew.SalesTaxMapId = SalesTaxMapFromCompany.MapId;
                                    SalesTaxMapNew.TaxPercentage = 0.0F;
                                    TempData1.Add(SalesTaxMapNew);
                                }
                            }
                        }
                        //else if (SalesTaxMapLocal.Count > 0)
                        //{
                        else if (SalesTaxMapLocal.FirstOrDefault(x => x.SalesTaxMapId == SalesTaxMapFromCompany.MapId && x.EffectiveFrom <= Global.TransactionDate && x.EffectiveTo >= Global.TransactionDate) != null)
                        {
                            TempData1.Add(SalesTaxMapLocal.FirstOrDefault(x => x.SalesTaxMapId == SalesTaxMapFromCompany.MapId && x.EffectiveFrom <= Global.TransactionDate && x.EffectiveTo >= Global.TransactionDate));
                        }
                        //    else 
                        //    {
                        //        CountrySaleTax countrySaleTax=CountryManager.Instance.CountryTaxById((long)SalesTaxMapFromCompany.CountrySaleTaxId);
                        //        if (countrySaleTax != null)
                        //        {
                        //            if (SalesTaxMapLocal.FirstOrDefault(x => x.SalesTaxMapId == SalesTaxMapFromCompany.MapId && countrySaleTax.EffectiveFrom <= Global.TransactionDate && countrySaleTax.EffectiveTo >= Global.TransactionDate) != null)
                        //            {
                        //                CatalogItemSalesTaxMap SalesTaxMapNew = new CatalogItemSalesTaxMap();
                        //                SalesTaxMapNew.SalesTaxMapId = SalesTaxMapFromCompany.MapId;
                        //                SalesTaxMapNew.TaxPercentage = 0.0F;
                        //                TempData1.Add(SalesTaxMapNew);
                        //            }
                        //        }
                        //    }
                        //}
                    }

                    if (TempData1.Count > 0)
                    {
                        SalesTaxMaps = TempData1;
                    }
                    return SalesTaxMaps;
                }
                else
                {
                    return new List<CatalogItemSalesTaxMap>();
                }
            }
            set
            {
                if (value!=null && value.Count > 0)
                {
                    List<CatalogItemSalesTaxMap> CatalogItemSalesTaxMapLocal = value.ToList();
                    foreach (CatalogItemSalesTaxMap CatalogItemSalesTaxMap in CatalogItemSalesTaxMapLocal)
                    {
                        if (ParentId != null)
                        {
                            Parent = CategoryManager.Instance.TaxMap((long)ParentId);
                            CatalogItemSalesTaxMap SalesTaxLocalEntry = Parent.SalesTaxMapLocal.FirstOrDefault(x => x.SalesTaxMapId == CatalogItemSalesTaxMap.SalesTaxMapId);
                            if (SalesTaxLocalEntry != null)
                            {
                                if (SalesTaxLocalEntry.TaxPercentage != CatalogItemSalesTaxMap.TaxPercentage)
                                {
                                    TempData.Add(CatalogItemSalesTaxMap);
                                }
                                continue;
                            }

                            List<CatalogItemSalesTaxMap> lCatalogItemSalesTaxMapLocal = new List<CatalogItemSalesTaxMap>();
                            lCatalogItemSalesTaxMapLocal.Add(CatalogItemSalesTaxMap);
                            Parent.SalesTax = lCatalogItemSalesTaxMapLocal;
                            if (Parent.TempData.Count > 0)
                            {
                                this.TempData.Add(Parent.TempData.First());
                            }

                        }
                        else if (ParentId == null)
                        {
                            if (CatalogItemSalesTaxMap.TaxPercentage != 0)
                            {
                                TempData.Add(CatalogItemSalesTaxMap);
                                continue;
                            }
                        }
                    }
                    if (TempData.Count > 0)
                    {
                        this.SalesTaxMapLocal = TempData;
                    }
                    Parent = null;
                }

            }
        }
        public string Manufacturer { get; set; }
        public long? SupplierId { get; set; }
        [ForeignKey("SupplierId")]
        public virtual Supplier Supplier { get; set; }
        public string SupplierName { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }
    //[Table("Categories")]
    public class Category : CatalogItem
    {        
        public Category()
        {
            this.Type = CatalogItemType.CATEGORY;
        }
       
    }

    //[Table("ProductFamilies")]
    public class ProductFamily : CatalogItem
    {        
        public ProductFamily()
        {
            this.Type = CatalogItemType.PRODUCTFAMILY;
        }
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
       
    }

    //[Table("Products")]
    public class Product : CatalogItem
    {
        public long ProductFamilyId { get; set; }
        [ForeignKey("ProductFamilyId")]
        public virtual ProductFamily ProductFamily { get; set; }
        public Product()
        {
            this.Type = CatalogItemType.PRODUCT;
        }
        [MaxLength(14)]
        public string MaterialId { get; set; }
        [MaxLength(14)]
        public string HSNCode { get; set; }
        [MaxLength(14)]
        public string UOM { get; set; }
        public string RetailUOM { get; set; }
        public int RetailXFactor { get; set; }
        public string WholesaleUOM { get; set; }
        public int WholesaleXFactor { get; set; }
        public float PurchasePrice { get; set; }
        public float CostPrice { get; set; }
        public float RetailPrice { get; set; }
        public float WholdSalePrice { get; set; }
        public float Msrp { get; set; }
        public double QuantityOnHand { get; set; }
        public bool UseHsnTax { get; set; }
        public string RackNumber { get; set; }
        public string Schedule { get; set; }
        public virtual ICollection<Inventory> Inventorys { get;} = new List<Inventory>();
        public virtual ICollection<SupplierProduct> SupplierProducts { get; set; } = new List<SupplierProduct>();
    }

    public class CatalogItemSalesTaxMap 
    {
        [Key]
        public long Id { get; set; }
        public long? CatalogItemId { get; set; }
        [ForeignKey("CatalogItemId")]
        public virtual CatalogItem CatalogItem { get; set; }
        public long? SalesTaxMapId { get; set; }
        [ForeignKey("SalesTaxMapId")]
        public virtual CompanySalesTaxAccountMap CompanySalesTaxAccountMap { get; set; }
        public float TaxPercentage { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime EffectiveTo { get; set; }  //default to 01/01/2400


    }

    public enum CatalogItemType
    {
        CATEGORY=1, PRODUCTFAMILY=2, PRODUCT=3
    }
}
