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
            logger.Trace("POST - PlayersInTeamController");
            //Converting teamData to Player
            Fantasy_team fantasy_Team = JsonConvert.DeserializeObject<Fantasy_team>(teamData.ToString());
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



            //var list = new List<KeyValuePair<string, dynamic>>();
            //JObject playersInTeamJson = null;
            //int count = 1;
//            JObject playersInTeamJson = null;

//            playersInTeamJson =
//new JObject(
//    new JProperty($"player{count}",
//        new JObject(
//            new JProperty(nameof(pl1.user_id), players[0].user_id),
//            new JProperty(nameof(pl1.nickname), players[0].nickname),
//            new JProperty(nameof(pl1.picture), players[0].picture),
//            new JProperty(nameof(pl1.player_score), players[0].player_score),
//            new JProperty(nameof(pl1.games_played), players[0].games_played),
//            new JProperty(nameof(pl1.total_wins), players[0].total_wins),
//            new JProperty(nameof(pl1.total_goals_scored), players[0].total_goals_scored),
//            new JProperty(nameof(pl1.total_assists), players[0].total_assists),
//            new JProperty(nameof(pl1.total_pen_missed), players[0].total_pen_missed),
//            new JProperty(nameof(pl1.total_goals_recieved), players[0].total_goals_recieved),
//            new JProperty(nameof(pl1.league_manager), players[0].league_manager)
//            )));

JObject players_In_Team = JObject.FromObject(new
            {
                player1 = new
                {
                    user_id = players[0].user_id,
                    nickname = players[0].nickname,
                    picture = players[0].picture,
                    player_score = players[0].player_score,
                    games_played = players[0].games_played,
                    total_wins = players[0].total_wins,
                    total_goals_scored = players[0].total_goals_scored,
                    total_assists = players[0].total_assists,
                    total_pen_missed = players[0].total_pen_missed,
                    total_goals_recieved = players[0].total_goals_recieved,
                    league_manager = players[0].league_manager
                },
                player2 = new
                {
                    user_id = players[1].user_id,
                    nickname = players[1].nickname,
                    picture = players[1].picture,
                    player_score = players[1].player_score,
                    games_played = players[1].games_played,
                    total_wins = players[1].total_wins,
                    total_goals_scored = players[1].total_goals_scored,
                    total_assists = players[1].total_assists,
                    total_pen_missed = players[1].total_pen_missed,
                    total_goals_recieved = players[1].total_goals_recieved,
                    league_manager = players[1].league_manager
                },
                player3 = new
                {
                    user_id = players[2].user_id,
                    nickname = players[2].nickname,
                    picture = players[2].picture,
                    player_score = players[2].player_score,
                    games_played = players[2].games_played,
                    total_wins = players[2].total_wins,
                    total_goals_scored = players[2].total_goals_scored,
                    total_assists = players[2].total_assists,
                    total_pen_missed = players[2].total_pen_missed,
                    total_goals_recieved = players[2].total_goals_recieved,
                    league_manager = players[2].league_manager
                },
                player4 = new
                {
                    user_id = players[3].user_id,
                    nickname = players[3].nickname,
                    picture = players[3].picture,
                    player_score = players[3].player_score,
                    games_played = players[3].games_played,
                    total_wins = players[3].total_wins,
                    total_goals_scored = players[3].total_goals_scored,
                    total_assists = players[3].total_assists,
                    total_pen_missed = players[3].total_pen_missed,
                    total_goals_recieved = players[3].total_goals_recieved,
                    league_manager = players[3].league_manager
                }
            });

            //list.Add(new KeyValuePair<string, dynamic>(nameof(item.user_id), item.user_id));
            //list.Add(new KeyValuePair<string, dynamic>(nameof(item.nickname), item.nickname));
            //list.Add(new KeyValuePair<string, dynamic>(nameof(item.picture), item.picture));
            //list.Add(new KeyValuePair<string, dynamic>(nameof(item.player_score), item.player_score));
            //list.Add(new KeyValuePair<string, dynamic>(nameof(item.games_played), item.games_played));
            //list.Add(new KeyValuePair<string, dynamic>(nameof(item.total_wins), item.total_wins));
            //list.Add(new KeyValuePair<string, dynamic>(nameof(item.total_goals_scored), item.total_goals_scored));
            //list.Add(new KeyValuePair<string, dynamic>(nameof(item.total_assists), item.total_assists));
            //list.Add(new KeyValuePair<string, dynamic>(nameof(item.total_pen_missed), item.total_pen_missed));
            //list.Add(new KeyValuePair<string, dynamic>(nameof(item.total_goals_recieved), item.total_goals_recieved));
            //list.Add(new KeyValuePair<string, dynamic>(nameof(item.league_manager), item.league_manager));

            //        JObject playersInTeamJson =
            //new JObject(
            //    new JProperty("player",
            //        new JObject(
            //            new JProperty("title", "James Newton-King"),
            //            new JProperty("link", "http://james.newtonking.com"),
            //            new JProperty("description", "James Newton-King's blog."),
            //            new JProperty("item",
            //                new JArray(
            //                    from p in players
            //                    orderby p.Title
            //                    select new JObject(
            //                        new JProperty("title", p.Title),
            //                        new JProperty("description", p.Description),
            //                        new JProperty("link", p.Link),
            //                        new JProperty("category",
            //                            new JArray(
            //                                from c in p.Categories
            //                                select new JValue(c)))))))));

            //var list = new List<KeyValuePair<string, int>>();
            //list.Add(new KeyValuePair<string, int>(key.ToString(), players[0].user_id));
            //list.Add(new KeyValuePair<string, int>("Mouse", 2));
            //list.Add(new KeyValuePair<string, int>("Keyboard", 4));

            //for (int i = 0; i < 4; i++)
            //{
            //    yield return (

            //    players[i].user_id,
            //    players[i].nickname,
            //    players[i].picture,
            //    players[i].player_score,
            //    players[i].games_played,
            //    players[i].total_wins,
            //    players[i].total_goals_scored,
            //    players[i].total_assists,
            //    players[i].total_pen_missed,
            //    players[i].total_goals_recieved,
            //    players[i].league_manager

            //    );
            //}

            return Request.CreateResponse(HttpStatusCode.OK, new
            {
                //list
                players_In_Team
            }, JsonMediaTypeFormatter.DefaultMediaType);
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
