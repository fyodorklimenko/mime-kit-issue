using Org.BouncyCastle.OpenSsl;
using System;

namespace MimeKitTests
{
    public class TestPasswordFinder : IPasswordFinder
    {
        private readonly string _password;

        public TestPasswordFinder(string password)
        {
            _password = string.IsNullOrWhiteSpace(password)
                ? throw new ArgumentNullException(nameof(password))
                : password;
        }

        public char[] GetPassword()
        {
            return _password.ToCharArray();
        }
    }
}
