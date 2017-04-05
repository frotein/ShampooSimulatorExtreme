using UnityEngine;
using System.Collections;

public static class Extensions  
{
	public static Vector2 XZ(this Vector3 vec)
    {
        return new Vector2(vec.x, vec.z);
    }

    public static Vector2 XY(this Vector3 vec)
    {
    	return new Vector2(vec.x, vec.y);
    }

    public static Vector3 XYZ(this Vector2 vec, float z)
    {
    	return new Vector3(vec.x, vec.y, z);
    } 
	
	public static float DistanceSqr(this Vector2 p1, Vector2 p2)
	{
		float distX = p1.x - p2.x;
		float distY = p1.y - p2.y;

		return (distX * distX) + (distY * distY);
	}

    public static float Angle(this Vector2 p_vector2)
    {
        if (p_vector2.x < 0)
        {
            return 360 - (Mathf.Atan2(p_vector2.x, p_vector2.y) * Mathf.Rad2Deg * -1);
        }
        else
        {
            return Mathf.Atan2(p_vector2.x, p_vector2.y) * Mathf.Rad2Deg;
        }
    }


    public static Vector2 DegreeToVector2(this float degree)
    {
        degree -= 90;
        return RadianToVector2(degree * Mathf.Deg2Rad);
    }

    public static Vector2 RadianToVector2(this float radian)
    {
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }
}
