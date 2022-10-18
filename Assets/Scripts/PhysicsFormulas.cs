using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsFormulas 
{
    public static float Angle=45;

    public static float CalcDistance(GameObject frog, Vector3 target)
    {
        return Vector3.Distance(frog.transform.position, target);
    }
  

    public static float GetInitialVelocity(Vector3 pos, GameObject frog)
    {
        float initialVelocity;
        Vector3 target = pos;
        float distance = Vector3.Distance(frog.transform.position, target);
        initialVelocity = Mathf.Sqrt((distance * 9.81f) / Mathf.Sin(2 * Angle));
        return initialVelocity;
    }

    public static float GetTime(float vo)
    {
        return Mathf.Abs(((2f * vo * Mathf.Sin(45)) / Physics.gravity.y));
    }

    public static float GetAngle(Vector3 mousePressDownPos, Vector3 mouseReleasePos)
    {

        float signedAngle;
        float adjacentSide = mousePressDownPos.y - mouseReleasePos.y;
        float oppositeSide = Mathf.Abs(mouseReleasePos.x - (Screen.width / 2)) ;

        if (mouseReleasePos.x > (Screen.width / 2))
        {
            signedAngle = -(Mathf.Atan2(oppositeSide, adjacentSide) * Mathf.Rad2Deg);
        }
        else
        {
            signedAngle = (Mathf.Atan2(oppositeSide, adjacentSide) * Mathf.Rad2Deg);

        }

        return signedAngle;
    }


    public static Vector3 GetEndPoint(float v0, Transform frogTransform)
    {
        float maxDistance = 0;
        Vector3 endPos;
        maxDistance = (v0 * PhysicsFormulas.GetTime(v0));
        endPos = (new Vector3((frogTransform.forward.x * maxDistance ) + frogTransform.position.x, frogTransform.forward.y , (frogTransform.forward.z * maxDistance) + frogTransform.position.z));

        return endPos;
    }
    public static Vector3 GetHighestPoint(float v0, Transform frogTransform)
    {
        Vector3 pos = GetEndPoint(v0, frogTransform);
        Vector3 point = Lerp(frogTransform.position, pos, 0.5f);
       
        float y = (frogTransform.position.y + (-Physics.gravity.y * Mathf.Pow(PhysicsFormulas.GetTime(v0), 2)) / 2);

        return new Vector3(point.x, y, point.z);
    }
    public static Vector3 Lerp(Vector3 start, Vector3 end, float percentage)
    {
        return (start + percentage * (end - start));
    }
}
