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

    public class MatchController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        bgroup89_prodEntities db = new bgroup89_prodEntities();
        // GET: api/Match
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET: api/Match/5
        //recive match_id. return Match
        //get match details
        public HttpResponseMessage Get(JObject matchData)
        {
            Match match = JsonConvert.DeserializeObject<Match>(matchData.ToString());

            try
            {
                var m1 = db.Match.Where(m => m.match_id == match.match_id).Select(x => new { x.match_id, x.league_id, x.location, x.match_date, x.match_time, x.team_color1, x.team_color2 }).FirstOrDefault();

                if (m1 == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, $"Match {match.match_id} was not found");
                }

                return Request.CreateResponse(HttpStatusCode.OK, new { m1 }, JsonMediaTypeFormatter.DefaultMediaType);
            }
            catch (Exception e)
            {

                logger.Error("Bad Request, could not edit data for match: " + match.match_id + "=======>" + e);
                return Request.CreateResponse(HttpStatusCode.BadRequest, e);
            }


        }

        // POST: api/Match
        //Create new Match
        //recive match_date, match_time, location, team_color1, team_color2, league_id. return Match
        public HttpResponseMessage Post(JObject matchData)
        {
            logger.Trace("POST - MatchController");
            //Converting userData to User
            try
            {
                Match match = JsonConvert.DeserializeObject<Match>(matchData.ToString());
                League league = JsonConvert.DeserializeObject<League>(matchData.ToString());
                Player player = JsonConvert.DeserializeObject<Player>(matchData.ToString());
               // string strLocation = JsonConvert.DeserializeObject<string>(str.ToString());

                League l1 = db.League.Where(l => l.league_id == league.league_id).FirstOrDefault();
                if (l1 == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, $"League {league.league_id} not found");
                }

                string strhDate = matchData["match_date"].ToString(); //Convert.ToDateTime(match.match_date);
                string strTime = matchData["match_time"].ToString();
                //var strLocation = string.Format("POINT({0})", matchData["location"].ToString());
                //int intLocation = Convert.ToInt32(matchData["location"]);
                //string strLocation = matchData["location"].ToString();

                //try to take  every property like location instead of Desarialze match

               // string strLocation = "-122.336106, 47.605049";
                //string str1 = strLocation.Substring(0, strLocation.IndexOf(",") );
                //string str2 = strLocation.Substring(strLocation.IndexOf(",") + 1);
                //strLocation = str1 + str2;

                DateTime matchDate = DateTime.Parse(strhDate);
                TimeSpan matchTime = TimeSpan.Parse(strTime);
                System.Data.Entity.Spatial.DbGeography matchLocation = System.Data.Entity.Spatial.DbGeography.FromText("POINT(47.605049 -82.336106)",4326);

                Match m1 = new Match()
                {
                    match_date = matchDate,
                    match_time = matchTime,
                    location = matchLocation,
                    team_color1 = match.team_color1,
                    team_color2 = match.team_color2,
                    league_id = l1.league_id
                };

                //TimeSpan time = TimeSpan.Parse("07:35");

                db.Match.Add(m1);
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    m1.match_id,
                    m1.match_date,
                    m1.match_time,
                    m1.location,
                    m1.team_color1,
                    m1.team_color2,
                    m1.league_id,
                }, JsonMediaTypeFormatter.DefaultMediaType);
            }
            catch (Exception e)
            {
                logger.Error("Bad Request" + e);
                return Request.CreateResponse(HttpStatusCode.BadRequest, e);
            }
        }

        // PUT: api/Match/5
        public HttpResponseMessage Put(JObject matchData)
        {
            try
            {
                Match match = JsonConvert.DeserializeObject<Match>(matchData.ToString());

                Match m1 = db.Match.Where(m => m.match_id == match.match_id).FirstOrDefault();

                if (m1 == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, $"Match {match.match_id} was not found");
                }

                m1.match_date = match.match_date;
                m1.match_time = match.match_time;
                //m1.location = match.location;
                m1.team_color1 = match.team_color1;
                m1.team_color2 = match.team_color2;
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    m1.match_id,
                    m1.match_date,
                    m1.match_time,
                    //m1.location,
                    m1.team_color1,
                    m1.team_color2,
                    m1.league_id
                }, JsonMediaTypeFormatter.DefaultMediaType);
            }
            catch (Exception e)
            {
                logger.Error("Bad Request" + e);
                return Request.CreateResponse(HttpStatusCode.BadRequest, e.InnerException);
            }
        }


        //}

        // DELETE: api/Match/5
        //Delete Match
        //recive match_id. return ActiveIn
        public HttpResponseMessage Delete(JObject matchData)
        {
            try
            {
                Match match = JsonConvert.DeserializeObject<Match>(matchData.ToString());

                Match m1 = db.Match.Where(m => m.match_id == match.match_id).FirstOrDefault();

                if (m1 == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, $"Match {match.match_id} was not found");
                }

                db.Match.Remove(m1);
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, new
                {

                    m1.match_id,
                    m1.match_date,
                    m1.match_time,
                    m1.location,
                    m1.league_id


                }, JsonMediaTypeFormatter.DefaultMediaType);
            }
            catch (Exception e)
            {
                logger.Error("Bad Request" + e);
                return Request.CreateResponse(HttpStatusCode.BadRequest, e);
            }

        }

    }
}
