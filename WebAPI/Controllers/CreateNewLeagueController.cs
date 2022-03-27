﻿using System;
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

        // GET: api/CreateNewLeague/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/CreateNewLeague
        public HttpResponseMessage Post(JObject leagueData)
        {
            try
            {
                //
                League league = JsonConvert.DeserializeObject<League>(leagueData.ToString());
                League l = new League() { league_name = league.league_name,
                                          league_picture = league.league_picture, 
                                          league_rules = league.league_rules, 
                                          invite_url = "https://cdn.bleacherreport.net/images_root/slides/photos/000/607/604/funny_cat_soccer_problem_original.jpg?1294007705" };

                db.League.Add(l);
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, l);
            }
            catch (Exception e)
            {

                return Request.CreateResponse(HttpStatusCode.BadRequest, e);
            }
        }
    

        // PUT: api/CreateNewLeague/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/CreateNewLeague/5
        public void Delete(int id)
        {
        }
    }
}
