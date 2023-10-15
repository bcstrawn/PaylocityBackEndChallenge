using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;
using AutoMapper;

namespace Api.Mapping;

public class PaylocityProfile : Profile
{
    public PaylocityProfile()
    {
        CreateMap<Employee, GetEmployeeDto>()
            .ForMember(dest => dest.Dependents, opt => opt.MapFrom(src => src.Dependents));

        CreateMap<Dependent, GetDependentDto>();

        CreateMap<CreateEmployeeDto, Employee>()
            .ForMember(dest => dest.Dependents, opt => opt.MapFrom(src => src.Dependents));

        CreateMap<CreateDependentDto, Dependent>();
    }
}