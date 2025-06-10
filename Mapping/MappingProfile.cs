using AutoMapper;
using BilleteraCryptoProjectAPI.DTO.HistorialPrecios;
using BilleteraCryptoProjectAPI.Models;

namespace BilleteraCryptoProjectAPI.Mapping {
    public class MappingProfile : Profile {

        public MappingProfile() {
            CreateMap<Cliente, ClienteCreateDTO > ();
            CreateMap<Cliente, ClienteReadDTO> ();

            CreateMap<ClienteCreateDTO, Cliente>()
                .ForMember(dest => dest.ClienteId, opt => opt.Ignore()); // Ignorar ClienteId al crear un nuevo cliente
            CreateMap<ClienteReadDTO, Cliente>()
                .ForMember(dest => dest.ClienteId, opt => opt.Ignore()); // Ignorar ClienteId al mapear desde ClientesDTO a Cliente

            CreateMap<Movimiento, MovimientoCreateDTO> ();
            CreateMap<Movimiento, MovimientoReadDTO>();
            CreateMap<MovimientoCreateDTO, Movimiento>()
                .ForMember(dest => dest.MovimientoId, opt => opt.Ignore()); // Ignorar MovimientoId al crear un nuevo movimiento
            CreateMap<MovimientoReadDTO, Movimiento>()
                .ForMember(dest => dest.MovimientoId, opt => opt.Ignore()); // Ignorar MovimientoId al mapear desde MostrarMovimientosDTO a Movimiento
            CreateMap<Operacione, OperacionCreateDTO>();
            CreateMap<Operacione, OperacionReadDTO>();
            CreateMap<OperacionCreateDTO, Operacione>()
                .ForMember(dest => dest.OperacionId, opt => opt.Ignore()); // Ignorar OperacionId al crear una nueva operacion
            CreateMap<OperacionReadDTO, Operacione>()
                .ForMember(dest => dest.OperacionId, opt => opt.Ignore()); // Ignorar OperacionId al mapear desde MostrarOperacionesDTO a Operacione
            CreateMap<Cripto, CriptoReadDTO>();
            CreateMap<Cripto, CriptoCreateDTO>();
            CreateMap<CriptoCreateDTO, Cripto>()
                .ForMember(dest => dest.CriptoCode, opt => opt.Ignore()); // Ignorar CriptoCode al crear una nueva cripto
            CreateMap<CriptoReadDTO, Cripto>()
                .ForMember(dest => dest.CriptoCode, opt => opt.Ignore()); // Ignorar CriptoCode al mapear desde MostrarCryptoDTO a Cripto

            CreateMap<HistorialPrecio, HistorialPrecioReadDTO>();
            CreateMap<HistorialPrecio, HistorialPrecioCreateDTO>();
            CreateMap<HistorialPrecioCreateDTO, HistorialPrecio>();
            CreateMap<HistorialPrecioUpdateDTO, HistorialPrecio>();
            CreateMap<HistorialPrecio, HistorialPrecioUpdateDTO>();

            CreateMap<HistorialPrecioReadDTO, HistorialPrecio>(); // Ignorar HistorialPrecioId al mapear desde MostrarHistorialDePrecioDTO a HistorialPrecio




        }

    }
}
