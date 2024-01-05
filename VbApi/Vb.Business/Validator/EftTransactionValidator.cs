using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Vb.Data;
using Vb.Data.Entity;
using Vb.Schema;

namespace Vb.Business.Validator;

public class CreateEftTransactionValidator : AbstractValidator<EftTransactionRequest>
{
    private readonly VbDbContext vbDbContext;
    public CreateEftTransactionValidator(VbDbContext vbDbContext)
    {
        {
            this.vbDbContext = vbDbContext;
        }

        RuleFor(x => x.ReferenceNumber).NotNull().NotEmpty()
            .MaximumLength(50)
            .Must((referenceNumber) => 
                !IsUnique(ReferenceNumber: referenceNumber));

        RuleFor(x => x.Id).NotNull().NotEmpty()
            .Must((id) => 
                !IsUnique(Id: id));
        RuleFor(x => x.Amount).NotNull()
            .NotEmpty()
            .InclusiveBetween(
                (decimal)0.0000, (decimal)999999999999999999.9999)
            .ScalePrecision(4, 18, false);
        RuleFor(x => x.Description).MaximumLength(300);

        RuleFor(x => x.TransactionDate)
            .NotNull()
            .NotEmpty();
    }

    private bool IsUnique(string? ReferenceNumber = null, int? Id = null) =>
        vbDbContext.Set<EftTransaction>().Any(e => e.Id == Id ||
            string.Equals(ReferenceNumber, e.ReferenceNumber));
    
    
}
