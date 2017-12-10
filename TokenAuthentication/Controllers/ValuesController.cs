using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace TokenAuthentication.Controllers
{
    public class ValuesController : ApiController
    {
        [Authorize]
        public IHttpActionResult Get()
        {
            // .Identity.Name
            return Ok(new string[] { "value1", "value2" });
        }


        [Authorize(Roles = "admin")]
        public IHttpActionResult Get(int id)
        {
            return Ok($"value{id}");
        }
    }
}