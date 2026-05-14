using MediFlowApi.DTOs;
using MediFlowApi.Models;

namespace MediFlowApi.Interfaces
{
    public interface IMedicineServices
    {
        Task<IEnumerable<MedicineReadDto>> GetAllAsync(string? searchTerm,int pageNumber=1,int pageSize=10);
        Task<MedicineReadDto?> GetByIdAsync(int id);
        Task<MedicineReadDto> AddAsync(MedicineCreateDto dto);
        Task DeleteAsync(int id);
        Task<bool> UpdateAsync(int id,MedicineReadDto dto);
    }
}
