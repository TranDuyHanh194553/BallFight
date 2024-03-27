using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helper : MonoBehaviour
{

    public static Vector3 GetMinPosition(List<Vector3> positions)
    {
        if (positions.Count == 0)
        {
            Debug.LogError("Position list is empty.");
            return Vector3.zero;
        }

        Vector3 minPosition = positions[0];
        for (int i = 1; i < positions.Count; i++)
        {
            if (positions[i].x < minPosition.x)
                minPosition.x = positions[i].x;
            if (positions[i].y < minPosition.y)
                minPosition.y = positions[i].y;
            if (positions[i].z < minPosition.z)
                minPosition.z = positions[i].z;
        }

        return minPosition;
    }



    public static Vector3 GetMaxPosition(List<Vector3> positions)
    {
        if (positions.Count == 0)
        {
            Debug.LogError("Position list is empty.");
            return Vector3.zero;
        }

        Vector3 maxPosition = positions[0];
        for (int i = 1; i < positions.Count; i++)
        {
            if (positions[i].x > maxPosition.x)
                maxPosition.x = positions[i].x;
            if (positions[i].y > maxPosition.y)
                maxPosition.y = positions[i].y;
            if (positions[i].z > maxPosition.z)
                maxPosition.z = positions[i].z;
        }

        return maxPosition;
    }

}
