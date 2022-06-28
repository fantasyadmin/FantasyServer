using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ClassLibrary2;
using Newtonsoft.Json;
using System.Net.Mail;
using Newtonsoft.Json.Linq;
using System.Net.Http.Formatting;
using NLog;

namespace WebAPI.Controllers
{
    public class RegisterController : ApiController
    {

        private static Logger logger = LogManager.GetCurrentClassLogger();

        bgroup89_prod_Entities db = new bgroup89_prod_Entities();

        // POST: api/Register
        //Recieve userName, email, Password, existing league id number
        public HttpResponseMessage Post(/*JObject confirmationCode, JObject userCodeInput, */JObject userData)
        {
            logger.Trace("POST - RegisterController");

            //Converting userData to User
            User user = JsonConvert.DeserializeObject<User>(userData.ToString());

            Player player = JsonConvert.DeserializeObject<Player>(userData.ToString());
            
            League league = JsonConvert.DeserializeObject<League>(userData.ToString());

            Confirm confirm = JsonConvert.DeserializeObject<Confirm>(userData.ToString());

            try
            {
                if (user == null || player == null)
                {
                    logger.Error("POST - Empty reference - user: " + user + " | player: " + player);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Fetching user input - Oops... Something Went Wrong!");
                }

                Confirm c1 = db.Confirm.Where(c => c.email == user.email && c.confirmation_code == confirm.confirmation_code).FirstOrDefault();

                if (c1 == null)
                {
                    logger.Error("Invalid Confirmation Code");
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Invalid Confirmation Code");
                }

                User u1 = db.User.Where(a => a.email == user.email).FirstOrDefault();
                logger.Trace("POST - DB connection by - " + user.email);

                if (u1 != null)
                {
                    logger.Error("POST - Occupied Value in - user: " + u1);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Email already exist");
                }

                User u = new User() { email = user.email, password = user.password };

                db.User.Add(u);
                logger.Trace("User added to DB" + u.email);
                db.SaveChanges();

                //User u2 = db.User.Where(a => a.email == user.email).FirstOrDefault();


                League l1 = db.League.Where(l => l.league_id == league.league_id).FirstOrDefault();

                if (l1 != null)
                {
                    Player p = new Player() { nickname = player.nickname, user_id = u.user_id };

                    db.Player.Add(p);
                    logger.Trace("Player added to DB" + p.nickname);
                    db.SaveChanges();
                    logger.Trace("Changes Saved to DB - SUCCESS!");

                    Listed_in ls = new Listed_in()
                    {
                        league_id = l1.league_id,
                        user_id = p.user_id,
                        nickname = p.nickname,
                        registration_date = DateTime.Now
                    };

                    db.Listed_in.Add(ls);
                    db.SaveChanges();
                    logger.Trace("POST - added player to existing league - " + league.league_id + " added user: - " + u.user_id);
                    Fantasy_team ft = new Fantasy_team()
                    {
                        user_id = p.user_id,
                        league_id = l1.league_id,
                        team_budget = 100
                    };
                    db.Fantasy_team.Add(ft);
                    db.SaveChanges();
                    logger.Trace("POST - added Fantasy-Team to League - " + league.league_id);

                    return Request.CreateResponse(HttpStatusCode.OK, new { u.user_id, p.nickname, l1.league_id }, JsonMediaTypeFormatter.DefaultMediaType);

                }
                else
                {
                    //============================================================================================
                    Player p = new Player() { nickname = player.nickname, user_id = u.user_id, league_manager=true };

                    db.Player.Add(p);
                    logger.Trace("Player added to DB" + p.nickname);
                    db.SaveChanges();
                    logger.Trace("Changes Saved to DB - SUCCESS!");

                    League l = new League()
                    {
                        league_name = "",
                        invite_url = "https://cdn.bleacherreport.net/images_root/slides/photos/000/607/604/funny_cat_soccer_problem_original.jpg?1294007705"
                    };

                    db.League.Add(l);
                    db.SaveChanges();
                    logger.Trace("POST - Created new (Generic) League - " + league.league_id);

                    Listed_in ls1 = new Listed_in()
                    {
                        league_id = l.league_id,
                        user_id = p.user_id,
                        nickname = p.nickname,
                        registration_date = DateTime.Now
                    };

                    db.Listed_in.Add(ls1);
                    db.SaveChanges();
                    logger.Trace("POST - added player to existing league - " + league.league_id + " added user: - " + u.user_id);

                    Fantasy_team ft1 = new Fantasy_team()
                    {
                        user_id = p.user_id,
                        league_id = l.league_id,
                        team_budget = 100
                    };
                    db.Fantasy_team.Add(ft1);
                    db.SaveChanges();
                    logger.Trace("POST - added Fantasy-Team to League - " + l.league_id);

                    return Request.CreateResponse(HttpStatusCode.OK, new { u.user_id, p.nickname, l.league_id }, JsonMediaTypeFormatter.DefaultMediaType);

                }


                //return Request.CreateResponse(HttpStatusCode.OK, u.user_id);
            }
            catch (Exception e)
            {
                logger.Error("Bad Request, data received = user: "+ user + e);
                return Request.CreateResponse(HttpStatusCode.BadRequest,"Error" + e.InnerException);
            }
        }

        // PUT: api/Register/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/Register/5
        public void Delete(int id)
        {
        }
    }
}
