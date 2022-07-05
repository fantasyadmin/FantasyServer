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
    public class ForgotPasswordController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        bgroup89_prod_Entities db = new bgroup89_prod_Entities();
        // GET: api/ForgotPassword
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET: api/ForgotPassword/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/ForgotPassword
        public HttpResponseMessage Post(JObject emailData)
        {
            logger.Trace("POST - RegisterController");
            User user = JsonConvert.DeserializeObject<User>(emailData.ToString());

            try
            {
                User u1 = db.User.Where(u => u.email == user.email).FirstOrDefault();

                if (u1 == null)
                {
                    logger.Error($"We didn't find any user registered with this address{user.email}");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, $"We didn't find any user registered with this address{user.email}");
                }
                Random random = new Random();
                //int rand = random.Next(1000, 9999);
                //string newPassword = System.Web.Security.Membership.GeneratePassword(6, 0);

                var chars = "abcdefghijklmnopqrstuvwxyz1234567890 ?;:ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
                string newPassword = "";

                for (int i = 0; i <= 6; i++)
                {
                    if (i != 2 && i != 4)
                    {
                        string randNum = random.Next(0, 9).ToString();
                        newPassword += randNum;
                    }
                    else
                    {
                        int randChar = random.Next(0, 9);

                        newPassword += chars[randChar];
                    }

                }

                string body = File.ReadAllText(HttpContext.Current.Server.MapPath("~/NewPassswordEmail.html"));

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
                MailAddress toEmail = new MailAddress(u1.email, "New User");
                MailMessage message = new MailMessage()
                {
                    From = fromEmail,
                    Subject = "⚽Fantasy-League צ'כונה⚽ new password",
                    Body = body

                    //$"Need to choosse new players for your Fantasy Team before next Match and you forgot your password??⛔🥅\n\nNo worries!😎⛱️\n\nUse this password instead to Go and Win the League:\n\n\n       Password:      {newPassword}\n\n                ┏ヽ( ｀0´)ﾉ ┓　 ○⌒θ┐(｀ﾍ´；)\n",
                };

                message.IsBodyHtml = true;

                body = body.Replace("{rand}", newPassword.ToString());

                AlternateView avHtml = AlternateView.CreateAlternateViewFromString(body, null, MediaTypeNames.Text.Html);

                message.AlternateViews.Add(avHtml);

                message.To.Add(toEmail);
                try
                {
                    client.Send(message);

                    u1.password = newPassword;
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

        // PUT: api/ForgotPassword/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ForgotPassword/5
        public void Delete(int id)
        {
        }
    }
}
