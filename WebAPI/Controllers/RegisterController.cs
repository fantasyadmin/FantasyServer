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

namespace WebAPI.Controllers
{
    public class RegisterController : ApiController
    {
        bgroup89_test2Entities db = new bgroup89_test2Entities();
        //GET: api/Register
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // Post: api/Register
        //send email & password. return HttpMessage (Ok, Not Found, Bad Request) & confirmationCode (send it back for email confirmation)
        //
        //public HttpResponseMessage Get([FromBody] JObject userData)
        //{
        //    //Converting userData to User
        //    //User user = JsonConvert.DeserializeObject<User>(userData.ToString());
        //    //if (user == null)
        //    //{
        //    //    return Request.CreateResponse(HttpStatusCode.BadRequest, "Fetching user input - Oops... Something Went Wrong!");
        //    //}
        //    ////Creating Confirmtaion Code
        //    //Random rnd = new Random();
        //    //int confirmationCode = rnd.Next(1000, 9999);

        //    ////Sending Confirmation email using smtp
        //    //try
        //    //{
        //    //    string fromAdress = "fantasyleaguehood@gmail.com";
        //    //    string fromPass = "dorguygal2022";
        //    //    MailMessage message = new MailMessage();
        //    //    SmtpClient smtp = new SmtpClient();
        //    //    message.From = new MailAddress(fromAdress);
        //    //    message.To.Add(new MailAddress(user.email));
        //    //    message.Subject = "Confirm Registration";
        //    //    message.IsBodyHtml = false; //to make message body as html  
        //    //    message.Body = "Registration Code: " + (confirmationCode);
        //    //    smtp.Port = 587;
        //    //    smtp.Host = "smtp.gmail.com"; //for gmail host 
        //    //    smtp.EnableSsl = true;
        //    //    smtp.UseDefaultCredentials = false;
        //    //    smtp.Credentials = new NetworkCredential(fromAdress, fromPass);
        //    //    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
        //    //    smtp.Send(message);

        //    //    //Sending the confirmationCode to front to rcieve it back for Confirmtaion
        //    //    return Request.CreateResponse(HttpStatusCode.OK, confirmationCode);
        //    //}
        //    //catch { return Request.CreateResponse(HttpStatusCode.BadRequest, $"Failed to send email to {user.email}"); };

        //}

        // POST: api/Register
        //Recieve userName, email, Password
        public HttpResponseMessage Post(/*JObject confirmationCode, JObject userCodeInput, */JObject userData)
        {
            try
            {
                //Converting userData to User
                User user = JsonConvert.DeserializeObject<User>(userData.ToString());
                Player player = JsonConvert.DeserializeObject<Player>(userData.ToString());

                //int confirmation = JsonConvert.DeserializeObject<int>(confirmationCode.ToString());
                //int codeInput = JsonConvert.DeserializeObject<int>(userCodeInput.ToString());
                if (user == null || player == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Fetching user input - Oops... Something Went Wrong!");
                }

                //if (codeInput == confirmation)
                //{
                User u = new User() { email = user.email, password = user.password };
                Player p = new Player() { nickname = player.nickname, user_id = u.user_id };
                //Fantasy_team f = new Fantasy_team() {user_id = p.user_id };

                //Player p = new Player() { league_manager = true };
                db.User.Add(u);
                db.Player.Add(p);
                
                db.SaveChanges();


                return Request.CreateResponse(HttpStatusCode.OK, u);
                //return Request.CreateResponse(HttpStatusCode.OK,new { u, p},JsonMediaTypeFormatter.DefaultMediaType);

                //return Request.CreateResponse(HttpStatusCode.OK, u);
                //}

                //return Request.CreateResponse(HttpStatusCode.NotFound, "Wrong Confirmation Code");
            }
            catch
            {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Creating Player - Oops... Something Went Wrong!");   
            }
        }

        // PUT: api/Register/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Register/5
        public void Delete(int id)
        {

        }
    }
}
