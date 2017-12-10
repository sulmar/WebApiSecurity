using BasicAuthentication.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Filters;

namespace BasicAuthentication.Filters
{
    public class CustomAuthenticationFilterAttribute : Attribute, IAuthenticationFilter
    {
        public bool AllowMultiple => false;

        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            HttpRequestMessage request = context.Request;
            AuthenticationHeaderValue authorization = request.Headers.Authorization;

            if (authorization == null)
            {
                return;
            }

            if (authorization.Scheme != "Basic")
            {
                return;
            }

            var userNameAndPasword = ExtractUserNameAndPassword(authorization.Parameter);

            IPrincipal principal = await AuthenticateAsync(userNameAndPasword.username, userNameAndPasword.password, cancellationToken);

            if (principal == null)
            {
                context.ErrorResult = new AuthenticationFailureResult("Invalid credentials", request);
            }
            else
            {
                context.Principal = principal;
            }

            


        }

        private async Task<IPrincipal> AuthenticateAsync(string username, string password, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (username != "testuser" || password != "Pass1word")
            {
                // No user with userName/password exists.
                return null;
            }

            Claim nameClaim = new Claim(ClaimTypes.Name, username);
            List<Claim> claims = new List<Claim> { nameClaim };

            ClaimsIdentity identity = new ClaimsIdentity(claims, "Basic");

            var principal = new ClaimsPrincipal(identity);

            return principal;
        }

        private static (string username, string password) ExtractUserNameAndPassword(string authorizationParameter)
        {
            var credentialBytes = Convert.FromBase64String(authorizationParameter);

            Encoding encoding = Encoding.ASCII;
            // Make a writable copy of the encoding to enable setting a decoder fallback.
            encoding = (Encoding)encoding.Clone();

            var decodedCredentials = encoding.GetString(credentialBytes);

            var array = decodedCredentials.Split(':');

            string username = array[0];
            string password = array[1];

            return (username, password);

        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            context.Result = new AddBasicChallengeResult(context.Result, "Basic");

            return Task.FromResult(0);

        }
    }
}