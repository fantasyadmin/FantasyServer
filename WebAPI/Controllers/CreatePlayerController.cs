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
    public class CreatePlayerController : ApiController
    {
        bgroup89_test2Entities db = new bgroup89_test2Entities();
        // GET: api/CreatePlayer
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET: api/CreatePlayer/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/CreatePlayer
        public HttpResponseMessage Post(JObject userData)
        {
            try
            {
                //
                Player player = JsonConvert.DeserializeObject<Player>(userData.ToString());

                if (player == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Fetching Data - Oops... Something Went Wrong!");
                }

                Player p = new Player() { 
                    nickname = player.nickname,
                    picture = player.picture,
                    player_score = player.player_score,
                    total_assists = player.total_assists,
                    games_played = player.games_played,
                    total_goals_recieved = player.total_goals_recieved,
                    total_goals_scored = player.total_goals_scored,
                    total_pen_missed = player.total_pen_missed,
                    total_wins = player.total_wins};
                db.Player.Add(p);
                db.SaveChanges();
                
                return Request.CreateResponse(HttpStatusCode.OK, p);
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Creating Player - Oops... Something Went Wrong!");
            }
        }

        // PUT: api/CreatePlayer/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/CreatePlayer/5
        public void Delete(int id)
        {
        }
    }
}
