using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ClassLibrary2;
using Newtonsoft.Json;


namespace WebAPI.Controllers
{
    public class LogInController : ApiController
    {
        bgroup89_test2Entities db = new bgroup89_test2Entities();

        // GET: api/LogIn
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/LogIn/5
        public HttpResponseMessage Get(dynamic userData)
        {
            try 
            { 
            User user = JsonConvert.DeserializeObject<User>(userData.ToString());
            User userEmail = db.User.Where(e => e.email.Equals(user.email)).FirstOrDefault();
            User userPassword = db.User.Where(p => p.password.Equals(user.password)).FirstOrDefault();
            if(userEmail != null && userPassword != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, user);
                
            }
            return Request.CreateResponse(HttpStatusCode.NotFound, "");
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "");
            }
        }

        // POST: api/LogIn
        public void Post([FromBody]string value)
        {

        }

        // PUT: api/LogIn/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/LogIn/5
        public void Delete(int id)
        {
        }
    }
}
