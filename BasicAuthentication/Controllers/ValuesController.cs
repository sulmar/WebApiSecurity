﻿using BasicAuthentication.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BasicAuthentication.Controllers
{
    [CustomAuthenticationFilterAttribute] // Enable authentication via an ASP.NET Identity user name and password
    [Authorize]
    //[BasicAuthenticationFilter]
    public class ValuesController : ApiController
    {
        // GET api/values
       
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
