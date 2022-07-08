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

        bgroup89_prod_Entities db = new bgroup89_prod_Entities();
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
                var m1 = db.Match.Where(m => m.match_id == match.match_id).Select(x => new { x.match_id, x.league_id, x.lng, x.lat, x.match_date, x.match_time, x.team_color1, x.team_color2 }).FirstOrDefault();

                if (m1 == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, $"Match {match.match_id} was not found");
                }

                string matchDateStr = m1.match_date.ToString().Substring(0, 10);

                return Request.CreateResponse(HttpStatusCode.OK, new { m1.match_id, m1.league_id, m1.match_time, matchDateStr, m1.lat, m1.lng, m1.team_color1, m1.team_color2 }, JsonMediaTypeFormatter.DefaultMediaType);
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
                double lng = (double)match.lng;
                double lat = (double)match.lat;


                DateTime matchDate = DateTime.Parse(strhDate);
                TimeSpan matchTime = TimeSpan.Parse(strTime);
                //System.Data.Entity.Spatial.DbGeography matchLocation = System.Data.Entity.Spatial.DbGeography.FromText("POINT(47.605049 -82.336106)",4326);

                Match m1 = new Match()
                {
                    match_date = matchDate,
                    match_time = matchTime,
                    lng = lng,
                    lat = lat,
                    team_color1 = match.team_color1,
                    team_color2 = match.team_color2,
                    league_id = l1.league_id
                };

                //TimeSpan time = TimeSpan.Parse("07:35");

                db.Match.Add(m1);
                db.SaveChanges();


                //int length = m1.match_date.ToString().Length;

                string day = m1.match_date.Day.ToString();
                string month = m1.match_date.Month.ToString();
                string year = m1.match_date.Year.ToString();

                string matchDateStr = day + "/" + month + "/" + year;

                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    m1.match_id,
                    matchDateStr,
                    m1.match_time,
                    m1.lng,
                    m1.lat,
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

                string matchDateStr = m1.match_date.ToString().Substring(0, 10);

                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    m1.match_id,
                    matchDateStr,
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
                Active_in active_In = JsonConvert.DeserializeObject<Active_in>(matchData.ToString());

                Match m1 = db.Match.Where(m => m.match_id == match.match_id).FirstOrDefault();

                if (m1 == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, $"Match {match.match_id} was not found");
                }

                var ac1 = db.Active_in.Where(ac => ac.match_id == m1.match_id).ToList();
                foreach (var item in ac1)
                {
                    if (ac1 != null)
                    {
                        db.Active_in.Remove(item);
                        db.SaveChanges();
                    }
                }


                db.Match.Remove(m1);
                db.SaveChanges();

                string matchDateStr = m1.match_date.ToString().Substring(0, 10);

                return Request.CreateResponse(HttpStatusCode.OK, new
                {

                    m1.match_id,
                    matchDateStr,
                    m1.match_time,
                    m1.lng,
                    m1.lat,
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
