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
    public class GetFantasyTeamController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        bgroup89_prod_Entities db = new bgroup89_prod_Entities();
        // GET: api/GetFantasyTeam
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET: api/GetFantasyTeam/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/GetFantasyTeam
        public HttpResponseMessage Post(JObject teamData)
        {
            logger.Trace("POST - ManageFantasyTeamController");
            Fantasy_team fantasy_Team = JsonConvert.DeserializeObject<Fantasy_team>(teamData.ToString());

            Fantasy_team fs = db.Fantasy_team.Where(f => f.team_id == fantasy_Team.team_id).FirstOrDefault();

            if (fs == null)
            {
                logger.Error("POST - Empty reference - team: " + fs);
                return Request.CreateResponse(HttpStatusCode.NotFound, "Team not found");
            }

            var player1 = db.Player.Where(p => p.user_id == fs.player1).Select(x => new { x.user_id, x.nickname, x.picture, x.player_score, x.games_played, x.total_assists, x.total_goals_recieved, x.total_goals_scored, x.total_pen_missed, x.total_wins }).FirstOrDefault();
            var player2 = db.Player.Where(p => p.user_id == fs.player2).Select(x => new { x.user_id, x.nickname, x.picture, x.player_score, x.games_played, x.total_assists, x.total_goals_recieved, x.total_goals_scored, x.total_pen_missed, x.total_wins }).FirstOrDefault();
            var player3 = db.Player.Where(p => p.user_id == fs.player3).Select(x => new { x.user_id, x.nickname, x.picture, x.player_score, x.games_played, x.total_assists, x.total_goals_recieved, x.total_goals_scored, x.total_pen_missed, x.total_wins }).FirstOrDefault();
            var player4 = db.Player.Where(p => p.user_id == fs.player4).Select(x => new { x.user_id, x.nickname, x.picture, x.player_score, x.games_played, x.total_assists, x.total_goals_recieved, x.total_goals_scored, x.total_pen_missed, x.total_wins }).FirstOrDefault();



            //fs.team_points = player1.player_score + player2.player_score + player3.player_score + player4.player_score;

            //List<Player> players_in_team = db.Player.Where(x => x.user_id == player1.user_id || x.user_id == player2.user_id || x.user_id == player3.user_id || x.user_id == player4.user_id).ToList();
            //logger.Error(players_in_team);

            //Player[] ftPlayers = new Player[teamSize];
            //int counter = 0;

            //foreach (var item in players_in_team)
            //{
            //    ftPlayers[counter] = item;
            //    counter++;
            //}

            return Request.CreateResponse(HttpStatusCode.OK, new
            {
                //team details
                fs.league_id,
                fs.team_id,
                fs.user_id,
                fs.team_budget,
                fs.team_points,
                //players in team details
                player1,
                player2,
                player3,
                player4

            }, JsonMediaTypeFormatter.DefaultMediaType);
        }

        // PUT: api/GetFantasyTeam/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/GetFantasyTeam/5
        public void Delete(int id)
        {
        }
    }
}
