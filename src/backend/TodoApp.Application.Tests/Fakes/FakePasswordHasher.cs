using TodoApp.Application.Abstractions;

namespace TodoApp.Application.Tests.Fakes;

internal sealed class FakePasswordHasher : PasswordHasher
{
    public string Hash(string plaintext)
        => $"HASH::{plaintext}";

    public bool Verify(string plaintext, string hash)
        => hash == Hash(plaintext);
}
