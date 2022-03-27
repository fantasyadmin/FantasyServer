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
    public class EditPlayerController : ApiController
    {
        bgroup89_test2Entities db = new bgroup89_test2Entities();


        // GET: api/EditPlayer
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/EditPlayer/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/EditPlayer
        //Recive user_id, picture, nickname. return player

        public HttpResponseMessage Post(JObject userData)
        {
            try
            {
                Player player = JsonConvert.DeserializeObject<Player>(userData.ToString());

                if (player == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Fetching Data - Oops... Something Went Wrong!");
                }

                Player p1 = db.Player.Where(p => p.user_id == player.user_id).FirstOrDefault();

                if (player == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Fetching Data - Oops... Player not found!");
                }

                p1.nickname = player.nickname;
                p1.picture = player.picture;

                db.Player.Add(p1);

                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, p1);
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Creating Player - Oops... Something Went Wrong!");
            }
        }

        // PUT: api/EditPlayer/5
        //Recive user_id, picture, nickname. return player
        public HttpResponseMessage Put(JObject userData)
        {
            try
            {
                Player player = JsonConvert.DeserializeObject<Player>(userData.ToString());

                if (player == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Fetching Data - Oops... Something Went Wrong!");
                }

                Player p1 = db.Player.Where(p => p.user_id == player.user_id).FirstOrDefault();

                p1.nickname = player.nickname;
                //p1.picture = player.picture;

                db.Player.Add(p1);
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, p1);
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Creating Player - Oops... Something Went Wrong!");
            }
        }

        // DELETE: api/EditPlayer/5
        public void Delete(int id)
        {
        }
    }
}
