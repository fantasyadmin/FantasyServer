using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ClassLibrary2;
using Newtonsoft.Json;
using System.Net.Mail;
using Newtonsoft.Json.Linq;
using System.Net.Http.Formatting;
using NLog;

namespace WebAPI.Controllers
{
    public class RegisterController : ApiController
    {

        private static Logger logger = LogManager.GetCurrentClassLogger();

        bgroup89_test2Entities db = new bgroup89_test2Entities();

        // POST: api/Register
        public HttpResponseMessage Post(int confirmationCode, int userCodeInput, dynamic userData)
        {
            logger.Trace("POST - RegisterController");


            //Converting userData to User
            User user = JsonConvert.DeserializeObject<User>(userData.ToString());
            User u = new User() { email = user.email, password = user.password };

            Player player = JsonConvert.DeserializeObject<Player>(userData.ToString());
            Player p = new Player() { nickname = player.nickname, user_id = u.user_id };


            try
            {


                if (user == null || player == null)
                {
                    logger.Error("POST - Empty reference - user: " + user + " | player: " + player);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Fetching user input - Oops... Something Went Wrong!");
                }

                User u1 = db.User.Where(a => a.email == user.email).FirstOrDefault();
                logger.Trace("POST - DB connection by - " + user.email + "returned - " + u.email);

                if (u1 != null)
                {
                    logger.Error("POST - Occupied Value in - user: " + u1);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Email already exist");
                }

                db.User.Add(u);
                logger.Trace("User added to DB" + u.email);
                db.Player.Add(p);
                logger.Trace("Player added to DB" + p.nickname);
                db.SaveChanges();
                logger.Trace("Changes Saved to DB - SUCCESS!");
                return Request.CreateResponse(HttpStatusCode.OK, u);
            }
            catch (Exception e)
            {
                logger.Error("Bad Request, data received = user: " + u + " | player: " + p + ", " + e);
                return Request.CreateResponse(HttpStatusCode.BadRequest, e);
            }
        }

        // PUT: api/Register/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/Register/5
        public void Delete(int id)
        {
        }
    }
}
