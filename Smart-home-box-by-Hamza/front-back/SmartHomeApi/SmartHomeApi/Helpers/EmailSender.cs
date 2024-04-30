using Duende.IdentityServer.Test;
using SmartHomeApi.Data;
using SmartHomeApi.Data.Tablice;
using System.Net;
using System.Net.Mail;
using static Duende.IdentityServer.Models.IdentityResources;

namespace SmartHomeApi.Helpers
{
    public class EmailSender
    {
        private readonly SmtpClient smtpClient;
        private readonly MyDBContext db;
        public EmailSender (MyDBContext _db)
        {
            db = _db;
            smtpClient = new SmtpClient ("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("adszarada55@gmail.com", "uqpe mcph hjyk bzgc"),
                EnableSsl = true 
            };
        }
        public async Task<bool> sendEmail(string message)
        {
            Korisnik korisnik = db.Korisnici.First();
            if (korisnik.EmailSlanje)
            {
                try
                {
                    var subject = "Smart home box by Hamza - UPOZORENJE";
                    MailMessage mailMessage = new MailMessage
                    {
                        From = new MailAddress("adszarada55@gmail.com"),
                        Subject = subject,
                        Body = message,
                        IsBodyHtml = false
                    };

                    mailMessage.To.Add(korisnik.Email);

                    await smtpClient.SendMailAsync(mailMessage);

                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
