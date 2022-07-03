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
    public class ChangeManagerController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        bgroup89_prod_Entities db = new bgroup89_prod_Entities();
        // GET: api/ChangeManager
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET: api/ChangeManager/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/ChangeManager
        public HttpResponseMessage Post(JObject playerhData)
        {
            Player player = JsonConvert.DeserializeObject<Player>(playerhData.ToString());

            try
            {
                Player p1 = db.Player.Where(p => p.user_id == player.user_id).FirstOrDefault();

                if (p1 == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, $"User {player.user_id}, was not found");
                }

                p1.league_manager = true;
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                logger.Error($"Bad Request, could not change league manager {player.user_id}.\n{e}");
                return Request.CreateResponse(HttpStatusCode.BadRequest, e);
            }
        }

        // PUT: api/ChangeManager/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ChangeManager/5
        public void Delete(int id)
        {
        }
    }
}
