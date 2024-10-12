using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Application.Models.ApplicationModels
{
    public static class AuthOptions
    {
        public static readonly string Issuer = "AdvertisementServiceIssuer";
        public static readonly string Client = "AdvertisementServiceClient";
        private static readonly string Key = "Pj6pstoijkGh45pdTmpoiJkp964oMmhkt";
        public static readonly int Lifetime = 10;
        public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
    }
}
