using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Vb.Base.Response;
using Vb.Business.Cqrs;
using Vb.Data;
using Vb.Data.Entity;
using Vb.Schema;

namespace Vb.Business.Command;

public class AccountTransactionCommandHandler :
    IRequestHandler<CreateAccountTransactionCommand, ApiResponse<AccountTransactionResponse>>,
    IRequestHandler<UpdateAccountTransactionCommand, ApiResponse>,
    IRequestHandler<DeleteAccountTransactionCommand, ApiResponse>

{
    private readonly VbDbContext dbContext;
    private readonly IMapper mapper;

    public AccountTransactionCommandHandler(
        VbDbContext dbContext,
        IMapper mapper,
        AccountCommandHandler accountCommandHandler)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    public async Task<ApiResponse<AccountTransactionResponse>> Handle(CreateAccountTransactionCommand request, CancellationToken cancellationToken)
    {
        var checkIdentity = await dbContext.Set<AccountTransaction>().Where(x => x.ReferenceNumber == request.Model.ReferenceNumber)
            .FirstOrDefaultAsync(cancellationToken);
        if (checkIdentity != null)
        {
            return new ApiResponse<AccountTransactionResponse>($"{request.Model.ReferenceNumber} is used by another AccountTransaction.");
        }
        var entity = mapper.Map<AccountTransactionRequest, AccountTransaction>(request.Model);
        entity.ReferenceNumber = Guid.NewGuid().ToString();

        var entityResult = await dbContext.AddAsync(entity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        var account = await dbContext.Set<Account>()
            .FirstOrDefaultAsync(x => 
                x.AccountNumber == entity.AccountId, cancellationToken);
        
        if(account is not null) 
            await UpdateAccountBalanceAsync(account, entity.Amount, cancellationToken);

        var mapped = mapper.Map<AccountTransaction, AccountTransactionResponse>(entityResult.Entity);
        return new ApiResponse<AccountTransactionResponse>(mapped);
    }

    private async Task UpdateAccountBalanceAsync(
        Account account, decimal amount, 
        CancellationToken cancellationToken)
    {   
        account!.Balance += amount; 
        dbContext.Update(account);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<ApiResponse> Handle(UpdateAccountTransactionCommand request, CancellationToken cancellationToken)
    {
        var fromDb = await dbContext.Set<AccountTransaction>().Where(x => x.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);
        if (fromDb == null)
        {
            return new ApiResponse("Record not found");
        }
        
        
        await dbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }

    public async Task<ApiResponse> Handle(DeleteAccountTransactionCommand request, CancellationToken cancellationToken)
    {
        var fromDb = await dbContext.Set<AccountTransaction>().Where(x => x.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);
        
        if (fromDb == null)
        {
            return new ApiResponse("Record not found");
        }
        //dbContext.Set<AccountTransaction>().Remove(fromDb);
        
        fromDb.IsActive = false;
        await dbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }
}