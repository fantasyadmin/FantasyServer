using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using ClassLibrary2;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;

namespace WebAPI.Controllers
{
    public class ChangePasswordController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        bgroup89_prod_Entities db = new bgroup89_prod_Entities();
        // GET: api/ChangePassword
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET: api/ChangePassword/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/ChangePassword
        public HttpResponseMessage Post(JObject userhData)
        {
            User user = JsonConvert.DeserializeObject<User>(userhData.ToString());

            try
            {
                User u1 = db.User.Where(u => u.user_id == user.user_id).FirstOrDefault();

                if (u1 == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, $"User {user.user_id}, was not found");
                }

                u1.password = user.password;
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {

                logger.Error("Bad Request, could not find Match in League {match.league_id" + e);
                return Request.CreateResponse(HttpStatusCode.BadRequest, e);
            }
        }

        // PUT: api/ChangePassword/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ChangePassword/5
        public void Delete(int id)
        {
        }
    }
}
