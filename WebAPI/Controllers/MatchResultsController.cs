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
    public class MatchResultsController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        bgroup89_prod_Entities db = new bgroup89_prod_Entities();
        // GET: api/MatchResults
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //get active in data
        // GET: api/MatchResults/5
        public HttpResponseMessage Get(JObject matchData)
        {
            Active_in active_In = JsonConvert.DeserializeObject<Active_in>(matchData.ToString());
            try
            {
                if (active_In == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }

                Active_in ac1 = db.Active_in.Where(a => a.user_id == active_In.user_id && a.match_id == active_In.match_id).FirstOrDefault();
                Player p1 = db.Player.Where(p => p.user_id == ac1.user_id).FirstOrDefault();

                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    //player data
                    ac1.user_id,
                    p1.nickname,
                    p1.picture,
                    //league data
                    ac1.league_id,
                    //match data
                    ac1.match_id,
                    ac1.wins,
                    ac1.pen_missed,
                    ac1.match_color,
                    ac1.goals_scored,
                    ac1.goals_recieved,
                    ac1.assists
                }, JsonMediaTypeFormatter.DefaultMediaType);
            }
            catch (Exception e)
            {
                logger.Error("Bad Request, could not edit data for match: " + active_In.match_id + "and Player: " + active_In.user_id + "=======>" + e);
                return Request.CreateResponse(HttpStatusCode.BadRequest, e);
            }
        }

        //create new Active IN
        //recive user_id, match_id, league_id, pen_missed, wins, match_color, goals_scored, goals_recieved, assists. return new Active_in
        // POST: api/MatchResults
        public HttpResponseMessage Post(JObject matchData)
        {
            Active_in active_In = JsonConvert.DeserializeObject<Active_in>(matchData.ToString());

            try
            {
                if (active_In == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }

                Active_in ac = db.Active_in.Where(a => a.user_id == active_In.user_id && a.match_id == active_In.match_id).FirstOrDefault();

                if (ac != null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Can't submit more than 1 Match Results form. Please wait for League Manager's approval, or Edit your existing form");
                }

                Active_in ac1 = new Active_in()
                {
                    assists = active_In.assists,
                    goals_recieved = active_In.goals_recieved,
                    goals_scored = active_In.goals_scored,
                    match_color = active_In.match_color,
                    pen_missed = active_In.pen_missed,
                    wins = active_In.wins,
                    //attending = active_In.attending,
                    league_id = active_In.league_id,
                    match_id = active_In.match_id,
                    user_id = active_In.user_id
                };

                db.Active_in.Add(ac1);
                db.SaveChanges();
                //ac1 = db.Active_in.Where(a => a.user_id == active_In.user_id && a.match_id == active_In.match_id).FirstOrDefault();

                //Keep results until league manager approves if player attended match
                //if (ac1.attending == 1)
                //{
                //    ac1.assists = active_In.assists;
                //    ac1.goals_recieved = active_In.goals_recieved;
                //    ac1.goals_scored = active_In.goals_scored;
                //    ac1.match_color = active_In.match_color;
                //    ac1.pen_missed = active_In.pen_missed;
                //}

                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    ac1.user_id,
                    ac1.match_id,
                    ac1.league_id,
                    ac1.wins,
                    ac1.pen_missed,
                    ac1.match_color,
                    ac1.goals_scored,
                    ac1.goals_recieved,
                    ac1.assists
                }, JsonMediaTypeFormatter.DefaultMediaType);
            }
            catch (Exception e)
            {
                logger.Error("Bad Request, could not edit data for match: " + active_In.match_id + "and Player: " + active_In.user_id + "=======>" + e);
                return Request.CreateResponse(HttpStatusCode.BadRequest, e);
            }
        }

        // PUT: api/MatchResults/5
        public HttpResponseMessage Put(JObject matchData)
        {
            Active_in active_In = JsonConvert.DeserializeObject<Active_in>(matchData.ToString());
            try
            {
                if (active_In == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }

                Active_in ac1 = db.Active_in.Where(a => a.user_id == active_In.user_id && a.match_id == active_In.match_id).FirstOrDefault();

                //Keep results until league manager approves if player attended match
                    ac1.assists = active_In.assists;
                    ac1.goals_recieved = active_In.goals_recieved;
                    ac1.goals_scored = active_In.goals_scored;
                    ac1.match_color = active_In.match_color;
                    ac1.pen_missed = active_In.pen_missed;

                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    ac1.user_id,
                    ac1.match_id,
                    ac1.league_id,
                    ac1.wins,
                    ac1.pen_missed,
                    ac1.match_color,
                    ac1.goals_scored,
                    ac1.goals_recieved,
                    ac1.assists
                }, JsonMediaTypeFormatter.DefaultMediaType);
            }
            catch (Exception e)
            {
                logger.Error("Bad Request, could not edit data for match: " + active_In.match_id + "and Player: " + active_In.user_id + "=======>" + e);
                return Request.CreateResponse(HttpStatusCode.BadRequest, e);
            }
        }

        // DELETE: api/MatchResults/5
        public HttpResponseMessage Delete(JObject matchData)
        {
            Active_in active_In = JsonConvert.DeserializeObject<Active_in>(matchData.ToString());

            try
            {
                Active_in ac1 = db.Active_in.Where(a => a.user_id == active_In.user_id && a.match_id == active_In.match_id).FirstOrDefault();

                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    ac1.user_id,
                    ac1.match_id,
                    ac1.league_id,
                    ac1.wins,
                    ac1.pen_missed,
                    ac1.match_color,
                    ac1.goals_scored,
                    ac1.goals_recieved,
                    ac1.assists
                }, JsonMediaTypeFormatter.DefaultMediaType);
            }
            catch (Exception e)
            {
                logger.Error("Bad Request, could not delete data for match: " + active_In.match_id + "and Player: " + active_In.user_id + "=======>" + e);
                return Request.CreateResponse(HttpStatusCode.BadRequest, e);
            }
        }
    }
}
