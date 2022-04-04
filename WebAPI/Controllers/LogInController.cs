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
    public class LogInController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        bgroup89_test2Entities db = new bgroup89_test2Entities();

        // POST: api/LogIn/5
        //recive email, password. return Ok + User
        public HttpResponseMessage Post(JObject userData)
        {
            logger.Trace("POST - LogInController");
            //Converting userData to User
            User user = JsonConvert.DeserializeObject<User>(userData.ToString());

            try
            {
                //find the user
                User u = db.User.Where(e => e.email == user.email).FirstOrDefault();
                Player p = db.Player.Where(a => a.user_id == u.user_id).FirstOrDefault();
                //u.Fantasy_team = null;


                if (u.email != null && u.password != null && u.password == user.password)
                {
                    logger.Trace("POST - DB connection by - " + user.email + " returned - " + u.email);

                    return Request.CreateResponse(HttpStatusCode.OK, new { u.user_id, p.nickname, p.picture }, JsonMediaTypeFormatter.DefaultMediaType);
                }

                logger.Info("POST - DB connection by - " + user.email + " returned - " + u.email);
                return Request.CreateResponse(HttpStatusCode.NotFound, "User not found, Check your email or password");
            }
            catch (Exception e)
            {
                logger.Error("Bad Request, data received = " + user.email + " | =======> " + e);
                return Request.CreateResponse(HttpStatusCode.BadRequest, e);
            }
        }


        //public void Post([FromBody]string value)
        //{

        //}

        // PUT: api/LogIn/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/LogIn/5
        public void Delete(int id)
        {
        }
    }
}
