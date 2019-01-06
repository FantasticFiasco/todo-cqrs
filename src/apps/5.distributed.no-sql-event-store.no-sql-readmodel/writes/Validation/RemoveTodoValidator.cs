using System;
using FluentValidation;
using Todo;

namespace Writes.Validation
{
    public class RemoveTodoValidator : AbstractValidator<RemoveTodo>
    {
        public RemoveTodoValidator()
        {
            RuleFor(self => self.Id).NotEqual(Guid.Empty);
        }
    }
}
