using System;
using FluentValidation;
using Todo;

namespace Writes.Validation
{
    public class IncompleteTodoValidator : AbstractValidator<IncompleteTodo>
    {
        public IncompleteTodoValidator()
        {
            RuleFor(self => self.Id).NotEqual(Guid.Empty);
        }
    }
}
