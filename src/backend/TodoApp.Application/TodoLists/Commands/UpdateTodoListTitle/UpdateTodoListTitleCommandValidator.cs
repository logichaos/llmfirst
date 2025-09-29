using FluentValidation;

namespace TodoApp.Application.TodoLists.Commands.UpdateTodoListTitle;

public sealed class UpdateTodoListTitleCommandValidator : AbstractValidator<UpdateTodoListTitleCommand>
{
    public UpdateTodoListTitleCommandValidator()
    {
        RuleFor(x => x.OwnerUserId)
            .NotEmpty();

        RuleFor(x => x.TodoListId)
            .NotEmpty();

        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(200);
    }
}
