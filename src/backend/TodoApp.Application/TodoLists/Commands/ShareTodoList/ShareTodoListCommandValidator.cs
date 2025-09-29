using FluentValidation;

namespace TodoApp.Application.TodoLists.Commands.ShareTodoList;

public sealed class ShareTodoListCommandValidator : AbstractValidator<ShareTodoListCommand>
{
    public ShareTodoListCommandValidator()
    {
        RuleFor(x => x.OwnerUserId)
            .NotEmpty();

        RuleFor(x => x.TodoListId)
            .NotEmpty();

        RuleFor(x => x.FriendUserId)
            .NotEmpty();
    }
}
