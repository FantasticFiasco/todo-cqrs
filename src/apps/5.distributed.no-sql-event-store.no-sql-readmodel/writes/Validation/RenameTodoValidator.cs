using System;
using FluentValidation;
using Todo;

namespace Writes.Validation
{
    public class RenameTodoValidator : AbstractValidator<RenameTodo>
    {
        public RenameTodoValidator()
        {
            RuleFor(self => self.Id).NotEqual(Guid.Empty);
            RuleFor(self => self.NewTitle).NotNull();
        }
    }
}
