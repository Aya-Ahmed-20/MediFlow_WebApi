using MediFlowApi.DTOs;
using MediFlowApi.Models;

namespace MediFlowApi.Services
{
    public interface IPrescriptionService
    {
        Task<PrescriptionResponseDto> CreatePrescriptionAsync(CreatePrescriptionDto dto);
        Task<PrescriptionResponseDto> GetPrescriptionByIdAsync(int id);
    }
}