using FluentValidation;

namespace TodoApp.Application.Friendships.Commands.SendFriendRequest;

public sealed class SendFriendRequestCommandValidator : AbstractValidator<SendFriendRequestCommand>
{
    public SendFriendRequestCommandValidator()
    {
        RuleFor(x => x.RequesterUserId)
            .NotEmpty();

        RuleFor(x => x.FriendEmail)
            .NotEmpty()
            .EmailAddress();
    }
}
