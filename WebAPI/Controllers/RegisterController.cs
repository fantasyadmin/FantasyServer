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

namespace WebAPI.Controllers
{
    public class RegisterController : ApiController
    {
        bgroup89_test2Entities db = new bgroup89_test2Entities();
        //GET: api/Register
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // POST: api/Register
        //Recieve userName, email, Password
        public HttpResponseMessage Post(/*JObject confirmationCode, JObject userCodeInput, */JObject userData)
        {
            try
            {
                //Converting userData to User
                User user = JsonConvert.DeserializeObject<User>(userData.ToString());
                Player player = JsonConvert.DeserializeObject<Player>(userData.ToString());

                if (user == null || player == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Fetching user input - Oops... Something Went Wrong!");
                }

                User u = new User() { email = user.email, password = user.password };
                Player p = new Player() { nickname = player.nickname, user_id = u.user_id };

                db.User.Add(u);
                db.Player.Add(p);
                
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, u);
            }
            catch
            {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Creating Player - Oops... Something Went Wrong!");   
            }
        }

        // PUT: api/Register/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Register/5
        public void Delete(int id)
        {

        }
    }
}
