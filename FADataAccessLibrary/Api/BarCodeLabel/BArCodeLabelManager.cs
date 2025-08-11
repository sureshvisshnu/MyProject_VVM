using fa.context;
using fa.model.Catalog;
using FADataAccessLibrary.Model.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FADataAccessLibrary.Api.BarCodeLabel
{
    public class BarCodeLabelManager
    {
        private static BarCodeLabelManager _instance;
        private static object syncRoot = new Object();
        public static BarCodeLabelManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new BarCodeLabelManager();
                }
                return _instance;
            }
        }
        BarCodeLabelManager()
        {
            // Initialize any resources or settings here if needed
        }
        // Add methods to manage bar code labels, e.g., create, update, delete, retrieve labels

        public LabelStockMaster AddBarCodeLabel(LabelStockMaster labelStock)
        {
            using (AccountMasterContext context = new AccountMasterContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        // Ensure no related entities cause unintended inserts
                        context.LabelStockMasters.Add(labelStock);
                        context.SaveChanges();

                        transaction.Commit();
                        return labelStock; // return the saved entity
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
        public LabelStockMaster LoadNewLabelRoll(LabelStockMaster newRoll)
        {
            using (var context = new AccountMasterContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        // 1. End any currently active roll of the same type
                        var activeRoll = context.LabelStockMasters
                            .FirstOrDefault(r => r.LabelType == newRoll.LabelType && r.IsActive);

                        if (activeRoll != null)
                        {
                            activeRoll.IsActive = false;
                            activeRoll.DateEnded = DateTime.Now;
                            context.LabelStockMasters.Update(activeRoll);
                        }

                        // 2. Prepare the new roll
                        newRoll.LabelsUsed = 0;
                        newRoll.WastedLabelCount = 0;
                        newRoll.RemainingCount = newRoll.TotalLabelCount;
                        newRoll.IsActive = true;
                        newRoll.DateLoaded = DateTime.Now;

                        // 3. Save it
                        context.LabelStockMasters.Add(newRoll);
                        context.SaveChanges();

                        transaction.Commit();
                        return newRoll;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public void LoadNewLabelRollOld(LabelStockMaster newRoll)
        {
            using (var context = new AccountMasterContext())
            {
                // Mark the old active roll as ended
                var oldRoll = context.LabelStockMasters
                    .Where(ls => ls.LabelType == newRoll.LabelType && ls.IsActive)
                    .OrderByDescending(ls => ls.DateLoaded)
                    .FirstOrDefault();

                if (oldRoll != null)
                {
                    oldRoll.IsActive = false;
                    oldRoll.DateEnded = DateTime.Now;
                }

                // Set initial values for the new roll
                newRoll.LabelsUsed = 0;
                newRoll.WastedLabelCount = 0;
                newRoll.RemainingCount = newRoll.TotalLabelCount;
                newRoll.DateLoaded = DateTime.Now;
                newRoll.IsActive = true;

                context.LabelStockMasters.Add(newRoll);
                context.SaveChanges();
            }
        }

        public LabelStockInfoDto GetLabelStockInfo(string labelType)
        {
            using (var context = new AccountMasterContext())
            {
                var stock = context.LabelStockMasters
                    .Where(ls => ls.LabelType == labelType && ls.IsActive)
                    .OrderByDescending(ls => ls.DateLoaded)
                    .FirstOrDefault();

                if (stock == null)
                    return null;

                // Calculate today's usage
                var todayPrinted = context.LabelStockUsages
                    .Where(u => u.LabelStockId == stock.Id && u.DatePrinted.Date == DateTime.Today)
                    .Sum(u => (int?)u.PrintedCount ?? 0);

                return new LabelStockInfoDto
                {
                    LabelType = stock.LabelType,
                    TotalLabelCount = stock.TotalLabelCount,
                    RemainingCount = stock.RemainingCount,
                    TodayPrinted = todayPrinted,
                    DateLoaded = stock.DateLoaded
                };
            }
        }

        public void UpdateLabelUsage(string labelType, int qtyPrinted, int wastedLabels = 0)
        {
            using (var context = new AccountMasterContext())
            {
                var stock = context.LabelStockMasters
                    .Where(ls => ls.LabelType == labelType && ls.IsActive)
                    .OrderByDescending(ls => ls.DateLoaded)
                    .FirstOrDefault();

                if (stock != null)
                {
                    stock.LabelsUsed += qtyPrinted;
                    stock.WastedLabelCount += wastedLabels;
                    stock.RemainingCount = stock.TotalLabelCount - stock.LabelsUsed - stock.WastedLabelCount;

                    context.LabelStockUsages.Add(new LabelStockUsage
                    {
                        LabelStockId = stock.Id,
                        DatePrinted = DateTime.Now,
                        PrintedCount = qtyPrinted,
                        WastedCount = wastedLabels,
                        PrintedBy = Environment.UserName
                    });

                    context.SaveChanges();
                }
            }
        }

        public LabelStockMaster UpdateLabelUsage(string labelType, int qtyPrinted, int wastedLabels = 0, string referenceId = null)
        {
            using (var context = new AccountMasterContext())
            {
                var stock = context.LabelStockMasters
                    .Where(ls => ls.LabelType == labelType && ls.IsActive)
                    .OrderByDescending(ls => ls.DateLoaded)
                    .FirstOrDefault();

                if (stock == null)
                    throw new InvalidOperationException($"No active label stock found for type '{labelType}'.");

                // Update master stock
                stock.LabelsUsed += qtyPrinted;
                stock.WastedLabelCount += wastedLabels;
                stock.RemainingCount = stock.TotalLabelCount - stock.LabelsUsed - stock.WastedLabelCount;

                // Log usage
                context.LabelStockUsages.Add(new LabelStockUsage
                {
                    LabelStockId = stock.Id,
                    DatePrinted = DateTime.Now,
                    PrintedCount = qtyPrinted,
                    WastedCount = wastedLabels,
                    ReferenceId = referenceId,
                    PrintedBy = Environment.UserName
                });

                context.SaveChanges();

                return stock; // Return updated stock info
            }
        }
        public void UpdateLabelUsageNew(string labelType, int qtyPrinted, int wastedLabels, string referenceId)
        {
            using (var context = new AccountMasterContext())
            {
                var stock = context.LabelStockMasters
                    .Where(ls => ls.LabelType == labelType && ls.IsActive)
                    .OrderByDescending(ls => ls.DateLoaded)
                    .FirstOrDefault();

                if (stock != null)
                {
                    // Update counts
                    stock.LabelsUsed += qtyPrinted;
                    stock.WastedLabelCount += wastedLabels;
                    stock.RemainingCount = stock.TotalLabelCount - stock.LabelsUsed - stock.WastedLabelCount;

                    // Add usage history
                    context.LabelStockUsages.Add(new LabelStockUsage
                    {
                        LabelStockId = stock.Id,
                        DatePrinted = DateTime.Now,
                        PrintedCount = qtyPrinted,
                        WastedCount = wastedLabels,
                        ReferenceId = referenceId,
                        PrintedBy = Environment.UserName
                    });

                    context.SaveChanges();
                }
            }
        }
    }
    public class LabelStockInfoDto
    {
        public long LabelStockId { get; set; }
        public string LabelType { get; set; }
        public int TotalLabelCount { get; set; }
        public int RunningCount { get; set; } // total printed so far
        public int WastedLabel { get; set; }
        public int RemainingCount { get; set; }
        public int TodayPrinted { get; set; }
        public DateTime DateLoaded { get; set; }
        public DateTime? DateEnded { get; set; }
        public int FlagCount { get; set; }

        // Add this property
        public int LabelsPrintedToday { get; set; }
    }

}
