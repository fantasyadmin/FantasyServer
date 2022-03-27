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
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // POST: api/LogIn/5
        public HttpResponseMessage POST( JObject userData)
        {
            try 
            { 
            //Converting userData to User
            User user = JsonConvert.DeserializeObject<User>(userData.ToString());
            //find the user
            User u = db.User.Where(e => e.email == user.email).FirstOrDefault();
            
            if(u.email != null && u.password != null && u.password == user.password)
            {
                    Player p = new Player()
                    {
                        nickname = u.Player.Select(x=>x.nickname).FirstOrDefault(),
                        picture = u.Player.Select(x => x.picture).FirstOrDefault(),
                        player_score = u.Player.Select(x => x.player_score).FirstOrDefault(),
                        total_assists = u.Player.Select(x => x.total_assists).FirstOrDefault(),
                        games_played = u.Player.Select(x => x.games_played).FirstOrDefault(),
                        total_goals_recieved = u.Player.Select(x => x.total_goals_recieved).FirstOrDefault(),
                        total_goals_scored = u.Player.Select(x => x.total_goals_scored).FirstOrDefault(),
                        total_pen_missed = u.Player.Select(x => x.total_pen_missed).FirstOrDefault(),
                        total_wins = u.Player.Select(x => x.total_wins).FirstOrDefault()
                    };
                    return Request.CreateResponse(HttpStatusCode.OK, p);
                
            }
            return Request.CreateResponse(HttpStatusCode.NotFound, "LogIn - Wrong Email or Password");
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "LogIn - Oops... Something went wrong"+e);
            }
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
