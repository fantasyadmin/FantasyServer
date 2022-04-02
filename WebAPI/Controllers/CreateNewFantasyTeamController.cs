using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ClassLibrary2;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;

namespace WebAPI.Controllers
{
    public class CreateNewFantasyTeamController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        bgroup89_test2Entities db = new bgroup89_test2Entities();


        // POST: api/CreateNewFantasyTeam
        // recive league_id, user_id. return Fantasy Team
        public HttpResponseMessage Post(JObject teamData)
        {
            logger.Trace("POST - CreateNewFantasyTeamController");
            Player player = JsonConvert.DeserializeObject<Player>(teamData.ToString());
            League league = JsonConvert.DeserializeObject<League>(teamData.ToString());


            try
            {
                if (player == null || league == null)
                {
                    logger.Error("POST - Empty reference - league: " + league + " | player: " + player);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Failed to convert Data from Json To Object");
                }

                Player p1 = db.Player.Where(p => p.user_id == player.user_id).FirstOrDefault();
                logger.Trace("POST - DB connection by - " + player.user_id + " returned - " + p1.user_id);

                if (p1 == null)
                {
                    logger.Error("POST - Player " + player.nickname + " does not exist in DB");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, $"Player with user id: {player.user_id} does not exist");
                }

                Fantasy_team fs = new Fantasy_team()
                {
                    user_id = player.user_id,
                    league_id = league.league_id,
                    team_budget = 100
                };


                db.Fantasy_team.Add(fs);
                logger.Trace("Fantasy team added to DB for user - " + fs.user_id + " in league - " + fs.league_id);
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, fs);
            }
            catch (Exception e)
            {
                logger.Error("Bad Request, could not create team for player: " + player.user_id + " | league: " + league.league_id + "=======> " + e);
                return Request.CreateResponse(HttpStatusCode.BadRequest, e);
            }


        }

        // PUT: api/CreateNewFantasyTeam/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/CreateNewFantasyTeam/5
        public void Delete(int id)
        {
        }
    }
}
