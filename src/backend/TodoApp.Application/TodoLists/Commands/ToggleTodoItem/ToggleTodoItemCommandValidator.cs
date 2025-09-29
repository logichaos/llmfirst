using FluentValidation;

namespace TodoApp.Application.TodoLists.Commands.ToggleTodoItem;

public sealed class ToggleTodoItemCommandValidator : AbstractValidator<ToggleTodoItemCommand>
{
    public ToggleTodoItemCommandValidator()
    {
        RuleFor(x => x.ActorUserId)
            .NotEmpty();

        RuleFor(x => x.TodoListId)
            .NotEmpty();

        RuleFor(x => x.TodoItemId)
            .NotEmpty();
    }
}
