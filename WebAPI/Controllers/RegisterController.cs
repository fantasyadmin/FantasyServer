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
    public class RegisterController : ApiController
    {

        bgroup89_test2Entities db = new bgroup89_test2Entities();

        // GET: api/Register
        public IEnumerable<string> Get()
        {
            return new string[] { "value1111", "value2" };
        }

        // GET: api/Register/email,pass
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Register
        public HttpResponseMessage Post(dynamic userData)
        {
            try
            {
                //
                User user = JsonConvert.DeserializeObject<User>(userData.user.ToString());

                if (user == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "User");
                }

                Player player = JsonConvert.DeserializeObject<Player>(userData.player.ToString());

                if (player == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Player");
                }

                User u = new User() { email = user.email, password = user.password };
                db.User.Add(u);

                Player newPlayer = player;
                newPlayer.user_id = u.user_id;
                db.Player.Add(newPlayer);
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, user.user_id, player.nickname);
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "");
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
