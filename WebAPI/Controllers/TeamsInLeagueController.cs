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
    public class TeamsInLeagueController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        bgroup89_prod_Entities db = new bgroup89_prod_Entities();
        // GET: api/TeamsInLeague
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/TeamsInLeague/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/TeamsInLeague
        public IEnumerable <object> Post(JObject teamData)
        {
            logger.Trace("POST - ManageFantasyTeamController");

            //get the users fantasy team
            Fantasy_team fantasy_Team = JsonConvert.DeserializeObject<Fantasy_team>(teamData.ToString());
            var ft = db.Fantasy_team.Where(a => a.league_id == fantasy_Team.league_id).ToList().Select(x => new { x.user_id, x.team_id, x.league_id, x.team_points, x.team_budget, x.player1, x.player2, x.player3, x.player4});

            foreach (var item in ft)
            {
                var pl1 = db.Player.Where(p => p.user_id == item.player1).Select(x => new
                {
                    x.user_id,
                    x.nickname,
                    x.picture,
                    x.player_score,
                    x.games_played,
                    x.total_wins,
                    x.total_goals_scored,
                    x.total_assists,
                    x.total_pen_missed,
                    x.total_goals_recieved,
                    x.league_manager
                }).ToList();
                var pl2 = db.Player.Where(p => p.user_id == item.player2).Select(x => new
                {
                    x.user_id,
                    x.nickname,
                    x.picture,
                    x.player_score,
                    x.games_played,
                    x.total_wins,
                    x.total_goals_scored,
                    x.total_assists,
                    x.total_pen_missed,
                    x.total_goals_recieved,
                    x.league_manager
                }).ToList();
                var pl3 = db.Player.Where(p => p.user_id == item.player3).Select(x => new
                {
                    x.user_id,
                    x.nickname,
                    x.picture,
                    x.player_score,
                    x.games_played,
                    x.total_wins,
                    x.total_goals_scored,
                    x.total_assists,
                    x.total_pen_missed,
                    x.total_goals_recieved,
                    x.league_manager
                }).ToList();
                var pl4 = db.Player.Where(p => p.user_id == item.player4).Select(x => new
                {
                    x.user_id,
                    x.nickname,
                    x.picture,
                    x.player_score,
                    x.games_played,
                    x.total_wins,
                    x.total_goals_scored,
                    x.total_assists,
                    x.total_pen_missed,
                    x.total_goals_recieved,
                    x.league_manager
                }).ToList();
                yield return new
                {
                    item.user_id,
                    item.team_id,
                    item.league_id,
                    item.team_points,
                    item.team_budget,
                    pl1,
                    pl2,
                    pl3,
                    pl4
                };
            }
        }

        // PUT: api/TeamsInLeague/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/TeamsInLeague/5
        public void Delete(int id)
        {
        }
    }
}
