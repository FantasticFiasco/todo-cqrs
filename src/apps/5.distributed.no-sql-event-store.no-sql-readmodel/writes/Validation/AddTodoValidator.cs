using System;
using FluentValidation;
using Todo;

namespace Writes.Validation
{
    public class AddTodoValidator : AbstractValidator<AddTodo>
    {
        public AddTodoValidator()
        {
            RuleFor(self => self.Id).NotEqual(Guid.Empty);
            RuleFor(self => self.Title).NotNull();
        }
    }
}
