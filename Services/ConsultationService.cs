using MediFlowApi.Data;
using MediFlowApi.DTOs;
using MediFlowApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MediFlowApi.Services
{
    public class ConsultationService: IConsultationsService
    {
      private readonly  AppDbContext _dbContext;
        public ConsultationService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> CreateConsultationAsync(CreateConsultationDto dto)
        {
            var isValidDoctor = await _dbContext.Doctors.AnyAsync(a=>a.Id==dto.DoctorId);
            var isValidPatient = await _dbContext.Patient.AnyAsync(a => a.Id == dto.PatientId);

            if (!isValidDoctor)
            {
                throw new KeyNotFoundException($"Doctor with ID {dto.DoctorId} was not found.");
            }
            if (!isValidPatient) 
            {
                throw new KeyNotFoundException($"Patient with ID {dto.PatientId} was not found.");
            }
            var entity = ConsultationMapper.ToEntity(dto);
           await _dbContext.Consultations.AddAsync(entity);
           await _dbContext.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<ReadConsultationDto> ReadConsultationAsync(int id)
        {
            var cons = await _dbContext.Consultations.Include(e => e.Doctor).Include(e => e.Patient).FirstOrDefaultAsync(a => a.Id == id);
            
            if (cons==null)
            {
                throw new KeyNotFoundException($"Consultation with Id {id} is not found.");
            }
            var dto = ConsultationMapper.ToDto(cons);
            return dto;
        }

    }
}
