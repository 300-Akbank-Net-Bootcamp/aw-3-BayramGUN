using MediatR;
using Microsoft.AspNetCore.Mvc;
using Vb.Base.Response;
using Vb.Business.Cqrs;
using Vb.Schema;

namespace VbApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ContactsController : ControllerBase
{
    private readonly IMediator mediator;
    public ContactsController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet]
    public async Task<ApiResponse<List<ContactResponse>>> Get()
    {
        var operation = new GetAllContactQuery();
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpGet("CustomerFirstName")]
    public async Task<ApiResponse<List<ContactResponse>>> GetByFirstName([FromQuery] string customerFirstName)
    {
        var operation = new GetContactByParameterQuery(CustomerFirstName: customerFirstName);
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpGet("CustomerLastName")]
    public async Task<ApiResponse<List<ContactResponse>>> CustomerLastName([FromQuery] string CustomerLastName)
    {
        var operation = new GetContactByParameterQuery(CustomerLastName: CustomerLastName);
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpGet("GetByInformation")]
    public async Task<ApiResponse<List<ContactResponse>>> GetBy([FromQuery] string information)
    {
        var operation = new GetContactByParameterQuery(Information: information);
        var result = await mediator.Send(operation);
        return result;
    }
    [HttpGet("GetBy")]
    public async Task<ApiResponse<List<ContactResponse>>> GetBy(
        [FromQuery] string?  information = null, 
        [FromQuery] string? customerFirstName = null,
        [FromQuery] string? customerLastName = null)
    {
        var operation = new GetContactByParameterQuery(Information: information,
                                                       CustomerFirstName: customerFirstName,
                                                       CustomerLastName: customerLastName);
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpGet("{id}")]
    public async Task<ApiResponse<ContactResponse>> Get(int id)
    {
        var operation = new GetContactByIdQuery(id);
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpPost]
    public async Task<ApiResponse<ContactResponse>> Post([FromBody] ContactRequest contact)
    {
        var operation = new CreateContactCommand(contact);
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpPut("{id}")]
    public async Task<ApiResponse> Put(int id, [FromBody] ContactRequest contact)
    {
        var operation = new UpdateContactCommand(id, contact);
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpDelete("{id}")]
    public async Task<ApiResponse> Delete(int id)
    {
        var operation = new DeleteContactCommand(id);
        var result = await mediator.Send(operation);
        return result;
    }
}