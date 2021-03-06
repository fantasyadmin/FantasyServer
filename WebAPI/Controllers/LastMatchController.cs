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
    public class LastMatchController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        bgroup89_prod_Entities db = new bgroup89_prod_Entities();
        // GET: api/LastMatch
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET: api/LastMatch/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/LastMatch
        public HttpResponseMessage Post(JObject matchData)
        {
            Match match = JsonConvert.DeserializeObject<Match>(matchData.ToString());

            try
            {
                DateTime today = DateTime.Today;
                DateTime now = DateTime.Now ;
                TimeSpan time = now - today;
                

                var m2 = db.Match.Where(m => m.league_id == match.league_id && m.match_date <= today).OrderByDescending(a => a.match_date).Select(x => new { x.match_id, x.league_id, x.lng, x.lat, x.match_date, x.match_time, x.team_color1, x.team_color2 }).ToList();

                if (m2 == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, $"There was no Match in League {match.league_id} that is still active");
                }

                //var m3 = m2.FirstOrDefault();
                
                //var m4 = m2.Where(x => x.match_date <= m3.match_date).OrderByDescending(a => a.match_date).ToList();

                var m3 = m2.FirstOrDefault();

                var m1 = m3;

                if (m3.match_date == today)
                {
                    var m4 = m2.Where(x => x.match_date == m3.match_date && x.match_time < time).OrderByDescending(a => a.match_time).ToList();
                    m1 = m4.FirstOrDefault();
                }
                if (m1 == null || m3.match_date != today)
                {
                    var m4 = m2.Where(x => x.match_date < today).OrderByDescending(a => a.match_date).ToList();
                    m3 = m4.FirstOrDefault();
                    m1 = m4.Where(b => b.match_date == m3.match_date).OrderByDescending(a => a.match_time).FirstOrDefault();
                }


                string day = m1.match_date.Day.ToString();
                string month = m1.match_date.Month.ToString();
                string year = m1.match_date.Year.ToString();

                string matchDateStr = day + "/" + month + "/" + year;
                string color1 = m1.team_color1.Substring(0, m1.team_color1.IndexOf(" "));
                string color2 = m1.team_color2.Substring(0, m1.team_color2.IndexOf(" "));

                return Request.CreateResponse(HttpStatusCode.OK, new { m1.match_id, m1.league_id, m1.match_time, matchDateStr, m1.lat, m1.lng, color1, color2, time }, JsonMediaTypeFormatter.DefaultMediaType);
            }
            catch (Exception e)
            {

                logger.Error($"Bad Request, could not find  Match in League {match.league_id} that is still active");
                return Request.CreateResponse(HttpStatusCode.BadRequest, e);
            }
        }

        // PUT: api/LastMatch/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/LastMatch/5
        public void Delete(int id)
        {

        }
    }
}
