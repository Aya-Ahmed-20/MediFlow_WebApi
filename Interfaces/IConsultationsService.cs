using MediFlowApi.DTOs;

namespace MediFlowApi.Interfaces
{
    public interface IConsultationsService
    {
        public Task<int> CreateConsultationAsync(CreateConsultationDto dto);
        public Task<ReadConsultationDto> ReadConsultationAsync(int id);

    }
}
