using TodoApp.Application.Contracts;
using TodoApp.Domain.Friendships;
using TodoApp.Domain.Shared;
using TodoApp.Domain.TodoLists;
using TodoApp.Domain.TodoLists.Entities;
using TodoApp.Domain.Users;

namespace TodoApp.Application.Mappings;

internal static class DtoMappings
{
    public static UserDto ToDto(this User user)
        => new(user.Id.Value, user.Email.Value, user.DisplayName);

    public static TodoItemDto ToDto(this TodoItem item)
        => new(item.Id.Value, item.Description, item.IsCompleted);

    public static SharedListEntryDto ToDto(this SharedList shared)
        => new(shared.SharedWith.Value, shared.Permission.ToString());

    public static TodoListDto ToDto(this TodoList list, bool isOwner)
        => new(
            list.Id.Value,
            list.Title,
            list.Items.Select(ToDto).ToArray(),
            list.SharedWith.Select(ToDto).ToArray(),
            isOwner);

    public static FriendDto ToDto(this Friendship friendship, User friend)
    {
        var status = friendship.Status.ToString();
        return new FriendDto(friendship.Id.Value, friend.Id.Value, friend.Email.Value, friend.DisplayName, status);
    }
}
