namespace SmartHomeApi.Helpers
{
    public class TokenGenerator
    {
        public static string generisiToken ()
        {
            var znakovi = "1234567890qwertzuiopasdfghjklyxcvbnm!#$%&/()=?+*";
            var random = new Random();
            var token = "";
            for(int i =0;i<6;i++)
            {
                token += znakovi[random.Next(0,znakovi.Length)];
            }
            return token;
        }
    }
}
