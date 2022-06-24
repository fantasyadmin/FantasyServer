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
    public class DeletePLController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        bgroup89_prod_Entities db = new bgroup89_prod_Entities();

        // GET: api/DeletePL
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET: api/DeletePL/5

        public string Get(int id)
        {
            return "value";
        }

        // POST: api/DeletePL
        //Delete Player from League
        //recive user_id, league_id. return ListedIn
        public HttpResponseMessage Post(JObject leagueData)
        {
            logger.Trace("POST - ManageLeagueController - Delete player from league");
            Listed_in listed = JsonConvert.DeserializeObject<Listed_in>(leagueData.ToString());
            //League league = JsonConvert.DeserializeObject<League>(leagueData.ToString());
            try
            {

                //Player player = JsonConvert.DeserializeObject<Player>(leagueData.ToString());

                Listed_in ls = db.Listed_in.Where(p => p.user_id == listed.user_id && p.league_id == listed.league_id).FirstOrDefault();
                League l1 = db.League.Where(l => l.league_id == listed.league_id).FirstOrDefault();

                if (ls == null)
                {
                    logger.Error("POST - Empty reference - user: " + ls);
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Player not found");
                }

                db.Listed_in.Remove(ls);
                db.SaveChanges();
                logger.Trace("User removed from league - name: " + ls.user_id);

                var usres_in_league = db.Listed_in.Where(x => x.league_id == l1.league_id).Select(x => new { x.user_id, x.nickname, x.Player.player_score, x.Player.picture }).ToList();


                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    l1.league_id,
                    l1.league_name,
                    l1.league_picture,
                    l1.league_rules,
                    usres_in_league
                }, JsonMediaTypeFormatter.DefaultMediaType);
            }
            catch (Exception e)
            {
                logger.Error("Bad Request, could not Delete user: " + listed.user_id + "=======> " + e);
                return Request.CreateResponse(HttpStatusCode.BadRequest, e.Message); ;
            }
        }

        // PUT: api/DeletePL/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/DeletePL/5
        public void Delete(int id)
        {
        }
    }
}
