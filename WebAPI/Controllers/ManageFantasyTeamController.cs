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
    public class ManageFantasyTeamController : ApiController
    {
        bgroup89_test2Entities db = new bgroup89_test2Entities();

        // GET: api/ManageFantasyTeam
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/ManageFantasyTeam/5
        //Get FantasyTeam Details
        //recive team_id. return Fantasy Team .
        public HttpResponseMessage Get(JObject teamData)
        {
            Fantasy_team fantasy_Team = JsonConvert.DeserializeObject<Fantasy_team>(teamData.ToString());

            Fantasy_team fs = db.Fantasy_team.Where(f => f.team_id == fantasy_Team.team_id).FirstOrDefault();

            if (fs == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Team not found");
            }

            return Request.CreateResponse(HttpStatusCode.OK, fs);
        }

        // POST: api/ManageFantasyTeam
        public void Post([FromBody]string value)
        {

        }

        // PUT: api/ManageFantasyTeam/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ManageFantasyTeam/5
        public void Delete(int id)
        {
        }
    }
}
