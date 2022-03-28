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
    public class CreateNewLeagueController : ApiController
    {
        bgroup89_test2Entities db = new bgroup89_test2Entities();

        // GET: api/CreateNewLeague
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET: api/CreateNewLeague/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/CreateNewLeague
        //send user_id, league_name, league_picture, league_rules. return (Ok, Not Found, Bad Request)
        public HttpResponseMessage Post(JObject leagueData)
        {
            try
            {
                //
                League league = JsonConvert.DeserializeObject<League>(leagueData.ToString());
                Player player = JsonConvert.DeserializeObject<Player>(leagueData.ToString());

                Player p1 = db.Player.Where(p => p.user_id == player.user_id).FirstOrDefault();
                p1.league_manager = true;

                League l = new League()
                {
                    league_name = league.league_name,
                    league_picture = league.league_picture,
                    league_rules = league.league_rules,
                    invite_url = "https://cdn.bleacherreport.net/images_root/slides/photos/000/607/604/funny_cat_soccer_problem_original.jpg?1294007705"
                };
                //Player p1 = db.Player.Where(p => p.user_id == player.user_id).Where(l => l.Listed_in)

                db.Player.Append(p1);
                db.SaveChanges();
                db.League.Add(l);
                db.SaveChanges();

                Listed_in ls = new Listed_in()
                {
                    user_id = p1.user_id,
                    registration_date = DateTime.Now,
                    league_id = l.league_id,
                    nickname = p1.nickname

                }; //db.Listed_in.Where(r => r.user_id == p1.user_id).FirstOrDefault();


                db.Listed_in.Add(ls);
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, l);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, e);
            }
        }


        // PUT: api/CreateNewLeague/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/CreateNewLeague/5
        public void Delete(int id)
        {
        }
    }
}
