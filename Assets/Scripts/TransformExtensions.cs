using UnityEngine;

public static class TransformExtensions
{
    public static bool DotTest(this Transform transform, Transform other, Vector2 testDirection)
    {
        Vector2 directionToOther = (other.position - transform.position).normalized;
        float dot = Vector2.Dot(directionToOther, testDirection);
        return dot > 0.5f;
    }
}
