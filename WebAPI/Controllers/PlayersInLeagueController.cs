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
    public class PlayersInLeagueController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        bgroup89_prodEntities db = new bgroup89_prodEntities();
        // GET: api/PlayersInLeague
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET: api/PlayersInLeague/5
        //public List<Player> Get(JObject leagueData)
        //{
        //    logger.Trace("POST - PlayersInLeagueController");
        //    //Converting teamData to Player
        //    Listed_in listed_In = JsonConvert.DeserializeObject<Listed_in>(leagueData.ToString());
        //    List<Listed_in> ls = db.Listed_in.Where(l => l.league_id == listed_In.league_id).ToList();

        //    listPerson = (from student in listStudent
        //                  select new Person
        //                  {
        //                      FirstName = student.FirstName,
        //                      LastName = student.LastName
        //                  }).ToList<Person>();
        //}

        // POST: api/PlayersInLeague
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/PlayersInLeague/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/PlayersInLeague/5
        public void Delete(int id)
        {
        }
    }
}
