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
    public class ChangeResultsController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        bgroup89_prod_Entities db = new bgroup89_prod_Entities();
        // GET: api/ChangeResults
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET: api/ChangeResults/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/ChangeResults
        public HttpResponseMessage Post(JObject matchData)
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

        // PUT: api/ChangeResults/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ChangeResults/5
        public void Delete(int id)
        {
        }
    }
}
