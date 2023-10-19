using System.Security.Cryptography;

namespace Localidades.Application.Services;

public class PasswordHasherService
{

    private const int SALT_SIZE = 16;
    private const int KEY_SIZE = 32;
    private const int ITERATIONS = 10000;
    private static readonly HashAlgorithmName _hashAlgorithmName = HashAlgorithmName.SHA256;
    private const char DELIMITER = ';';

    public string Hash(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SALT_SIZE);
        var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, ITERATIONS, _hashAlgorithmName, KEY_SIZE);

        return string.Join(DELIMITER, Convert.ToBase64String(salt), Convert.ToBase64String(hash));
    }

    public bool Verify(string passwordHash, string inputPassword)
    {
        var elements = passwordHash.Split(DELIMITER);
        var salt = Convert.FromBase64String(elements[0]);
        var hash = Convert.FromBase64String(elements[1]);

        var hashInput = Rfc2898DeriveBytes.Pbkdf2(inputPassword, salt, ITERATIONS, _hashAlgorithmName, KEY_SIZE);

        return CryptographicOperations.FixedTimeEquals(hash, hashInput);
    }
}

