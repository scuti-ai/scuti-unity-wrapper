using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;
using Newtonsoft.Json;

namespace Scuti.Net
{
    /// <summary>
    /// The networking client for the Scuti SDK. Provides the following:
    /// - A managed <see cref="ScutiGQLClient"/> instance
    /// - A <see cref="RESTClient"/> instance
    /// - Exception types for the provided methods
    /// </summary>
    /// <remarks>
    /// <see cref="ScutiNetClient"/> provides authentication, automatic token refresh when needed and adds the required headers for requests.
    /// </remarks>
    public class ScutiNetClient
    {
        
        public string GameId { get; private set; }
        public int SeedOffers { get; private set; }
        public string RefreshToken { get; private set; }
        JWT decodedRefreshToken;
        public DateTime LastTokenReceivingTime { get; private set; }
        public bool IsInitialized { get; private set; }
        public bool FinishedOnBoarding { get; private set; }
        public bool HasRefreshableToken { get { return !string.IsNullOrEmpty(RefreshToken) && (decodedRefreshToken.ConvertExpToDateTime() - DateTime.UtcNow).TotalHours > 1; } }


        private Texture2D _currencyIcon;
        private const string PLAYERPREFS_KEY = "scutiToken";
        private static ScutiNetClient instance;
        public static ScutiNetClient Instance
        {
            get
            {
                return instance;
            }
        }

        public Action OnAuthenticated;
        public Action OnLogout;
        public Action OnInitialization;
        public bool IsAuthenticated { get { return !string.IsNullOrEmpty(GraphQL.AccessToken); } }
        
        public ScutiGQLClient GraphQL { get; private set; }
        /*
        public RESTClient REST { get; private set; }

        public GameInfo gameInfo { get; private set; }
        public OfferService _offerService { get; internal set; }
        public OfferCache _offerCache { get; internal set; }

        public OfferService Offer
        {
            get
            {
                return _offerService;
            }
        }

        public OfferCache OfferCache
        {
            get
            {
                return _offerCache;
            }
        }


        private string _restEndpoint;
        private string _checkoutEndPoint;
        private string _graphEndpoint;

        public ScutiNetClient(string restEndpoint)
        {
            if (instance != null) throw new Exception("Duplicate ScutiNetClients are being created.");
            instance = this;
            ScutiGQLClient.Init();
            _checkoutEndPoint = ScutiConstants.CheckoutURL;
            _restEndpoint = restEndpoint;
            _graphEndpoint = _restEndpoint + "/graphql";


            GraphQL = new ScutiGQLClient(_graphEndpoint);
            GraphQL.JWTExpired += Logout;

            // Initialize the REST client with the required headers as default
            RESTClient.Init();
            REST = new RESTClient
            { 
                defaultHeaders = new Dictionary<string, string>{
                    {"Accept", "application/json" },
                    {"Content-Type", "application/json" }
                }
            };

            _offerCache = new OfferCache();
            _offerService = new OfferService();
            _offerService.InjectDependencies(GraphQL);
            _offerCache.InjectDependencies(_offerService);
        }

        /// <summary>
        /// Tries to load the refresh token from the player prefs and if the expiration date has more than a day of life-time then return true
        /// otherwise it returns false which can be due to no refresh token existing (before first login or after logout) or because the token will expire soon
        /// 24 hours is too pesimistic because a user will not run their game for 24 hours even if it is WOW but i want to prevent the situation where
        /// you see a token cannot be refreshed anymore and you have to log in again
        /// </summary>
        /// <returns></returns>
        public bool TryGetAutoLoginToken()
        {
            RefreshToken = LoadTokenFromPrefs();

            if (!string.IsNullOrEmpty(RefreshToken))
            {
                decodedRefreshToken = DecodeToken(RefreshToken);
                if ((decodedRefreshToken.ConvertExpToDateTime() - DateTime.UtcNow).TotalHours > 24)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public async Task Init(ScutiSettings settings, string device)
        {
            if (IsInitialized)
                return;
            ScutiLogger.Log($"Initializing");
            Newtonsoft.Json.Utilities.AotHelper.EnsureList<string>();

            GraphQL.debugMode = REST.debugMode = (settings.LogSettings == ScutiLog.Verbose);

            //SeedOffers = Mathf.Abs(UnityEngine.Random.seed);
            SeedOffers = UnityEngine.Random.Range(0, 999999);          

            GameId = settings.developerKey;
            await GetGameInfo();

            IsInitialized = true;

            ScutiAPI.CreateSession(device);
            await ScutiAPI.InitSDKMetric();
            bool hasPrevToken = TryGetAutoLoginToken();

            if (hasPrevToken)
            {
                try
                {
                    await RefreshAuthToken();
                    await CheckFinishedOnBoarding();
                }
                catch (Exception ex)
                {
                    ScutiLogger.LogException(ex);
                    ScutiNetClient.instance.Logout();
                }
            }

            ScutiLogger.Log($"Initializing complete");
            OnInitialization?.Invoke();
        }
 

        #region AUTH

        public async Task<UserRegistrationResponse> RegisterUser(string email, string password, string fullName, string gender, string birthDate)
        {
            UserRegistrationRequest req = new UserRegistrationRequest
            {
                email = email,
                password = password,
                fullName = fullName,
                gender = gender,
                birthDate = birthDate
            };
            try
            {
                var url = $"{_restEndpoint}/user/register";
                var response = await REST.Post(req.SerializeJSON().ToUTF8Bytes(), url, ScutiAPI.GetSessionHeaders());
                return response.ToUTF8String().DeserializeJSON<UserRegistrationResponse>();
            }
            catch (Exception e)
            {
                throw new UserRegistrationException(req, e);
            }
        }


        public class AuthenticatedRefreshException : Exception
        {
            public AuthenticatedRefreshException(string message) : base(message) { }
        }

        // USER AUTHENTICATION
        public class UserAuthenticationRequest
        {
            public string email;
            public string password;

            public override string ToString()
            {
                return $"User Authentication Request-->\nEmail: {email}\nPassword: {password}";
            }
        }

        public class UserAuthenticationResponse
        {
            public string token;
            public string refreshToken;

            public override string ToString()
            {
                return $"User Authentication Response-->\nToken: {token}\nRefresh Token: {refreshToken}";
            }
        }

        public class UserAuthenticationException : Exception
        {
            public UserAuthenticationRequest Request { get; private set; }

            public UserAuthenticationException(UserAuthenticationRequest request, Exception innerException)
            : base("User Authentication Failed", innerException)
            {
                Request = request;
            }

            public override string ToString()
            {
                return $"User Authentication Exception-->\nMessage: {base.Message}\nRequest: {Request}";
            }
        }

        public async Task<UserAuthenticationResponse> AuthenticateUser(string email, string password)
        {
            UserAuthenticationRequest req = new UserAuthenticationRequest
            {
                email = email,
                password = password
            };

            try
            {

                var url = $"{_restEndpoint}/auth/login";
                var responseBytes = await REST.Post(req.SerializeJSON().ToUTF8Bytes(), url, ScutiAPI.GetSessionHeaders());
                var response = responseBytes.ToUTF8String().DeserializeJSON<UserAuthenticationResponse>();

                // Update state
                GraphQL.AccessToken = response.token;
                RefreshToken = response.refreshToken;
                decodedRefreshToken = DecodeToken(RefreshToken);
                //decodedAccessToken = DecodeToken(GraphQL.AccessToken);
                await CheckFinishedOnBoarding();
                LastTokenReceivingTime = DateTime.Now;
                SaveTokenToPlayerPrefs();
                await GetGameInfo();
                OnAuthenticated?.Invoke();
                return response;
            }
            catch (Exception e)
            {
                throw new UserAuthenticationException(req, e);
            }
        }

        /// <summary>
        /// Create a token in checkout for payment
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<CheckoutTokenResponse> CreateCheckoutToken(CheckoutTokenRequest request)
        {
            try
            {
                var url = $"{_checkoutEndPoint}/tokens";
                var responseBytes = await REST.Post(request.SerializeJSON().ToUTF8Bytes(), url, new Dictionary<string, string> { { "Authorization", "pk_test_721b5e99-b4bc-4640-9c7d-cdb6edf6bdad" } });
                var response = responseBytes.ToUTF8String().DeserializeJSON<CheckoutTokenResponse>();
                return response;
            }
            catch (Exception e)
            {
                ScutiLogger.LogException(e);
                throw e;
            }
        }

        private async Task CheckFinishedOnBoarding()
        {
            Preferences categories = await ScutiAPI.GetCategories();
            FinishedOnBoarding = categories != null && categories.Categories != null && categories.Categories.Count > 0;
        }

        private async Task<GameInfo> GetGameInfo()
        {
            // cache check
            if (gameInfo == null)
            {
                try
                {
                    gameInfo = await ScutiAPI.GetGameInfo();
                    if (gameInfo != null)
                    {
                        if (gameInfo.Currency != null)
                        {
                            DownloadCurrencyIcon(3);

                            if(!string.IsNullOrEmpty(gameInfo.Banner))
                                _offerService.InjectGameBanner(gameInfo.Banner, "SCT:ACCOUNT/WALLET");
                        }
                    }
                    else
                    {
                        Debug.LogError("Null game info");
                    }
                } catch(Exception e)
                {
                    ScutiLogger.LogException(e);
                    //Debug.LogException(e);
                }
            }
            return gameInfo;
        }


        private void DownloadCurrencyIcon(int tryCount = 0)
        {
            if (gameInfo != null && gameInfo.Currency != null && !string.IsNullOrEmpty(gameInfo.Currency.Thumbnail))
            {
                ImageDownloader downloader = ImageDownloader.New();
                downloader.Download(gameInfo.Currency.Thumbnail,
                    img =>
                    {
                        _currencyIcon = img;
                    },
                    err =>
                    {
                        ScutiLogger.LogException(err);
                        if (tryCount < 5)
                            DownloadCurrencyIcon(tryCount + 1);
                    });
            }
        }

        public Sprite CurrencyIconToSprite()
        {
            if (_currencyIcon != null)
            {
                return _currencyIcon.ToSprite();
            }
            return null;
        }

        public static async Task<int> TryToActivateRewards()
        {
            if (!ScutiNetClient.Instance.IsAuthenticated) return 0;

            Wallet wallet = null;
            wallet = await ScutiAPI.GetWallet(false);

            var rewards = await ScutiAPI.GetRewards();
            await ScutiAPI.ActivateReward(rewards.Select(x => x.Id.ToString()).ToArray());
 
                var newWallet = await ScutiAPI.GetWallet(false);

                var newTotal = newWallet.Promotional.Value + newWallet.Purchase.Value;
                var oldTotal = wallet.Promotional.Value + wallet.Purchase.Value;

                var diff = newTotal - oldTotal;
             
            return (int)diff;
                
        }

        private JWT DecodeToken(string t)
        {
            string input = t.Split('.')[1].Replace('-', '+').Replace('_', '/');
            var padLength = 4 - input.Length % 4;
            if (padLength < 4)
                input += new string('=', padLength);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<JWT>(System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(input)));
        }

        // AUTHENTICATION REFRESH
        public class AuthenticationRefreshResponse
        {
            public string token;
        }

        public class AuthenticationRefreshException : Exception
        {
            public string RefreshToken { get; private set; }

            public AuthenticationRefreshException(string message) : base(message)
            {
                RefreshToken = string.Empty;
            }

            public AuthenticationRefreshException(string message, string refreshToken, Exception e)
            : base(message, e)
            {
                RefreshToken = RefreshToken;
            }
        }

        // Auth refresh
        public Task UpdateAuthenticationIsRequired()
        {
            if (GraphQL.AccessToken == null)
                throw new AuthenticatedRefreshException("Authentication is required!");

            if (string.IsNullOrEmpty(RefreshToken))
                throw new AuthenticatedRefreshException("No Refresh Token found!");

            if ((DateTime.Now - LastTokenReceivingTime).TotalMinutes > 1)
                return RefreshAuthToken();

            return Task.CompletedTask;
        }

        public async Task<AuthenticationRefreshResponse> RefreshAuthToken(int attemptsLeft = 5)
        {
            RefreshAuthRequest req = new RefreshAuthRequest
            {
                refreshToken = RefreshToken
            };

            try
            {


                var url = $"{_restEndpoint}/auth/refresh";//?access_token={RefreshToken}";
                Dictionary<string, string> headers = ScutiAPI.GetSessionHeaders();
                headers["Authorization"] = "Bearer " + RefreshToken;

                var response = await REST.Post(req.SerializeJSON().ToUTF8Bytes(), url, headers);
                var refreshResponse = response.ToUTF8String().DeserializeJSON<AuthenticationRefreshResponse>();

                GraphQL.AccessToken = refreshResponse.token;
                //decodedAccessToken = DecodeToken(GraphQL.AccessToken);
                LastTokenReceivingTime = DateTime.Now;
                OnAuthenticated?.Invoke();
                return refreshResponse;
            }
            catch (Exception e)
            {
                if (e is GQLException)
                {
                    var gql = e as GQLException;
                    switch (gql.responseCode)
                    {
                        // unauthorized / token expired... no reason to try again
                        case 401:
                            attemptsLeft = 0;
                            break;
                        case 403:
                            attemptsLeft = 0;
                            break;
                        default:
                            Debug.Log("Failed auth with code: " + gql.responseCode);
                            break;
                    }
                }

                if (attemptsLeft > 0)
                {
                    await Task.Delay(100);
                    return await RefreshAuthToken(attemptsLeft - 1);
                }
                else
                    throw new AuthenticationRefreshException("Unable to refresh authentication token. Please login again.", RefreshToken, e);
            }
        }
        #endregion


        public async Task<bool> InSupportedCountry()
        {
            var countryCode = await GetCountryCode();
            return ScutiConstants.SUPPORTED_COUNTRY_CODES.Contains(countryCode.ToUpper());
        }
        public async Task<string> GetCountryCode()
        {
            var url = $"http://ip-api.com/json";
            var response = await REST.Get(url);
            var localizationData =  response.ToUTF8String().DeserializeJSON<LocalizationData.Data>();
            return localizationData.countryCode;
        }

        public async Task<ResetPasswordByEmailResponse> ResetPasswordByEmail(string email)
        {
            ResetPasswordByEmailRequest req = new ResetPasswordByEmailRequest
            {
                email = email
            };

            try
            {
                var url = $"{_restEndpoint}/user/password/reset";
                var response = await REST.Post(req.SerializeJSON().ToUTF8Bytes(), url, ScutiAPI.GetSessionHeaders());
                return response.ToUTF8String().DeserializeJSON<ResetPasswordByEmailResponse>();
            }
            catch (Exception e)
            {
                throw new ResetPasswordByEmailException("Request to reset password by email failed.", e);
            }
        }

        public async Task<TwoFactorSMSCodeResponse> RequestTwoFactorSMSCode()
        {
            TwoFactorSMSCodeRequest req = new TwoFactorSMSCodeRequest();

            try
            {
                await UpdateAuthenticationIsRequired();
                var url = $"{_restEndpoint}/auth/2fa/code";

                var response = await REST.Post(req.SerializeJSON().ToUTF8Bytes(), url, GetAccessTokenHeader());
                return response.ToUTF8String().DeserializeJSON<TwoFactorSMSCodeResponse>();
            }
            catch (Exception e)
            {
                throw new TwoFactorSMSCodeException(req, "Two Factor password reset failed.", e);
            }
        }

        private Dictionary<string, string> GetAccessTokenHeader()
        {
            Dictionary<string, string> header = new Dictionary<string, string>();
            header["Authorization"] = "Bearer " + GraphQL.AccessToken;
            return header;
        }
 
        public void FinishedOnBoardingProccess()
        {
            FinishedOnBoarding = true;
        }

        public async Task<TwoFactorSMSCodeVerificationResponse> VerifyTwoFactorSMSCode(string code)
        {
            try
            {
                await UpdateAuthenticationIsRequired();
                TwoFactorSMSCodeVerificationRequest req = new TwoFactorSMSCodeVerificationRequest
                {
                    code = code
                };
                var url = $"{_restEndpoint}/user/password/2fa";
                var response = await REST.Post(req.SerializeJSON().ToUTF8Bytes(), url,GetAccessTokenHeader());
                return response.ToUTF8String().DeserializeJSON<TwoFactorSMSCodeVerificationResponse>();
            }
            catch (Exception e)
            {
                throw new TwoFactorSMSCodeVerificationException("Two Factor SMS Code validation failed", e);
            }
        }

        public async Task<PasswordChangeResponse> ChangePasswordWithTwoFactorToken(string password, string token)
        {
            PasswordChangeRequestFor2FA req = new PasswordChangeRequestFor2FA
            {
                password = password
            };
            try
            {
                await UpdateAuthenticationIsRequired();
                var url = $"{_restEndpoint}/user/password?access_token={token}";
                var response = await REST.Post(req.SerializeJSON().ToUTF8Bytes(), url, ScutiAPI.GetSessionHeaders());
                return response.ToUTF8String().DeserializeJSON<PasswordChangeResponse>();
            }
            catch (Exception e)
            {
                throw new PasswordChangeException(req, "Password Update request failed.", e);
            }
        }

        public async Task<PasswordChangeResponse> ChangePassword(string oldPassword, string password)
        {
            PasswordChangeRequest req = new PasswordChangeRequest
            {
                password = password,
                oldPassword = oldPassword
            };
            try
            {
                await UpdateAuthenticationIsRequired();
                var url = $"{_restEndpoint}/user/change-password";
                var response = await REST.Post(req.SerializeJSON().ToUTF8Bytes(), url, GetAccessTokenHeader());
                return response.ToUTF8String().DeserializeJSON<PasswordChangeResponse>();
            }
            catch (Exception e)
            {
                ScutiLogger.LogException(e);
                throw e;
            }
        }

        public async Task<PhoneRegistrationResponse> RegisterPhone(string phoneNumber, int countryCode)
        {
            PhoneRegistrationRequest req = new PhoneRegistrationRequest
            {
                phone = phoneNumber,
                countryCode = countryCode
            };

            try
            {
                await UpdateAuthenticationIsRequired();
                var url = $"{_restEndpoint}/user/phone/register";
                var response = await REST.Post(req.SerializeJSON().ToUTF8Bytes(), url, GetAccessTokenHeader());
                return response.ToUTF8String().DeserializeJSON<PhoneRegistrationResponse>();
            }
            catch (Exception e)
            {
                throw new PhoneRegistrationException(req, "Phone Registration request failed", e);
            }
        }

        public async Task<PhoneVerificationResponse> VerifyPhone(string code)
        {
            PhoneVerificationRequest req = new PhoneVerificationRequest
            {
                code = code
            };

            try
            {
                await UpdateAuthenticationIsRequired();
                var url = $"{_restEndpoint}/user/phone/verify";
                var response = await REST.Post(req.SerializeJSON().ToUTF8Bytes(), url, GetAccessTokenHeader());
                return response.ToUTF8String().DeserializeJSON<PhoneVerificationResponse>();
            }
            catch (Exception e)
            {
                throw new PhoneVerificationException(req, "Phone registration code verification failed", e);
            }
        }

        public async Task<byte[]> DeleteAccount()
        {
            try
            {
                await UpdateAuthenticationIsRequired();       
                var url = $"{_restEndpoint}/user/me";
                var response = await REST.Delete(url, GetAccessTokenHeader());
                return response;
            }
            catch (Exception e)
            {
                ScutiLogger.LogException(e);
                throw e;
            }
        }


        public async Task<GetAccountInfoResponse> GetAccountInfo()
        {
            try
            {
                await UpdateAuthenticationIsRequired();
                var url = $"{_restEndpoint}/user/me";
                var responseBytes = await REST.Get(url, GetAccessTokenHeader());
                return responseBytes.ToUTF8String().DeserializeJSON<GetAccountInfoResponse>();
            }
            catch (Exception e)
            {
                ScutiLogger.LogException(e);
                throw e;
            }
        }
        
        public async Task<string> RegisterEvent(MetricType metricType, Dictionary<string, string> sessionHeaders, string gameId = "", string sessionId = "", string offerId = null, int? duration = null, Dictionary<string, object> customData = null)
        {
            RegisterEventBody req = new RegisterEventBody
            {
                gameId = gameId,
                offerId = offerId,
                sessionId = sessionId,
                metricType = metricType,
                duration = duration,
                customData = customData
            };
            try
            {
                //await UpdateAuthenticationIsRequired();
                var url = $"{_restEndpoint}/event/register";
                var response = await REST.Post(req.SerializeJSON().ToUTF8Bytes(), url, sessionHeaders);
                return response.ToUTF8String();
            }
            catch (Exception e)
            {
                ScutiLogger.LogException(e);
                throw e;
            }
        }



        [ContextMenu("Logout")]
        public void Logout()
        {
            GraphQL.AccessToken = null;
            RefreshToken = null;
            PlayerPrefs.DeleteKey(PLAYERPREFS_KEY);
            ScutiAPI.RefreshWallet(0,0);
            OnLogout?.Invoke();
        }

        private string LoadTokenFromPrefs()
        {
            if (PlayerPrefs.HasKey(PLAYERPREFS_KEY))
                return PlayerPrefs.GetString(PLAYERPREFS_KEY);
            return null;
        }

        private void SaveTokenToPlayerPrefs()
        {
            PlayerPrefs.SetString(PLAYERPREFS_KEY, RefreshToken);
        }
        */
    }
}
public class JWT
{
    public string id;
    public string email;
    public string[] roles;
    public int exp;
    public int iat;
    public DateTime ConvertExpToDateTime()
    {
        return UnixTimeStampToDateTime(exp);
    }

    private static DateTime UnixTimeStampToDateTime(int unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
        dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
        return dtDateTime;
    }
}

//public class CheckoutTokenRequest
//{
//    /// <summary>
//    /// It should be card, apple_pay or google_pay
//    /// </summary>
//    public string type;
//    /// <summary>
//    /// The card number
//    /// </summary>
//    public string number;
//    public int expiry_month;
//    public int expiry_year;
//}

//public class CheckoutTokenResponse
//{
//    public string type;

//    /// <summary>
//    /// We need this in our server to use the source for payment
//    /// </summary>
//    public string token;
//    public string expires_on;
//    public int expiry_month;
//    public int expiry_year;
//    public string scheme;
    
//    /// <summary>
//    /// This in our server will help us to show the user a relevant UI for chosing his/her card
//    /// </summary>
//    public string last4;
//    public string bin;
//    public string card_type;
//    public string card_category;
//    public string issuer;
//    public string issuer_country;
//    public string product_id;
//    public string product_type;
//}

//public class RegisterEventBody
//{
//    public string gameId;
//    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
//    public string offerId;
//    public string sessionId;
//    public MetricType metricType;
//    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
//    public int? duration;
//    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
//    public Dictionary<string, object> customData;
//}