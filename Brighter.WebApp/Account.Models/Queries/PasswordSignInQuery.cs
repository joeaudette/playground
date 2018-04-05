using Paramore.Darker;

namespace Account.Models.Queries
{
    public class PasswordSignInQuery : IQuery<PasswordSignInQuery.Result>
    {
        public string Email { get; }
        public string Password { get; }
        public bool RememberMe { get; }
        
        public PasswordSignInQuery(
            string email,
            string password,
            bool rememberMe
            )
        {
            Email = email;
            Password = password;
            RememberMe = rememberMe;
           
        }

        public sealed class Result //: IQueryResponse
        {
            public Result(bool succeeded, bool isLockedOut, bool isNotAllowed, bool requiresTwoFactor)
            {
                Succeeded = succeeded;
                IsLockedOut = isLockedOut;
                IsNotAllowed = isNotAllowed;
                RequiresTwoFactor = requiresTwoFactor;
            }

            //
            // Summary:
            //     Returns a flag indication whether the sign-in was successful.
            public bool Succeeded { get; }
            //
            // Summary:
            //     Returns a flag indication whether the user attempting to sign-in is locked out.
            public bool IsLockedOut { get; }
            //
            // Summary:
            //     Returns a flag indication whether the user attempting to sign-in is not allowed
            //     to sign-in.
            public bool IsNotAllowed { get; }
            //
            // Summary:
            //     Returns a flag indication whether the user attempting to sign-in requires two
            //     factor authentication.
            public bool RequiresTwoFactor { get; }

        }
    }
}
