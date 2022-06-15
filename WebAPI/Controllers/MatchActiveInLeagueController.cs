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
    public class MatchActiveInLeagueController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        bgroup89_prod_Entities db = new bgroup89_prod_Entities();
        // GET: api/MatchActiveInLeague
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET: api/MatchActiveInLeague/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/MatchActiveInLeague
        public HttpResponseMessage Post(JObject matchData)
        {
            Active_in active_In = JsonConvert.DeserializeObject<Active_in>(matchData.ToString());

            try
            {

                var m1 = db.Active_in.Where(a => a.league_id == active_In.league_id).Select(x => new { x.match_id, x.league_id, x.user_id, x.apporval_status, x.wins, x.assists, x.goals_recieved, x.goals_scored, x.match_color, x.pen_missed}).ToList();

                if (m1 == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, $"ActiveIn {active_In.match_id} was not found");
                }

                return Request.CreateResponse(HttpStatusCode.OK, new { m1 }, JsonMediaTypeFormatter.DefaultMediaType);
            }
            catch (Exception e)
            {

                logger.Error("Bad Request, could not edit data for match: " + active_In.match_id + "=======>" + e);
                return Request.CreateResponse(HttpStatusCode.BadRequest, e);
            }
        }

        // PUT: api/MatchActiveInLeague/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/MatchActiveInLeague/5
        public void Delete(int id)
        {
        }
    }
}
