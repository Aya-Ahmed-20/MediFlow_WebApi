using AutoMapper;
using MediFlowApi.DTOs;
using MediFlowApi.Models;

namespace MediFlowApi.Profiles
{
    public class MedicineProfile :Profile
    {
        public MedicineProfile()
        { 
            CreateMap<Medicine,MedicineCreateDto>();
            CreateMap<Medicine, MedicineReadDto>();
            CreateMap<MedicineReadDto, Medicine>();
            CreateMap< MedicineCreateDto ,Medicine>();

        }
    }
}
