using AutoMapper;
using BilleteraCryptoProjectAPI.DTO.HistorialPrecios;
using BilleteraCryptoProjectAPI.Models;

namespace BilleteraCryptoProjectAPI.Mapping {
    public class MappingProfile : Profile {

        public MappingProfile() {
            CreateMap<Cliente, ClienteCreateDTO > ();
            CreateMap<Cliente, ClienteReadDTO> ();

            CreateMap<ClienteCreateDTO, Cliente>()
                .ForMember(dest => dest.ClienteId, opt => opt.Ignore());
            CreateMap<ClienteReadDTO, Cliente>()
                .ForMember(dest => dest.ClienteId, opt => opt.Ignore());

            CreateMap<Movimiento, MovimientoCreateDTO> ();
            CreateMap<Movimiento, MovimientoReadDTO>();
            CreateMap<MovimientoCreateDTO, Movimiento>()
                .ForMember(dest => dest.MovimientoId, opt => opt.Ignore()); 
            CreateMap<MovimientoReadDTO, Movimiento>()
                .ForMember(dest => dest.MovimientoId, opt => opt.Ignore());
            CreateMap<Operacione, OperacionCreateDTO>();
            CreateMap<Operacione, OperacionReadDTO>();
            CreateMap<OperacionCreateDTO, Operacione>()
                .ForMember(dest => dest.OperacionId, opt => opt.Ignore()); 
            CreateMap<OperacionReadDTO, Operacione>()
                .ForMember(dest => dest.OperacionId, opt => opt.Ignore());
            CreateMap<Cripto, CriptoReadDTO>();
            CreateMap<Cripto, CriptoCreateDTO>();
            CreateMap<CriptoCreateDTO, Cripto>()
                .ForMember(dest => dest.CriptoCode, opt => opt.Ignore()); 
            CreateMap<CriptoReadDTO, Cripto>()
                .ForMember(dest => dest.CriptoCode, opt => opt.Ignore()); 

            CreateMap<HistorialPrecio, HistorialPrecioReadDTO>();
            CreateMap<HistorialPrecio, HistorialPrecioCreateDTO>();
            CreateMap<HistorialPrecioCreateDTO, HistorialPrecio>();
            CreateMap<HistorialPrecioUpdateDTO, HistorialPrecio>();
            CreateMap<HistorialPrecio, HistorialPrecioUpdateDTO>();

            CreateMap<HistorialPrecioReadDTO, HistorialPrecio>(); 




        }

    }
}
