using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Vb.Data;
using Vb.Data.Entity;
using Vb.Schema;

namespace Vb.Business.Validator;

public class CreateAccountValidator : AbstractValidator<AccountRequest>
{
    private readonly VbDbContext vbDbContext;
    public CreateAccountValidator(VbDbContext vbDbContext)
    {
        {
            this.vbDbContext = vbDbContext;
        }
         
        RuleFor(x => x.CustomerId).NotNull().NotEmpty();
        RuleFor(x => x.AccountNumber).NotNull().NotEmpty()
        .Must((accountNumber) => 
            !IsUnique(AccountNumber: accountNumber)
        );
        RuleFor(x => x.IBAN).NotNull().NotEmpty()
        .Must((IBAN) => 
            !IsUnique(IBAN: IBAN)
        );
        RuleFor(x => x.Balance).NotNull()
            .NotEmpty()
            .InclusiveBetween(
                (decimal)-999999999999999999.9999, (decimal)999999999999999999.9999)
            .ScalePrecision(4, 18, false);
        RuleFor(x => x.CurrencyType).NotEmpty().MaximumLength(3);
        RuleFor(x => x.Name).MaximumLength(100);
    }

    private bool IsUnique(int? AccountNumber = null, string? IBAN = null) =>
        vbDbContext.Set<Account>().Any(e => e.AccountNumber == AccountNumber ||
            string.Equals(IBAN, e.IBAN));
    
}
