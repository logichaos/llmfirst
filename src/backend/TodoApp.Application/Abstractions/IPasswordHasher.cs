namespace TodoApp.Application.Abstractions;

public interface PasswordHasher
{
    string Hash(string plaintext);
    bool Verify(string plaintext, string hash);
}
