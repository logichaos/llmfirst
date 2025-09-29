using FluentValidation;

namespace TodoApp.Application.Friendships.Commands.RespondToFriendRequest;

public sealed class RespondToFriendRequestCommandValidator : AbstractValidator<RespondToFriendRequestCommand>
{
    public RespondToFriendRequestCommandValidator()
    {
        RuleFor(x => x.FriendshipId)
            .NotEmpty();

        RuleFor(x => x.ResponderUserId)
            .NotEmpty();
    }
}
