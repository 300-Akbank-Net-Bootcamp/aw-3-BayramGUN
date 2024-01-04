using MediatR;
using Microsoft.AspNetCore.Mvc;
using Vb.Base.Response;
using Vb.Business.Cqrs;
using Vb.Schema;

namespace VbApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomersController : ControllerBase
{
    private readonly IMediator mediator;
    public CustomersController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet]
    public async Task<ApiResponse<List<CustomerResponse>>> Get()
    {
        var operation = new GetAllCustomerQuery();
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpGet("getByFirstName")]
    public async Task<ApiResponse<List<CustomerResponse>>> GetByFirstName([FromQuery] string firstName)
    {
        var operation = new GetCustomerByParameterQuery(FirstName: firstName);
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpGet("getByLastName")]
    public async Task<ApiResponse<List<CustomerResponse>>> GetByLastName([FromQuery] string lastName)
    {
        var operation = new GetCustomerByParameterQuery(LastName: lastName);
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpGet("GetByIdentityNumber")]
    public async Task<ApiResponse<List<CustomerResponse>>> GetByIdentityNumber([FromQuery] string identityNumber)
    {
        var operation = new GetCustomerByParameterQuery(IdentityNumber: identityNumber);
        var result = await mediator.Send(operation);
        return result;
    }
    [HttpGet("GetBy")]
    public async Task<ApiResponse<List<CustomerResponse>>> GetBy(
        [FromQuery] string? identityNumber = null, 
        [FromQuery] string? firstName = null,
        [FromQuery] string? lastName = null)
    {
        var operation = new GetCustomerByParameterQuery(IdentityNumber: identityNumber, FirstName: firstName, LastName: lastName);
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpGet("{id}")]
    public async Task<ApiResponse<CustomerResponse>> Get(int id)
    {
        var operation = new GetCustomerByIdQuery(id);
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpPost]
    public async Task<ApiResponse<CustomerResponse>> Post([FromBody] CustomerRequest customer)
    {
        var operation = new CreateCustomerCommand(customer);
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpPut("{id}")]
    public async Task<ApiResponse> Put(int id, [FromBody] CustomerRequest customer)
    {
        var operation = new UpdateCustomerCommand(id,customer);
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpDelete("{id}")]
    public async Task<ApiResponse> Delete(int id)
    {
        var operation = new DeleteCustomerCommand(id);
        var result = await mediator.Send(operation);
        return result;
    }
}