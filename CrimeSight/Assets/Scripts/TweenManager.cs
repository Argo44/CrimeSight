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
    MatColor
}

// Describes the rate of tweening from start to end
public enum TweenShape
{
    Linear,
    EaseInOut
}

public static class TweenManager
{
    // Fields
    private static List<Tween> tweens = new List<Tween>();

    // Methods
    public static void CreateTween(Transform target, TweenType type, Vector3 endVal, float duration)
    {
        tweens.Add(new Tween(target, type, endVal, duration, TweenShape.Linear, null));
    }
    public static void CreateTween(Material target, TweenType type, Color endVal, float duration)
    {
        tweens.Add(new Tween(target, type, endVal, duration, TweenShape.Linear, null));
    }
    public static void CreateTween(Transform target, TweenType type, Vector3 endVal, float duration, UnityAction callback)
    {
        tweens.Add(new Tween(target, type, endVal, duration, TweenShape.Linear, callback));
    }
    public static void CreateTween(Material target, TweenType type, Color endVal, float duration, UnityAction callback)
    {
        tweens.Add(new Tween(target, type, endVal, duration, TweenShape.Linear, callback));
    }

    public static void UpdateTweens()
    {
        float lerpVal = 0f;

        for (int i = 0; i < tweens.Count; i++)
        {
            // Update tween time
            tweens[i].currTime = Mathf.Min(tweens[i].currTime + Time.deltaTime, tweens[i].endTime);

            // Determine tween 
            switch (tweens[i].shape)
            {
                case TweenShape.Linear:
                    lerpVal = tweens[i].CurrNormTime;
                    break;
                case TweenShape.EaseInOut: // Uses formula: y = cbrt(0.25 * (x - 0.5)) + 0.5
                    lerpVal = Mathf.Pow( 0.25f * (tweens[i].CurrNormTime - 0.5f), 1.0f / 3.0f) + 0.5f;
                    break;
            }

            // Tween each separate type
            switch (tweens[i].type)
            {
                case TweenType.Translation:
                    switch (tweens[i].shape)
                    {
                        case TweenShape.Linear:
                            tweens[i].transform.localPosition = Vector3.Lerp(tweens[i].startV3, tweens[i].endV3, lerpVal);
                            break;
                    }
                    break;
                case TweenType.Rotation:
                    switch (tweens[i].shape)
                    {
                        case TweenShape.Linear:
                            Quaternion q = Quaternion.identity;
                            q.eulerAngles = Vector3.Lerp(tweens[i].startV3, tweens[i].endV3, lerpVal);
                            tweens[i].transform.localRotation = q;
                            break;
                    }
                    break;
                case TweenType.Scale:
                    switch (tweens[i].shape)
                    {
                        case TweenShape.Linear:
                            tweens[i].transform.localScale = Vector3.Lerp(tweens[i].startV3, tweens[i].endV3, lerpVal);
                            break;
                    }
                    break;
                case TweenType.MatColor:
                    switch (tweens[i].shape)
                    {
                        case TweenShape.Linear:
                            tweens[i].material.color = Color.Lerp(tweens[i].startColor, tweens[i].endColor, lerpVal);
                            break;
                    }
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
    }
}
