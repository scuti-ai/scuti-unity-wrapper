using System;
using UnityEngine;

namespace Scuti
{
    public class ScutiSDK : Singleton<ScutiSDK>
    {

        public ScutiSettings settings { get; private set; }
        /*
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void CreateScutiGameObject()
        {
            var go = GameObject.Instantiate(Resources.Load<GameObject>(ScutiConstants.MAIN_PREFAB_NAME));
            go.name = go.name.Substring(0, go.name.Length - 7);
            //go.hideFlags = HideFlags.HideInHierarchy;
        }


        public Action<int> OnCurrencyExchanged;
        public Action OnExitScuti;

        private int _SessionTimeout = 600;
        private DateTime PausedAppTime;

        public string developerKey
        {
            get
            {
                return settings.developerKey;
            }
        }

        float browseTime;
        bool _recording = false;
        private GameObject UIContainer;
        public ServicesInitializer servicesInitializer;


        private bool _cachedPortraitMode;
        private bool _cachedPortraitUpsideDown;
        private ScreenOrientation _cachedOrientation;
        private string _device;


        protected void Awake()
        {
            PausedAppTime = DateTime.MinValue;
            _device = UnityEngine.SystemInfo.deviceModel;
        }

        async void Start()
        {
            settings = Resources.Load<ScutiSettings>($"{ScutiConstants.SCUTI_SETTINGS_PATH_FILE}{ScutiConstants.SCUTI_SETTINGS_FILE_WITHOUTEXTENSION}");
            if (settings == null)
            {
                ScutiLogger.LogError("You should initialize your scuti before being able to use it");
                return;
            }
            if (string.IsNullOrEmpty(settings.developerKey))
            {
                ScutiLogger.LogError("Set your developer ID in scuti settings using the Scuti>Settings menu in the editor before trying to use scuti");
                return;
            }

            try
            {
                await servicesInitializer.BootUp();
            } catch(Exception e)
            {
                Debug.LogException(e);
            }

            if (!ScutiNetClient.Instance.IsInitialized)
                await ScutiNetClient.Instance.Init(settings, _device);
            DontDestroyOnLoad(this.gameObject);
        }

        public void LoadUI()
        {
            if (UIContainer == null)
            {
                ScutiNetClient.Instance.OfferCache.CheckForWipe();
                ScutiNetClient.Instance.OfferCache.LazyCachingEnabled(true);
                _cachedOrientation = Screen.orientation;
                _cachedPortraitMode = Screen.autorotateToPortrait;
                _cachedPortraitUpsideDown = Screen.autorotateToPortraitUpsideDown;
                Screen.orientation = _cachedOrientation;
                
                //if (ScutiConstants.FORCE_LANDSCAPE)
                //{
                //    Screen.autorotateToPortrait = false;
                //    Screen.autorotateToPortraitUpsideDown = false;
                //    Screen.orientation = ScreenOrientation.Landscape;
                //}
                UIContainer = Instantiate(Resources.Load<GameObject>(ScutiConstants.UI_PREFAB_NAME));
                StartBrowsing();
            }
        }

         

        public void UnloadUI()
        {
            if (UIContainer != null)
            {
                ScutiNetClient.Instance.OfferCache.LazyCachingEnabled(false);
                // Reset prior orientations
                Screen.autorotateToPortrait = _cachedPortraitMode;
                Screen.autorotateToPortraitUpsideDown = _cachedPortraitUpsideDown;
                //Screen.orientation = _cachedOrientation;
                switch (settings.Orientation)
                {
                    case ScutiSettings.AppOrientation.Landscape:
                        Screen.orientation = _cachedOrientation == ScreenOrientation.Landscape || _cachedOrientation == ScreenOrientation.LandscapeRight ? _cachedOrientation : ScreenOrientation.Landscape;
                        break;
                    case ScutiSettings.AppOrientation.Portrait:
                        Screen.orientation = _cachedOrientation == ScreenOrientation.Portrait || _cachedOrientation == ScreenOrientation.PortraitUpsideDown ? _cachedOrientation : ScreenOrientation.Portrait;
                        break;
                    case ScutiSettings.AppOrientation.Autorotation:
                        Screen.orientation = ScreenOrientation.AutoRotation;
                        break;
                    default:
                        Screen.orientation = _cachedOrientation;
                        break;
                }
                Destroy(UIContainer);
                StopBrowsing();
                UIContainer = null;

                Resources.UnloadUnusedAssets();

                OnExitScuti?.Invoke();
            }
        }

        public void GrantCurrency(int reward)
        {
            // Sanity check to ensure the store is active
            if (UIContainer != null)
            {
                OnCurrencyExchanged?.Invoke(reward);
                if (OnCurrencyExchanged == null)
                {
                    ScutiLogger.LogError("Nothing is listening for a currency exchange and the ScutiSDK settings are set to client based exchange integration. Please either change the settings to be server based or listen to ScutiSDK.Instance.OnCurrencyExchanged to know when to grant your user currency.");
                }
            }
        }

        // Helpers
        private void ResumeApplication()
        {
            if (ScutiNetClient.Instance != null && ScutiNetClient.Instance.IsInitialized)
            {
                var diff = DateTime.Now - PausedAppTime;
                if (diff.TotalSeconds > _SessionTimeout)
                {
                    ScutiAPI.CreateSession(_device);
                    ScutiAPI.InitSDKMetric();
                    PausedAppTime = new DateTime();
                }

                if (UIContainer != null)
                {
                    StartBrowsing();
                }
            }
        }

        private void LeaveApplication()
        {
            if (UIContainer != null)
            {
                StopBrowsing();
            }
            PausedAppTime = DateTime.Now;
        }



        private void StartBrowsing()
        {
            if (!_recording)
            {
                _recording = true;
                browseTime = Time.time;
            }
        }

        private void StopBrowsing()
        {
            if(_recording)
            {
                _recording = false;
                var diff = Time.time - browseTime; 
                ScutiAPI.TimeSpentPerusingMetric((int)diff); 
            }
        }

        // Handlers
        private void OnApplicationQuit()
        {
            LeaveApplication();
        }

        void OnApplicationFocus ( bool focus )
        {
            if ( focus )    ResumeApplication ();
            else            LeaveApplication ();
        }

        void OnApplicationPause(bool pause)
        {
            if (pause) LeaveApplication();
            else ResumeApplication();
        }
    */
    }
}