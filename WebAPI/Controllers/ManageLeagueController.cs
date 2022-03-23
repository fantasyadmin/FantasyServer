using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ClassLibrary2;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WebAPI.Controllers
{
    public class ManageLeagueController : ApiController
    {
        bgroup89_test2Entities db = new bgroup89_test2Entities();
        // GET: api/ManageLeague
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/ManageLeague/5
        public HttpResponseMessage Get(int league_id)
        {
            try
            {
                League league = db.League.Where(l => l.league_id == league_id).FirstOrDefault();
                return Request.CreateResponse(HttpStatusCode.OK,league);
            }
            
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "");
            }
            
        }

        // POST: api/ManageLeague
        public HttpResponseMessage Post(int league_id, JObject new_league_details)
        {
            try
            {
                League league = JsonConvert.DeserializeObject<League>(new_league_details.ToString());
                League newLeague = db.League.Where(p => p.league_id == league_id).FirstOrDefault();

                newLeague.league_name = league.league_name;
                newLeague.league_picture = league.league_picture;
                newLeague.league_rules = league.league_rules;
                db.League.Add(newLeague);
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, newLeague);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest,"Update League Details - Oops... Something went wrong");
            }
        }

        // PUT: api/ManageLeague/5
        public string Get(int id, string value)
        {
            return "Kasda Be Rosh Tov";
        }

        // DELETE: api/ManageLeague/5
        public void Delete(int id)
        {
        }
    }
}
