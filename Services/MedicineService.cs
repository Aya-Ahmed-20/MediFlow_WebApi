using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediFlowApi.Data;
using MediFlowApi.DTOs;
using MediFlowApi.Interfaces;
using MediFlowApi.Models;
using MediFlowApi.Profiles;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace MediFlowApi.Services
{
    public class MedicineServices : IMedicineServices
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public MedicineServices(AppDbContext context,IMapper mapper) { 
             _context = context;
             _mapper = mapper;
        }
        public async Task<IEnumerable<MedicineReadDto>> GetAllAsync(string?searchTerm, int pageSize,int pageNumber)
        {
            var query =_context.Medicines.AsNoTracking().AsQueryable();
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(x => x.Name.Contains(searchTerm) || x.Description.Contains(searchTerm));
            }
            return await query.OrderBy(q => q.Id)
               .Skip(pageNumber - 1)
               .Take(pageSize)
               .ProjectTo<MedicineReadDto>(_mapper.ConfigurationProvider)
               .ToListAsync();
        }
        public async Task<MedicineReadDto> GetByIdAsync(int id)
        {
            var med= await _context.Medicines.AsNoTracking().FirstOrDefaultAsync(a=>a.Id==id);

            return _mapper.Map<MedicineReadDto>(med);
        }
        public async Task<MedicineReadDto> AddAsync(MedicineCreateDto dto)
        {
            var entity = _mapper.Map<Medicine>(dto);
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            var ReadDto= _mapper.Map<MedicineReadDto>(entity);
            return ReadDto;
        }
        public async Task<bool> UpdateAsync(int id, MedicineReadDto dto)
        {

            // 1. البحث عن العنصر الأصلي (Tracking)
            var existingMed = await _context.Medicines.FindAsync(id);
            if (existingMed == null) return false;

            // 2. نقل التعديلات من الـ DTO للـ Entity الأصلية
            _mapper.Map(dto, existingMed);

            // 3. الحفظ (الآن EF Core سيعرف ما الذي تغير بالضبط)
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task DeleteAsync(int id)
        {
            var med = await _context.Medicines.FindAsync(id);
            if (med != null)
            {
                _context.Medicines.Remove(med);
            }
           await _context.SaveChangesAsync();
        }
    }
}
