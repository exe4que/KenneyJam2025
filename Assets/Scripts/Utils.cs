using System;
using System.Collections;
using UnityEngine;

namespace KenneyJam2025
{
    public static class Utils
    {
        /// <summary>
        /// Re-scales a value between a new float range.
        /// </summary>
        public static float Remap(float OldMin, float OldMax, float NewMin, float NewMax, float OldValue, bool clamp = true)
        {
            if (clamp)
                OldValue = Mathf.Clamp(OldValue, OldMin, OldMax);
            return (((OldValue - OldMin) * (NewMax - NewMin)) / (OldMax - OldMin)) + NewMin;
        }

        /// <summary>
        /// Calls a method delayed by a certain number of frames
        /// </summary>
        /// <param name="caller"></param>
        /// <param name="method"></param>
        /// <param name="frames">Amount of frames to wait</param>
        public static Coroutine DelayedCallInFrames(this MonoBehaviour caller, Action method, int frames)
        {
            if (caller.isActiveAndEnabled)
                return caller.StartCoroutine(DelayedCallInFramesCo(method, frames));
            return null;
        }

        /// <summary>
        /// Calls a method delayed in seconds
        /// </summary>
        /// <param name="caller"></param>
        /// <param name="method"></param>
        /// <param name="frames">Time to wait in seconds</param>
        public static Coroutine DelayedCallInSeconds(this MonoBehaviour caller, Action method, float seconds, bool unscalled = false)
        {
            if (caller.isActiveAndEnabled)
                return caller.StartCoroutine(DelayedCallInSecondsCo(method, seconds, unscalled));
            return null;
        }

        private static IEnumerator DelayedCallInFramesCo(Action method, int frames)
        {
            int frameCount = 0;
            while (frameCount < frames)
            {
                yield return null;
                frameCount++;
            }
            method.Invoke();
        }

        private static IEnumerator DelayedCallInSecondsCo(Action method, float seconds, bool unscaled = false)
        {
            float startTime = unscaled ? Time.unscaledTime : Time.time;
            float time = 0;
            do
            {
                yield return null;
                time = unscaled ? Time.unscaledTime : Time.time;
            }
            while (time < startTime + seconds);
            method.Invoke();
        }
        
        public static string Color(this string str, Color c)
        {
            string hexColor = ColorUtility.ToHtmlStringRGB(c);
            return Color(str, hexColor);

        }

        public static string Color(this string str, string hexColor)
        {
            return $"<color=#{hexColor}>{str}</color>";
        }
    }
}