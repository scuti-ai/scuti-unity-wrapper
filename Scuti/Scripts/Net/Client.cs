using System;
using System.Text;
using System.Collections;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

using Headers = System.Collections.Generic.Dictionary<string, string>;
using Newtonsoft.Json.Linq;
using Scuti.Net;

namespace Scuti.GraphQL {
    /// <summary>
    /// Send the Query method to the url using a POST request
    /// </summary>
    public class GQLClient {
        static GQLExecutor executor;

        public bool debugMode;
        public string defaultURL;
        public Headers defaultHeaders = new Headers();

        public delegate void RequestCustomizationHandler(UnityWebRequest request);
        public RequestCustomizationHandler CustomizeRequest;

        public Action JWTExpired;

        public static void Init() {
            if (executor == null) {
                executor = new GameObject("GQLExecutor").AddComponent<GQLExecutor>();
                executor.hideFlags = HideFlags.HideAndDontSave;
                MonoBehaviour.DontDestroyOnLoad(executor.gameObject);
            }
        }

        public GQLClient(string defaultUrl){
            this.defaultURL = defaultUrl;
        }
        
        void Log(object message) {
            if (debugMode)
                ScutiLogger.Log(message);
        }

        public Task<GQLResponse> Send(GQLQuery query, Headers headers, bool ignoreDefaultHeaders = false) {
            var source = new TaskCompletionSource<GQLResponse>();
            Send(query,
                result => source.SetResult(result),
                error => source.SetException(error),
                headers,
                ignoreDefaultHeaders
            );
            return source.Task;
        }
        
        public void Send(GQLQuery query, Action<GQLResponse> onSuccess, Action<Exception> onFailure,  Headers headers = null, bool ignoreDefaultHeaders = false) {
            var queryString = JsonUtility.ToJson(query);
            var bytes = Encoding.UTF8.GetBytes(queryString);
            var request = CreateRequest(bytes,  headers, ignoreDefaultHeaders);
            CustomizeRequest?.Invoke(request);
            //Log(headers.ToJson());
            executor.StartCoroutine(SendRequest(request, onSuccess, onFailure));
        }

        UnityWebRequest CreateRequest(byte[] body,   Headers headers = null, bool ignoreDefaultHeaders = false) {
            var request = new UnityWebRequest {
                url = defaultURL,// string.IsNullOrEmpty(customUrl) ? defaultURL : customUrl,
                method = "POST",
                downloadHandler = new DownloadHandlerBuffer(),
                uploadHandler = new UploadHandlerRaw(body)
            };

            if (headers != null)
                foreach (var header in headers)
                    request.SetRequestHeader(header.Key, header.Value);

            if (!ignoreDefaultHeaders && defaultHeaders != null)
                foreach (var header in defaultHeaders)
                    request.SetRequestHeader(header.Key, header.Value);

            return request;
        }

        IEnumerator SendRequest(UnityWebRequest request, Action<GQLResponse> onSuccess, Action<GQLException> onFailure) {
            var body = Encoding.UTF8.GetString(request.uploadHandler.data);
            var url = request.url;


            DateTime timer = DateTime.Now;
            var debugOut =  body.Replace("\\r\\n", string.Empty) ;
            debugOut = debugOut.Replace("\\\"", "\"") ;
            Log($"Sending to {url} --> {debugOut} ");
 

            yield return request.SendWebRequest();

            var text = request.downloadHandler.text;
            var error = request.error;
            var code = request.responseCode;


            var span = DateTime.Now - timer;
            Log($"Received --> {code} {error} {text} time: {span.TotalSeconds}");


            if (request.isNetworkError || request.isHttpError) {

                if (request.responseCode == 401 && text.Contains("jwt expired"))
                {
                    JWTExpired?.Invoke();
                }

                onFailure?.Invoke(new GQLException(request.responseCode, request.error, text));
                yield break;
            }

            var response = new GQLResponse(text);

            JToken errors;
            if(response.TryGetValue("errors", out errors))
            {
                var statusCode = -1;
                var fullError = errors.ToString();
                var errorText = "Unknown Error";

                try
                {
                    var firstError = errors[0];
                    fullError = firstError.ToString();

                    var message = firstError["message"];
                    if(message is JValue)
                    {
                        errorText = message.ToString();
                        if(errorText!=null && errorText.Equals("jwt expired"))
                        {
                            statusCode = 401;
                            var ext = firstError["extensions"];
                            if(ext!=null)
                                fullError = ext.ToString();
                        }
                    }
                    else if (message != null)
                    {
                        statusCode = message["statusCode"].Value<int>();
                        errorText = message["error"].Value<string>();
                        fullError = message["message"][0].ToString();
                    }
                }  catch (Exception e)
                {
                    Log("Unable to parse error. Full Text" + text );
                    Log(e);
                }

                if (statusCode == 401 && errorText.Contains("jwt expired"))
                {
                    JWTExpired?.Invoke();
                }

                ScutiLogger.LogError("Failed: " + statusCode + " " + errorText + " and full: " + fullError  +" while trying to load: "+url +" >>> "+ debugOut);
                onFailure?.Invoke(new GQLException(statusCode, errorText, fullError));
                yield break;
            }
            onSuccess?.Invoke(response);
        }
    }
}
