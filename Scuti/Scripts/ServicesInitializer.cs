using System;
using System.Threading.Tasks;

using UnityEngine;


using Scuti.Net;
using System.Collections.Generic;

namespace Scuti {
    public class ServicesInitializer : MonoBehaviour {

        public enum ServerEnvironment
        {
            Development = 0,
            Staging = 1,
            Production = 2
        }

        [Serializable]
        public struct EnvironmentData
        {
            public ServerEnvironment Environment;
            public string URL;
        }


        public static event Action OnBootupComplete;

        public List<EnvironmentData> Environments;
        public ServerEnvironment TargetEnvironment;


        private string _restEndpoint;

        public async Task BootUp() {
                Dispatcher.Init();


                foreach (var evn in Environments)
                {
                    if (evn.Environment == TargetEnvironment)
                    {
                        _restEndpoint = evn.URL;
                        break;
                    }
                }

                InitializeServices();

                await Task.Delay(100);
                OnBootupComplete?.Invoke();
        }

        void InitializeServices() {
            new ScutiNetClient(_restEndpoint);
            new SentryAPI().Init();
        }
    }
}
