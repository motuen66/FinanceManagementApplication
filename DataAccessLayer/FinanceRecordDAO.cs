using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class FinanceRecordDAO
    {
        public static List<FinanceRecord> GetFinanceRecords()
        {
            try
            {
                var context = new FinanceManagementApplicationContext();
                var FinanceRecords = context.FinanceRecords
                                    .Include(r => r.User)
                                    .ToList();

                return FinanceRecords;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void CreateNewFinanceRecord(FinanceRecord financeRecord)
        {
            try
            {
                var context = new FinanceManagementApplicationContext();
                context.FinanceRecords.Add(financeRecord);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void UpdateFinanceRecord(FinanceRecord financeRecord)
        {
            try
            {
                var context = new FinanceManagementApplicationContext();
                var FinanceRecordToUpdate = context.FinanceRecords.FirstOrDefault(
                    r => r.UserId == financeRecord.UserId);
                if (FinanceRecordToUpdate != null)
                {
                    context.FinanceRecords.Update(FinanceRecordToUpdate);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Record does not exist");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void DeleteFinanceRecord(FinanceRecord financeRecord)
        {
            try
            {
                var context = new FinanceManagementApplicationContext();
                var FinanceRecordToDelete = context.FinanceRecords.FirstOrDefault(
                    r => r.UserId == financeRecord.UserId);
                if (FinanceRecordToDelete != null)
                {
                    context.FinanceRecords.Remove(FinanceRecordToDelete);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Record does not exist");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static FinanceRecord GetFinanceRecordByUserId(int userId)
        {
            try
            {
                var context = new FinanceManagementApplicationContext();
                return context.FinanceRecords.FirstOrDefault(r => r.UserId == userId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
