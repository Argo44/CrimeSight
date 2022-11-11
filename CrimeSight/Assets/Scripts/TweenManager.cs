using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    Linear
}

public static class TweenManager
{
    // Fields
    private static List<Tween> tweens = new List<Tween>();

    // Methods
    public static void CreateTransformTween(Transform target, TweenType type, Vector3 endVal, float duration, TweenShape shape)
    {
        tweens.Add(new Tween(target, type, endVal, duration, shape));
    }
    public static void CreateMaterialTween(Material target, TweenType type, Color endVal, float duration, TweenShape shape)
    {
        tweens.Add(new Tween(target, type, endVal, duration, shape));
    }

    public static void UpdateTweens()
    {
        for (int i = 0; i < tweens.Count; i++)
        {
            // Update tween time
            tweens[i].currTime = Mathf.Min(tweens[i].currTime + Time.deltaTime, tweens[i].endTime);

            // Tween each separate type
            switch (tweens[i].type)
            {
                case TweenType.Translation:
                    switch (tweens[i].shape)
                    {
                        case TweenShape.Linear:
                            tweens[i].transform.localPosition = Vector3.Lerp(tweens[i].startV3, tweens[i].endV3, tweens[i].currTime);
                            break;
                    }
                    break;
                case TweenType.Rotation:
                    switch (tweens[i].shape)
                    {
                        case TweenShape.Linear:
                            Quaternion q = Quaternion.identity;
                            q.eulerAngles = Vector3.Lerp(tweens[i].startV3, tweens[i].endV3, tweens[i].currTime);
                            tweens[i].transform.localRotation = q;
                            break;
                    }
                    break;
                case TweenType.Scale:
                    switch (tweens[i].shape)
                    {
                        case TweenShape.Linear:
                            tweens[i].transform.localScale = Vector3.Lerp(tweens[i].startV3, tweens[i].endV3, tweens[i].currTime);
                            break;
                    }
                    break;
                case TweenType.MatColor:
                    switch (tweens[i].shape)
                    {
                        case TweenShape.Linear:
                            tweens[i].material.color = Color.Lerp(tweens[i].startColor, tweens[i].endColor, tweens[i].currTime);
                            break;
                    }
                    break;
            }

            // Delete tween when finished
            if (tweens[i].currTime >= tweens[i].endTime)
                tweens.RemoveAt(i--);
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

        public Tween(Transform target, TweenType type, Vector3 endVal, float duration, TweenShape shape)
        {
            transform = target;
            this.type = type;
            endTime = duration;
            this.shape = shape;
            endV3 = endVal;

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

        public Tween(Material target, TweenType type, Color endVal, float duration, TweenShape shape)
        {
            material = target;
            startColor = material.color;
            this.type = type;
            endTime = duration;
            this.shape = shape;
            endColor = endVal;
        }
    }
}
