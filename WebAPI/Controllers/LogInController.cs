﻿using System;
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
    public class LogInController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        bgroup89_prodEntities db = new bgroup89_prodEntities();

        // POST: api/LogIn/5
        //recive email, password. return Ok + User
        public HttpResponseMessage Post(JObject userData)
        {
            logger.Trace("POST - LogInController");
            //Converting userData to User
            User user = JsonConvert.DeserializeObject<User>(userData.ToString());

            try
            {
                //find the user
                User u1 = db.User.Where(u => u.email == user.email).FirstOrDefault();

                if (u1.email != null && u1.password != null && u1.password == user.password)
                {
                    logger.Trace("POST - DB connection by - " + user.email + " returned - " + u1.email);

                    Player p1 = db.Player.Where(p => p.user_id == u1.user_id).FirstOrDefault();
                    Listed_in ls1 = db.Listed_in.Where(ls => ls.user_id == p1.user_id).FirstOrDefault();
                    League l1 = db.League.Where(l => l.league_id == ls1.league_id).FirstOrDefault();
                    Fantasy_team fs = db.Fantasy_team.Where(f => f.league_id == l1.league_id && f.user_id == p1.user_id).FirstOrDefault();

                    List<Listed_in> usres_in_league = db.Listed_in.Where(x => x.league_id == l1.league_id).ToList();
                    logger.Error(usres_in_league);

                    int?[] listing = new int?[usres_in_league.Count];
                    int counter_u = 0;

                    foreach (var item in usres_in_league)
                    {
                        listing[counter_u] = item.user_id;

                        counter_u++;
                    }

                    Player player1 = db.Player.Where(p => p.user_id == fs.player1).FirstOrDefault();
                    Player player2 = db.Player.Where(p => p.user_id == fs.player2).FirstOrDefault();
                    Player player3 = db.Player.Where(p => p.user_id == fs.player3).FirstOrDefault();
                    Player player4 = db.Player.Where(p => p.user_id == fs.player4).FirstOrDefault();


                    //List<Player> players_in_league = db.Player.Where(x => x.user_id == p1.user_id).ToList();
                    //logger.Error(players_in_league);

                    //Player[] listing_player = new Player[usres_in_league.Count];
                    //int counter_p = 0;

                    //foreach (var item in players_in_league)
                    //{
                    //    listing_player[counter_p] = JsonConvert.DeserializeObject<Player>(item.ToString()); 
                    //    counter_p++;
                    //}

                    return Request.CreateResponse(HttpStatusCode.OK, new 
                    {
                        //user
                        u1.user_id,
                        //player
                        p1.nickname,
                        p1.picture,
                        p1.league_manager,
                        p1.player_score,
                        p1.total_assists,
                        p1.total_goals_recieved,
                        p1.total_goals_scored,
                        p1.total_pen_missed,
                        p1.total_wins,
                        //listed in
                        listing,
                        //league
                        l1.league_id,
                        l1.league_name,
                        l1.league_picture,
                        l1.league_rules,
                        //Fantasy Team
                        fs.player1,
                        fs.player2,
                        fs.player3,
                        fs.player4,
                        fs.team_budget,
                        fs.team_id,
                        fs.team_points

                    }, JsonMediaTypeFormatter.DefaultMediaType);
                }

                logger.Info("POST - DB connection by - " + user.email + " returned - " + u1.email);
                return Request.CreateResponse(HttpStatusCode.NotFound, "User not found, Check your email or password");
            }
            catch (Exception e)
            {
                logger.Error("Bad Request, data received = " + user.email + " | =======> " + e);
                return Request.CreateResponse(HttpStatusCode.BadRequest, e.Message);
            }
        }


        //public void Post([FromBody]string value)
        //{

        //}

        // PUT: api/LogIn/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/LogIn/5
        public void Delete(int id)
        {
        }
    }
}
