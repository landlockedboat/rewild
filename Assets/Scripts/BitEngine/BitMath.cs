using UnityEngine;

public static class BitMath
{
    private static float _closeEnough = .05f;
    
    public static float AngleBetween(Vector2 origin, Vector2 destiantion)
    {
        var diference = destiantion - origin;
        var sign = destiantion.y < origin.y ? -1.0f : 1.0f;
        return Vector2.Angle(Vector2.right, diference) * sign;
    }

    public static Vector3Int ToVector3Int(Vector2Int vector2Int)
    {
        return new Vector3Int(vector2Int.x, vector2Int.y, 0);
    }
    
    public static Vector2Int ToVector2Int(Vector3Int vector3Int)
    {
        return new Vector2Int(vector3Int.x, vector3Int.y);
    }
    
    public static float DifferenceSqrMagnitude(Vector2 origin, Vector2 destiantion)
    {
        var diference = destiantion - origin;
        return diference.sqrMagnitude;
    }

    public static Vector3Int RoundToInt(Vector3 vector)
    {
        return new Vector3Int(
            Mathf.RoundToInt(vector.x),
            Mathf.RoundToInt(vector.y),
            Mathf.RoundToInt(vector.z)
            );
    }
    
    public static Vector2Int RoundToInt(Vector2 vector)
    {
        return new Vector2Int(
            Mathf.RoundToInt(vector.x),
            Mathf.RoundToInt(vector.y)
        );
    }

    public static bool CloseEnough(Vector2 rigidbody2DPosition, Vector2Int destination)
    {
        return Vector2.Distance(rigidbody2DPosition, destination) <= _closeEnough;
    }
}