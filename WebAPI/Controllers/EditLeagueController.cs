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
    public class EditLeagueController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        bgroup89_prod_Entities db = new bgroup89_prod_Entities();
        // GET: api/EditLeague
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET: api/EditLeague/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/EditLeague
        public HttpResponseMessage Post(JObject leagueData)
        {
            logger.Trace("POST - ManageLeagueController - Add player to league");
            League league = JsonConvert.DeserializeObject<League>(leagueData.ToString());

            try
            {
                League l1 = db.League.Where(l => l.league_id == league.league_id).FirstOrDefault();
                logger.Trace("POST - DB connection by - " + league.league_id + " returned - " + l1.league_id);

                if (l1 == null)
                {
                    logger.Error("POST - League " + league.league_id + " does not exist in DB");
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Could not find League");
                }

                if (league.league_name != "")
                {
                    l1.league_name = league.league_name;
                }
                if (league.league_picture != "")
                {
                    l1.league_picture = league.league_picture;
                }
                if (league.invite_url != "")
                {
                    l1.invite_url = league.invite_url;
                }
                l1.league_rules = league.league_rules;

                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    l1.league_id,
                    l1.league_name,
                    l1.league_picture,
                    l1.league_rules,
                    l1.invite_url
                }, JsonMediaTypeFormatter.DefaultMediaType);
            }
            catch (Exception e)
            {
                logger.Error($"Bad Request, could update League: {league.league_id} --> {e}");
                return Request.CreateResponse(HttpStatusCode.BadRequest, e.Message);
            }
        }

        // PUT: api/EditLeague/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/EditLeague/5
        public void Delete(int id)
        {
        }
    }
}
