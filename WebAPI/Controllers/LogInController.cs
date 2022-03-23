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
            //Converting userData to User
            User user = JsonConvert.DeserializeObject<User>(userData.user.ToString());
            //find the user
            User u = db.User.Where(e => e.email == user.email).FirstOrDefault();
            
            if(u.email != null && u.password != null && u.password == user.password)
            {
                return Request.CreateResponse(HttpStatusCode.OK, user);
                
            }
            return Request.CreateResponse(HttpStatusCode.NotFound, "LogIn - Wrong Email or Password");
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "LogIn - Oops... Something went wrong");
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
