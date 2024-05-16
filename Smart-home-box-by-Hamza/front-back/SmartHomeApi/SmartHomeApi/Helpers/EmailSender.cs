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
        private readonly AuthService auth;
        public EmailSender(MyDBContext _db, AuthService _auth)
        {
            db = _db;
            auth = _auth;
            smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("adszarada55@gmail.com", "uqpe mcph hjyk bzgc"),
                EnableSsl = true
            };
        }
        public async Task<bool> sendPassword(string emailTo, string password, string username)
        {
            try
            {
                var subject = "Smart home box by Hamza - PASSWORD CHANGE";
                MailMessage mailMessage = new MailMessage
                {
                    From = new MailAddress("adszarada55@gmail.com"),
                    Subject = subject,
                    Body = $"Your new application login details are:\n " +
                    $"Username: {username}\n" +
                    $"Password: {password}",
                    IsBodyHtml = false
                };

                mailMessage.To.Add(emailTo);

                await smtpClient.SendMailAsync(mailMessage);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    public async Task<bool> sendEmail(string message)
    {
        var prijavainfo = await auth.getInfo();
        Korisnik korisnik = prijavainfo.Prijava.Korisnik;
        if (korisnik.EmailSlanje)
        {
            try
            {
                var subject = "Smart home box by Hamza - WARNING";
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
