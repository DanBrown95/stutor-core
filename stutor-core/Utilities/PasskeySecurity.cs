using BC = BCrypt.Net.BCrypt;

namespace stutor_core.Utilities
{
    public static class PasskeySecurity
    {
        public static string Hash(string passkey)
        {
            // hash passkey
            return BC.HashPassword(passkey);
        }

        public static bool Authenticate(string userPasskey, string hashedPasskey)
        {
            // check account found and verify password
            return (BC.Verify(userPasskey, hashedPasskey));
        }
    }
}
