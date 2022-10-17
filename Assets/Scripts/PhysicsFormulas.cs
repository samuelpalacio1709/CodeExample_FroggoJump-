using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsFormulas : MonoBehaviour
{
    public static float CalcDistance(GameObject frog, Vector3 target)
    {
        return Vector3.Distance(frog.transform.position, target);
    }
    public static float GetTime(float distance, float v0, GameObject frog)
    {
        return ((distance) / (v0 * Mathf.Cos(45)));
    }


    public static float GetInitialVelocity(Vector3 pos, GameObject frog)
    {
        float initialVelocity;
        Vector3 target = pos;
        float distance = Vector3.Distance(frog.transform.position, target);
        initialVelocity = Mathf.Sqrt((distance * 9.81f) / Mathf.Sin(2 * 45));
        return initialVelocity;
    }

    public static float GetTime(float vo)
    {
        return ((2f * vo * Mathf.Sin(45)) / Physics.gravity.y);
    }

    public static float GetAngle(Vector3 mousePressDownPos, Vector3 mouseReleasePos)
    {

        float signedAngle;
        float a = mousePressDownPos.y - mouseReleasePos.y;
        float h = Mathf.Abs(mouseReleasePos.x - (Screen.width / 2)) + 0.0001f;

        if (mouseReleasePos.x > (Screen.width / 2))
        {
            signedAngle = -(Mathf.Atan2(h, a) * Mathf.Rad2Deg);
        }
        else
        {
            signedAngle = (Mathf.Atan2(h, a) * Mathf.Rad2Deg);

        }


        return signedAngle;
    }


}
