using MediatR;
using Microsoft.AspNetCore.Mvc;
using Vb.Base.Response;
using Vb.Business.Cqrs;
using Vb.Schema;

namespace VbApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountsController : ControllerBase
{
    private readonly IMediator mediator;
    public AccountsController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet]
    public async Task<ApiResponse<List<AccountResponse>>> Get()
    {
        var operation = new GetAllAccountQuery();
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpGet("{id}")]
    public async Task<ApiResponse<AccountResponse>> Get(int id)
    {
        var operation = new GetAccountByIdQuery(id);
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpGet("GetByCustomerId")]
    public async Task<ApiResponse<List<AccountResponse>>> GetByCustomerId([FromQuery] int customerId)
    {
        var operation = new GetAccountByParameterQuery(CustomerId: customerId);
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpGet("GetByAccountName")]
    public async Task<ApiResponse<List<AccountResponse>>> GetByAccountName([FromQuery] string accountName)
    {
        var operation = new GetAccountByParameterQuery(AccountName: accountName);
        var result = await mediator.Send(operation);
        return result;
    }


    [HttpGet("GetBy")]
    public async Task<ApiResponse<List<AccountResponse>>> GetBy(
        [FromQuery] int?  customerId = null, 
        [FromQuery] string? accountName = null)
    {
        var operation = new GetAccountByParameterQuery(CustomerId: customerId, AccountName: accountName);
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpPost]
    public async Task<ApiResponse<AccountResponse>> Post([FromBody] AccountRequest account)
    {
        var operation = new CreateAccountCommand(account);
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpPut("{id}")]
    public async Task<ApiResponse> Put(int id, [FromBody] AccountRequest account)
    {
        var operation = new UpdateAccountCommand(id, account);
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpDelete("{id}")]
    public async Task<ApiResponse> Delete(int id)
    {
        var operation = new DeleteAccountCommand(id);
        var result = await mediator.Send(operation);
        return result;
    }


}