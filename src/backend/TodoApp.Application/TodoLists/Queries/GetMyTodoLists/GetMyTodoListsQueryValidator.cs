using FluentValidation;

namespace TodoApp.Application.TodoLists.Queries.GetMyTodoLists;

public sealed class GetMyTodoListsQueryValidator : AbstractValidator<GetMyTodoListsQuery>
{
    public GetMyTodoListsQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();
    }
}
