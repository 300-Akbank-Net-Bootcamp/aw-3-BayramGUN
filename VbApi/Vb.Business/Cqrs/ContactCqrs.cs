using MediatR;
using Vb.Base.Response;
using Vb.Schema;

namespace Vb.Business.Cqrs;


public record CreateContactCommand(ContactRequest Model) : IRequest<ApiResponse<ContactResponse>>;
public record UpdateContactCommand(int Id, ContactRequest Model) : IRequest<ApiResponse>;
public record DeleteContactCommand(int Id) : IRequest<ApiResponse>;

public record GetAllContactQuery() : IRequest<ApiResponse<List<ContactResponse>>>;
public record GetContactByParameterQuery(string? Information = null, string? CustomerFirstName = null, string? CustomerLastName = null) : IRequest<ApiResponse<List<ContactResponse>>>;
public record GetContactByIdQuery(int Id) : IRequest<ApiResponse<ContactResponse>>;