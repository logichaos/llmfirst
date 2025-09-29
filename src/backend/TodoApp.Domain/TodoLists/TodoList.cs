using TodoApp.Domain.Shared;
using TodoApp.Domain.TodoLists.Entities;
using TodoApp.Domain.TodoLists.Events;

namespace TodoApp.Domain.TodoLists;

public sealed class TodoList : AggregateRoot<TodoListId>
{
    private readonly List<TodoItem> _items = new();
    private readonly List<SharedList> _sharedWith = new();

    private TodoList(TodoListId id, UserId ownerId, string title)
    {
        Id = id;
        OwnerId = ownerId;
        Title = title;
    }

    public UserId OwnerId { get; }
    public string Title { get; private set; }

    public IReadOnlyCollection<TodoItem> Items => _items.AsReadOnly();
    public IReadOnlyCollection<SharedList> SharedWith => _sharedWith.AsReadOnly();

    public static TodoList Create(UserId ownerId, string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new DomainException("Todo list title cannot be empty");
        }

        var normalized = title.Trim();
        var list = new TodoList(TodoListId.New(), ownerId, normalized);
        list.RaiseDomainEvent(new TodoListCreatedDomainEvent(list.Id, list.OwnerId, list.Title));
        return list;
    }

    public TodoItem AddItem(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            throw new DomainException("Todo item description cannot be empty");
        }

        var normalized = description.Trim();
        var item = TodoItem.Create(TodoItemId.New(), normalized);
        _items.Add(item);
        RaiseDomainEvent(new TodoItemAddedDomainEvent(Id, item.Id, item.Description));
        return item;
    }

    public void ToggleItem(TodoItemId todoItemId, bool isCompleted)
    {
        var item = _items.SingleOrDefault(i => i.Id == todoItemId)
            ?? throw new DomainException($"Todo item {todoItemId} was not found in list {Id}");

        item.SetCompletion(isCompleted);
        RaiseDomainEvent(new TodoItemCompletionChangedDomainEvent(Id, item.Id, item.IsCompleted));
    }

    public void ShareWith(UserId sharedWith, SharePermission permission)
    {
        if (_sharedWith.Any(s => s.SharedWith == sharedWith))
        {
            var existing = _sharedWith.Single(s => s.SharedWith == sharedWith);
            existing.UpdatePermission(permission);
            return;
        }

    var shared = SharedList.Create(Id, OwnerId, sharedWith, permission);
        _sharedWith.Add(shared);
        RaiseDomainEvent(new TodoListSharedDomainEvent(Id, OwnerId, sharedWith, permission));
    }

    public void Rename(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new DomainException("Todo list title cannot be empty");
        }

        Title = title.Trim();
        RaiseDomainEvent(new TodoListRenamedDomainEvent(Id, Title));
    }

    public bool CanView(UserId userId)
        => OwnerId == userId || _sharedWith.Any(s => s.SharedWith == userId);

    public bool CanEdit(UserId userId)
        => OwnerId == userId || _sharedWith.Any(s => s.SharedWith == userId && s.Permission == SharePermission.Editor);

    public static TodoList Restore(
        TodoListId id,
        UserId ownerId,
        string title,
        IEnumerable<TodoItem> items,
        IEnumerable<SharedList> sharedWith)
    {
        var list = new TodoList(id, ownerId, title);

        foreach (var item in items)
        {
            list._items.Add(item);
        }

        foreach (var shared in sharedWith)
        {
            list._sharedWith.Add(shared);
        }

        list.ClearDomainEvents();
        return list;
    }
}
