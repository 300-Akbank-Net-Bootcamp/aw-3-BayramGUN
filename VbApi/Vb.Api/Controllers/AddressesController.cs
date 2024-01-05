using MediatR;
using Microsoft.AspNetCore.Mvc;
using Vb.Base.Response;
using Vb.Business.Cqrs;
using Vb.Schema;

namespace VbApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AddressesController : ControllerBase
{
    private readonly IMediator mediator;
    public AddressesController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet]
    public async Task<ApiResponse<List<AddressResponse>>> Get()
    {
        var operation = new GetAllAddressQuery();
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpGet("{id}")]
    public async Task<ApiResponse<AddressResponse>> Get(int id)
    {
        var operation = new GetAddressByIdQuery(id);
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpGet("GetByCity")]
    public async Task<ApiResponse<List<AddressResponse>>> GetByFirstName([FromQuery] string city)
    {
        var operation = new GetAddressByParameterQuery(City: city);
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpGet("GetByPostalCode")]
    public async Task<ApiResponse<List<AddressResponse>>> GetByPostalCode([FromQuery] string postalCode)
    {
        var operation = new GetAddressByParameterQuery(PostalCode: postalCode);
        var result = await mediator.Send(operation);
        return result;
    }


    [HttpGet("GetBy")]
    public async Task<ApiResponse<List<AddressResponse>>> GetBy(
        [FromQuery] string?  city = null, 
        [FromQuery] string? postalCode = null)
    {
        var operation = new GetAddressByParameterQuery(City: city, PostalCode: postalCode);
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpPost]
    public async Task<ApiResponse<AddressResponse>> Post([FromBody] AddressRequest address)
    {
        var operation = new CreateAddressCommand(address);
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpPut("{id}")]
    public async Task<ApiResponse> Put(int id, [FromBody] AddressRequest Address)
    {
        var operation = new UpdateAddressCommand(id, Address);
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpDelete("{id}")]
    public async Task<ApiResponse> Delete(int id)
    {
        var operation = new DeleteAddressCommand(id);
        var result = await mediator.Send(operation);
        return result;
    }
    
}