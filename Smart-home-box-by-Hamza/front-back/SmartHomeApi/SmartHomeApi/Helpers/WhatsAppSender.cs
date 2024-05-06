﻿using SmartHomeApi.Data;
using SmartHomeApi.Data.Tablice;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace SmartHomeApi.Helpers
{
    public class WhatsAppSender
    {
        private readonly MyDBContext db;
        private readonly AuthService auth;
        public WhatsAppSender(MyDBContext _db, AuthService _auth)
        {
            db = _db;
            auth = _auth;
        }
        public async Task<bool> sendMessage(string message)
        {
            var prijavainfo = await auth.getInfo();
            Korisnik korisnik = prijavainfo.Prijava.Korisnik;
                var accountSid = "AC63cc1500ff6b56016386debfc187a369";
                var authToken = "a552698078babdc40e90f6882dadb4b9";
                TwilioClient.Init(accountSid, authToken);
                PhoneNumber sender = new PhoneNumber($"whatsapp:+14155238886");
                PhoneNumber target = new PhoneNumber($"whatsapp:+{korisnik.BrojTelefona}");
                CreateMessageOptions options = new CreateMessageOptions(target);
                options.From = sender;
                options.Body = message;
                try
                {
                    await MessageResource.CreateAsync(options);
                    return true;
                }
                catch(Exception ex)
                {
                    return false;
                }
            }
        }
    }

