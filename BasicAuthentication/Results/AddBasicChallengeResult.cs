using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace BasicAuthentication.Results
{
    public class AddBasicChallengeResult : IHttpActionResult
    {
        private IHttpActionResult innerResult;
        private string realm;

        public AddBasicChallengeResult(IHttpActionResult innerResult, string realm)
        {
            this.innerResult = innerResult;
            this.realm = realm;
        }

        public async Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = await innerResult.ExecuteAsync(cancellationToken);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
                response.Headers.WwwAuthenticate.Add(new AuthenticationHeaderValue("Basic", String.Format("realm=\"{0}\"", realm)));
            return response;
        }
    }
}