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
        const int teamSize = 4;

        private static Logger logger = LogManager.GetCurrentClassLogger();

        bgroup89_prodEntities db = new bgroup89_prodEntities();


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


        //receive fantasy team_id and user_id to buy
        //add player to team
        //POST: api/ManageFantasyTeam
        public HttpResponseMessage Post(JObject teamData)
        {
            logger.Trace("POST - ManageFantasyTeamController");

            //get the users fantasy team players
            Fantasy_team fantasy_Team = JsonConvert.DeserializeObject<Fantasy_team>(teamData.ToString());
            Fantasy_team ft = db.Fantasy_team.Where(a => a.team_id == fantasy_Team.team_id).FirstOrDefault();

            //Get the player that user wants to add to his team
            Player player = JsonConvert.DeserializeObject<Player>(teamData.ToString());
            Player p1 = db.Player.Where(p => p.user_id == player.user_id).FirstOrDefault();
            //Listed_in listed = db.Listed_in.Where(l => l.league_id == fs.league_id).FirstOrDefault();

            //look for user in fantasy team player

            if (p1 != null)
            {
                if (ft.team_budget < p1.player_score)
                {
                    logger.Error("POST - team budget is too low to buy this player: " + p1.user_id);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, $"Team Budget is too low to buy Player {p1.user_id}");
                }

                int?[] teamMates = new int?[] { ft.player1, ft.player2, ft.player3, ft.player4 };

                //prevent double purchase
                foreach (var item in teamMates)
                {
                    if (item == p1.user_id)
                    {
                        logger.Error("POST - player already exists in team: " + p1);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Player Already in team!");
                    }
                }
                //find empty spot in team
                int count = 1;
                foreach (var item in teamMates)
                {
                    if (item == null)
                    {
                        switch (count)
                        {
                            case 1:
                                ft.player1 = p1.user_id;
                                break;
                            case 2:
                                ft.player2 = p1.user_id;
                                break;
                            case 3:
                                ft.player3 = p1.user_id;
                                break;
                            case 4:
                                ft.player4 = p1.user_id;
                                break;
                        }

                        ft.team_budget -= p1.player_score; 

                        db.SaveChanges();

                        var player1 = db.Player.Where(p => p.user_id == ft.player1).Select(x => new { x.user_id, x.nickname, x.picture, x.player_score, x.games_played, x.total_assists, x.total_goals_recieved, x.total_goals_scored, x.total_pen_missed, x.total_wins }).FirstOrDefault();
                        var player2 = db.Player.Where(p => p.user_id == ft.player2).Select(x => new { x.user_id, x.nickname, x.picture, x.player_score, x.games_played, x.total_assists, x.total_goals_recieved, x.total_goals_scored, x.total_pen_missed, x.total_wins }).FirstOrDefault();
                        var player3 = db.Player.Where(p => p.user_id == ft.player3).Select(x => new { x.user_id, x.nickname, x.picture, x.player_score, x.games_played, x.total_assists, x.total_goals_recieved, x.total_goals_scored, x.total_pen_missed, x.total_wins }).FirstOrDefault();
                        var player4 = db.Player.Where(p => p.user_id == ft.player4).Select(x => new { x.user_id, x.nickname, x.picture, x.player_score, x.games_played, x.total_assists, x.total_goals_recieved, x.total_goals_scored, x.total_pen_missed, x.total_wins }).FirstOrDefault();

                        logger.Error("POST - player added to team: " + ft.team_id);
                        return Request.CreateResponse(HttpStatusCode.OK, new { 
                            ft.league_id, 
                            ft.user_id, 
                            ft.team_id, 
                            ft.team_points, 
                            ft.player1, 
                            ft.team_budget, 
                            player2, 
                            player3, 
                            player4 
                        }, JsonMediaTypeFormatter.DefaultMediaType);
                    }
                    count++;
                }
                logger.Error("POST - not enough room for player in team: " + ft.team_id);
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Not Enough room in team to buy player, sell a player and try again");
            }

            return Request.CreateResponse(HttpStatusCode.NotFound, "Something went wront");

        }

        // PUT: api/ManageFantasyTeam/5
        // remove playr from fantasy team
        //recive user_id, team_id .  return team data
        public HttpResponseMessage Put(JObject teamData)
        {

            Fantasy_team fantasy_Team = JsonConvert.DeserializeObject<Fantasy_team>(teamData.ToString());
            Player player = JsonConvert.DeserializeObject<Player>(teamData.ToString());
            try
            {
                Fantasy_team ft = db.Fantasy_team.Where(a => a.team_id == fantasy_Team.team_id).FirstOrDefault();

                if (ft == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, $"Team {fantasy_Team.team_id} does not exist in DB");
                }

                Player p1 = db.Player.Where(p => p.user_id == player.user_id).FirstOrDefault();

                if (p1 == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, $"Player {player.user_id} does not exist in DB");
                }

                int?[] players_in_team = new int?[] { ft.player1, ft.player2, ft.player3, ft.player4 };


                for (int i = 1; i <= players_in_team.Length; i++)
                {
                    if (players_in_team[i-1] == p1.user_id)
                    {
                        switch (i)
                        {
                            case 1:
                                ft.player1 = null;
                                break;
                            case 2:
                                ft.player2 = null;
                                break;
                            case 3:
                                ft.player3 = null;
                                break;
                            case 4:
                                ft.player4 = null;
                                break;
                        }
                    }
                }

                ft.team_budget += p1.player_score;


                db.SaveChanges();

                var player1 = db.Player.Where(p => p.user_id == ft.player1).Select(x => new { x.user_id, x.nickname, x.picture, x.player_score, x.games_played, x.total_assists, x.total_goals_recieved, x.total_goals_scored, x.total_pen_missed, x.total_wins }).FirstOrDefault();
                var player2 = db.Player.Where(p => p.user_id == ft.player2).Select(x => new { x.user_id, x.nickname, x.picture, x.player_score, x.games_played, x.total_assists, x.total_goals_recieved, x.total_goals_scored, x.total_pen_missed, x.total_wins }).FirstOrDefault();
                var player3 = db.Player.Where(p => p.user_id == ft.player3).Select(x => new { x.user_id, x.nickname, x.picture, x.player_score, x.games_played, x.total_assists, x.total_goals_recieved, x.total_goals_scored, x.total_pen_missed, x.total_wins }).FirstOrDefault();
                var player4 = db.Player.Where(p => p.user_id == ft.player4).Select(x => new { x.user_id, x.nickname, x.picture, x.player_score, x.games_played, x.total_assists, x.total_goals_recieved, x.total_goals_scored, x.total_pen_missed, x.total_wins }).FirstOrDefault();


                logger.Error($"POST - player {p1} removed from team: " + ft);
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    ft.league_id,
                    ft.user_id,
                    ft.team_id,
                    ft.team_points,
                    ft.team_budget,
                    player1,
                    player2,
                    player3,
                    player4
                }, JsonMediaTypeFormatter.DefaultMediaType);

            }
            catch (Exception e)
            {
                logger.Error($"Bad Request, could not Delete player: {JsonConvert.DeserializeObject<Fantasy_team>(teamData.ToString()).user_id}=======> " + e);
                return Request.CreateResponse(HttpStatusCode.BadRequest, e);
            }
        }

        // DELETE: api/ManageFantasyTeam/5
        public void Delete(int id)
        {
            
        }
    }
}
