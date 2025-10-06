using BestApp.Abstraction.General.Infasructures.Exceptions;
using BestApp.Abstraction.General.Infasructures.REST;
using BestApp.Abstraction.General.Platform;
using Common.Abstrtactions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestApp.Impl.Cross.Infasructures.REST
{
    internal class AuthTokenService : IAuthTokenService
    {
        private readonly Lazy<IPreferencesService> preferencesService;
        private readonly Lazy<ILoggingService> loggingService;
        private const string TAG = $"{nameof(AuthTokenService)}: ";
        public AuthTokenService(Lazy<IPreferencesService> preferencesService, Lazy<ILoggingService> loggingService)
        {
            this.preferencesService = preferencesService;
            this.loggingService = loggingService;
        }

        private AuthTokenDetails authToken;
        /// <summary>
        /// Make sure that token is not expired otherwise throws AuthExpiredException
        /// </summary>
        /// <returns></returns>
        /// <exception cref="AuthExpiredException"></exception>
        public async Task EnsureAuthValid()
        {
            if (authToken == null)
            {
                authToken = await GetAuthTokenDetails();
            }
            
            if (authToken == null)
            {
                loggingService.Value.LogWarning($"{TAG}Skip checking access token because authToken is null");
                return;
            }

            var expireDate = authToken.Expired;
            if (expireDate.AddDays(-2) < DateTime.Now)
            {
                loggingService.Value.LogWarning($"{TAG}Access token is expired(expiredDate - 2days) {expireDate.AddDays(-2)} < {DateTime.Now}, actual expired date: {expireDate}");
                throw new AuthExpiredException();
            }
        }

        public async Task<string> GetToken()
        {
            var authToken = await GetAuthTokenDetails();
            if (authToken != null)
            {
                return authToken.Token;
            }
            else
            {
                return string.Empty;
            }
        }


        public Task<AuthTokenDetails> GetAuthTokenDetails()
        {
            return Task.Run<AuthTokenDetails>(() =>
            {
                var authTokenJson = preferencesService.Value.Get("user_at", string.Empty);
                if (!string.IsNullOrEmpty(authTokenJson))
                {
                    var authToken = JsonConvert.DeserializeObject<AuthTokenDetails>(authTokenJson);
                    return authToken;
                }
                else
                {
                    return null;
                }
            });            
        }       

        public Task SaveAuthTokenDetails(AuthTokenDetails authToken)
        {
            var json = JsonConvert.SerializeObject(authToken);
            preferencesService.Value.Set("user_at", json);

            return Task.CompletedTask;
        }
    }
}
