using FluentValidation;

namespace TodoApp.Application.TodoLists.Commands.DeleteTodoList;

public sealed class DeleteTodoListCommandValidator : AbstractValidator<DeleteTodoListCommand>
{
    public DeleteTodoListCommandValidator()
    {
        RuleFor(x => x.OwnerUserId)
            .NotEmpty();

        RuleFor(x => x.TodoListId)
            .NotEmpty();
    }
}
