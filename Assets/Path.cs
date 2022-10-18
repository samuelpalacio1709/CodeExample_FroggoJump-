using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{

    [SerializeField] Material lineRenderMaterial;
    private Transform frogTransform;
    public static Vector3 highestPoint;
    public static Vector3 endPosition;
    private static LineRenderer lineRenderer;
    public static bool active;
    public int dotCount = 9;
    private List<Vector3> pointList;

    private void Awake()
    {
        pointList = new List<Vector3>();
        TryGetComponent(out lineRenderer);
        lineRenderer.widthMultiplier = 0;
        frogTransform = this.transform;
        Deactivate();
    }

    void Update()
    {
        if (!active) return;
        
        pointList.Clear();
        for (float ratio = 0; ratio <= dotCount; ratio += 1.0f / dotCount)
        {
            Vector3 tangentLineVertex1 = Vector3.Lerp(frogTransform.position, highestPoint, ratio);
            Vector3 tangentLineVertex2 = Vector3.Lerp(highestPoint, endPosition, ratio);
            Vector3 bezierpoint = Vector3.Lerp(tangentLineVertex1, tangentLineVertex2, ratio);
            pointList.Add(bezierpoint);
        }
        lineRenderer.positionCount = pointList.Count;
        lineRenderer.SetPositions(pointList.ToArray());
    }

    public static void Activate(Vector3 endPos, Vector3 highestP)
    {
        active = true;
        endPosition = endPos;
        highestPoint = highestP;
        lineRenderer.widthMultiplier = 1;

    }

    public static void Deactivate()
    {
        active = false;
        lineRenderer.widthMultiplier = 0;

    }


}
