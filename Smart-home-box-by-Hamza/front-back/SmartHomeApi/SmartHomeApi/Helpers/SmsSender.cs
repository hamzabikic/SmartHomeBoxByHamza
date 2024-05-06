using SmartHomeApi.Data.Tablice;
using SmartHomeApi.Data;
using Twilio.Types;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace SmartHomeApi.Helpers
{
    public class SmsSender
    {
        private readonly MyDBContext db;
        private readonly AuthService auth;
        public SmsSender(MyDBContext _db, AuthService _auth)
        {
            db = _db;
            auth = _auth;
        }
        public async Task<bool> sendMessage(string message)
        {
            var prijavainfo = await auth.getInfo();
            Korisnik korisnik = prijavainfo.Prijava.Korisnik;
            if (korisnik.SmsSlanje)
            {
                var accountSid = "AC63cc1500ff6b56016386debfc187a369";
                var authToken = "a552698078babdc40e90f6882dadb4b9";
                TwilioClient.Init(accountSid, authToken);
                PhoneNumber sender = new PhoneNumber("+18454933455");
                PhoneNumber target = new PhoneNumber($"+{korisnik.BrojTelefona}");
                var messageOptions = new CreateMessageOptions(
                      target);
                messageOptions.From = sender;
                messageOptions.Body = message;
                try
                {
                    var messageSend = await MessageResource.CreateAsync(messageOptions);
                    return true;
                }
                catch(Exception ex)
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
