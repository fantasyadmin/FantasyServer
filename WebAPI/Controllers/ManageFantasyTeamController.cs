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
    public class ManageFantasyTeamController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        bgroup89_test2Entities db = new bgroup89_test2Entities();


        // GET: api/ManageFantasyTeam/5

        //Get FantasyTeam Details
        //recive team_id. return Fantasy Team.
        public HttpResponseMessage Get(JObject teamData)
        {
            logger.Trace("POST - ManageFantasyTeamController");
            Fantasy_team fantasy_Team = JsonConvert.DeserializeObject<Fantasy_team>(teamData.ToString());

            Fantasy_team fs = db.Fantasy_team.Where(f => f.team_id == fantasy_Team.team_id).FirstOrDefault();

            if (fs == null)
            {
                logger.Error("POST - Empty reference - team: " + fs);
                return Request.CreateResponse(HttpStatusCode.NotFound, "Team not found");
            }

            return Request.CreateResponse(HttpStatusCode.OK, new { fs.league_id, fs.team_id, fs.user_id, fs.team_budget, fs.team_points }, JsonMediaTypeFormatter.DefaultMediaType);
        }


        //receive fantasy team_id and user_id to buy
        //add player to team
        // POST: api/ManageFantasyTeam
        public HttpResponseMessage Post(JObject teamData)
        {
            logger.Trace("POST - ManageFantasyTeamController");

            //get the users fantasy team players
            Fantasy_team fantasy_Team = JsonConvert.DeserializeObject<Fantasy_team>(teamData.ToString());
            Fantasy_team_players fs = db.Fantasy_team_playersSet.Where(a => a.user_id == fantasy_Team.user_id).FirstOrDefault();

            //Get the player that user wants to add to his team
            Player player = JsonConvert.DeserializeObject<Player>(teamData.ToString());
            Player p1 = db.Player.Where(p => p.user_id == player.user_id).FirstOrDefault();
            //Listed_in listed = db.Listed_in.Where(l => l.league_id == fs.league_id).FirstOrDefault();

            //look for user in fantasy team playesr

            if (p1 != null)
            {

                int?[] teamMates = new int?[] { fs.player1, fs.player2, fs.player3, fs.player4 };

                //prevent double purchase
                foreach (var item in teamMates)
                {
                    if (item == p1.user_id)
                    {
                        logger.Error("POST - player already exists in team: " + p1);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Player Already in team!");
                    }
                }
                int count = 0;
                foreach (var item in teamMates)
                {
                    if (item == null)
                    {
                        teamMates[count] = p1.user_id;

                        //fs.player1 = teamMates[0];
                        //fs.player2 = teamMates[1];
                        //fs.player3 = teamMates[2];
                        //fs.player4 = teamMates[3];

                        db.SaveChanges();

                        logger.Error("POST - player added to team: " + fs);
                        return Request.CreateResponse(HttpStatusCode.OK, new { fs.league_id, fs.user_id, fs.nickname, fs.team_id, fs.player1, fs.player2, fs.player3, fs.player4 }, JsonMediaTypeFormatter.DefaultMediaType);
                    }
                    count++;
                }
                logger.Error("POST - not enough room for player in team: " + fs);
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Not Enough room in team to buy player, sell a player and try again");
            }

            return Request.CreateResponse(HttpStatusCode.NotFound, "Something went wront");

        }

        // PUT: api/ManageFantasyTeam/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ManageFantasyTeam/5
        public void Delete(int id)
        {
        }
    }
}
