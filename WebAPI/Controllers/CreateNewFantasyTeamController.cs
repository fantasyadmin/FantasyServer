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
    public class CreateNewFantasyTeamController : ApiController
    {
        bgroup89_test2Entities db = new bgroup89_test2Entities();

        // GET: api/CreateNewFantasyTeam
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/CreateNewFantasyTeam/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/CreateNewFantasyTeam
        // recive league_id, user_id. return Fantasy Team
        public HttpResponseMessage Post(JObject teamData)
        {
            try
            {
               Player player = JsonConvert.DeserializeObject<Player>(teamData.ToString());
               League league = JsonConvert.DeserializeObject<League>(teamData.ToString());

             if (player == null || league == null)
                {
                 return Request.CreateResponse(HttpStatusCode.BadRequest,"Failed to convert Data from Json To Object");
                }

                Player p1 = db.Player.Where(p => p.user_id == player.user_id).FirstOrDefault();

                if (p1 == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, $"Player with user id: {player.user_id} does not exist");
                }

                Fantasy_team fs = new Fantasy_team()
                {
                    user_id = player.user_id,
                    league_id = league.league_id,
                    //team_budget = 100
                };
                db.Fantasy_team.Add(fs);
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, p1);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, e);
            }


        }

        // PUT: api/CreateNewFantasyTeam/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/CreateNewFantasyTeam/5
        public void Delete(int id)
        {
        }
    }
}
