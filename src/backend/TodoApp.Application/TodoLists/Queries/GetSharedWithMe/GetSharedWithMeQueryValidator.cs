using FluentValidation;

namespace TodoApp.Application.TodoLists.Queries.GetSharedWithMe;

public sealed class GetSharedWithMeQueryValidator : AbstractValidator<GetSharedWithMeQuery>
{
    public GetSharedWithMeQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();
    }
}
