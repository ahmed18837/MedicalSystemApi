﻿using MedicalSystemApi.Data;
using MedicalSystemApi.Models.Entities;
using MedicalSystemApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MedicalSystemApi.Repository.Implement
{
    public class BillRepository : GenericRepository<Bill>, IBillRepository
    {
        private readonly AppDbContext _dbContext;
        public BillRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Bill> GetBillsByPatientIdAsync(int patientId)
        {
            return await _dbContext.Bills
            .AsNoTracking()
            .Where(b => b.PatientId == patientId)
            .OrderByDescending(b => b.DateIssued)
            .FirstOrDefaultAsync();
        }

        public async Task<bool> PatientExistsAsync(int patientId)
        {
            return await _dbContext.Patients
                 .AnyAsync(i => i.Id == patientId);
        }

        public async Task<bool> UpdateTotalAmountAsync(int billId, decimal amount)
        {
            var bill = await _dbContext.Bills.FindAsync(billId);
            bill!.TotalAmount = amount;
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
