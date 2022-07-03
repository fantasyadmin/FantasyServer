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
    public class NextMatchController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        bgroup89_prod_Entities db = new bgroup89_prod_Entities();
        // GET: api/NextMatch
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET: api/NextMatch/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/NextMatch
        public HttpResponseMessage Post(JObject matchData)
        {
            Match match = JsonConvert.DeserializeObject<Match>(matchData.ToString());

            try
            {
                DateTime today = DateTime.Today;
                DateTime now = DateTime.Now;
                TimeSpan time = now - today;

                var m2 = db.Match.Where(m => m.league_id == match.league_id && m.match_date >= today).OrderBy(a => a.match_date).Select(x => new { x.match_id, x.league_id, x.lng, x.lat, x.match_date, x.match_time, x.team_color1, x.team_color2 }).ToList();

                if (m2 == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, $"There is no Match in League {match.league_id}");
                }

                var m1 = m2.Where(x => x.match_date == m2.FirstOrDefault().match_date).OrderBy(a => a.match_time).FirstOrDefault();

                //var m1 = m4.OrderBy(x => x.match_time).FirstOrDefault();

                string day = m1.match_date.Day.ToString();
                string month = m1.match_date.Month.ToString();
                string year = m1.match_date.Year.ToString();

                //string str = m1.match_date.ToString();

                //if (m1.match_date.ToString().IndexOf(" ") != 0 && m1.match_date.ToString().IndexOf(" ") != -1)
                //{
                //    str = m1.match_date.ToString().Substring(0, m1.match_date.ToString().IndexOf(" "));
                //}
                //string day = str.Substring(0, 2);
                //string month = str.Substring(str.Length - 6, 2);
                //string year = str.Substring(str.Length -4, 4);
                string matchDateStr = day + "/" + month + "/" + year;

                string color1 = m1.team_color1;
                string color2 = m1.team_color2;

                if (m1.team_color1.IndexOf(" ") != 0 && m1.team_color1.IndexOf(" ") != -1)
                {
                    color1 = m1.team_color1.Substring(0, m1.team_color1.IndexOf(" "));
                }
                else if (m1.team_color2.IndexOf(" ") != 0 && m1.team_color1.IndexOf(" ") != -1)
                {
                    color2 = m1.team_color2.Substring(0, m1.team_color2.IndexOf(" "));
                }

                return Request.CreateResponse(HttpStatusCode.OK, new { m1.match_id, m1.league_id, m1.match_time, matchDateStr, m1.lat, m1.lng, color1, color2}, JsonMediaTypeFormatter.DefaultMediaType);
            }
            catch (Exception e)
            {

                logger.Error("Bad Request, could not find Match in League {match.league_id" + e);
                return Request.CreateResponse(HttpStatusCode.BadRequest, e);
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // PUT: api/NextMatch/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/NextMatch/5
        public void Delete(int id)
        {
        }
    }
}
