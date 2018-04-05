using MediatR;
//using Paramore.Darker;

namespace Features.Account
{
    //public class PasswordSignInQuery : IQuery<AuthenticationResult>
    public class PasswordSignInQuery : IRequest<AuthenticationResult>
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

        
    }
}
