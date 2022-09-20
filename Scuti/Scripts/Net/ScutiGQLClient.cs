using Scuti.GraphQL;
using System.Collections.Generic;

namespace Scuti.Net
{
    /// <summary>
    /// A Scuti specific GQL client that provides default HTTP request headers
    /// as well as handles access token and its HTTP request header entry
    /// </summary>
    public class ScutiGQLClient : GQLClient
    {
        new public static void Init()
        {
            GQLClient.Init();
        }

        static ScutiGQLClient()
        {
            GQLClient.Init();
        }

        public ScutiGQLClient(string defaultUrl) : base(defaultUrl)
        {
            defaultHeaders = new Dictionary<string, string>() {
                {"Accept", "application/json" },
                {"Content-Type", "application/json" }
            };
        }

        string accessToken;
        public string AccessToken
        {
            set
            {
                accessToken = value;
                CustomizeRequest = request =>
                {

                    if (!accessToken.IsNullOrEmpty())
                        request.SetRequestHeader("Authorization", $"Bearer {accessToken}");
                };
            }
            get { return accessToken; }
        }
    }
}