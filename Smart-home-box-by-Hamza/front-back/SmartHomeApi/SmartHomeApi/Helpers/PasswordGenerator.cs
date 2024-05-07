namespace SmartHomeApi.Helpers
{
    public class PasswordGenerator
    {
        public static string GenerisiLozinku()
        {
            var znakovi = "qwertzuiopasdfghjklyxcvbnm1234567890QWERTZUIOPASDFGHJKLYXCVBNM";
            var lozinka = "";
            var random = new Random();
            for(int i =0;i<8; i++)
            {
                lozinka+= znakovi[random.Next(0,znakovi.Length)];
            }
            return lozinka;
        }
    }
}
