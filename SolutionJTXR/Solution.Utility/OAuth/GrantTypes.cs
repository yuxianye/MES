using System.Collections.Generic;

namespace Solution.Utility.OAuth
{
    public static class GrantTypes
    {
        private const string KeyName = "grant_type";

        public static KeyValuePair<string, string> ClientCredentials
        {
            get { return new KeyValuePair<string, string>(KeyName, "client_credentials"); }
        }

        public static KeyValuePair<string, string> RefreshToken
        {
            get { return new KeyValuePair<string, string>(KeyName, "refresh_token"); }
        }

        public static KeyValuePair<string, string> OwnerCredentials
        {
            get { return new KeyValuePair<string, string>(KeyName, "password"); }
        }
    }
}
