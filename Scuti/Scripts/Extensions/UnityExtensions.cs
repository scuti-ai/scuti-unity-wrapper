using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.UI;

namespace Scuti {
    public static class UnityExtensions {
        public static Sprite ToSprite(this Texture2D tex) {

            return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(.5f, .5f), 100, 0, SpriteMeshType.FullRect);
        }

        public static string GetEnumMemberValue<T>(this T value) where T : Enum
        {
            return typeof(T)
                .GetTypeInfo()
                .DeclaredMembers
                .SingleOrDefault(x => x.Name == value.ToString())
                ?.GetCustomAttribute<EnumMemberAttribute>(false)
                ?.Value;
        }

        public static void Destroy(this Sprite sprite) {
            try {
                if (sprite.texture != null)
                    MonoBehaviour.Destroy(sprite.texture);
                MonoBehaviour.Destroy(sprite);
            }
            catch { }
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            for (int i = 0; i < n; i++)
            {
                var temp = list[i];
                int randomIndex = UnityEngine.Random.Range(i, n-1);
                list[i] = list[randomIndex];
                list[randomIndex] = temp;
            }
        }

        public static void ReplaceSprite(this Image image, Sprite sprite) {
            image.sprite.Destroy();
            image.sprite = sprite;
        }

        public static void Hide(this CanvasGroup group) {
            group.interactable = false;
            group.alpha = 0;
            group.blocksRaycasts = false;
        }

        public static void Show(this CanvasGroup group) {
            group.interactable = true;
            group.alpha = 1;
            group.blocksRaycasts = true;
        }

        public static void RefreshImmediate(this GameObject go) {
            var initialState = go.activeSelf;

            go.SetActive(!initialState);
            go.SetActive(initialState);
        }

        //public static Task Refresh(this GameObject go, int frameDelay = 1) {
        //    var source = new TaskCompletionSource<bool>();
        //    go.Refresh(() => source.SetResult(true), frameDelay);
        //    return source.Task;
        //}

        //async public static void Refresh(this GameObject go, Action callback, int frameDelay = 1) {
        //    var initialState = go.activeSelf;

        //    go.SetActive(!initialState);
        //    await TaskX.WaitForFrames(frameDelay);
        //    go.SetActive(initialState);
        //    callback?.Invoke();
        //}

        public static void Rebuild(this GameObject go) {
            RectTransform rt = go.GetComponent<RectTransform>();

            if (rt != null)
                LayoutRebuilder.MarkLayoutForRebuild(rt);
        }

        public static void ForceRebuild(this GameObject go) {
            RectTransform rt = go.GetComponent<RectTransform>();

            if (rt != null)
                LayoutRebuilder.ForceRebuildLayoutImmediate(rt);
        }

        public static CancellationTokenSource Play(this Animator anim, string clipName, Action onComplete, int timeoutSeconds = 60) {
            return anim.Play(Animator.StringToHash(clipName), onComplete, timeoutSeconds);
        }

        public static CancellationTokenSource Play(this Animator anim, int stateNameHash, Action onComplete, int timeoutSeconds = 60) {
            if (!anim.isActiveAndEnabled) {
                onComplete?.Invoke();
                return null;
            }
            var cts = new CancellationTokenSource();
            anim.Play(stateNameHash);
            DelayCall(anim, cts.Token, onComplete, timeoutSeconds);
            return cts;
        }

        private static async void DelayCall(Animator anim, CancellationToken token, Action onComplete, int timeout) {
            if (anim == null) return;

            float timePassed = 0;
            while (anim!=null && anim.isActiveAndEnabled && anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1 && !anim.IsInTransition(0)) {
                if (timeout > 0 && timePassed > timeout) {
                    Debug.LogError("Animation failed to complete: " + anim.gameObject);
                    return;
                }
                await Task.Delay(50);
                timePassed += 0.05f;
            }
            if (!token.IsCancellationRequested)
                onComplete?.Invoke();
        }

        public static void SetDropDown(this Dropdown dropDown, string val)
        {
            int index = -1;
            for (int i = 0; i < dropDown.options.Count; ++i)
            {
                if (dropDown.options[i].text.Trim() == val.Trim())
                {
                    index = i;
                    break;
                }
            }
            dropDown.value = index;
        }


    }
}
