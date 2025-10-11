namespace Base.Abstractions.REST
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
