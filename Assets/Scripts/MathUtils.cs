using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathUtils : MonoBehaviour
{
    /**
     * Determines whether a target entity is within the viewing frustrum of the viewing entity.
     * viewAngle - the viewing angle of the viewing entity. Measured in radians.
     * viewirection - the direction the viewing entity is facing. Measured in radians from the horizontal.
     * viewX - the viewing entity's X position.
     * viewY - the viewing entity's Y position.
     * targetX - the target entity's X position.
     * targetY - the target entity's Y position.
     */
    public static bool IsWithinViewFrustrum(float viewAngle, float viewDirection, float viewX, float viewY, float targetX, float targetY)
    {
        float viewLOS = Mathf.Cos(viewAngle / 2);
        Vector2 viewVector = new Vector2(Mathf.Cos(viewDirection), Mathf.Sin(viewDirection));
        Vector2 targetVector = new Vector2(targetX - viewX, targetY - viewY);

        float dotProd = Vector2.Dot(viewVector, targetVector.normalized);
        bool rtn = dotProd > viewLOS;

        if (rtn)
            Debug.Log("The viewing entity can see the target entity.");
        else
            Debug.Log("The viewing entity cannot see the target entity.");
        Debug.Log("Dot Product: " + dotProd);
        Debug.Log("cos(" + viewAngle/2 + ") = " + viewLOS);

        return rtn;
    }

    public static float DistanceBetweenGameObjects(GameObject obj1, GameObject obj2)
    {
        //Cast positions into Vector2's because we do not want to include their z positions to calculate the magnitude.
        return ((Vector2) obj1.transform.position - (Vector2) obj2.transform.position).magnitude;
    }
}
