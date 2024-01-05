using MediatR;
using Microsoft.AspNetCore.Mvc;
using Vb.Base.Response;
using Vb.Business.Cqrs;
using Vb.Schema;

namespace VbApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountTransactionsController : ControllerBase
{
    private readonly IMediator mediator;
    public AccountTransactionsController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet]
    public async Task<ApiResponse<List<AccountTransactionResponse>>> Get()
    {
        var operation = new GetAllAccountTransactionQuery();
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpGet("{id}")]
    public async Task<ApiResponse<AccountTransactionResponse>> Get(int id)
    {
        var operation = new GetAccountTransactionByIdQuery(id);
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpGet("getByReferenceNumber")]
    public async Task<ApiResponse<List<AccountTransactionResponse>>> GetByReferenceNumber([FromQuery] string referenceNumber)
    {
        var operation = new GetAccountTransactionByParameterQuery(ReferenceNumber: referenceNumber);
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpGet("getByTransactionDate")]
    public async Task<ApiResponse<List<AccountTransactionResponse>>> GetByTransactionDate([FromQuery] DateTime transactionDate)
    {
        var operation = new GetAccountTransactionByParameterQuery(TransactionDate: transactionDate);
        var result = await mediator.Send(operation);
        return result;
    }

    
    [HttpGet("GetBy")]
    public async Task<ApiResponse<List<AccountTransactionResponse>>> GetBy(
        [FromQuery] string? referenceNumber = null, 
        [FromQuery] DateTime? transactionDate = null)
    {
        var operation = new GetAccountTransactionByParameterQuery(ReferenceNumber: referenceNumber, TransactionDate: transactionDate);
        var result = await mediator.Send(operation);
        return result;
    }


    [HttpPost]
    public async Task<ApiResponse<AccountTransactionResponse>> Post([FromBody] AccountTransactionRequest accountTransaction)
    {
        var operation = new CreateAccountTransactionCommand(accountTransaction);
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpPut("{id}")]
    public async Task<ApiResponse> Put(int id, [FromBody] AccountTransactionRequest accountTransaction)
    {
        var operation = new UpdateAccountTransactionCommand(id, accountTransaction);
        var result = await mediator.Send(operation);
        return result;
    }

    [HttpDelete("{id}")]
    public async Task<ApiResponse> Delete(int id)
    {
        var operation = new DeleteAccountTransactionCommand(id);
        var result = await mediator.Send(operation);
        return result;
    }
}