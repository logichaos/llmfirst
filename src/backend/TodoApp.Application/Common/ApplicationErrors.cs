using FluentResults;

namespace TodoApp.Application.Common;

public static class ApplicationErrors
{
    public static class Users
    {
        public static Error EmailAlreadyTaken(string email)
            => new Error($"Email '{email}' is already in use").WithMetadata("Code", "Users.EmailAlreadyTaken");

        public static Error NotFound
            => new Error("User was not found").WithMetadata("Code", "Users.NotFound");

        public static Error InvalidCredentials
            => new Error("Email or password is invalid").WithMetadata("Code", "Users.InvalidCredentials");
    }

    public static class TodoLists
    {
        public static Error NotFound
            => new Error("Todo list was not found").WithMetadata("Code", "TodoLists.NotFound");

        public static Error UnauthorizedAccess
            => new Error("You do not have access to this todo list").WithMetadata("Code", "TodoLists.UnauthorizedAccess");
    }

    public static class Friendships
    {
        public static Error AlreadyRequested
            => new Error("A friendship request already exists between these users").WithMetadata("Code", "Friendships.AlreadyRequested");

        public static Error NotFound
            => new Error("Friendship was not found").WithMetadata("Code", "Friendships.NotFound");

        public static Error NotAuthorized
            => new Error("Only the addressee can respond to this request").WithMetadata("Code", "Friendships.NotAuthorized");
    }
}
