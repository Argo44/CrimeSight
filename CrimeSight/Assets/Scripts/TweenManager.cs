using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Describes the target object member to tween
public enum TweenType
{
    Translation,
    Rotation,
    Scale,
    MatColor,
    PrtSysColor
}

// Describes the rate of tweening from start to end
public enum TweenShape
{
    Linear,
    EaseIn,
    EaseOut,
    EaseInOut
}

public static class TweenManager
{
    // Fields
    private static List<Tween> tweens = new List<Tween>();

    // Methods
    public static void CreateTween(Transform target, TweenType type, Vector3 endVal, float duration)
    {
        CreateTween(target, type, endVal, duration, null);
    }
    public static void CreateTween(Material target, Color endVal, float duration)
    {
        CreateTween(target, endVal, duration, null);
    }
    public static void CreateTween(ParticleSystem target, Color endVal, float duration)
    {
        tweens.Add(new Tween(target, TweenType.PrtSysColor, endVal, duration, TweenShape.Linear, null));
    }
    public static void CreateTween(Transform target, TweenType type, Vector3 endVal, float duration, UnityAction callback)
    {
        CreateTween(target, type, endVal, duration, TweenShape.Linear, callback);
    }
    public static void CreateTween(Material target, Color endVal, float duration, UnityAction callback)
    {
        CreateTween(target, endVal, duration, TweenShape.Linear, callback);
    }
    public static void CreateTween(Transform target, TweenType type, Vector3 endVal, float duration, TweenShape shape, UnityAction callback)
    {
        tweens.Add(new Tween(target, type, endVal, duration, shape, callback));
    }
    public static void CreateTween(Material target, Color endVal, float duration, TweenShape shape, UnityAction callback)
    {
        tweens.Add(new Tween(target, TweenType.MatColor, endVal, duration, shape, callback));
    }

    public static void UpdateTweens()
    {
        float lerpVal;

        for (int i = 0; i < tweens.Count; i++)
        {
            // Update tween time
            tweens[i].currTime = Mathf.Min(tweens[i].currTime + Time.deltaTime, tweens[i].endTime);

            // Determine shape of tween
            switch (tweens[i].shape)
            {
                case TweenShape.EaseIn:
                    lerpVal = Mathf.Pow(tweens[i].CurrNormTime, 2f);
                    break;
                case TweenShape.EaseOut:
                    lerpVal = 1 - Mathf.Pow(1 - tweens[i].CurrNormTime, 2f);
                    break;
                case TweenShape.EaseInOut: // Lerps between EaseIn and EaseOut
                    lerpVal = Mathf.Lerp(Mathf.Pow(tweens[i].CurrNormTime, 2f),
                        1 - Mathf.Pow(1 - tweens[i].CurrNormTime, 2f), 
                        tweens[i].CurrNormTime);
                    break;

                // Default to Linear if shape is not defined
                default:
                case TweenShape.Linear:
                    lerpVal = tweens[i].CurrNormTime;
                    break;
            }

            // Tween by type
            switch (tweens[i].type)
            {
                case TweenType.Translation:
                        tweens[i].transform.localPosition = Vector3.Lerp(tweens[i].startV3, tweens[i].endV3, lerpVal);
                    break;

                case TweenType.Rotation:
                        Quaternion q = Quaternion.identity;
                        q.eulerAngles = Vector3.Lerp(tweens[i].startV3, tweens[i].endV3, lerpVal);
                        tweens[i].transform.localRotation = q;
                    break;

                case TweenType.Scale:
                        tweens[i].transform.localScale = Vector3.Lerp(tweens[i].startV3, tweens[i].endV3, lerpVal);
                    break;

                case TweenType.MatColor:
                        tweens[i].material.color = Color.Lerp(tweens[i].startColor, tweens[i].endColor, lerpVal);
                    break;

                case TweenType.PrtSysColor:
                    tweens[i].pMMG.color = Color.Lerp(tweens[i].startColor, tweens[i].endColor, lerpVal);
                    tweens[i].pMainMod.startColor = tweens[i].pMMG;
                    break;
            }

            // Delete tween when finished
            if (tweens[i].currTime >= tweens[i].endTime)
            {
                // Invoke callback function if it exists
                tweens[i].onCallback?.Invoke();
                tweens.RemoveAt(i--);
            }
        }
    }

    /// TweenManager handles all Tweens: they are not accessible otherwise
    private class Tween
    {
        // Fields
        public TweenType type;
        public Transform transform;
        public Material material;
        public ParticleSystem pSystem;
        public ParticleSystem.MainModule pMainMod;
        public ParticleSystem.MinMaxGradient pMMG;

        public Vector3 startV3;
        public Vector3 endV3;
        public Color startColor;
        public Color endColor;
        public float currTime = 0;
        public float endTime;
        public TweenShape shape;
        public UnityAction onCallback;

        // Properties

        /// <summary>
        /// A value in the range [0, 1] representing how far along the tween is
        /// </summary>
        public float CurrNormTime
        {
            get { return currTime / endTime; }
        }

        // Constructors
        public Tween(Transform target, TweenType type, Vector3 endVal, float duration, TweenShape shape, UnityAction callback)
        {
            transform = target;
            this.type = type;
            endTime = duration;
            this.shape = shape;
            endV3 = endVal;
            onCallback = callback;

            switch (type)
            {
                case TweenType.Translation:
                    startV3 = transform.localPosition;
                    break;
                case TweenType.Rotation:
                    startV3 = transform.localRotation.eulerAngles;
                    break;
                case TweenType.Scale:
                    startV3 = transform.localScale;
                    break;
            }
        }

        public Tween(Material target, TweenType type, Color endVal, float duration, TweenShape shape, UnityAction callback)
        {
            material = target;
            startColor = material.color;
            this.type = type;
            endTime = duration;
            this.shape = shape;
            endColor = endVal;
            onCallback = callback;
        }

        public Tween(ParticleSystem target, TweenType type, Color endVal, float duration, TweenShape shape, UnityAction callback)
        {
            pSystem = target;
            pMainMod = pSystem.main;
            pMMG = pMainMod.startColor;

            startColor = pSystem.main.startColor.color;
            this.type = type;
            endTime = duration;
            this.shape = shape;
            endColor = endVal;
            onCallback = callback;
        }
    }
}
