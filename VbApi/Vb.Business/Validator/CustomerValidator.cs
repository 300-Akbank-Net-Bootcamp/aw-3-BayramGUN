using FluentValidation;
using Vb.Data;
using Vb.Data.Entity;
using Vb.Schema;

namespace Vb.Business.Validator;

public class CreateCustomerValidator : AbstractValidator<CustomerRequest>
{
    private readonly VbDbContext vbDbContext;
    public CreateCustomerValidator(VbDbContext vbDbContext)
    {
        {
            this.vbDbContext = vbDbContext;
        }
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.IdentityNumber).NotEmpty().MaximumLength(11).WithName("Customer tax or identity number").Must((identityNumber) => 
        IsUnique(identityNumber));
        RuleFor(x => x.DateOfBirth).NotEmpty();

        RuleForEach(x => x.Addresses).SetValidator(new CreateAddressValidator());
    }

    private bool IsUnique(string identityNumber) =>
        vbDbContext.Set<Customer>().Any(e => e.IdentityNumber == identityNumber);

}