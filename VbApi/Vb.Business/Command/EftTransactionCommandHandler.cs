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

public class EftTransactionCommandHandler :
    IRequestHandler<CreateEftTransactionCommand, ApiResponse<EftTransactionResponse>>,
    IRequestHandler<UpdateEftTransactionCommand, ApiResponse>,
    IRequestHandler<DeleteEftTransactionCommand, ApiResponse>

{
    private readonly VbDbContext dbContext;
    private readonly IMapper mapper;

    public EftTransactionCommandHandler(
        VbDbContext dbContext,
        IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    public async Task<ApiResponse<EftTransactionResponse>> Handle(CreateEftTransactionCommand request, CancellationToken cancellationToken)
    {
        var checkIdentity = await dbContext.Set<EftTransaction>().Where(x => x.ReferenceNumber == request.Model.ReferenceNumber)
            .FirstOrDefaultAsync(cancellationToken);
        if (checkIdentity != null)
        {
            return new ApiResponse<EftTransactionResponse>($"{request.Model.ReferenceNumber} is used by another EftTransaction.");
        }
        var entity = mapper.Map<EftTransactionRequest, EftTransaction>(request.Model);
        entity.ReferenceNumber = Guid.NewGuid().ToString();

        var entityResult = await dbContext.AddAsync(entity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        var senderAccount = await dbContext.Set<Account>()
            .FirstOrDefaultAsync(x => 
                x.IBAN == entity.SenderIban, cancellationToken);

        var receiverAccount = await dbContext.Set<Account>()
            .FirstOrDefaultAsync(x => 
                x.AccountNumber == entity.AccountId, cancellationToken);
        
        if(senderAccount is not null && 
            receiverAccount is not null )
            await UpdateEftBalanceAsync(senderAccount, entity.Amount, receiverAccount, cancellationToken);

        var mapped = mapper.Map<EftTransaction, EftTransactionResponse>(entityResult.Entity);
        return new ApiResponse<EftTransactionResponse>(mapped);
    }

    private async Task UpdateEftBalanceAsync(
        Account eftSender, decimal amount, Account eftRecipient,
        CancellationToken cancellationToken)
    {   
        eftSender!.Balance -= amount; 
        eftRecipient!.Balance += amount;
        dbContext.Update(eftSender);
        await Task.Delay(2000, cancellationToken);
        dbContext.Update(eftRecipient);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<ApiResponse> Handle(UpdateEftTransactionCommand request, CancellationToken cancellationToken)
    {
        var fromDb = await dbContext.Set<EftTransaction>().Where(x => x.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);
        if (fromDb == null)
        {
            return new ApiResponse("Record not found");
        }
        
        
        await dbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }

    public async Task<ApiResponse> Handle(
        DeleteEftTransactionCommand request,
        CancellationToken cancellationToken)
    {
        var fromDb = await dbContext.Set<EftTransaction>()
                                    .Where(x => x.Id == request.Id)
                                    .FirstOrDefaultAsync(cancellationToken);
        
        if (fromDb == null)
        {
            return new ApiResponse("Record not found");
        }
        //dbContext.Set<EftTransaction>().Remove(fromDb);
        
        fromDb.IsActive = false;
        await dbContext.SaveChangesAsync(cancellationToken);
        return new ApiResponse();
    }
}