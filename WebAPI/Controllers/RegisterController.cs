using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ClassLibrary2;
using Newtonsoft.Json;
using System.Net.Mail;

namespace WebAPI.Controllers
{
    public class RegisterController : ApiController
    {
        bgroup89_test2Entities db = new bgroup89_test2Entities();
        // GET: api/Register
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Register/email,pass
        public string Get(int id)
        {
            return "value" ;
        }

        // POST: api/Register
        public HttpResponseMessage Post(dynamic userData, int confirmationCode)
        {
            try
            {
                //
                User user = JsonConvert.DeserializeObject<User>(userData.user.ToString());

                if (user == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Creating User - Oops... Something Went Wrong!");
                }

                try
                {
                    string fromAdress = "fantasyleaguehood@gmail.com";
                    string fromPass = "dorguygal2022";
                    MailMessage message = new MailMessage();
                    SmtpClient smtp = new SmtpClient();
                    message.From = new MailAddress(fromAdress);
                    message.To.Add(new MailAddress(user.email));
                    message.Subject = "Confirm Registration";
                    message.IsBodyHtml = false; //to make message body as html  
                    message.Body = "Registration Code: " + (user.user_id+ 100);
                    smtp.Port = 587;
                    smtp.Host = "smtp.gmail.com"; //for gmail host 
                    smtp.EnableSsl = true;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(fromAdress, fromPass);
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.Send(message);
                }
                catch { return Request.CreateResponse(HttpStatusCode.BadRequest, "Failed to send email"); }

                if (confirmationCode != user.user_id + 100)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Inserted Incorrect Conmirfmation Code");
                }

                User u = new User() { email = user.email, password = user.password };
                db.User.Add(u);
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, u);
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
