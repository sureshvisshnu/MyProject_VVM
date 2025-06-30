using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fa.context;
using fa.report;
using fa.model.Catalog;
using fa.api.OrderManagement;
using fa.report.sales;
using fa.model.OrderManagement;
using fa.api.Hms;
using FaData.Utils;

namespace fa.report.Inventory
{
    public class RptStock : Report
    {
        public long[] MaterialIds { get; set; }
        public string Loctn;
        public bool BWFlag;
        long Lid = 0L;
        DateTime dfd;

        IList<StockReportLine> _LineStocks = new List<StockReportLine>();
        public IList<StockReportLine> LineStocks
        {
            get
            {
                return _LineStocks;
            }
        }

        public override string ReportTitle()
        {
            return String.Format("Stock: From: {0} To: {1}", DateUtils.FormatDate(this.FromDate, Company.DateFormat), DateUtils.FormatDate(this.ToDate, Company.DateFormat));

        }

        public override string ReportName()
        {
            string CurrentDate = DateTime.Now.ToString(Company.DateFormat);
            char Separator = CurrentDate.Contains("-") ? '-' : CurrentDate.Contains("/") ? '/' : '.';
            string[] Date = CurrentDate.Split(Separator);
            return String.Format("Stock {0}-{1}-{2} {3}.{4}.{5}", Date[0], Date[1], Date[2], DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

        }

        public override void GenerateReport()
        {
            if(BWFlag==true)
            {
                ReportBatchwise();
            }
            else
            {
                ReportWithoutBatch();
            }
        }
        private void ReportBatchwise()
        {
            
            List<StockReport> Stocks = new List<StockReport>();
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    if (this.Loctn != string.Empty)
                    {
                        fa.model.OrderManagement.InventoryLocation lLocatonId = HospitalInventoryManager.Instance.GetLocationByName(Loctn, Company.CompanyId);
                        if (lLocatonId != null)
                        {
                            Lid = lLocatonId.Id;
                            //Get Account year Begining Date;
                            var SelFYSD = (from fsd in Context.CompanyFinancialPeriods select fsd);

                            foreach (var fsd in SelFYSD)
                            {
                                dfd = fsd.Begin;
                            }

                            var SelPrdct = (from prd in Context.Products
                                            join inv in Context.Inventories on prd.Id equals inv.ProductId  //                                             where inv.CreatedDate >= this.FromDate && inv.CreatedDate <= this.ToDate && 
                                            where inv.InventoryLocationId == Lid
                                            group new { prd.MaterialId, inv } by prd into g
                                            select new
                                            {
                                                Mmid = g.Key.MaterialId,
                                                Nnam = g.Key.Name,
                                                Uuom = g.Key.UOM,
                                                Ppid = g.Key.Id,

                                            }).Distinct().ToList();

                            foreach (var Prdctlist in SelPrdct)
                            {

                                // CALCULATION OF OPENING STOCK
                                long OPSTK = 0L;
                                long POPSTK = 0L;
                                long PROPSTK = 0L;
                                long SOPSTK = 0L;
                                long SROPSTK = 0L;
                                long IOPSTK = 0L;
                                long OOPSTK = 0L;
                                long PCOPSTK = 0L;
                                long DOPSTK = 0L;
                                long CLOSTK = 0L;

                                // var batcel = (from ibatch in Context.InventoryBatches
                                //              where ibatch.ProductId == Ppid )

                                
                                var Peidq = (from pure in Context.PurchaseEntry
                                             where pure.InventoryLocationId == Lid
                                             select new { peid = pure.Id }).ToList();

                                foreach (var Purent in Peidq)
                                {
                                    var OPSPQqueryIL = (from prd in Context.Products
                                                        join inv in Context.Inventories on prd.Id equals inv.ProductId  //                                             where inv.CreatedDate >= this.FromDate && inv.CreatedDate <= this.ToDate && 
                                                        where inv.InventoryLocationId == Lid
                                                        join pur in Context.PurchaseDetails on prd.Id equals pur.ProductId
                                                        where pur.CreatedDate >= dfd && pur.CreatedDate < this.FromDate &&
                                                        pur.MaterialId == Prdctlist.Mmid && pur.PurchaseDetailsId == null &&
                                                        pur.PurchaseEntryId == Purent.peid
                                                        group new { prd.MaterialId, pur, } by prd into g
                                                        select new
                                                        {
                                                            OSSUMPURQTY = g.Sum(p => p.pur.Quantity)

                                                        }).Distinct().ToList();

                                    if (OPSPQqueryIL.Count > 0)
                                    {
                                        POPSTK += (long)OPSPQqueryIL.Sum(p => p.OSSUMPURQTY);
                                    }

                                    var OPSPRQqueryIL = (from prd in Context.Products
                                                         join inv in Context.Inventories on prd.Id equals inv.ProductId  //                                             where inv.CreatedDate >= this.FromDate && inv.CreatedDate <= this.ToDate && 
                                                         where inv.InventoryLocationId == Lid
                                                         join pur in Context.PurchaseDetails on prd.Id equals pur.ProductId
                                                         where pur.CreatedDate >= dfd && pur.CreatedDate < this.FromDate &&
                                                         pur.MaterialId == Prdctlist.Mmid && pur.PurchaseDetailsId != null &&
                                                         pur.PurchaseEntryId == Purent.peid
                                                         group new { prd.MaterialId, pur, } by prd into g
                                                         select new
                                                         {
                                                             OSSUMPURRQTY = g.Sum(p => p.pur.Quantity)

                                                         }).Distinct().ToList();
                                    if (OPSPRQqueryIL.Count > 0)
                                    {
                                        PROPSTK += (long)OPSPRQqueryIL.Sum(p => p.OSSUMPURRQTY);
                                    }
                                }

                                var Seidq = (from salent in Context.SaleEntry
                                             where salent.InventoryLocationId == Lid
                                             select new { seid = salent.Id }).ToList();

                                foreach (var Salent in Seidq)
                                {
                                    var OPSQqueryIL = (from prd in Context.Products
                                                       join inv in Context.Inventories on prd.Id equals inv.ProductId  //                                             where inv.CreatedDate >= this.FromDate && inv.CreatedDate <= this.ToDate && 
                                                       where inv.InventoryLocationId == Lid
                                                       join sal in Context.SaleDetail on prd.Id equals sal.ProductId
                                                       where sal.CreatedDate >= dfd && sal.CreatedDate < this.FromDate &&
                                                       sal.MaterialId == Prdctlist.Mmid && sal.SaleDetailId == null &&
                                                       sal.SaleId == Salent.seid
                                                       group new { prd.MaterialId, sal } by prd into g
                                                       select new
                                                       {
                                                           OSSUMSALQTY = g.Sum(s => s.sal.Quantity),
                                                       }).Distinct().ToList();
                                    if (OPSQqueryIL.Count > 0)
                                    {
                                        SOPSTK += (long)OPSQqueryIL.Sum(s => s.OSSUMSALQTY);
                                    }

                                    var OPSRQqueryIL = (from prd in Context.Products
                                                        join inv in Context.Inventories on prd.Id equals inv.ProductId  //                                             where inv.CreatedDate >= this.FromDate && inv.CreatedDate <= this.ToDate && 
                                                        where inv.InventoryLocationId == Lid
                                                        join sal in Context.SaleDetail on prd.Id equals sal.ProductId
                                                        where sal.CreatedDate >= dfd && sal.CreatedDate < this.FromDate &&
                                                        sal.MaterialId == Prdctlist.Mmid && sal.SaleDetailId != null &&
                                                        sal.SaleId == Salent.seid
                                                        group new { prd.MaterialId, sal } by prd into g
                                                        select new
                                                        {
                                                            OSSUMSALRQTY = g.Sum(s => s.sal.Quantity),

                                                        }).Distinct().ToList();
                                    if (OPSRQqueryIL.Count > 0)
                                    {
                                        SROPSTK += (long)OPSRQqueryIL.Sum(s => s.OSSUMSALRQTY);
                                    }
                                }

                                var Smeidq = (from sment in Context.StockMovement
                                              where sment.InventoryStockLocationId == Lid
                                              select new { smeid = sment.Id }).ToList();

                                foreach (var Sment in Smeidq)
                                {
                                    var OSSMIQqueryIL = (from prd in Context.Products
                                                         join inv in Context.Inventories on prd.Id equals inv.ProductId  //                                             where inv.CreatedDate >= this.FromDate && inv.CreatedDate <= this.ToDate && 
                                                         where inv.InventoryLocationId == Lid
                                                         join smv in Context.StockMovementDetail on prd.Id equals smv.ProductId
                                                         where smv.CreatedDate >= dfd && smv.CreatedDate < this.FromDate &&
                                                         smv.MaterialId == Prdctlist.Mmid && smv.StockMovement.Type == InventoryJournalType.STOCK_IN &&
                                                         smv.StockMovementId == Sment.smeid
                                                         group new { prd.MaterialId, smv } by prd into g
                                                         select new
                                                         {
                                                             OSSUMSMINQTY = g.Sum(s => s.smv.Quantity),

                                                         }).Distinct().ToList();
                                    if (OSSMIQqueryIL.Count > 0)
                                    {
                                        IOPSTK += (long)OSSMIQqueryIL.Sum(si => si.OSSUMSMINQTY);
                                    }

                                    var OSSMOQqueryIL = (from prd in Context.Products
                                                         join inv in Context.Inventories on prd.Id equals inv.ProductId  //                                             where inv.CreatedDate >= this.FromDate && inv.CreatedDate <= this.ToDate && 
                                                         where inv.InventoryLocationId == Lid
                                                         join smv in Context.StockMovementDetail on prd.Id equals smv.ProductId
                                                         where smv.CreatedDate >= dfd && smv.CreatedDate < this.FromDate &&
                                                         smv.MaterialId == Prdctlist.Mmid && smv.StockMovement.Type == InventoryJournalType.STOCK_OUT &&
                                                         smv.StockMovementId == Sment.smeid
                                                         group new { prd.MaterialId, smv } by prd into g
                                                         select new
                                                         {
                                                             OSSUMSMOUTQTY = g.Sum(s => s.smv.Quantity),

                                                         }).Distinct().ToList();
                                    if (OSSMOQqueryIL.Count > 0)
                                    {
                                        OOPSTK += (long)OSSMOQqueryIL.Sum(so => so.OSSUMSMOUTQTY);
                                    }

                                    var OSSMPQqueryIL = (from prd in Context.Products
                                                         join inv in Context.Inventories on prd.Id equals inv.ProductId  //                                             where inv.CreatedDate >= this.FromDate && inv.CreatedDate <= this.ToDate && 
                                                         where inv.InventoryLocationId == Lid
                                                         join smv in Context.StockMovementDetail on prd.Id equals smv.ProductId
                                                         where smv.CreatedDate >= dfd && smv.CreatedDate < this.FromDate &&
                                                         smv.MaterialId == Prdctlist.Mmid && smv.StockMovement.Type == InventoryJournalType.PATIENT_USE &&
                                                         smv.StockMovementId == Sment.smeid
                                                         group new { prd.MaterialId, smv } by prd into g
                                                         select new
                                                         {
                                                             OSSUMSMPQTY = g.Sum(s => s.smv.Quantity),

                                                         }).Distinct().ToList();
                                    if (OSSMPQqueryIL.Count > 0)
                                    {
                                        PCOPSTK += (long)OSSMPQqueryIL.Sum(pc => pc.OSSUMSMPQTY);
                                    }

                                    var OSSMDQqueryIL = (from prd in Context.Products
                                                         join inv in Context.Inventories on prd.Id equals inv.ProductId  //                                             where inv.CreatedDate >= this.FromDate && inv.CreatedDate <= this.ToDate && 
                                                         where inv.InventoryLocationId == Lid
                                                         join smv in Context.StockMovementDetail on prd.Id equals smv.ProductId
                                                         where smv.CreatedDate >= dfd && smv.CreatedDate < this.FromDate &&
                                                         smv.MaterialId == Prdctlist.Mmid && smv.StockMovement.Type == InventoryJournalType.DAMAGED &&
                                                         smv.StockMovementId == Sment.smeid
                                                         group new { prd.MaterialId, smv } by prd into g
                                                         select new
                                                         {
                                                             OSSUMSMDUTQTY = g.Sum(s => s.smv.Quantity),

                                                         }).Distinct().ToList();
                                    if (OSSMDQqueryIL.Count > 0)
                                    {
                                        DOPSTK += (long)OSSMDQqueryIL.Sum(sd => sd.OSSUMSMDUTQTY);
                                    }
                                }

                                var OPSK = (from inv in Context.Inventories
                                            where inv.ProductId == Prdctlist.Ppid && inv.InventoryLocationId == Lid
                                            select new
                                            {
                                                OPSTK = inv.OpeningStock,

                                            }).Distinct().ToList();

                                foreach (var oqty in OPSK)
                                {
                                    OPSTK = (long)oqty.OPSTK;
                                }

                                CLOSTK = (OPSTK + POPSTK - PROPSTK - SOPSTK + SROPSTK - OOPSTK + IOPSTK - PCOPSTK - DOPSTK);

                                //FETCHING DATA FOR QUANTITY CONSUMED AND CLOSING STOCK CALCULATION
                                long PurchaseQty = 0L;
                                long PQ = 0L;
                                long PurchaseRtnQty = 0L;
                                long PurFreeQty = 0L;
                                long SalesQty = 0L;
                                long SQ = 0L;
                                long SalesRtnQty = 0L;
                                long SalFreeQty = 0L;
                                long StockMovedInQty = 0L;
                                long StockMovedOutQty = 0L;
                                long PatientConsumedQty = 0L;
                                long DamagedQty = 0L;

                                
                                // CHECK FOR BATCH ENTRIES

                                var batcel = (from prd in Context.Products
                                              join ibatch in Context.InventoryBatches on prd.Id equals ibatch.ProductId
                                              where ibatch.ProductId == Prdctlist.Ppid
                                              join inv in Context.Inventories on prd.Id equals inv.ProductId
                                              group new { prd.MaterialId, ibatch } by ibatch into ig
                                              select new
                                              {
                                                  BatchNos = ig.Key.BatchNo,
                                                  BatchExp = ig.Key.ExpDate,
                                                  BatchPrdct = ig.Key.ProductId,
                                                  Batchinvid = ig.Key.InventoryId,
                                              }).Distinct().ToList();
                                // do work here
                                if (batcel.Count == 0)
                                {
                                    foreach (var Purent in Peidq)
                                    {

                                        var PQqueryIL = (from prd in Context.Products
                                                         join inv in Context.Inventories on prd.Id equals inv.ProductId  //                                             where inv.CreatedDate >= this.FromDate && inv.CreatedDate <= this.ToDate && 
                                                         where inv.InventoryLocationId == Lid
                                                         join pur in Context.PurchaseDetails on prd.Id equals pur.ProductId
                                                         where pur.CreatedDate >= this.FromDate && pur.CreatedDate <= this.ToDate &&
                                                         pur.MaterialId == Prdctlist.Mmid && pur.PurchaseDetailsId == null &&
                                                         pur.PurchaseEntryId == Purent.peid
                                                         group new { prd.MaterialId, pur, } by prd into g
                                                         select new
                                                         {
                                                             SUMPURQTY = g.Sum(p => p.pur.Quantity),
                                                             SUMFRPQTY = g.Sum(P => P.pur.FreeQuantity)

                                                         }).Distinct().ToList();
                                        if (PQqueryIL.Count > 0)
                                        {
                                            PurchaseQty += (long)PQqueryIL.Sum(p => p.SUMPURQTY);
                                            PurFreeQty += (long)PQqueryIL.Sum(p => p.SUMFRPQTY);
                                            PQ = PurchaseQty + PurFreeQty;
                                        }

                                        var PRQqueryIL = (from prd in Context.Products
                                                          join inv in Context.Inventories on prd.Id equals inv.ProductId  //                                             where inv.CreatedDate >= this.FromDate && inv.CreatedDate <= this.ToDate && 
                                                          where inv.InventoryLocationId == Lid
                                                          join pur in Context.PurchaseDetails on prd.Id equals pur.ProductId
                                                          where pur.CreatedDate >= this.FromDate && pur.CreatedDate <= this.ToDate &&
                                                          pur.MaterialId == Prdctlist.Mmid && pur.PurchaseDetailsId != null &&
                                                          pur.PurchaseEntryId == Purent.peid
                                                          group new { prd.MaterialId, pur, } by prd into g
                                                          select new
                                                          {
                                                              SUMPURRQTY = g.Sum(p => p.pur.Quantity)

                                                          }).Distinct().ToList();
                                        if (PRQqueryIL.Count > 0)
                                        {
                                            PurchaseRtnQty += (long)PRQqueryIL.Sum(pr => pr.SUMPURRQTY);
                                        }
                                    }

                                    foreach (var Salent in Seidq)
                                    {
                                        var SQqueryIL = (from prd in Context.Products
                                                         join inv in Context.Inventories on prd.Id equals inv.ProductId  //                                             where inv.CreatedDate >= this.FromDate && inv.CreatedDate <= this.ToDate && 
                                                         where inv.InventoryLocationId == Lid
                                                         join sal in Context.SaleDetail on prd.Id equals sal.ProductId
                                                         where sal.CreatedDate >= this.FromDate && sal.CreatedDate <= this.ToDate &&
                                                         sal.MaterialId == Prdctlist.Mmid && sal.SaleDetailId == null &&
                                                         sal.SaleId == Salent.seid
                                                         group new { prd.MaterialId, sal } by prd into g
                                                         select new
                                                         {
                                                             SUMSALQTY = g.Sum(s => s.sal.Quantity),
                                                             SUMFRSQTY = g.Sum(s => s.sal.FreeQuantity)

                                                         }).Distinct().ToList();
                                        if (SQqueryIL.Count > 0)
                                        {
                                            SalesQty += (long)SQqueryIL.Sum(s => s.SUMSALQTY);
                                            SalFreeQty += (long)SQqueryIL.Sum(s => s.SUMFRSQTY);
                                            SQ = SalesQty + SalFreeQty;

                                        }

                                        var SRQqueryIL = (from prd in Context.Products
                                                          join inv in Context.Inventories on prd.Id equals inv.ProductId  //                                             where inv.CreatedDate >= this.FromDate && inv.CreatedDate <= this.ToDate && 
                                                          where inv.InventoryLocationId == Lid
                                                          join sal in Context.SaleDetail on prd.Id equals sal.ProductId
                                                          where sal.CreatedDate >= this.FromDate && sal.CreatedDate <= this.ToDate &&
                                                          sal.MaterialId == Prdctlist.Mmid && sal.SaleDetailId != null &&
                                                          sal.SaleId == Salent.seid
                                                          group new { prd.MaterialId, sal } by prd into g
                                                          select new
                                                          {
                                                              SUMSALRQTY = g.Sum(s => s.sal.Quantity),

                                                          }).Distinct().ToList();
                                        if (SRQqueryIL.Count > 0)
                                        {
                                            SalesRtnQty += (long)SRQqueryIL.Sum(sr => sr.SUMSALRQTY);
                                        }

                                    }

                                    foreach (var Sment in Smeidq)
                                    {
                                        var SMIQqueryIL = (from prd in Context.Products
                                                           join inv in Context.Inventories on prd.Id equals inv.ProductId  //                                             where inv.CreatedDate >= this.FromDate && inv.CreatedDate <= this.ToDate && 
                                                           where inv.InventoryLocationId == Lid
                                                           join smv in Context.StockMovementDetail on prd.Id equals smv.ProductId
                                                           where smv.CreatedDate >= this.FromDate && smv.CreatedDate <= this.ToDate &&
                                                           smv.MaterialId == Prdctlist.Mmid && smv.StockMovement.Type == InventoryJournalType.STOCK_IN &&
                                                           smv.StockMovementId == Sment.smeid
                                                           group new { prd.MaterialId, smv } by prd into g
                                                           select new
                                                           {
                                                               SUMSMINQTY = g.Sum(s => s.smv.Quantity),

                                                           }).Distinct().ToList();

                                        if (SMIQqueryIL.Count > 0)
                                        {
                                            StockMovedInQty += (long)SMIQqueryIL.Sum(smi => smi.SUMSMINQTY);
                                        }

                                        var SMOQqueryIL = (from prd in Context.Products
                                                           join inv in Context.Inventories on prd.Id equals inv.ProductId  //                                             where inv.CreatedDate >= this.FromDate && inv.CreatedDate <= this.ToDate && 
                                                           where inv.InventoryLocationId == Lid
                                                           join smv in Context.StockMovementDetail on prd.Id equals smv.ProductId
                                                           where smv.CreatedDate >= this.FromDate && smv.CreatedDate <= this.ToDate &&
                                                           smv.MaterialId == Prdctlist.Mmid && smv.StockMovement.Type == InventoryJournalType.STOCK_OUT &&
                                                           smv.StockMovementId == Sment.smeid
                                                           group new { prd.MaterialId, smv } by prd into g
                                                           select new
                                                           {
                                                               SUMSMOUTQTY = g.Sum(s => s.smv.Quantity),

                                                           }).Distinct().ToList();

                                        if (SMOQqueryIL.Count > 0)
                                        {
                                            StockMovedOutQty += (long)SMOQqueryIL.Sum(smo => smo.SUMSMOUTQTY);
                                        }

                                        var SMPQqueryIL = (from prd in Context.Products
                                                           join inv in Context.Inventories on prd.Id equals inv.ProductId  //                                             where inv.CreatedDate >= this.FromDate && inv.CreatedDate <= this.ToDate && 
                                                           where inv.InventoryLocationId == Lid
                                                           join smv in Context.StockMovementDetail on prd.Id equals smv.ProductId
                                                           where smv.CreatedDate >= this.FromDate && smv.CreatedDate <= this.ToDate &&
                                                           smv.MaterialId == Prdctlist.Mmid && smv.StockMovement.Type == InventoryJournalType.PATIENT_USE &&
                                                           smv.StockMovementId == Sment.smeid
                                                           group new { prd.MaterialId, smv } by prd into g
                                                           select new
                                                           {
                                                               SUMSMPQTY = g.Sum(s => s.smv.Quantity),

                                                           }).Distinct().ToList();


                                        if (SMPQqueryIL.Count > 0)
                                        {
                                            PatientConsumedQty += (long)SMPQqueryIL.Sum(smp => smp.SUMSMPQTY);
                                        }

                                        var SMDQqueryIL = (from prd in Context.Products
                                                           join inv in Context.Inventories on prd.Id equals inv.ProductId  //                                             where inv.CreatedDate >= this.FromDate && inv.CreatedDate <= this.ToDate && 
                                                           where inv.InventoryLocationId == Lid
                                                           join smv in Context.StockMovementDetail on prd.Id equals smv.ProductId
                                                           where smv.CreatedDate >= this.FromDate && smv.CreatedDate <= this.ToDate &&
                                                           smv.MaterialId == Prdctlist.Mmid && smv.StockMovement.Type == InventoryJournalType.DAMAGED &&
                                                           smv.StockMovementId == Sment.smeid
                                                           group new { prd.MaterialId, smv } by prd into g
                                                           select new
                                                           {
                                                               SUMSMDUTQTY = g.Sum(s => s.smv.Quantity),

                                                           }).Distinct().ToList();

                                        if (SMDQqueryIL.Count > 0)
                                        {
                                            DamagedQty += (long)SMDQqueryIL.Sum(smd => smd.SUMSMDUTQTY);
                                        }
                                    }

                                    StockReport temp = new StockReport();

                                    temp.Batch = false;
                                    temp.MaterialId = Prdctlist.Mmid;
                                    temp.Name = Prdctlist.Nnam;
                                    temp.uom = Prdctlist.Uuom;
                                    temp.OpenStock = CLOSTK;
                                    temp.PurchaseQty = PQ; // PurchaseQty; 
                                    temp.PurchaseRtnQty = PurchaseRtnQty;
                                    temp.SalesQty = SQ; // SalesQty; 
                                    temp.SalesRtnQty = SalesRtnQty;
                                    temp.StockMovedInQty = StockMovedInQty;
                                    temp.StockMovedOutQty = StockMovedOutQty;
                                    temp.PatientConsumedQty = PatientConsumedQty;
                                    temp.DamagedQty = DamagedQty;

                                    Stocks.Add(temp);
                                }
                                else
                                {
                                    foreach (var Batchlist in batcel)
                                    {
                                        long BPurchaseQty = 0L;
                                        long BPQ = 0L;
                                        long BPurchaseRtnQty = 0L;
                                        long BPurFreeQty = 0L;
                                        long BSalesQty = 0L;
                                        long BSQ = 0L;
                                        long BSalesRtnQty = 0L;
                                        long BSalFreeQty = 0L;
                                        long BStockMovedInQty = 0L;
                                        long BStockMovedOutQty = 0L;
                                        long BPatientConsumedQty = 0L;
                                        long BDamagedQty = 0L;

                                        long BPOPSTK = 0L;
                                        long BPROPSTK = 0L;
                                        long BSOPSTK = 0L;
                                        long BSROPSTK = 0L;
                                        long BIOPSTK = 0L;
                                        long BOOPSTK = 0L;
                                        long BPCOPSTK = 0L;
                                        long BDOPSTK = 0L;
                                        long BCLOSTK = 0L;
                                        long BOPSTK = 0L;

                                        var BPeidq = (from pure in Context.InventoryBatches
                                                      where pure.BatchNo == Batchlist.BatchNos
                                                      select new { beid = pure.Id }).ToList();

                                        foreach (var Purent in BPeidq)
                                        {
                                            var OPSPQqueryIL = (from prd in Context.Products
                                                                join inv in Context.InventoryBatches on prd.Id equals inv.ProductId //                                             where inv.CreatedDate >= this.FromDate && inv.CreatedDate <= this.ToDate && 
                                                                where inv.Id == Purent.beid
                                                                join pur in Context.PurchaseDetails on prd.Id equals pur.ProductId
                                                                where pur.CreatedDate >= dfd && pur.CreatedDate < this.FromDate &&
                                                                pur.MaterialId == Prdctlist.Mmid && pur.PurchaseDetailsId == null &&
                                                                pur.BatchNo == Batchlist.BatchNos
                                                                group new { pur, inv.BatchNo } by prd into g
                                                                select new
                                                                {
                                                                    OSSUMPURQTY = g.Sum(p => p.pur.Quantity)

                                                                }).Distinct().ToList();

                                            if (OPSPQqueryIL.Count > 0)
                                            {
                                                BPOPSTK += (long)OPSPQqueryIL.Sum(p => p.OSSUMPURQTY);
                                            }

                                            var OPSPRQqueryIL = (from prd in Context.Products
                                                                 join inv in Context.InventoryBatches on prd.Id equals inv.ProductId //                                             where inv.CreatedDate >= this.FromDate && inv.CreatedDate <= this.ToDate && 
                                                                 where inv.Id == Batchlist.Batchinvid
                                                                 join pur in Context.PurchaseDetails on prd.Id equals pur.ProductId
                                                                 where pur.CreatedDate >= dfd && pur.CreatedDate < this.FromDate &&
                                                                 pur.MaterialId == Prdctlist.Mmid && pur.PurchaseDetailsId != null &&
                                                                 pur.BatchNo == Batchlist.BatchNos
                                                                 group new { prd.MaterialId, pur, } by prd into g
                                                                 select new
                                                                 {
                                                                     OSSUMPURRQTY = g.Sum(p => p.pur.Quantity)

                                                                 }).Distinct().ToList();
                                            if (OPSPRQqueryIL.Count > 0)
                                            {
                                                BPROPSTK += (long)OPSPRQqueryIL.Sum(p => p.OSSUMPURRQTY);
                                            }
                                        }

                                        foreach (var Salent in BPeidq)
                                        {
                                            var OPSQqueryIL = (from prd in Context.Products
                                                               join inv in Context.InventoryBatches on prd.Id equals inv.ProductId //                                             where inv.CreatedDate >= this.FromDate && inv.CreatedDate <= this.ToDate && 
                                                               where inv.Id == Salent.beid
                                                               join sal in Context.SaleDetail on prd.Id equals sal.ProductId
                                                               where sal.CreatedDate >= dfd && sal.CreatedDate < this.FromDate &&
                                                               sal.MaterialId == Prdctlist.Mmid && sal.SaleDetailId == null &&
                                                               sal.BatchNo == Batchlist.BatchNos
                                                               group new { prd.MaterialId, sal } by prd into g
                                                               select new
                                                               {
                                                                   OSSUMSALQTY = g.Sum(s => s.sal.Quantity),
                                                               }).Distinct().ToList();
                                            if (OPSQqueryIL.Count > 0)
                                            {
                                                BSOPSTK += (long)OPSQqueryIL.Sum(s => s.OSSUMSALQTY);
                                            }

                                            var OPSRQqueryIL = (from prd in Context.Products
                                                                join inv in Context.InventoryBatches on prd.Id equals inv.ProductId //                                             where inv.CreatedDate >= this.FromDate && inv.CreatedDate <= this.ToDate && 
                                                                where inv.Id == Salent.beid
                                                                join sal in Context.SaleDetail on prd.Id equals sal.ProductId
                                                                where sal.CreatedDate >= dfd && sal.CreatedDate < this.FromDate &&
                                                                sal.MaterialId == Prdctlist.Mmid && sal.SaleDetailId != null &&
                                                                sal.BatchNo == Batchlist.BatchNos
                                                                group new { prd.MaterialId, sal } by prd into g
                                                                select new
                                                                {
                                                                    OSSUMSALRQTY = g.Sum(s => s.sal.Quantity),

                                                                }).Distinct().ToList();
                                            if (OPSRQqueryIL.Count > 0)
                                            {
                                                BSROPSTK += (long)OPSRQqueryIL.Sum(s => s.OSSUMSALRQTY);
                                            }
                                        }

                                        foreach (var Sment in BPeidq)
                                        {
                                            var OSSMIQqueryIL = (from prd in Context.Products
                                                                 join inv in Context.InventoryBatches on prd.Id equals inv.ProductId //                                             where inv.CreatedDate >= this.FromDate && inv.CreatedDate <= this.ToDate && 
                                                                 where inv.Id == Batchlist.Batchinvid
                                                                 join smv in Context.StockMovementDetail on prd.Id equals smv.ProductId
                                                                 where smv.CreatedDate >= dfd && smv.CreatedDate < this.FromDate &&
                                                                 smv.MaterialId == Prdctlist.Mmid && smv.StockMovement.Type == InventoryJournalType.STOCK_IN &&
                                                                 smv.BatchNo == Batchlist.BatchNos
                                                                 group new { prd.MaterialId, smv } by prd into g
                                                                 select new
                                                                 {
                                                                     OSSUMSMINQTY = g.Sum(s => s.smv.Quantity),

                                                                 }).Distinct().ToList();
                                            if (OSSMIQqueryIL.Count > 0)
                                            {
                                                BIOPSTK += (long)OSSMIQqueryIL.Sum(si => si.OSSUMSMINQTY);
                                            }

                                            var OSSMOQqueryIL = (from prd in Context.Products
                                                                 join inv in Context.InventoryBatches on prd.Id equals inv.ProductId //                                             where inv.CreatedDate >= this.FromDate && inv.CreatedDate <= this.ToDate && 
                                                                 where inv.Id == Batchlist.Batchinvid
                                                                 join smv in Context.StockMovementDetail on prd.Id equals smv.ProductId
                                                                 where smv.CreatedDate >= dfd && smv.CreatedDate < this.FromDate &&
                                                                 smv.MaterialId == Prdctlist.Mmid && smv.StockMovement.Type == InventoryJournalType.STOCK_OUT &&
                                                                 smv.BatchNo == Batchlist.BatchNos
                                                                 group new { prd.MaterialId, smv } by prd into g
                                                                 select new
                                                                 {
                                                                     OSSUMSMOUTQTY = g.Sum(s => s.smv.Quantity),

                                                                 }).Distinct().ToList();
                                            if (OSSMOQqueryIL.Count > 0)
                                            {
                                                BOOPSTK += (long)OSSMOQqueryIL.Sum(so => so.OSSUMSMOUTQTY);
                                            }

                                            var OSSMPQqueryIL = (from prd in Context.Products
                                                                 join inv in Context.InventoryBatches on prd.Id equals inv.ProductId //                                             where inv.CreatedDate >= this.FromDate && inv.CreatedDate <= this.ToDate && 
                                                                 where inv.Id == Batchlist.Batchinvid
                                                                 join smv in Context.StockMovementDetail on prd.Id equals smv.ProductId
                                                                 where smv.CreatedDate >= dfd && smv.CreatedDate < this.FromDate &&
                                                                 smv.MaterialId == Prdctlist.Mmid && smv.StockMovement.Type == InventoryJournalType.PATIENT_USE &&
                                                                 smv.BatchNo == Batchlist.BatchNos
                                                                 group new { prd.MaterialId, smv } by prd into g
                                                                 select new
                                                                 {
                                                                     OSSUMSMPQTY = g.Sum(s => s.smv.Quantity),

                                                                 }).Distinct().ToList();
                                            if (OSSMPQqueryIL.Count > 0)
                                            {
                                                BPCOPSTK += (long)OSSMPQqueryIL.Sum(pc => pc.OSSUMSMPQTY);
                                            }

                                            var OSSMDQqueryIL = (from prd in Context.Products
                                                                 join inv in Context.InventoryBatches on prd.Id equals inv.ProductId //                                             where inv.CreatedDate >= this.FromDate && inv.CreatedDate <= this.ToDate && 
                                                                 where inv.Id == Batchlist.Batchinvid
                                                                 join smv in Context.StockMovementDetail on prd.Id equals smv.ProductId
                                                                 where smv.CreatedDate >= dfd && smv.CreatedDate < this.FromDate &&
                                                                 smv.MaterialId == Prdctlist.Mmid && smv.StockMovement.Type == InventoryJournalType.DAMAGED &&
                                                                 smv.BatchNo == Batchlist.BatchNos
                                                                 group new { prd.MaterialId, smv } by prd into g
                                                                 select new
                                                                 {
                                                                     OSSUMSMDUTQTY = g.Sum(s => s.smv.Quantity),

                                                                 }).Distinct().ToList();
                                            if (OSSMDQqueryIL.Count > 0)
                                            {
                                                BDOPSTK += (long)OSSMDQqueryIL.Sum(sd => sd.OSSUMSMDUTQTY);
                                            }
                                        }

                                        var BOPSK = (from inv in Context.InventoryBatches
                                                     where inv.ProductId == Prdctlist.Ppid && inv.BatchNo == Batchlist.BatchNos
                                                     select new
                                                     {
                                                         OPSTK = inv.OpeningStock,

                                                     }).Distinct().ToList();

                                        foreach (var oqty in BOPSK)
                                        {
                                            BOPSTK = (long)oqty.OPSTK;
                                        }

                                        BCLOSTK = (BOPSTK + BPOPSTK - BPROPSTK - BSOPSTK + BSROPSTK - BOOPSTK + BIOPSTK - BPCOPSTK - BDOPSTK);

                                        // PURCHASE SALES STOCK MOVEMENT BATCH WISE CALCULATION

                                        foreach (var Purent in BPeidq)
                                        {
                                            var PQqueryIL = (from prd in Context.Products
                                                             join inv in Context.InventoryBatches on prd.Id equals inv.ProductId  //                                             where inv.CreatedDate >= this.FromDate && inv.CreatedDate <= this.ToDate && 
                                                             where inv.Id == Purent.beid
                                                             join pur in Context.PurchaseDetails on prd.Id equals pur.ProductId
                                                             where pur.CreatedDate >= this.FromDate && pur.CreatedDate <= this.ToDate &&
                                                             pur.MaterialId == Prdctlist.Mmid && pur.PurchaseDetailsId == null &&
                                                             pur.BatchNo == Batchlist.BatchNos
                                                             group new { pur, inv.BatchNo } by prd into g
                                                             select new
                                                             {
                                                                 SUMPURQTY = g.Sum(p => p.pur.Quantity),
                                                                 SUMFRPQTY = g.Sum(P => P.pur.FreeQuantity)
                                                             }).Distinct().ToList();
                                            if (PQqueryIL.Count > 0)
                                            {
                                                BPurchaseQty += (long)PQqueryIL.Sum(p => p.SUMPURQTY);
                                                BPurFreeQty += (long)PQqueryIL.Sum(p => p.SUMFRPQTY);
                                                BPQ = BPurchaseQty + BPurFreeQty;
                                            }

                                            var PRQqueryIL = (from prd in Context.Products
                                                              join inv in Context.InventoryBatches on prd.Id equals inv.ProductId  //                                             where inv.CreatedDate >= this.FromDate && inv.CreatedDate <= this.ToDate && 
                                                              where inv.Id == Purent.beid
                                                              join pur in Context.PurchaseDetails on prd.Id equals pur.ProductId
                                                              where pur.CreatedDate >= this.FromDate && pur.CreatedDate <= this.ToDate &&
                                                              pur.MaterialId == Prdctlist.Mmid && pur.PurchaseDetailsId != null &&
                                                              pur.BatchNo == Batchlist.BatchNos
                                                              group new { prd.MaterialId, pur, } by prd into g
                                                              select new
                                                              {
                                                                  SUMPURRQTY = g.Sum(p => p.pur.Quantity)

                                                              }).Distinct().ToList();
                                            if (PRQqueryIL.Count > 0)
                                            {
                                                BPurchaseRtnQty += (long)PRQqueryIL.Sum(pr => pr.SUMPURRQTY);
                                            }
                                            
                                            foreach (var Salent in BPeidq)
                                            {
                                                var SQqueryIL = (from prd in Context.Products
                                                                 join inv in Context.InventoryBatches on prd.Id equals inv.ProductId  //                                             where inv.CreatedDate >= this.FromDate && inv.CreatedDate <= this.ToDate && 
                                                                 where inv.Id == Salent.beid
                                                                 join sal in Context.SaleDetail on prd.Id equals sal.ProductId
                                                                 where sal.CreatedDate >= this.FromDate && sal.CreatedDate <= this.ToDate &&
                                                                 sal.MaterialId == Prdctlist.Mmid && sal.SaleDetailId == null &&
                                                                 sal.BatchNo == Batchlist.BatchNos
                                                                 group new { prd.MaterialId, sal } by prd into g
                                                                 select new
                                                                 {
                                                                     SUMSALQTY = g.Sum(s => s.sal.Quantity),
                                                                     SUMFRSQTY = g.Sum(s => s.sal.FreeQuantity)

                                                                 }).Distinct().ToList();
                                                if (SQqueryIL.Count > 0)
                                                {
                                                    BSalesQty += (long)SQqueryIL.Sum(s => s.SUMSALQTY);
                                                    BSalFreeQty += (long)SQqueryIL.Sum(s => s.SUMFRSQTY);
                                                    BSQ = SalesQty + SalFreeQty;

                                                }

                                                var SRQqueryIL = (from prd in Context.Products
                                                                  join inv in Context.InventoryBatches on prd.Id equals inv.ProductId  //                                             where inv.CreatedDate >= this.FromDate && inv.CreatedDate <= this.ToDate && 
                                                                  where inv.Id == Salent.beid
                                                                  join sal in Context.SaleDetail on prd.Id equals sal.ProductId
                                                                  where sal.CreatedDate >= this.FromDate && sal.CreatedDate <= this.ToDate &&
                                                                  sal.MaterialId == Prdctlist.Mmid && sal.SaleDetailId != null &&
                                                                  sal.SaleId == Salent.beid
                                                                  group new { prd.MaterialId, sal } by prd into g
                                                                  select new
                                                                  {
                                                                      SUMSALRQTY = g.Sum(s => s.sal.Quantity),

                                                                  }).Distinct().ToList();
                                                if (SRQqueryIL.Count > 0)
                                                {
                                                    BSalesRtnQty += (long)SRQqueryIL.Sum(sr => sr.SUMSALRQTY);
                                                }

                                                foreach (var Sment in BPeidq)
                                                {
                                                    var SMIQqueryIL = (from prd in Context.Products
                                                                       join inv in Context.InventoryBatches on prd.Id equals inv.ProductId  //                                             where inv.CreatedDate >= this.FromDate && inv.CreatedDate <= this.ToDate && 
                                                                       where inv.Id == Salent.beid
                                                                       join smv in Context.StockMovementDetail on prd.Id equals smv.ProductId
                                                                       where smv.CreatedDate >= this.FromDate && smv.CreatedDate <= this.ToDate &&
                                                                       smv.MaterialId == Prdctlist.Mmid && smv.StockMovement.Type == InventoryJournalType.STOCK_IN &&
                                                                       smv.StockMovementId == Sment.beid
                                                                       group new { prd.MaterialId, smv } by prd into g
                                                                       select new
                                                                       {
                                                                           SUMSMINQTY = g.Sum(s => s.smv.Quantity),

                                                                       }).Distinct().ToList();

                                                    if (SMIQqueryIL.Count > 0)
                                                    {
                                                        BStockMovedInQty += (long)SMIQqueryIL.Sum(smi => smi.SUMSMINQTY);
                                                    }

                                                    var SMOQqueryIL = (from prd in Context.Products
                                                                       join inv in Context.InventoryBatches on prd.Id equals inv.ProductId  //                                             where inv.CreatedDate >= this.FromDate && inv.CreatedDate <= this.ToDate && 
                                                                       where inv.Id == Salent.beid
                                                                       join smv in Context.StockMovementDetail on prd.Id equals smv.ProductId
                                                                       where smv.CreatedDate >= this.FromDate && smv.CreatedDate <= this.ToDate &&
                                                                       smv.MaterialId == Prdctlist.Mmid && smv.StockMovement.Type == InventoryJournalType.STOCK_OUT &&
                                                                       smv.StockMovementId == Sment.beid
                                                                       group new { prd.MaterialId, smv } by prd into g
                                                                       select new
                                                                       {
                                                                           SUMSMOUTQTY = g.Sum(s => s.smv.Quantity),

                                                                       }).Distinct().ToList();

                                                    if (SMOQqueryIL.Count > 0)
                                                    {
                                                        BStockMovedOutQty += (long)SMOQqueryIL.Sum(smo => smo.SUMSMOUTQTY);
                                                    }

                                                    var SMPQqueryIL = (from prd in Context.Products
                                                                       join inv in Context.InventoryBatches on prd.Id equals inv.ProductId  //                                             where inv.CreatedDate >= this.FromDate && inv.CreatedDate <= this.ToDate && 
                                                                       where inv.Id == Salent.beid
                                                                       join smv in Context.StockMovementDetail on prd.Id equals smv.ProductId
                                                                       where smv.CreatedDate >= this.FromDate && smv.CreatedDate <= this.ToDate &&
                                                                       smv.MaterialId == Prdctlist.Mmid && smv.StockMovement.Type == InventoryJournalType.PATIENT_USE &&
                                                                       smv.StockMovementId == Sment.beid
                                                                       group new { prd.MaterialId, smv } by prd into g
                                                                       select new
                                                                       {
                                                                           SUMSMPQTY = g.Sum(s => s.smv.Quantity),

                                                                       }).Distinct().ToList();


                                                    if (SMPQqueryIL.Count > 0)
                                                    {
                                                        BPatientConsumedQty += (long)SMPQqueryIL.Sum(smp => smp.SUMSMPQTY);
                                                    }

                                                    var SMDQqueryIL = (from prd in Context.Products
                                                                       join inv in Context.InventoryBatches on prd.Id equals inv.ProductId  //                                             where inv.CreatedDate >= this.FromDate && inv.CreatedDate <= this.ToDate && 
                                                                       where inv.Id == Salent.beid
                                                                       join smv in Context.StockMovementDetail on prd.Id equals smv.ProductId
                                                                       where smv.CreatedDate >= this.FromDate && smv.CreatedDate <= this.ToDate &&
                                                                       smv.MaterialId == Prdctlist.Mmid && smv.StockMovement.Type == InventoryJournalType.DAMAGED &&
                                                                       smv.StockMovementId == Sment.beid
                                                                       group new { prd.MaterialId, smv } by prd into g
                                                                       select new
                                                                       {
                                                                           SUMSMDUTQTY = g.Sum(s => s.smv.Quantity),

                                                                       }).Distinct().ToList();

                                                    if (SMDQqueryIL.Count > 0)
                                                    {
                                                        BDamagedQty += (long)SMDQqueryIL.Sum(smd => smd.SUMSMDUTQTY);
                                                    }
                                                }
                                                    
                                                StockReport temps = new StockReport();

                                                temps.Batch = true;
                                                temps.MaterialId = Prdctlist.Mmid;
                                                temps.Name = Prdctlist.Nnam;
                                                temps.uom = Prdctlist.Uuom;
                                                temps.BatchNos = Batchlist.BatchNos;
                                                temps.ExpDate = Batchlist.BatchExp;
                                                temps.OpenStock = BCLOSTK;
                                                temps.PurchaseQty = BPQ; // BPurchaseQty; 
                                                temps.PurchaseRtnQty = BPurchaseRtnQty;
                                                temps.SalesQty = BSQ; // BSalesQty; 
                                                temps.SalesRtnQty = BSalesRtnQty;
                                                temps.StockMovedInQty = BStockMovedInQty;
                                                temps.StockMovedOutQty = BStockMovedOutQty;
                                                temps.PatientConsumedQty = BPatientConsumedQty;
                                                temps.DamagedQty = BDamagedQty;

                                                Stocks.Add(temps);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    /* START OF ORIGINAL
                     // mysql = "SELECT products.productname, products.opnstock, (SELECT Sum(purchaseentry.qty) AS Pqty FROM purchaseentry WHERE products.productname=purchaseentry.productname AND purchaseentry.bdate <= CDate('" & d1 & "')) AS Expr1, (SELECT Sum(salesrtnentry.qty) AS Srqty FROM salesrtnentry WHERE products.productname = salesrtnentry.productname AND salesrtnentry.bdate <= Cdate('" & d1 & "')) AS Expr2, (SELECT Sum(salesentry.qty) AS Sqty FROM salesentry WHERE products.productname=salesentry.productname AND salesentry.bdate <= Cdate('" & d1 & "')) AS Expr3, (SELECT Sum(purchasrtneentry.qty) AS Prqty FROM purchasrtneentry WHERE products.productname=purchasrtneentry.productname AND purchasrtneentry.bdate < Cdate('" & d1 & "')) AS Expr4 FROM products)"
                      var query = (from s in Context.Products
                                 join cs in Context.SaleDetail on s.Id equals cs.ProductId
                                 join os in Context.PurchaseDetails on s.Id equals os.ProductId
                                 where cs.CreatedDate >= this.FromDate && cs.CreatedDate <= this.ToDate &&
                                 os.CreatedDate >= this.FromDate && os.CreatedDate <= this.ToDate
                                 select new
                                 {
                                     s.Name,
                                     s.MaterialId,
                                     SD = cs.CreatedDate,
                                     SQ = cs.Quantity,
                                     PD = os.CreatedDate,
                                     PQ = os.Quantity
                                 }).ToList();

                    //var result = (from prd in Context.Products
                    //              join sal in Context.SaleDetail
                    //              on prd.Id equals sal.ProductId
                    //              join pur in Context.PurchaseDetails
                    //              on prd.Id equals pur.ProductId                                  
                    //              select new
                    //              {
                    //                  prd.Name,
                    //                  prd.MaterialId,
                    //                  sq = sal.Quantity,
                    //                  SD = sal.CreatedDate,
                    //                  PD = pur.CreatedDate,
                    //                  pq = pur.Quantity
                    //         }).ToList(); //.Distinct()

                    foreach (var vnlist in query)
                    {
                        StockReport temp = new StockReport();
                        temp.MaterialId = vnlist.MaterialId;
                        temp.SalesQty = vnlist.SQ;
                        temp.SDate = vnlist.SD.Value;
                        temp.PDate = vnlist.PD.Value;
                        temp.PurchaseQty = vnlist.PQ;
                        temp.Name = vnlist.Name;

                        Stocks.Add(temp);
                    } * this is END OF ORGINAL */
                }
            }
            if (Stocks != null && Stocks.Count > 0)
            {
                foreach (StockReport Item in Stocks)
                {
                    StockReportLine LineItem = new StockReportLine(Item);
                    LineStocks.Add(LineItem);
                }
            }
        }

        private void ReportWithoutBatch()
        {
        
            List<StockReport> Stocks = new List<StockReport>();
            using (AccountMasterContext Context = new AccountMasterContext())
            {
                using (var dbContextTransaction = Context.Database.BeginTransaction())
                {
                    if (this.Loctn != string.Empty)
                    {
                        fa.model.OrderManagement.InventoryLocation lLocatonId = HospitalInventoryManager.Instance.GetLocationByName(Loctn, Company.CompanyId);
                        if (lLocatonId != null)
                        {
                            Lid = lLocatonId.Id;

                            // Get Accounting Year Beginig date

                            var SelFYSD = (from fsd in Context.CompanyFinancialPeriods select fsd);

                            foreach (var fsd in SelFYSD)
                            {
                                dfd = fsd.Begin;
                            }

                            var SelPrdct = (from prd in Context.Products
                                            join inv in Context.Inventories on prd.Id equals inv.ProductId  //                                             where inv.CreatedDate >= this.FromDate && inv.CreatedDate <= this.ToDate && 
                                            where inv.InventoryLocationId == Lid
                                            group new { prd.MaterialId, inv } by prd into g
                                            select new
                                            {
                                                Mmid = g.Key.MaterialId,
                                                Nnam = g.Key.Name,
                                                Uuom = g.Key.UOM,
                                                Ppid = g.Key.Id,

                                            }).Distinct().ToList();

                            foreach (var Prdctlist in SelPrdct)
                            {

                                // CALCULATION OF OPENING STOCK
                                long OPSTK = 0L;
                                long POPSTK = 0L;
                                long PROPSTK = 0L;
                                long SOPSTK = 0L;
                                long SROPSTK = 0L;
                                long IOPSTK = 0L;
                                long OOPSTK = 0L;
                                long PCOPSTK = 0L;
                                long DOPSTK = 0L;
                                long CLOSTK = 0L;

                                var Peidq = (from pure in Context.PurchaseEntry
                                             where pure.InventoryLocationId == Lid
                                             select new { peid = pure.Id }).ToList();

                                foreach (var Purent in Peidq)
                                {
                                    var OPSPQqueryIL = (from prd in Context.Products
                                                        join inv in Context.Inventories on prd.Id equals inv.ProductId  //                                             where inv.CreatedDate >= this.FromDate && inv.CreatedDate <= this.ToDate && 
                                                        where inv.InventoryLocationId == Lid
                                                        join pur in Context.PurchaseDetails on prd.Id equals pur.ProductId
                                                        where pur.CreatedDate >= dfd && pur.CreatedDate < this.FromDate &&
                                                        pur.MaterialId == Prdctlist.Mmid && pur.PurchaseDetailsId == null &&
                                                        pur.PurchaseEntryId == Purent.peid
                                                        group new { prd.MaterialId, pur, } by prd into g
                                                        select new
                                                        {
                                                            OSSUMPURQTY = g.Sum(p => p.pur.Quantity)

                                                        }).Distinct().ToList();

                                    if (OPSPQqueryIL.Count > 0)
                                    {
                                        POPSTK += (long)OPSPQqueryIL.Sum(p => p.OSSUMPURQTY);
                                    }

                                    var OPSPRQqueryIL = (from prd in Context.Products
                                                         join inv in Context.Inventories on prd.Id equals inv.ProductId  //                                             where inv.CreatedDate >= this.FromDate && inv.CreatedDate <= this.ToDate && 
                                                         where inv.InventoryLocationId == Lid
                                                         join pur in Context.PurchaseDetails on prd.Id equals pur.ProductId
                                                         where pur.CreatedDate >= dfd && pur.CreatedDate < this.FromDate &&
                                                         pur.MaterialId == Prdctlist.Mmid && pur.PurchaseDetailsId != null &&
                                                         pur.PurchaseEntryId == Purent.peid
                                                         group new { prd.MaterialId, pur, } by prd into g
                                                         select new
                                                         {
                                                             OSSUMPURRQTY = g.Sum(p => p.pur.Quantity)

                                                         }).Distinct().ToList();
                                    if (OPSPRQqueryIL.Count > 0)
                                    {
                                        PROPSTK += (long)OPSPRQqueryIL.Sum(p => p.OSSUMPURRQTY);
                                    }
                                }

                                var Seidq = (from salent in Context.SaleEntry
                                             where salent.InventoryLocationId == Lid
                                             select new { seid = salent.Id }).ToList();

                                foreach (var Salent in Seidq)
                                {
                                    var OPSQqueryIL = (from prd in Context.Products
                                                       join inv in Context.Inventories on prd.Id equals inv.ProductId  //                                             where inv.CreatedDate >= this.FromDate && inv.CreatedDate <= this.ToDate && 
                                                       where inv.InventoryLocationId == Lid
                                                       join sal in Context.SaleDetail on prd.Id equals sal.ProductId
                                                       where sal.CreatedDate >= dfd && sal.CreatedDate < this.FromDate &&
                                                       sal.MaterialId == Prdctlist.Mmid && sal.SaleDetailId == null &&
                                                       sal.SaleId == Salent.seid
                                                       group new { prd.MaterialId, sal } by prd into g
                                                       select new
                                                       {
                                                           OSSUMSALQTY = g.Sum(s => s.sal.Quantity),
                                                       }).Distinct().ToList();
                                    if (OPSQqueryIL.Count > 0)
                                    {
                                        SOPSTK += (long)OPSQqueryIL.Sum(s => s.OSSUMSALQTY);
                                    }

                                    var OPSRQqueryIL = (from prd in Context.Products
                                                        join inv in Context.Inventories on prd.Id equals inv.ProductId  //                                             where inv.CreatedDate >= this.FromDate && inv.CreatedDate <= this.ToDate && 
                                                        where inv.InventoryLocationId == Lid
                                                        join sal in Context.SaleDetail on prd.Id equals sal.ProductId
                                                        where sal.CreatedDate >= dfd && sal.CreatedDate < this.FromDate &&
                                                        sal.MaterialId == Prdctlist.Mmid && sal.SaleDetailId != null &&
                                                        sal.SaleId == Salent.seid
                                                        group new { prd.MaterialId, sal } by prd into g
                                                        select new
                                                        {
                                                            OSSUMSALRQTY = g.Sum(s => s.sal.Quantity),

                                                        }).Distinct().ToList();
                                    if (OPSRQqueryIL.Count > 0)
                                    {
                                        SROPSTK += (long)OPSRQqueryIL.Sum(s => s.OSSUMSALRQTY);
                                    }
                                }

                                var Smeidq = (from sment in Context.StockMovement
                                              where sment.InventoryStockLocationId == Lid
                                              select new { smeid = sment.Id }).ToList();

                                foreach (var Sment in Smeidq)
                                {
                                    var OSSMIQqueryIL = (from prd in Context.Products
                                                         join inv in Context.Inventories on prd.Id equals inv.ProductId  //                                             where inv.CreatedDate >= this.FromDate && inv.CreatedDate <= this.ToDate && 
                                                         where inv.InventoryLocationId == Lid
                                                         join smv in Context.StockMovementDetail on prd.Id equals smv.ProductId
                                                         where smv.CreatedDate >= dfd && smv.CreatedDate < this.FromDate &&
                                                         smv.MaterialId == Prdctlist.Mmid && smv.StockMovement.Type == InventoryJournalType.STOCK_IN &&
                                                         smv.StockMovementId == Sment.smeid
                                                         group new { prd.MaterialId, smv } by prd into g
                                                         select new
                                                         {
                                                             OSSUMSMINQTY = g.Sum(s => s.smv.Quantity),

                                                         }).Distinct().ToList();
                                    if (OSSMIQqueryIL.Count > 0)
                                    {
                                        IOPSTK += (long)OSSMIQqueryIL.Sum(si => si.OSSUMSMINQTY);
                                    }

                                    var OSSMOQqueryIL = (from prd in Context.Products
                                                         join inv in Context.Inventories on prd.Id equals inv.ProductId  //                                             where inv.CreatedDate >= this.FromDate && inv.CreatedDate <= this.ToDate && 
                                                         where inv.InventoryLocationId == Lid
                                                         join smv in Context.StockMovementDetail on prd.Id equals smv.ProductId
                                                         where smv.CreatedDate >= dfd && smv.CreatedDate < this.FromDate &&
                                                         smv.MaterialId == Prdctlist.Mmid && smv.StockMovement.Type == InventoryJournalType.STOCK_OUT &&
                                                         smv.StockMovementId == Sment.smeid
                                                         group new { prd.MaterialId, smv } by prd into g
                                                         select new
                                                         {
                                                             OSSUMSMOUTQTY = g.Sum(s => s.smv.Quantity),

                                                         }).Distinct().ToList();
                                    if (OSSMOQqueryIL.Count > 0)
                                    {
                                        OOPSTK += (long)OSSMOQqueryIL.Sum(so => so.OSSUMSMOUTQTY);
                                    }

                                    var OSSMPQqueryIL = (from prd in Context.Products
                                                         join inv in Context.Inventories on prd.Id equals inv.ProductId  //                                             where inv.CreatedDate >= this.FromDate && inv.CreatedDate <= this.ToDate && 
                                                         where inv.InventoryLocationId == Lid
                                                         join smv in Context.StockMovementDetail on prd.Id equals smv.ProductId
                                                         where smv.CreatedDate >= dfd && smv.CreatedDate < this.FromDate &&
                                                         smv.MaterialId == Prdctlist.Mmid && smv.StockMovement.Type == InventoryJournalType.PATIENT_USE &&
                                                         smv.StockMovementId == Sment.smeid
                                                         group new { prd.MaterialId, smv } by prd into g
                                                         select new
                                                         {
                                                             OSSUMSMPQTY = g.Sum(s => s.smv.Quantity),

                                                         }).Distinct().ToList();
                                    if (OSSMPQqueryIL.Count > 0)
                                    {
                                        PCOPSTK += (long)OSSMPQqueryIL.Sum(pc => pc.OSSUMSMPQTY);
                                    }

                                    var OSSMDQqueryIL = (from prd in Context.Products
                                                         join inv in Context.Inventories on prd.Id equals inv.ProductId  //                                             where inv.CreatedDate >= this.FromDate && inv.CreatedDate <= this.ToDate && 
                                                         where inv.InventoryLocationId == Lid
                                                         join smv in Context.StockMovementDetail on prd.Id equals smv.ProductId
                                                         where smv.CreatedDate >= dfd && smv.CreatedDate < this.FromDate &&
                                                         smv.MaterialId == Prdctlist.Mmid && smv.StockMovement.Type == InventoryJournalType.DAMAGED &&
                                                         smv.StockMovementId == Sment.smeid
                                                         group new { prd.MaterialId, smv } by prd into g
                                                         select new
                                                         {
                                                             OSSUMSMDUTQTY = g.Sum(s => s.smv.Quantity),

                                                         }).Distinct().ToList();
                                    if (OSSMDQqueryIL.Count > 0)
                                    {
                                        DOPSTK += (long)OSSMDQqueryIL.Sum(sd => sd.OSSUMSMDUTQTY);
                                    }
                                }

                                var OPSK = (from inv in Context.Inventories
                                            where inv.ProductId == Prdctlist.Ppid && inv.InventoryLocationId == Lid
                                            select new
                                            {
                                                OPSTK = inv.OpeningStock,

                                            }).Distinct().ToList();

                                foreach (var oqty in OPSK)
                                {
                                    OPSTK = (long)oqty.OPSTK;
                                }

                                CLOSTK = (OPSTK + POPSTK - PROPSTK - SOPSTK + SROPSTK - OOPSTK + IOPSTK - PCOPSTK - DOPSTK);

                                //FETCHING DATA FOR QUANTITY CONSUMED AND CLOSING STOCK CALCULATION
                                long PurchaseQty = 0L;
                                long PQ = 0L;
                                long PurchaseRtnQty = 0L;
                                long PurFreeQty = 0L;
                                long SalesQty = 0L;
                                long SQ = 0L;
                                long SalesRtnQty = 0L;
                                long SalFreeQty = 0L;
                                long StockMovedInQty = 0L;
                                long StockMovedOutQty = 0L;
                                long PatientConsumedQty = 0L;
                                long DamagedQty = 0L;

                                foreach (var Purent in Peidq)
                                {
                                    var PQqueryIL = (from prd in Context.Products
                                                     join inv in Context.Inventories on prd.Id equals inv.ProductId  //                                             where inv.CreatedDate >= this.FromDate && inv.CreatedDate <= this.ToDate && 
                                                     where inv.InventoryLocationId == Lid
                                                     join pur in Context.PurchaseDetails on prd.Id equals pur.ProductId
                                                     where pur.CreatedDate >= this.FromDate && pur.CreatedDate <= this.ToDate &&
                                                     pur.MaterialId == Prdctlist.Mmid && pur.PurchaseDetailsId == null &&
                                                     pur.PurchaseEntryId == Purent.peid
                                                     group new { prd.MaterialId, pur, } by prd into g
                                                     select new
                                                     {
                                                         SUMPURQTY = g.Sum(p => p.pur.Quantity),
                                                         SUMFRPQTY = g.Sum(P => P.pur.FreeQuantity)

                                                     }).Distinct().ToList();
                                    if (PQqueryIL.Count > 0)
                                    {
                                        PurchaseQty += (long)PQqueryIL.Sum(p => p.SUMPURQTY);
                                        PurFreeQty += (long)PQqueryIL.Sum(p => p.SUMFRPQTY);
                                        PQ = PurchaseQty + PurFreeQty;
                                    }

                                    var PRQqueryIL = (from prd in Context.Products
                                                      join inv in Context.Inventories on prd.Id equals inv.ProductId  //                                             where inv.CreatedDate >= this.FromDate && inv.CreatedDate <= this.ToDate && 
                                                      where inv.InventoryLocationId == Lid
                                                      join pur in Context.PurchaseDetails on prd.Id equals pur.ProductId
                                                      where pur.CreatedDate >= this.FromDate && pur.CreatedDate <= this.ToDate &&
                                                      pur.MaterialId == Prdctlist.Mmid && pur.PurchaseDetailsId != null &&
                                                      pur.PurchaseEntryId == Purent.peid
                                                      group new { prd.MaterialId, pur, } by prd into g
                                                      select new
                                                      {
                                                          SUMPURRQTY = g.Sum(p => p.pur.Quantity)

                                                      }).Distinct().ToList();
                                    if (PRQqueryIL.Count > 0)
                                    {
                                        PurchaseRtnQty += (long)PRQqueryIL.Sum(pr => pr.SUMPURRQTY);
                                    }
                                }

                                foreach (var Salent in Seidq)
                                {
                                    var SQqueryIL = (from prd in Context.Products
                                                     join inv in Context.Inventories on prd.Id equals inv.ProductId  //                                             where inv.CreatedDate >= this.FromDate && inv.CreatedDate <= this.ToDate && 
                                                     where inv.InventoryLocationId == Lid
                                                     join sal in Context.SaleDetail on prd.Id equals sal.ProductId
                                                     where sal.CreatedDate >= this.FromDate && sal.CreatedDate <= this.ToDate &&
                                                     sal.MaterialId == Prdctlist.Mmid && sal.SaleDetailId == null &&
                                                     sal.SaleId == Salent.seid
                                                     group new { prd.MaterialId, sal } by prd into g
                                                     select new
                                                     {
                                                         SUMSALQTY = g.Sum(s => s.sal.Quantity),
                                                         SUMFRSQTY = g.Sum(s => s.sal.FreeQuantity)

                                                     }).Distinct().ToList();
                                    if (SQqueryIL.Count > 0)
                                    {
                                        SalesQty += (long)SQqueryIL.Sum(s => s.SUMSALQTY);
                                        SalFreeQty += (long)SQqueryIL.Sum(s => s.SUMFRSQTY);
                                        SQ = SalesQty + SalFreeQty;

                                    }

                                    var SRQqueryIL = (from prd in Context.Products
                                                      join inv in Context.Inventories on prd.Id equals inv.ProductId  //                                             where inv.CreatedDate >= this.FromDate && inv.CreatedDate <= this.ToDate && 
                                                      where inv.InventoryLocationId == Lid
                                                      join sal in Context.SaleDetail on prd.Id equals sal.ProductId
                                                      where sal.CreatedDate >= this.FromDate && sal.CreatedDate <= this.ToDate &&
                                                      sal.MaterialId == Prdctlist.Mmid && sal.SaleDetailId != null &&
                                                      sal.SaleId == Salent.seid
                                                      group new { prd.MaterialId, sal } by prd into g
                                                      select new
                                                      {
                                                          SUMSALRQTY = g.Sum(s => s.sal.Quantity),

                                                      }).Distinct().ToList();
                                    if (SRQqueryIL.Count > 0)
                                    {
                                        SalesRtnQty += (long)SRQqueryIL.Sum(sr => sr.SUMSALRQTY);
                                    }

                                }

                                foreach (var Sment in Smeidq)
                                {
                                    var SMIQqueryIL = (from prd in Context.Products
                                                       join inv in Context.Inventories on prd.Id equals inv.ProductId  //                                             where inv.CreatedDate >= this.FromDate && inv.CreatedDate <= this.ToDate && 
                                                       where inv.InventoryLocationId == Lid
                                                       join smv in Context.StockMovementDetail on prd.Id equals smv.ProductId
                                                       where smv.CreatedDate >= this.FromDate && smv.CreatedDate <= this.ToDate &&
                                                       smv.MaterialId == Prdctlist.Mmid && smv.StockMovement.Type == InventoryJournalType.STOCK_IN &&
                                                       smv.StockMovementId == Sment.smeid
                                                       group new { prd.MaterialId, smv } by prd into g
                                                       select new
                                                       {
                                                           SUMSMINQTY = g.Sum(s => s.smv.Quantity),

                                                       }).Distinct().ToList();

                                    if (SMIQqueryIL.Count > 0)
                                    {
                                        StockMovedInQty += (long)SMIQqueryIL.Sum(smi => smi.SUMSMINQTY);
                                    }

                                    var SMOQqueryIL = (from prd in Context.Products
                                                       join inv in Context.Inventories on prd.Id equals inv.ProductId  //                                             where inv.CreatedDate >= this.FromDate && inv.CreatedDate <= this.ToDate && 
                                                       where inv.InventoryLocationId == Lid
                                                       join smv in Context.StockMovementDetail on prd.Id equals smv.ProductId
                                                       where smv.CreatedDate >= this.FromDate && smv.CreatedDate <= this.ToDate &&
                                                       smv.MaterialId == Prdctlist.Mmid && smv.StockMovement.Type == InventoryJournalType.STOCK_OUT &&
                                                       smv.StockMovementId == Sment.smeid
                                                       group new { prd.MaterialId, smv } by prd into g
                                                       select new
                                                       {
                                                           SUMSMOUTQTY = g.Sum(s => s.smv.Quantity),

                                                       }).Distinct().ToList();

                                    if (SMOQqueryIL.Count > 0)
                                    {
                                        StockMovedOutQty += (long)SMOQqueryIL.Sum(smo => smo.SUMSMOUTQTY);
                                    }

                                    var SMPQqueryIL = (from prd in Context.Products
                                                       join inv in Context.Inventories on prd.Id equals inv.ProductId  //                                             where inv.CreatedDate >= this.FromDate && inv.CreatedDate <= this.ToDate && 
                                                       where inv.InventoryLocationId == Lid
                                                       join smv in Context.StockMovementDetail on prd.Id equals smv.ProductId
                                                       where smv.CreatedDate >= this.FromDate && smv.CreatedDate <= this.ToDate &&
                                                       smv.MaterialId == Prdctlist.Mmid && smv.StockMovement.Type == InventoryJournalType.PATIENT_USE &&
                                                       smv.StockMovementId == Sment.smeid
                                                       group new { prd.MaterialId, smv } by prd into g
                                                       select new
                                                       {
                                                           SUMSMPQTY = g.Sum(s => s.smv.Quantity),

                                                       }).Distinct().ToList();


                                    if (SMPQqueryIL.Count > 0)
                                    {
                                        PatientConsumedQty += (long)SMPQqueryIL.Sum(smp => smp.SUMSMPQTY);
                                    }

                                    var SMDQqueryIL = (from prd in Context.Products
                                                       join inv in Context.Inventories on prd.Id equals inv.ProductId  //                                             where inv.CreatedDate >= this.FromDate && inv.CreatedDate <= this.ToDate && 
                                                       where inv.InventoryLocationId == Lid
                                                       join smv in Context.StockMovementDetail on prd.Id equals smv.ProductId
                                                       where smv.CreatedDate >= this.FromDate && smv.CreatedDate <= this.ToDate &&
                                                       smv.MaterialId == Prdctlist.Mmid && smv.StockMovement.Type == InventoryJournalType.DAMAGED &&
                                                       smv.StockMovementId == Sment.smeid
                                                       group new { prd.MaterialId, smv } by prd into g
                                                       select new
                                                       {
                                                           SUMSMDUTQTY = g.Sum(s => s.smv.Quantity),

                                                       }).Distinct().ToList();

                                    if (SMDQqueryIL.Count > 0)
                                    {
                                        DamagedQty += (long)SMDQqueryIL.Sum(smd => smd.SUMSMDUTQTY);
                                    }
                                }

                                StockReport temp = new StockReport();

                                temp.Batch = false;
                                temp.MaterialId = Prdctlist.Mmid;
                                temp.Name = Prdctlist.Nnam;
                                temp.uom = Prdctlist.Uuom;
                                temp.OpenStock = CLOSTK;
                                temp.PurchaseQty = PQ; // PurchaseQty; 
                                temp.PurchaseRtnQty = PurchaseRtnQty;
                                temp.SalesQty = SQ; // SalesQty; 
                                temp.SalesRtnQty = SalesRtnQty;
                                temp.StockMovedInQty = StockMovedInQty;
                                temp.StockMovedOutQty = StockMovedOutQty;
                                temp.PatientConsumedQty = PatientConsumedQty;
                                temp.DamagedQty = DamagedQty;

                                Stocks.Add(temp);
                            }
                        }
                    }
                }
            }
            if (Stocks != null && Stocks.Count > 0)
            {
                foreach (StockReport Item in Stocks)
                {
                    StockReportLine LineItem = new StockReportLine(Item);
                    LineStocks.Add(LineItem);
                }
            }
        }
    }

    public class StockReportLine
    {
        public String MaterialId { get; set; }
        public DateTime Date { get; set; }
        public String Name { get; set; }
        public String uom { get; set; }
        public bool Batch { get; set; }
        public String BatchN { get; set; }
        public DateTime BatchE { get; set; }
        public double OpenStock { get; set; }
        public double PurchaseQty { get; set; }
        public double SalesQty { get; set; }
        public double CloseingStock { get; set; }
        public double PurchaseRtnQty { get; set; }
        public double SalesRtnQty { get; set; }
        public double StockMovedInQty { get; set; }
        public double StockMovedOutQty { get; set; }
        public double PatientConsumedQty { get; set; }
        public double DamagedQty { get; set; }

        public StockReportLine()
        {
        }
                
        public StockReportLine(StockReport Stock)
        {

            
            if(Stock.Batch==false)
            {
                this.MaterialId = Stock.MaterialId;
                this.Date = Stock.Date;
                this.Name = Stock.Name;
                this.uom = Stock.uom;
                this.OpenStock = Stock.OpenStock;
                this.Batch = Stock.Batch;

                Stock.CloseingStock = (Stock.OpenStock + Stock.PurchaseQty - Stock.PurchaseRtnQty - Stock.SalesQty + Stock.SalesRtnQty - Stock.StockMovedOutQty + Stock.StockMovedInQty - Stock.PatientConsumedQty - Stock.DamagedQty);
                this.CloseingStock = Stock.CloseingStock;

                this.PurchaseQty = Stock.PurchaseQty;
                this.PurchaseRtnQty = Stock.PurchaseRtnQty;

                this.SalesQty = Stock.SalesQty;
                this.SalesRtnQty = Stock.SalesRtnQty;

                this.StockMovedInQty = Stock.StockMovedInQty; //Stock.StockMovedInQty
                this.StockMovedOutQty = Stock.StockMovedOutQty;

                this.DamagedQty = Stock.DamagedQty;
                this.PatientConsumedQty = Stock.PatientConsumedQty;

            }
            else
            {
                this.MaterialId = Stock.MaterialId;
                this.Date = Stock.Date;
                this.Name = Stock.Name;
                this.uom = Stock.uom;
                this.OpenStock = Stock.OpenStock;

                Stock.CloseingStock = (Stock.OpenStock + Stock.PurchaseQty - Stock.PurchaseRtnQty - Stock.SalesQty + Stock.SalesRtnQty - Stock.StockMovedOutQty + Stock.StockMovedInQty - Stock.PatientConsumedQty - Stock.DamagedQty);
                this.CloseingStock = Stock.CloseingStock;

                this.BatchN = Stock.BatchNos;
                this.BatchE =  Stock.ExpDate;
                this.Batch = Stock.Batch;

                this.PurchaseQty = Stock.PurchaseQty;
                this.PurchaseRtnQty = Stock.PurchaseRtnQty;

                this.SalesQty = Stock.SalesQty;
                this.SalesRtnQty = Stock.SalesRtnQty;

                this.StockMovedInQty = Stock.StockMovedInQty; //Stock.StockMovedInQty
                this.StockMovedOutQty = Stock.StockMovedOutQty;

                this.DamagedQty = Stock.DamagedQty;
                this.PatientConsumedQty = Stock.PatientConsumedQty;

            }
        }
    }
    public class StockReport : RptStock
    {
        public String MaterialId { get; set; }
        public DateTime Date { get; set; }
        public String Name { get; set; }
        public String uom { get; set; }
        public bool Batch { get; set; }
        public string BatchNos { get; set; }
        public DateTime ExpDate { get; set; }
        public double OpenStock { get; set; }
        public double PurchaseQty { get; set; }
        public DateTime SDate { get; set; }
        public DateTime PDate { get; set; }
        public double SalesQty { get; set; }
        public double CloseingStock { get; set; }
        public double PurchaseRtnQty { get; set; }
        public double SalesRtnQty { get; set; }
        public double StockMovedInQty { get; set; }
        public double StockMovedOutQty { get; set; }
        public double PatientConsumedQty { get; set; }
        public double DamagedQty { get; set; }
    }
}

