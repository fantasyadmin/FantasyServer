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
        //GET: api/ManageLeague
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
                if (league == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Could not find League");
                }
                return Request.CreateResponse(HttpStatusCode.OK,league);
            }
            
            catch
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Fetching League Data - Oops... Something went wrong");
            }
            
        }

        // POST: api/ManageLeague
        public HttpResponseMessage Post(int league_id, JObject new_league_details)
        {
            try
            {
                League league = JsonConvert.DeserializeObject<League>(new_league_details.ToString());
                League newLeague = db.League.Where(p => p.league_id == league_id).FirstOrDefault();
                if (newLeague == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Could not find League");
                }

                newLeague.league_name = league.league_name;
                newLeague.league_picture = league.league_picture;
                newLeague.league_rules = league.league_rules; 
                db.League.Add(newLeague);
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, newLeague);
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest,"Update League Details - Oops... Something went wrong");
            }
        }

        //POST: api/ManageLeague/5
        public HttpResponseMessage Post(int user_id, int league_id)
        {
            try
            {
                Player player = db.Player.Where(p => p.user_id == user_id).FirstOrDefault();
                if (player == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Player not found");
                }

                DateTime registration_date = DateTime.Now;

                Listed_in listed = new Listed_in() {user_id = player.user_id, nickname = player.nickname, registration_date = registration_date, league_id = league_id};
                db.Listed_in.Add(listed);
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, listed);
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Adding Player to League - Oops... Something went wrong");
            }
        }


        // DELETE: api/ManageLeague/5
        public HttpResponseMessage Delete(int user_id)
        {
            try
            {
                 Listed_in player_Listed_in = db.Listed_in.Where(p => p.user_id == user_id).FirstOrDefault();
                 if (player_Listed_in == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Player not found");
                }
            
               db.Listed_in.Remove(player_Listed_in);
               db.SaveChanges();
               return Request.CreateResponse(HttpStatusCode.OK, user_id);
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Delete Player from League - Oops... Something went wrong"); ;
            }

        }
    }
}
