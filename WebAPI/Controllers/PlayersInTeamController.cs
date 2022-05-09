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
    public class PlayersInTeamController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        bgroup89_prodEntities db = new bgroup89_prodEntities();

        // GET: api/PlayersInTeam
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //GET: api/PlayersInTeam/5
        public HttpResponseMessage Get(JObject teamData)
        {
            Fantasy_team fantasy_Team = JsonConvert.DeserializeObject<Fantasy_team>(teamData.ToString());
            if (fantasy_Team == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "no Data recieved");

            }
            try
            {
            logger.Trace("POST - PlayersInTeamController");
            //Converting teamData to Player

            Fantasy_team fs = db.Fantasy_team.Where(f => f.team_id == fantasy_Team.team_id).FirstOrDefault();
            List<Player> players = new List<Player>(4);

            Player pl = null;

            List<Fantasy_team> players_in_team = db.Fantasy_team.Where(x => x.team_id == fs.team_id).ToList();
            logger.Error(players_in_team);

            int?[] listing = new int?[4];

            listing[0] = fs.player1;
            listing[1] = fs.player2;
            listing[2] = fs.player3;
            listing[3] = fs.player4;

                int players_id;
            Player pl1;
            for (int i = 0; i < 4; i++)
            {
                players_id = (int)listing[i];
                pl = db.Player.Where(p => p.user_id == players_id).FirstOrDefault();
                if (i == 0)
                {
                    pl1 = pl;
                }
                players.Add(pl);
            }

            var playersInTeam = players.Select(x => new { 
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
                x.league_manager }).ToList();


            //    JObject players_In_Team = JObject.FromObject(new
            //{
            //    player1 = new
            //    {
            //        user_id = players[0].user_id,
            //        nickname = players[0].nickname,
            //        picture = players[0].picture,
            //        player_score = players[0].player_score,
            //        games_played = players[0].games_played,
            //        total_wins = players[0].total_wins,
            //        total_goals_scored = players[0].total_goals_scored,
            //        total_assists = players[0].total_assists,
            //        total_pen_missed = players[0].total_pen_missed,
            //        total_goals_recieved = players[0].total_goals_recieved,
            //        league_manager = players[0].league_manager
            //    },
            //    player2 = new
            //    {
            //        user_id = players[1].user_id,
            //        nickname = players[1].nickname,
            //        picture = players[1].picture,
            //        player_score = players[1].player_score,
            //        games_played = players[1].games_played,
            //        total_wins = players[1].total_wins,
            //        total_goals_scored = players[1].total_goals_scored,
            //        total_assists = players[1].total_assists,
            //        total_pen_missed = players[1].total_pen_missed,
            //        total_goals_recieved = players[1].total_goals_recieved,
            //        league_manager = players[1].league_manager
            //    },
            //    player3 = new
            //    {
            //        user_id = players[2].user_id,
            //        nickname = players[2].nickname,
            //        picture = players[2].picture,
            //        player_score = players[2].player_score,
            //        games_played = players[2].games_played,
            //        total_wins = players[2].total_wins,
            //        total_goals_scored = players[2].total_goals_scored,
            //        total_assists = players[2].total_assists,
            //        total_pen_missed = players[2].total_pen_missed,
            //        total_goals_recieved = players[2].total_goals_recieved,
            //        league_manager = players[2].league_manager
            //    },
            //    player4 = new
            //    {
            //        user_id = players[3].user_id,
            //        nickname = players[3].nickname,
            //        picture = players[3].picture,
            //        player_score = players[3].player_score,
            //        games_played = players[3].games_played,
            //        total_wins = players[3].total_wins,
            //        total_goals_scored = players[3].total_goals_scored,
            //        total_assists = players[3].total_assists,
            //        total_pen_missed = players[3].total_pen_missed,
            //        total_goals_recieved = players[3].total_goals_recieved,
            //        league_manager = players[3].league_manager
            //    }
            //});

            return Request.CreateResponse(HttpStatusCode.OK, new
            {
                //players_In_Team
                playersInTeam
            }, JsonMediaTypeFormatter.DefaultMediaType);
            }
            catch (Exception e)
            { 
                logger.Error("Bad Request, data received = " + fantasy_Team.team_id +  " | =======> " + e);
                return Request.CreateResponse(HttpStatusCode.BadRequest, e.Message);
            }

        }


        // POST: api/PlayersInTeam
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/PlayersInTeam/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/PlayersInTeam/5
        public void Delete(int id)
        {
        }
    }
}
