using MediatR;
using Vb.Base.Response;
using Vb.Schema;

namespace Vb.Business.Cqrs;


// Queries:
public record GetAllAddressQuery() : IRequest<ApiResponse<List<AddressResponse>>>;
public record GetAddressByIdQuery(int Id) : IRequest<ApiResponse<AddressResponse>>;
public record GetAddressByParameterQuery(string? City = null, string? PostalCode = null) : IRequest<ApiResponse<List<AddressResponse>>>;

// Commands:
public record CreateAddressCommand(AddressRequest Model) : IRequest<ApiResponse<AddressResponse>>;
public record UpdateAddressCommand(int Id,AddressRequest Model) : IRequest<ApiResponse>;
public record DeleteAddressCommand(int Id) : IRequest<ApiResponse>;
