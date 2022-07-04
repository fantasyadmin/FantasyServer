using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;
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
            logger.Trace("POST - RegisterController");
            Confirm confirm = JsonConvert.DeserializeObject<Confirm>(emailData.ToString());

            try
            {
                User user = JsonConvert.DeserializeObject<User>(emailData.ToString());

                Random random = new Random();
                int rand = random.Next(1000, 9999);

                string body = File.ReadAllText(HttpContext.Current.Server.MapPath("~/ConfirmationEmail.html"));

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
                    Subject = "⚽Fantasy-League צ'כונה⚽ Confirmation Code",
                    Body = body

                    //$"Welcome to ⚽Fantasy-League צ'כונה⚽ 🎉\n\nPlease enter the Confirmation Code in the Fantasy-League Hood app to complete your registration and become Top of your League       🏆🏆🏆🏆🏆🏆🏆🏆🏆🏆🏆🏆🏆\n\n\n             Confirmation Code:{rand}\n\n                ┏ヽ( ｀0´)ﾉ ┓　 ○⌒θ┐(｀ﾍ´；)\n",
                };
                message.IsBodyHtml = true;


                string htmlBody = body;
                AlternateView avHtml = AlternateView.CreateAlternateViewFromString
                    (htmlBody, null, MediaTypeNames.Text.Html);

                LinkedResource pic1 = new LinkedResource(@"C:\Users\Gal\Desktop\‏‏FantasyServer - עותק\WebAPI\images\abcde.jpg", MediaTypeNames.Image.Jpeg);
                pic1.ContentId = "Pic1";
                avHtml.LinkedResources.Add(pic1);
                message.AlternateViews.Add(avHtml);


                message.To.Add(toEmail);
                try
                {
                    client.Send(message);

                    Confirm c1 = new Confirm()
                    {
                        email = confirm.email,
                        confirmation_code = rand
                    };

                    db.Confirm.Add(c1);
                    db.SaveChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                catch (Exception e)
                {
                    logger.Error("Bad Request, could not send email to: " + toEmail + "=======>" + e);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, e);
                }

            }
            catch (Exception e)
            {
                logger.Error("Bad Request\n" + e);
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
