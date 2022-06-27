using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Mail;
using System.Web.Http;
using ClassLibrary2;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;

namespace WebAPI.Controllers
{
    public class SendEmailController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        bgroup89_prod_Entities db = new bgroup89_prod_Entities();
        // GET: api/SendEmail
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET: api/SendEmail/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/SendEmail
        public HttpResponseMessage Post(JObject emailData)
        {
            User user = JsonConvert.DeserializeObject<User>(emailData.ToString());

            Random random = new Random();
            int rand = random.Next(1000, 9999);

            SmtpClient client = new SmtpClient()
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential()
                {
                    UserName = "fantasyleaguehood@gmail.com",
                    Password = "ffxsebwzfcwxkvfh"
                }
            };

            MailAddress fromEmail = new MailAddress("fantasyleaguehood@gmail.com", "Fantasy-League צ'כונה");
            MailAddress toEmail = new MailAddress(user.email, "New User");
            MailMessage message = new MailMessage()
            {
                From = fromEmail,
                Subject = "Confirmation Code",
                Body = $"Welcome to Fantasy-League Hood!\n\nPlease enter the Confirmation Code in the Fantasy-League Hood app to complete your registration\n\n\n             Confirmation Code:{rand}",
            };
            message.To.Add(toEmail);
            try
            {
                client.Send(message);
                return Request.CreateResponse(HttpStatusCode.OK, true);
            }
            catch (Exception e)
            {
                logger.Error("Bad Request, could not send email to: " + toEmail + "=======>" + e);
                return Request.CreateResponse(HttpStatusCode.BadRequest, e);
            }
        }



        // PUT: api/SendEmail/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/SendEmail/5
        public void Delete(int id)
        {
        }
    }
}
