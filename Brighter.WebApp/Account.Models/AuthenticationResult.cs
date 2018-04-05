namespace Account.Models
{
    public sealed class AuthenticationResult 
    {
        public AuthenticationResult(bool succeeded, bool isLockedOut, bool isNotAllowed, bool requiresTwoFactor)
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
