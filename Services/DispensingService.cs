using MediFlowApi.Data;
using MediFlowApi.DTOs;
using MediFlowApi.Interfaces;
using MediFlowApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace MediFlowApi.Services
{
    public class DispensingService: IDispensingService
    {
      private readonly  AppDbContext _Context;
        public DispensingService(AppDbContext context) 
        { 
           _Context = context;
        }
        public async Task<bool> DispensingAsync(DispensePrescriptionDto dto, string pharmacistId)
        {
            var precriptionEntity=await _Context.Prescription.Include(x=>x.PrescriptionItems).ThenInclude(x=>x.Medicine).FirstOrDefaultAsync(x=>x.Id==dto.PrescriptionId);
            if (precriptionEntity == null) 
            {
                throw new Exception($"there is no Prescription With this Id= {dto.PrescriptionId}");
            }
            else if (precriptionEntity.IsDispensed == true) 
            {
                throw new Exception("This Prescription is alredy despensed before!");
            }
            var prescriptionsItemsList= precriptionEntity.PrescriptionItems.ToList();
            foreach (var item in prescriptionsItemsList) 
            {
                if (item.Medicine.StockQuantity < item.Quantity)
                {
                    throw new Exception($"Stock is Low for Medicine {item.MedicineId}");

                }
            }
            foreach (var item in prescriptionsItemsList)
            { 
                    item.Medicine.StockQuantity -= item.Quantity; 
            }
            precriptionEntity.IsDispensed = true;
            var dispensingObject=new DispensingRecord()
            { 
                DispensedAt = DateTime.UtcNow ,
                PharmacistId = pharmacistId,
                Notes=dto.Notes,
                PrescriptionId=dto.PrescriptionId,
            };
            await _Context.DispensingRecord.AddAsync(dispensingObject);
            await _Context.SaveChangesAsync();
            return true;

        }
    }
}
