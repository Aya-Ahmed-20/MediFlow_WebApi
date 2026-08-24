using MediFlowApi.DTOs;

namespace MediFlowApi.Interfaces
{
    public interface IDispensingService
    {
        public Task<bool> DispensingAsync(DispensePrescriptionDto dto, string pharmacistId);
    }
}
