namespace TodoApp.Application.Contracts;

public sealed record SharedListEntryDto(Guid SharedWithUserId, string Permission);
