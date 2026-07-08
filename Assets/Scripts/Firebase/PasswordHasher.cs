using System;
using System.Security.Cryptography;

/// <summary>
/// Hash de contraseñas con PBKDF2 (salt por usuario). Reemplaza el guardado en
/// texto plano que tenía SaveSystem con JSON local.
/// </summary>
public static class PasswordHasher
{
    private const int SaltSize = 16;
    private const int HashSize = 32;
    private const int Iterations = 100_000;

    public static (string hash, string salt) Hash(string password)
    {
        byte[] saltBytes = new byte[SaltSize];
        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(saltBytes);
        }

        byte[] hashBytes = Derive(password, saltBytes);
        return (Convert.ToBase64String(hashBytes), Convert.ToBase64String(saltBytes));
    }

    public static bool Verify(string password, string hash, string salt)
    {
        if (string.IsNullOrEmpty(hash) || string.IsNullOrEmpty(salt)) return false;

        byte[] saltBytes = Convert.FromBase64String(salt);
        byte[] computedHash = Derive(password, saltBytes);
        byte[] storedHash = Convert.FromBase64String(hash);
        return ConstantTimeEquals(computedHash, storedHash);
    }

    private static byte[] Derive(string password, byte[] salt)
    {
        using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
        return pbkdf2.GetBytes(HashSize);
    }

    private static bool ConstantTimeEquals(byte[] a, byte[] b)
    {
        if (a.Length != b.Length) return false;
        int diff = 0;
        for (int i = 0; i < a.Length; i++)
        {
            diff |= a[i] ^ b[i];
        }
        return diff == 0;
    }
}
