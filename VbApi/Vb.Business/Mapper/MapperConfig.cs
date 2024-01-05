using AutoMapper;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Vb.Data.Entity;
using Vb.Schema;

namespace Vb.Business.Mapper;

public class MapperConfig : Profile
{
    public MapperConfig()
    {
        CreateMap<CustomerRequest, Customer>();
        CreateMap<Customer, CustomerResponse>();
        
        CreateMap<AddressRequest, Address>();
        CreateMap<Address, AddressResponse>()
            .ForMember(dest => dest.CustomerName,
                src => src.MapFrom(x => x.Customer.FirstName + " " + x.Customer.LastName));
        
        
        CreateMap<ContactRequest, Contact>();
        CreateMap<Contact, ContactResponse>()
            .ForMember(dest => dest.CustomerName,
                src => src.MapFrom(x => x.Customer.FirstName + " " + x.Customer.LastName));
        
        CreateMap<AccountRequest, Account>();
        CreateMap<Account, AccountResponse>()
            .ForMember(dest => dest.CustomerName,
                src => src.MapFrom(x => x.Customer.FirstName + " " + x.Customer.LastName));

        
        CreateMap<AccountTransactionRequest, AccountTransaction>()
            .ForMember(dest => dest.TransactionDate,
                src => src.MapFrom(x => DateTime.UtcNow));

        CreateMap<AccountTransaction, AccountTransactionResponse>();
        
        CreateMap<EftTransactionRequest, EftTransaction>()
            .ForMember(dest => dest.TransactionDate,
                src => src.MapFrom(x => DateTime.UtcNow));

        CreateMap<EftTransaction, EftTransactionResponse>();
    }
}