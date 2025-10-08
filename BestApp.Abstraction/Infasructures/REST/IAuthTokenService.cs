using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestApp.Abstraction.Main.Infasructures.REST
{
    public interface IAuthTokenService
    {
        Task<string> GetToken();
        Task EnsureAuthValid();
        Task SaveAuthTokenDetails(AuthTokenDetails authToken);
        Task<AuthTokenDetails> GetAuthTokenDetails();
    }

    public class AuthTokenDetails
    {
        public string Token { get; set; }
        public DateTime Expired { get; set; }
        public string RefreshToken { get; set; }
    }


}
