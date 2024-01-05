using System.Data;
using FluentValidation;
using Vb.Data;
using Vb.Data.Entity;
using Vb.Schema;

namespace Vb.Business.Validator;

public class CreateAddressValidator : AbstractValidator<AddressRequest>
{
    private readonly VbDbContext vbDbContext;
    public CreateAddressValidator(VbDbContext vbDbContext)
    {
        {
            this.vbDbContext = vbDbContext;
        }
        RuleFor(x => x.PostalCode)
            .NotEmpty()
            .MaximumLength(6)
            .MinimumLength(6)
            .WithName("Zip code or postal code");

        RuleFor(x => x.Address1)
            .NotEmpty()
            .MinimumLength(20)
            .MaximumLength(100)
            .WithName("Customer address line 1");

        RuleFor(x => x.Address2)
            .NotEmpty()
            .MaximumLength(100)
            .WithName("Customer address line 2");

        RuleFor(x => x.Id).NotNull().NotEmpty()
            .Must((id) =>
            !IsUnique(Id: id));
    }
    private bool IsUnique(string? Information = null, int? Id = null) =>
        vbDbContext.Set<Address>().Any(e => e.Id == Id);
}