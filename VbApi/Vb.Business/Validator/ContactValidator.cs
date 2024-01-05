using System.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Vb.Data;
using Vb.Data.Entity;
using Vb.Schema;

namespace Vb.Business.Validator;

public class CreateContactValidator : AbstractValidator<ContactRequest>
{
    private readonly VbDbContext vbDbContext;
    public CreateContactValidator(VbDbContext vbDbContext)
    {
        {
            this.vbDbContext = vbDbContext;
        }
         
        RuleFor(x => x.CustomerId).NotNull().NotEmpty();

        RuleFor(x => x.Information).NotNull().NotEmpty()
            .Must((information) => 
                !IsUnique(Information: information));

        RuleFor(x => x.ContactType).NotNull().NotEmpty()
            .MaximumLength(10);
       /*  RuleFor(x => new { x.Information, x.ContactType })
            .Must((information, contactType) => 
                !IsUnique()); */
        RuleFor(x => x.ContactType).NotNull().NotEmpty();
        RuleFor(x => x.Id).NotNull().NotEmpty()
            .Must((id) => 
                !IsUnique(Id: id));
    }

    private bool IsUnique(string? Information = null, int? Id = null) =>
        vbDbContext.Set<Contact>().Any(e => 
            string.Equals(e.Information, Information, StringComparison.OrdinalIgnoreCase) || 
            e.Id == Id);
    
}
