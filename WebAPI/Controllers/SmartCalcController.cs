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
    public class SmartCalcController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        bgroup89_prodEntities db = new bgroup89_prodEntities();
        // GET: api/SmartCalc
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/SmartCalc/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/SmartCalc
        public void Post([FromBody] string value)
        {
        }

        //Recive user_id. return playerScore
        // PUT: api/SmartCalc/5
        public HttpResponseMessage Put(JObject playerData)
        {
            logger.Trace("POST - SmartCalcController");
            //Converting userData to User

            Active_in active_In = JsonConvert.DeserializeObject<Active_in>(playerData.ToString());
            try
            {
                Active_in ac1 = db.Active_in.Where(a => a.user_id == active_In.user_id && a.match_id == active_In.match_id).FirstOrDefault();

                ac1.apporval_status = active_In.apporval_status;

                Player p1 = db.Player.Where(p => p.user_id == ac1.user_id).FirstOrDefault();

                if (ac1.apporval_status == 0)
                {
                    logger.Info($"POST - score is unapproved for player: {p1.user_id} in match: {ac1.match_id}");
                    return Request.CreateResponse(HttpStatusCode.NotFound, $"Score is unapproved for player: {p1.user_id} in match: {ac1.match_id}");
                }

                p1.games_played++;
                p1.total_assists += ac1.assists;
                p1.total_goals_recieved += ac1.goals_recieved;
                p1.total_goals_scored += ac1.goals_scored;
                p1.total_pen_missed += ac1.pen_missed;
                p1.total_wins += ac1.wins;

                double attackRate = (p1.total_wins + p1.total_goals_scored - p1.total_pen_missed) / (p1.games_played);
                double goalieRate = p1.games_played / (p1.total_goals_recieved + 1);
                double teamPlayerRate = (p1.total_wins + p1.total_assists) / (p1.games_played);
                p1.player_score = Convert.ToInt32((attackRate + goalieRate + teamPlayerRate) / 3 * 100);

                db.SaveChanges();


                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    p1.user_id,
                    p1.nickname,
                    ac1.apporval_status,
                    p1.games_played,
                    p1.total_assists,
                    p1.total_goals_recieved,
                    p1.total_goals_scored,
                    p1.total_pen_missed,
                    p1.total_wins,
                    attackRate,
                    goalieRate,
                    teamPlayerRate,
                    p1.player_score
                }, JsonMediaTypeFormatter.DefaultMediaType);

            }
            catch (Exception e)
            {

                logger.Error("Bad Request, data received = " + active_In.user_id + " | =======> " + e);
                return Request.CreateResponse(HttpStatusCode.BadRequest, e);
            }
        }

        

        // DELETE: api/SmartCalc/5
        public void Delete(int id)
        {
        }
    }
}
