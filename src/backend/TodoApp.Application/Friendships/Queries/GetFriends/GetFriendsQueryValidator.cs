using FluentValidation;

namespace TodoApp.Application.Friendships.Queries.GetFriends;

public sealed class GetFriendsQueryValidator : AbstractValidator<GetFriendsQuery>
{
    public GetFriendsQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();
    }
}
