/*
 * Copyright (C) 2012 GREE, Inc.
 * 
 * This software is provided 'as-is', without any express or implied
 * warranty.  In no event will the authors be held liable for any damages
 * arising from the use of this software.
 * 
 * Permission is granted to anyone to use this software for any purpose,
 * including commercial applications, and to alter it and redistribute it
 * freely, subject to the following restrictions:
 * 
 * 1. The origin of this software must not be misrepresented; you must not
 *    claim that you wrote the original software. If you use this software
 *    in a product, an acknowledgment in the product documentation would be
 *    appreciated but is not required.
 * 2. Altered source versions must be plainly marked as such, and must not be
 *    misrepresented as being the original software.
 * 3. This notice may not be removed or altered from any source distribution.
 */

using Newtonsoft.Json.Linq;
using Scuti;
using Scuti.Net;
using System;
using System.Collections;
using System.Text;
using UnityEngine;
#if UNITY_2018_4_OR_NEWER
using UnityEngine.Networking;
#endif
using UnityEngine.UI;

public class ScutiWebView : MonoBehaviour
{
    public string Url;
    WebViewObject webViewObject;
    ScreenOrientation currentOrientation;

    public bool withSafeArea;

    IEnumerator Start()
    {
        currentOrientation = Screen.orientation;

        webViewObject = (new GameObject("WebViewObject")).AddComponent<WebViewObject>();
        webViewObject.Init(
//#if !UNITY_EDITOR
//// 
//#if UNITY_IOS
//            // iOS WKContentMode (0: recommended, 1: mobile, 2: desktop)
//            wkContentMode: 1,
//#elif UNITY_ANDROID
//            // Samsung Galaxy S8 user-agent
//            ua: "Mozilla/5.0 (Linux; Android 9; SM-G950N) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/101.0.4951.61 Mobile Safari/537.36",
//#endif

//#endif
            cb: (msg) =>
            {
                if (msg.ToLower().Equals("exit"))
                {
                    ScutiSDK.Instance.UnloadUI();
                    Destroy(webViewObject.gameObject);
                }
                else if (msg.ToLower().StartsWith("exchange"))
                {
                    Debug.LogError("Exchange::: => " + msg);
                    var messageSplit = msg.Split('!');
                    if(messageSplit.Length>1)
                    {
                        var payload = messageSplit[1];
                        Debug.Log("Payload: " + payload);
                        var jPayload = JObject.Parse(payload);
                    }
                }
                else { 
                    ScutiLogger.Log(string.Format("CallFromJS[{0}]", msg));
                }
            },
            err: (msg) =>
            {
                ScutiLogger.Log(string.Format("CallOnError[{0}]", msg));
            },
            httpErr: (msg) =>
            {
                ScutiLogger.Log(string.Format("CallOnHttpError[{0}]", msg));
            },
            started: (msg) =>
            {
                ScutiLogger.Log(string.Format("CallOnStarted[{0}]", msg));
            },
            hooked: (msg) =>
            {
                ScutiLogger.Log(string.Format("CallOnHooked[{0}]", msg));
            },
            ld: (msg) =>
            {
                ScutiLogger.Log(string.Format("CallOnLoaded[{0}]", msg));
#if UNITY_EDITOR_OSX || (!UNITY_ANDROID && !UNITY_WEBPLAYER && !UNITY_WEBGL)
                // NOTE: depending on the situation, you might prefer
                // the 'iframe' approach.
                // cf. https://github.com/gree/unity-webview/issues/189
#if true
                webViewObject.EvaluateJS(@"
                  if (window && window.webkit && window.webkit.messageHandlers && window.webkit.messageHandlers.unityControl) {
                    window.Unity = {
                      call: function(msg) {
                        window.webkit.messageHandlers.unityControl.postMessage(msg);
                      }
                    }
                  } else {
                    window.Unity = {
                      call: function(msg) {
                        window.location = 'unity:' + msg;
                      }
                    }
                  }
                ");
#else
                webViewObject.EvaluateJS(@"
                  if (window && window.webkit && window.webkit.messageHandlers && window.webkit.messageHandlers.unityControl) {
                    window.Unity = {
                      call: function(msg) {
                        window.webkit.messageHandlers.unityControl.postMessage(msg);
                      }
                    }
                  } else {
                    window.Unity = {
                      call: function(msg) {
                        var iframe = document.createElement('IFRAME');
                        iframe.setAttribute('src', 'unity:' + msg);
                        document.documentElement.appendChild(iframe);
                        iframe.parentNode.removeChild(iframe);
                        iframe = null;
                      }
                    }
                  }
                ");
#endif
#elif UNITY_WEBPLAYER || UNITY_WEBGL
                webViewObject.EvaluateJS(
                    "window.Unity = {" +
                    "   call:function(msg) {" +
                    "       parent.unityWebView.sendMessage('WebViewObject', msg)" +
                    "   }" +
                    "};");
#endif
                webViewObject.EvaluateJS(@"Unity.call('ua=' + navigator.userAgent)");
            }
            //transparent: false,
            //zoom: true,
            //ua: "custom user agent string",
            //// android
            //androidForceDarkMode: 0,  // 0: follow system setting, 1: force dark off, 2: force dark on
            //// ios
            //enableWKWebView: true,
            //wkContentMode: 0,  // 0: recommended, 1: mobile, 2: desktop
            //wkAllowsLinkPreview: true,
            //// editor
            //separated: false
            );
#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
        webViewObject.bitmapRefreshCycle = 1;
#endif
        // cf. https://github.com/gree/unity-webview/pull/512
        // Added alertDialogEnabled flag to enable/disable alert/confirm/prompt dialogs. by KojiNakamaru · Pull Request #512 · gree/unity-webview
        //webViewObject.SetAlertDialogEnabled(false);

        // cf. https://github.com/gree/unity-webview/pull/728
        //webViewObject.SetCameraAccess(true);
        //webViewObject.SetMicrophoneAccess(true);

        // cf. https://github.com/gree/unity-webview/pull/550
        // introduced SetURLPattern(..., hookPattern). by KojiNakamaru · Pull Request #550 · gree/unity-webview
        //webViewObject.SetURLPattern("", "^https://.*youtube.com", "^https://.*google.com");

        // cf. https://github.com/gree/unity-webview/pull/570
        // Add BASIC authentication feature (Android and iOS with WKWebView only) by takeh1k0 · Pull Request #570 · gree/unity-webview
        //webViewObject.SetBasicAuthInfo("id", "password");

        //webViewObject.SetScrollbarsVisibility(true);

        UpdateArea();

        if (Url.StartsWith("http"))
        {
            webViewObject.LoadURL(Url.Replace(" ", "%20"));
        }
        else
        {
            Debug.LogError("Resoltuion : " + Screen.width + " vs " + Screen.height);
            var dst = System.IO.Path.Combine(Application.persistentDataPath, Url);
            var scriptUrl = GetURL();
            string htmlContent = "<html><head><script src=\"XURLX\"></script>\n</head>\n<body style=\"margin: -10; overflow: hidden; padding: 0;\">\n    <div id=\"scuti-store\"></div>\n     <script>\n    (async function () {\n      await window.SCUTI_SDK.initialize(\"XAPPIDX\")\n      window.SCUTI_SDK.renderStore(\n        \"scuti-store\",\n        () => Unity.call(\'exit\'),\n        (payload) => Unity.call(\'exchange!\'+JSON.stringify(payload)),\n        { width: \'100%\', height: \'100%\' }\n      )\n    })()\n  </script>\n</body>\n</html>";
            htmlContent = htmlContent.Replace("XURLX", scriptUrl);
            htmlContent = htmlContent.Replace("XAPPIDX", ScutiNetClient.Instance.GameId);
            Debug.Log("ScutiNetClient : " + ScutiNetClient.Instance);
            byte[] result = Encoding.ASCII.GetBytes(htmlContent);
            System.IO.File.WriteAllBytes(dst, result);
            //webViewObject.LoadHTML
            webViewObject.LoadURL("file://" + dst.Replace(" ", "%20"));
            //webViewObject.
        }
        yield break;
    }

    private string GetURL()
    {
        return "https://staging.scuti-sdk.js.run.app.scuti.store/browser.js";
    }

    private void Update()
    {        
        if(currentOrientation != Screen.orientation)
        {
            currentOrientation = Screen.orientation;
            webViewObject.Resume();
            UpdateArea();
        }
    }

    private void UpdateArea()
    {

        if (withSafeArea)
            if (Screen.orientation == ScreenOrientation.LandscapeLeft || Screen.orientation == ScreenOrientation.LandscapeRight || Screen.orientation == ScreenOrientation.Landscape)
            {
                webViewObject.SetMargins((int)Screen.safeArea.xMin, 0, 0, 0, false);
            }
            else
            {
                // Portrait
                webViewObject.SetMargins(0, (int)Screen.safeArea.yMin, 0, 0, false);
            }
        else
            webViewObject.SetMargins(0, 0, 0, 0, false);
      
        webViewObject.SetTextZoom(100);  // android only. cf. https://stackoverflow.com/questions/21647641/android-webview-set-font-size-system-default/47017410#47017410
        webViewObject.SetVisibility(true);

    }

}
