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
        //Get League Data
        //recive league_id. return league
        public HttpResponseMessage Get(JObject leagueData)
        {
            try
            {
                League league = JsonConvert.DeserializeObject<League>(leagueData.ToString());
                if (league == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Fetching League Data -Oops... Something went wrong");
                }
                League l1 = db.League.Where(l => l.league_id == league.league_id).FirstOrDefault();
                if (l1 == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Could not find League");
                }
                return Request.CreateResponse(HttpStatusCode.OK,l1);
            }
            
            catch
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Fetching League Data - Oops... Something went wrong");
            }
            
        }

        // POST: api/ManageLeague
        //Add Player to League
        //recive league_id, user_id, nickname. return league
        public HttpResponseMessage Post(JObject leagueData)
        {
            try
            {
                League league = JsonConvert.DeserializeObject<League>(leagueData.ToString());
                Player player = JsonConvert.DeserializeObject<Player>(leagueData.ToString());

                Player p1 = db.Player.Where(p => p.user_id == player.user_id).FirstOrDefault();

                League l1 = db.League.Where(l => l.league_id == league.league_id).FirstOrDefault();

                if (p1 == null || l1 == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                Listed_in ls = new Listed_in()
                {
                    league_id = l1.league_id,
                    user_id = p1.user_id,
                    nickname = p1.nickname,
                    registration_date = DateTime.Now
                };

                db.Listed_in.Add(ls);
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, l1);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, e);
            }
        }

        //POST: api/ManageLeague/5
        //Edit League Data
        //recive league_id, league_name, league_picture, league_rules. return league
        public HttpResponseMessage Put(JObject leagueData)
        {
            try
            {
                League league = JsonConvert.DeserializeObject<League>(leagueData.ToString());
                League l1 = db.League.Where(l => l.league_id == league.league_id).FirstOrDefault();
                if (l1 == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Could not find League");
                }

                l1.league_name = league.league_name;
                l1.league_picture = league.league_picture;
                l1.league_rules = league.league_rules;
                db.League.Append(l1);
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, l1);
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Update League Details - Oops... Something went wrong");
            }
        }


        // DELETE: api/ManageLeague/5
        //Delete Player from League
        //recive user_id. return ListedIn
        public HttpResponseMessage Delete(JObject leagueData)
        {
            try
            {
                Listed_in listed = JsonConvert.DeserializeObject<Listed_in>(leagueData.ToString());
                //Player player = JsonConvert.DeserializeObject<Player>(leagueData.ToString());

                Listed_in ls = db.Listed_in.Where(p => p.user_id == listed.user_id).FirstOrDefault();

                 if (ls == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Player not found");
                }
            
               db.Listed_in.Remove(ls);
               db.SaveChanges();
               return Request.CreateResponse(HttpStatusCode.OK, ls);
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Delete Player from League - Oops... Something went wrong"); ;
            }

        }
    }
}
