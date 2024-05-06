using Microsoft.EntityFrameworkCore;
using SmartHomeApi.Data;
using SmartHomeApi.Data.Tablice;
using System.Linq.Expressions;

namespace SmartHomeApi.Helpers
{
    public class AuthService
    {
        private readonly MyDBContext db;
        private readonly IHttpContextAccessor context;
        public AuthService(MyDBContext _db,IHttpContextAccessor _httpcontext)
        {
            db = _db;
            context = _httpcontext;
        }
        public async Task<PrijavaInfo> getInfo()
        {
            string token = context.HttpContext.Request.Headers["my-token"];
            if(token == null) return new PrijavaInfo { jeLogiran = false, Prijava = null };
            var prijava = await  db.Prijave.Include(p => p.Korisnik).FirstOrDefaultAsync(p => p.Token == token);
            if (prijava == null) return new PrijavaInfo { jeLogiran = false, Prijava = null };
            return new PrijavaInfo { jeLogiran = true, Prijava = prijava };

        }
        
    }
    public class PrijavaInfo
    {
        public bool jeLogiran { get; set; }
        public Prijava Prijava { get; set; }
    }
}
