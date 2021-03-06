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
    public class ManageLeagueController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        bgroup89_prod_Entities db = new bgroup89_prod_Entities();


        // GET: api/ManageLeague/5
        //Get League Data
        //recive league_id. return league
        public HttpResponseMessage Get(JObject leagueData)
        {
            logger.Trace("GET - ManageLeagueController");
            League league = JsonConvert.DeserializeObject<League>(leagueData.ToString());
            try
            {

                if (league == null)
                {
                    logger.Error("POST - Empty reference - league: " + league);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Fetching League Data -Oops... Something went wrong");
                }

                League l1 = db.League.Where(l => l.league_id == league.league_id).FirstOrDefault();
                logger.Trace("POST - DB connection by - " + league.league_id + " returned - " + l1.league_id);

                if (l1 == null)
                {
                    logger.Error("POST - League " + league.league_id + " does not exist in DB");
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Could not find League");
                }

                var usres_in_league = db.Listed_in.Where(x => x.league_id == l1.league_id).Select(x => new { x.user_id, x.nickname, x.Player.player_score, x.Player.picture }).ToList();


                Listed_in manager = db.Listed_in.Where(l => l.league_id == l1.league_id && l.Player.league_manager).FirstOrDefault();

                logger.Trace("Fetched league successfully - " /*+ l1.league_id*/);
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    l1.league_id,
                    l1.league_name,
                    l1.league_picture,
                    l1.league_rules,
                    usres_in_league,
                    manager.user_id

                },
                JsonMediaTypeFormatter.DefaultMediaType);

            }
            catch (Exception e)
            {
                logger.Error("Bad Request, league id: " + league.league_id + "=======> " + e.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, e.Message);
            }

        }

        // POST: api/ManageLeague
        //Add Player to League
        //recive league_id, user_id. return league
        public HttpResponseMessage Post(JObject leagueData)
        {
            logger.Trace("POST - ManageLeagueController - Add player to league");
            League league = JsonConvert.DeserializeObject<League>(leagueData.ToString());
            Player player = JsonConvert.DeserializeObject<Player>(leagueData.ToString());

            try
            {


                Player p1 = db.Player.Where(p => p.user_id == player.user_id).FirstOrDefault();
                logger.Trace("POST - DB connection by - " + player.user_id + " returned - " + p1.user_id);

                League l1 = db.League.Where(l => l.league_id == league.league_id).FirstOrDefault();
                logger.Trace("POST - DB connection by - " + league.league_id + " returned - " + l1.league_id);

                if (p1 == null || l1 == null)
                {
                    logger.Error("POST - Empty reference - league: " + l1 + " | player: " + p1);
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                Listed_in ls = new Listed_in()
                {
                    league_id = l1.league_id,
                    user_id = p1.user_id,
                    nickname = p1.nickname,
                    registration_date = DateTime.Now
                };

                db.Listed_in.Add(ls);
                logger.Trace("Adding user to league - " + ls.league_id + " user - " + ls.user_id);
                db.SaveChanges();

                //// recive league_id, user_id. return new Fantasy Team
                //Fantasy_team ft = new Fantasy_team()
                //{
                //    league_id = ls.league_id,
                //    user_id = ls.user_id,
                //    team_budget = 100
                //};

                //db.Fantasy_team.Add(ft);
                logger.Trace("POST - DB connection by - " + league.league_id + " returned - " + l1.league_id);

                if (l1 == null)
                {
                    logger.Error("POST - League " + league.league_id + " does not exist in DB");
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Could not find League");
                }

                var usres_in_league = db.Listed_in.Where(x => x.league_id == l1.league_id).Select(x => new { x.user_id, x.nickname, x.Player.player_score, x.Player.picture }).ToList();

                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    l1.league_id,
                    l1.league_name,
                    l1.league_picture,
                    l1.league_rules,
                    l1.invite_url,
                    usres_in_league
                }, JsonMediaTypeFormatter.DefaultMediaType);
            }
            catch (Exception e)
            {
                logger.Error("Bad Request, could not add player: " + player.user_id + " | to league: " + league.league_id + "=======> " + e);
                return Request.CreateResponse(HttpStatusCode.BadRequest, e.Message);
            }
        }

        //PUT: api/ManageLeague/5
        //Edit League Data
        //recive league_id, league_name, league_picture, league_rules. return league
        public HttpResponseMessage Put(JObject leagueData)
        {
            logger.Trace("POST - ManageLeagueController - Edit League");
            League league = JsonConvert.DeserializeObject<League>(leagueData.ToString());

            try
            {

                var leagues = db.League.ToList();

                foreach (var item in leagues)
                {
                    if (item.invite_url != "")
                    {
                        League l1 = item;
                        l1.invite_url = "";
                        db.SaveChanges();
                    }
                }

                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, "Victory", JsonMediaTypeFormatter.DefaultMediaType);
            }
            catch (Exception e)
            {
                logger.Error("Bad Request, could not edit league: " + league.league_id + "=======> " + e);
                return Request.CreateResponse(HttpStatusCode.BadRequest, e.Message);
            }
        }


        // DELETE: api/ManageLeague/5
        //Delete Player from League
        //recive user_id, league_id. return ListedIn
        public HttpResponseMessage Delete(JObject leagueData)
        {
            logger.Trace("POST - ManageLeagueController - Delete player from league");
            Listed_in listed = JsonConvert.DeserializeObject<Listed_in>(leagueData.ToString());
            //League league = JsonConvert.DeserializeObject<League>(leagueData.ToString());
            try
            {
                //Player player = JsonConvert.DeserializeObject<Player>(leagueData.ToString());

                Listed_in ls = db.Listed_in.Where(p => p.user_id == listed.user_id && p.league_id == listed.league_id).FirstOrDefault();
                League l1 = db.League.Where(l => l.league_id == listed.league_id).FirstOrDefault();
                Fantasy_team fs = db.Fantasy_team.Where(f => f.user_id == ls.user_id).FirstOrDefault();

                if (ls == null)
                {
                    logger.Error("POST - Empty reference - user: " + ls);
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Player not found");
                }

                db.Fantasy_team.Remove(fs);
                db.SaveChanges();

                var fs1 = db.Fantasy_team.Where(s => s.league_id == listed.league_id).ToList();

                foreach (var team in fs1)
                {
                    Fantasy_team fs2 = db.Fantasy_team.Where(a => a.team_id == team.team_id).FirstOrDefault();

                    if (team.player1 == listed.user_id || team.player2 == listed.user_id || team.player3 == listed.user_id || team.player4 == listed.user_id)
                    {
                        if (team.player1 == listed.user_id)
                        {
                            fs2.player1 = null;
                        }
                        if (team.player2 == listed.user_id)
                        {
                            fs2.player2 = null;
                        }
                        if (team.player3 == listed.user_id)
                        {
                            fs2.player3 = null;
                        }
                        if (team.player4 == listed.user_id)
                        {
                            fs2.player4 = null;
                        }
                        
                        db.SaveChanges();

                        Player p1 = db.Player.Where(p => p.user_id == listed.user_id).FirstOrDefault();
                        fs2.team_budget += p1.player_score;
                        db.SaveChanges();

                        p1.player_score = 0;
                        p1.games_played = 0;
                        p1.total_assists = 0;
                        p1.total_goals_recieved = 0;
                        p1.total_goals_scored = 0;
                        p1.total_pen_missed = 0;
                        p1.total_wins = 0;
                        db.SaveChanges();
                    }
                }

                List <Active_in> ai = db.Active_in.Where(a => a.league_id == ls.league_id && a.user_id == ls.user_id).ToList();
                if (ai != null)
                {
                    foreach (var item in ai)
                    {
                        Active_in ai1 = db.Active_in.Where(a => a.league_id == item.league_id && a.user_id == item.user_id).FirstOrDefault();
                        db.Active_in.Remove(ai1);
                    }
                }

                db.Listed_in.Remove(ls);
                db.SaveChanges();
                logger.Trace("User removed from league - name: " + ls.user_id);
               
                var usres_in_league = db.Listed_in.Where(x => x.league_id == l1.league_id).Select(x => new { x.user_id, x.nickname, x.Player.player_score, x.Player.picture }).ToList();


                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    l1.league_id,
                    l1.league_name,
                    l1.league_picture,
                    l1.league_rules,
                    usres_in_league
                }, JsonMediaTypeFormatter.DefaultMediaType);
            }
            catch (Exception e)
            {
                logger.Error("Bad Request, could not Delete user: " + listed.user_id + "=======> " + e);
                return Request.CreateResponse(HttpStatusCode.BadRequest, e.Message); ;
            }

        }
    }
}
