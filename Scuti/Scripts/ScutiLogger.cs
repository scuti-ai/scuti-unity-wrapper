using Scuti.Net;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Scuti
{
    /// <summary>
    /// This class is a wrapper which all logs should go through
    /// </summary>
    public class ScutiLogger : Singleton<ScutiLogger>
    {
        private ScutiSettings _settings;
        public ScutiSettings settings
        {
            get
            {
                if (_settings == null)
                    _settings = ScutiSDK.Instance.settings;
                return _settings;
            }
        }

        private void _Log(string message)
        {
            if (settings.LogSettings >= ScutiLog.Verbose)
                Debug.Log(message);
        }

        public static void Log(string message)
        {
            Instance?._Log(message);
        }

        private void _Log(string message, UnityEngine.Object context)
        {
            if (settings.LogSettings >= ScutiLog.Verbose)
                Debug.Log(message, context);
        }

        public static void Log(string message, UnityEngine.Object context)
        {
            Instance?._Log(message, context);
        }

        private void _Log(object message)
        {
            if (settings?.LogSettings >= ScutiLog.Verbose)
                Debug.Log(message);
        }

        public static void Log(object message)
        {
            Instance?._Log(message);
        }

        private void _LogWarning(string message)
        {
            if (settings?.LogSettings >= ScutiLog.Verbose)
                Debug.LogWarning(message);
        }

        public static void LogWarning(string message)
        {
            Instance?._LogWarning(message);
        }

        private void _LogWarning(string message, UnityEngine.Object context)
        {
            if (settings.LogSettings >= ScutiLog.Verbose)
                Debug.LogWarning(message, context);
        }

        public static void LogWarning(string message, UnityEngine.Object context)
        {
            Instance?._LogWarning(message, context);
        }

        private void _LogWarning(object message)
        {
            if (settings.LogSettings >= ScutiLog.Verbose)
                Debug.LogWarning(message);
        }

        public static void LogWarning(object message)
        {
            Instance?._LogWarning(message);
        }

        private void _LogError(string message)
        {
            if (settings?.LogSettings >= ScutiLog.ErrorOnly)
                Debug.LogError(message);
        }

        public static void LogError(string message)
        {
            Instance?._LogError(message);
        }

        private void _LogError(string message, UnityEngine.Object context)
        {
            if (settings.LogSettings >= ScutiLog.ErrorOnly)
                Debug.LogError(message, context);
        }

        public static void LogError(string message, UnityEngine.Object context)
        {
            Instance?._LogError(message, context);
        }

        private void _LogError(object message)
        {
            if (settings.LogSettings >= ScutiLog.ErrorOnly)
                Debug.LogError(message);
        }

        public static void LogError(object message)
        {
            Instance?._LogError(message);
        }

        private void _LogException(System.Exception message)
        {
            //SentryAPI.Instance.ScheduleException("Exception: " + message.Message, message.StackTrace);
            if (settings.LogSettings >= ScutiLog.ErrorOnly)
                Debug.LogException(message);
        }

        public static void LogException(System.Exception message)
        {
            Instance?._LogException(message);
        }

        private void _LogException(System.Exception message, UnityEngine.Object context)
        {
            //SentryAPI.Instance.ScheduleException("Exception: " + message.Message, message.StackTrace);
            if (settings.LogSettings >= ScutiLog.ErrorOnly)
                Debug.LogException(message, context);
        }

        public static void LogException(System.Exception message, UnityEngine.Object context)
        {
            Instance?._LogException(message, context);
        }
    }
}