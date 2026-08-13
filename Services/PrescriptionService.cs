using MediFlowApi.Data;
using MediFlowApi.DTOs;
using MediFlowApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace MediFlowApi.Services
{
    public class PrescriptionService : IPrescriptionService
    {
        public readonly AppDbContext _Context;
        public readonly PrescriptionMapper _Mapper;


        public PrescriptionService(AppDbContext context, PrescriptionMapper mapper)
        {
            _Context = context;
            _Mapper = mapper;
        }
        public async Task<PrescriptionResponseDto> CreatePrescriptionAsync(CreatePrescriptionDto dto)
        {
            //making sure  that Consultation is existing in Db
            bool isExistingConsultation = await _Context.Consultations.AnyAsync(x => dto.ConsultationId == x.Id);
            if (!isExistingConsultation)
            {
                throw new Exception("Consultation not found");
            }
            var duprecatedCons = await _Context.Prescription.AnyAsync(x => x.ConsultationId == dto.ConsultationId);
            if (duprecatedCons)
            {
                throw new Exception("there is a prescription is already Existing for this Consultation");
            }

            //cheking that Medicines List is existing
            var medIds = dto.PrescriptionItems.Select(x => x.MedicineId).Distinct().ToList();

            var medList = _Context.Medicines.Where(x => medIds.Contains(x.Id));
            if (await medList.CountAsync() != medIds.Count())
            {
                throw new KeyNotFoundException();
            }
            var entity = _Mapper.ToEntity(dto);
            _Context.Prescription.Add(entity);
            await _Context.SaveChangesAsync();
            return _Mapper.ToResponseDto(entity);
            ;
        }
        public async Task<PrescriptionResponseDto> GetPrescriptionByIdAsync(int id)
        { 
            var prescription=await _Context.Prescription.Include(p=>p.PrescriptionItems).FirstOrDefaultAsync(x => x.Id == id);
            if (prescription == null)
            {
                throw new Exception("There is no Prescription with this Id.");
            }
            var res=_Mapper.ToResponseDto(prescription);
            return res;
        }
    }
}
