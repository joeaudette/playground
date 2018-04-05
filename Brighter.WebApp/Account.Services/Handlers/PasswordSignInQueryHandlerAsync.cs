using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Account.Models.Queries;
using Brighter.WebApp.Models;
using Paramore.Darker;
using Paramore.Darker.Attributes;
using Microsoft.AspNetCore.Identity;
using Paramore.Brighter;
using Paramore.Brighter.Logging.Attributes;
using Account.Models;

namespace Account.Services.Handlers
{
    public class PasswordSignInQueryHandlerAsync : QueryHandlerAsync<PasswordSignInQuery, AuthenticationResult>
    {
        public PasswordSignInQueryHandlerAsync(
            SignInManager<ApplicationUser> signInManager
            )
        {
            _signInManager = signInManager;
        }

        private SignInManager<ApplicationUser> _signInManager;

        [RequestLoggingAsync(step: 1, timing: HandlerTiming.Before)]
        //[RetryableQuery(2)]
        public override async Task<AuthenticationResult> ExecuteAsync(PasswordSignInQuery request, CancellationToken cancellationToken = default(CancellationToken))
        {
            var identityResult = await _signInManager.PasswordSignInAsync(request.Email, request.Password, request.RememberMe, lockoutOnFailure: false);
            return new AuthenticationResult(identityResult.Succeeded, identityResult.IsLockedOut, identityResult.IsNotAllowed, identityResult.RequiresTwoFactor);

        }

    }
}
