using System;
using FluentValidation;
using Todo;

namespace Writes.Validation
{
    public class CompleteTodoValidator : AbstractValidator<CompleteTodo>
    {
        public CompleteTodoValidator()
        {
            RuleFor(self => self.Id).NotEqual(Guid.Empty);
        }
    }
}
