using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ClassLibrary2;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WebAPI.Controllers
{
    public class LogInController : ApiController
    {
        bgroup89_test2Entities db = new bgroup89_test2Entities();

        // GET: api/LogIn
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // POST: api/LogIn/5
        //recive email, password. return Ok + User
        public HttpResponseMessage Post(JObject userData)
        {
            try 
            { 
            //Converting userData to User
            User user = JsonConvert.DeserializeObject<User>(userData.ToString());
            //find the user
            User u = db.User.Where(e => e.email == user.email).FirstOrDefault();
            
            if(u.email != null && u.password != null && u.password == user.password)
            {
                return Request.CreateResponse(HttpStatusCode.OK,u);
            }
            return Request.CreateResponse(HttpStatusCode.NotFound,"");
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest,"");
            }
        }

        // POST: api/LogIn
        //public void Post([FromBody]string value)
        //{

        //}

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
