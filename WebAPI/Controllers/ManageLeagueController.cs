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
    public class ManageLeagueController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        bgroup89_test2Entities db = new bgroup89_test2Entities();


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
                logger.Trace("Fetched league successfully - " + l1.league_id);
                return Request.CreateResponse(HttpStatusCode.OK, l1);
            }

            catch (Exception e)
            {
                logger.Error("Bad Request, league id: " + league.league_id + e);
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Fetching League Data - Oops... Something went wrong");
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

                return Request.CreateResponse(HttpStatusCode.OK, l1);
            }
            catch (Exception e)
            {
                logger.Error("Bad Request, could not add player: " + player.user_id + " | to league: " + league.league_id + ", " + e);
                return Request.CreateResponse(HttpStatusCode.BadRequest, e);
            }
        }

        //POST: api/ManageLeague/5
        //Edit League Data
        //recive league_id, league_name, league_picture, league_rules. return league
        public HttpResponseMessage Put(JObject leagueData)
        {
            logger.Trace("POST - ManageLeagueController - Edit League");
            League league = JsonConvert.DeserializeObject<League>(leagueData.ToString());

            try
            {

                League l1 = db.League.Where(l => l.league_id == league.league_id).FirstOrDefault();

                if (l1 == null)
                {
                    logger.Error("POST - Empty reference - league: " + l1);
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Could not find League");
                }

                l1.league_name = league.league_name;
                l1.league_picture = league.league_picture;
                l1.league_rules = league.league_rules;
                db.League.Append(l1);
                logger.Trace("Editing league - name: " + l1.league_name);
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, l1);
            }
            catch (Exception e)
            {
                logger.Error("Bad Request, could not edit league: " + league.league_id + ", " + e);
                return Request.CreateResponse(HttpStatusCode.BadRequest, e);
            }
        }


        // DELETE: api/ManageLeague/5
        //Delete Player from League
        //recive user_id. return ListedIn
        public HttpResponseMessage Delete(JObject leagueData)
        {
            logger.Trace("POST - ManageLeagueController - Delete player from league");
            Listed_in listed = JsonConvert.DeserializeObject<Listed_in>(leagueData.ToString());
            try
            {

                //Player player = JsonConvert.DeserializeObject<Player>(leagueData.ToString());

                Listed_in ls = db.Listed_in.Where(p => p.user_id == listed.user_id).FirstOrDefault();

                if (ls == null)
                {
                    logger.Error("POST - Empty reference - user: " + ls);
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Player not found");
                }

                db.Listed_in.Remove(ls);
                db.SaveChanges();
                logger.Trace("User removed from league - name: " + ls.user_id);
                return Request.CreateResponse(HttpStatusCode.OK, ls);
            }
            catch (Exception e)
            {
                logger.Error("Bad Request, could not Delete user: " + listed.user_id +", " + e);
                return Request.CreateResponse(HttpStatusCode.BadRequest, e); ;
            }

        }
    }
}
