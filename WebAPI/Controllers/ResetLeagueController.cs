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
    public class ResetLeagueController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        bgroup89_prod_Entities db = new bgroup89_prod_Entities();
        // GET: api/ResetLeague
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET: api/ResetLeague/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/ResetLeague
        public HttpResponseMessage Post(JObject leagueData)
        {
            logger.Trace("POST - ResetLeagueController - Add player to league");
            League league = JsonConvert.DeserializeObject<League>(leagueData.ToString());

            try
            {
                League l1 = db.League.Where(l => l.league_id == league.league_id).FirstOrDefault();
                logger.Trace("POST - DB connection by - " + league.league_id + " returned - " + l1.league_id);

                if (l1 == null)
                {
                    logger.Error("POST - League " + league.league_id + " does not exist in DB");
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Could not find League");
                }

                var active_In = db.Active_in.Where(a => a.league_id == l1.league_id).ToList();


                foreach (var item in active_In)
                {
                    Active_in ac1 = item;

                    db.Active_in.Remove(ac1);
                    db.SaveChanges();
                }

                var ls1 = db.Listed_in.Where(p => p.league_id == l1.league_id).ToList();

                foreach (var item in ls1)
                {
                    Player p1 = db.Player.Where(p => p.user_id == item.user_id).FirstOrDefault();
                    p1.games_played = 0;
                    p1.player_score = 0;
                    p1.total_assists = 0;
                    p1.total_goals_recieved = 0;
                    p1.total_goals_scored = 0;
                    p1.total_pen_missed = 0;
                    p1.total_wins = 0;
                    db.SaveChanges();
                }

                var fantasy_Teams = db.Fantasy_team.Where(f => f.league_id == l1.league_id).ToList();
                foreach (var item in fantasy_Teams)
                {
                    Fantasy_team fs1 = db.Fantasy_team.Where(fs => fs.team_id == item.team_id).FirstOrDefault();
                    fs1.player1 = null;
                    fs1.player2 = null;
                    fs1.player3 = null;
                    fs1.player4 = null;
                    db.SaveChanges();
                }

                var usres_in_league = db.Listed_in.Join(db.Fantasy_team, f => f.user_id, p => p.user_id, (f, p) => new { Listed_in = f, Fantasy_team = p }).Where(fp => fp.Listed_in.league_id == l1.league_id).GroupBy(g => g.Fantasy_team.user_id).Select(x => new { x.FirstOrDefault().Listed_in.user_id, x.FirstOrDefault().Listed_in.nickname, x.FirstOrDefault().Listed_in.Player.player_score, x.FirstOrDefault().Listed_in.Player.games_played, x.FirstOrDefault().Listed_in.Player.total_assists, x.FirstOrDefault().Listed_in.Player.total_goals_recieved, x.FirstOrDefault().Listed_in.Player.total_goals_scored, x.FirstOrDefault().Listed_in.Player.total_pen_missed, x.FirstOrDefault().Listed_in.Player.total_wins, x.FirstOrDefault().Listed_in.Player.picture, x.FirstOrDefault().Fantasy_team.team_id, x.FirstOrDefault().Fantasy_team.team_points }).ToList();

                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    usres_in_league
                }, JsonMediaTypeFormatter.DefaultMediaType);
            }
            catch (Exception e)
            {
                logger.Error($"Bad Request, could Reset League: {league.league_id} --> {e}");
                return Request.CreateResponse(HttpStatusCode.BadRequest, e.Message);
            }
        }

        // PUT: api/ResetLeague/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ResetLeague/5
        public void Delete(int id)
        {
        }
    }
}
