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
    public class EditPlayerController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        bgroup89_prod_Entities db = new bgroup89_prod_Entities();

        // Get: api/EditPlayer/5
        //Recive user_id. return player
        public HttpResponseMessage Get(JObject userData)
        {
            logger.Trace("Get - EditPlayerController");

            try
            {
                Player player = JsonConvert.DeserializeObject<Player>(userData.ToString());

                Player p = db.Player.Where(a => a.user_id == player.user_id).FirstOrDefault();

                if (p != null)
                {
                    logger.Trace("Get - DB connection by - " + player.user_id + " returned - " + p.user_id);

                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        p.user_id,
                        p.nickname,
                        p.picture,
                        p.games_played,
                        p.league_manager,
                        p.player_score,
                        p.total_assists,
                        p.total_goals_recieved,
                        p.total_goals_scored,
                        p.total_pen_missed,
                        p.total_wins
                    }, JsonMediaTypeFormatter.DefaultMediaType);
                }

                logger.Info("POST - DB connection by - " + player.user_id + " returned - " + p.user_id);
                return Request.CreateResponse(HttpStatusCode.NotFound, "Player not found");
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, e);
            }
        }

        // POST: api/EditPlayer
        //Recive user_id, picture. return player

        public HttpResponseMessage Post(JObject userData)
        {
            logger.Trace("POST - EditPlayerController");
            Player player = JsonConvert.DeserializeObject<Player>(userData.ToString());

            try
            {

                if (player == null)
                {
                    //logger.Error("POST - data was not recieved - Player: " + player);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Fetching Data - Oops... Something Went Wrong!");
                }

                Player p1 = db.Player.Where(p => p.user_id == player.user_id).FirstOrDefault();
                logger.Trace("POST - DB connection by - " + player.user_id + " returned - " + p1.user_id);

                if (p1 == null)
                {
                    logger.Error("POST - Empty reference - Player: " + p1);
                    return Request.CreateResponse(HttpStatusCode.NotFound, " Fetching Data - Oops... Player not found!");
                }

                //p1.nickname = player.nickname;
                p1.picture = player.picture;

                //db.Player.Append(p1);
                db.SaveChanges();
                logger.Trace("player data was edited in DB Player - " + p1.nickname);


                return Request.CreateResponse(HttpStatusCode.OK, new { p1.user_id, p1.nickname, p1.picture }, JsonMediaTypeFormatter.DefaultMediaType);

            }
            catch (Exception e)
            {
                //logger.Error("Bad Request, could not edit data for player: " + player.nickname + "=======>" + e);
                return Request.CreateResponse(HttpStatusCode.BadRequest, e);
            }
        }

        // PUT: api/EditPlayer/5
        //Recive user_id, picture, nickname. return player
        public HttpResponseMessage Put(JObject playerData)
        {
            Player player = JsonConvert.DeserializeObject<Player>(playerData.ToString());

            try
            {
                Player p1 = db.Player.Where(p => p.user_id == player.user_id).FirstOrDefault();

                //p1.games_played = player.games_played;
                if (p1.picture != null)
                {
                    p1.picture = player.picture;
                }
                //p1.total_assists = player.total_assists;
                //p1.total_goals_recieved = player.total_goals_recieved;
                //p1.total_goals_scored = player.total_goals_scored;
                //p1.total_pen_missed = player.total_pen_missed;
                //p1.total_wins = player.total_wins;

                db.SaveChanges();



                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    p1.user_id,
                    p1.nickname,
                    p1.games_played,
                    p1.picture,
                    p1.total_assists,
                    p1.total_goals_recieved,
                    p1.total_goals_scored,
                    p1.total_pen_missed,
                    p1.total_wins


                }, JsonMediaTypeFormatter.DefaultMediaType);
            }
            catch (Exception e)
            {
                logger.Error("Bad Request, data received = " + player.user_id + " | =======> " + e);
                return Request.CreateResponse(HttpStatusCode.BadRequest, e);
            }
        }

        // DELETE: api/EditPlayer/5
        public void Delete(int id)
        {

        }
    }
}
