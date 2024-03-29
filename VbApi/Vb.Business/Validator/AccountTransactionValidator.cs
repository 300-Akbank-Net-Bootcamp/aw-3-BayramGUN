using System.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Vb.Data;
using Vb.Data.Entity;
using Vb.Schema;

namespace Vb.Business.Validator;

public class CreateAccountTransactionValidator : AbstractValidator<AccountTransactionRequest>
{
    private readonly VbDbContext vbDbContext;
    public CreateAccountTransactionValidator(VbDbContext vbDbContext)
    {
        {
            this.vbDbContext = vbDbContext;
        }
        
        RuleFor(x => x.Id).NotNull().NotEmpty()
            .Must((model, id) => !IsUnique(Id: id));

        RuleFor(x => x.ReferenceNumber).NotNull().NotEmpty()
            .MaximumLength(50)
            .Must((model, referenceNumber) => 
                !IsUnique(ReferenceNumber: referenceNumber));

        RuleFor(x => x.AccountId).NotNull().NotEmpty()
            .Must((model, accountId) => 
                !IsUnique(AccountId: accountId));

        RuleFor(x => x.Amount).NotNull()
            .NotEmpty()
            .InclusiveBetween(
                (decimal)0.0000, (decimal)999999999999999999.9999)
            .ScalePrecision(4, 18, false);

        RuleFor(x => x.Description).MaximumLength(300);

        RuleFor(x => x.TransferType).NotNull().NotEmpty()
            .MaximumLength(10);

        RuleFor(x => x.TransactionDate)
            .NotNull()
            .NotEmpty();
        
    }

    private bool IsUnique(string? ReferenceNumber = null, int? Id = null, int? AccountId = null) =>
        vbDbContext.Set<AccountTransaction>().Any(e => e.Id == Id ||
            e.AccountId == AccountId ||
            string.Equals(ReferenceNumber, e.ReferenceNumber));
    
    
}
