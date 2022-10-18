using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{

    [SerializeField] Material lineRenderMaterial;
    public Transform frogTransform;
    Vector3 point2;
    Vector3 point3;
    Color invisible;
    public static Vector3 highestPoint;
    public static Vector3 endPosition;
    static LineRenderer lineRenderer;
    List<Vector3> pointList;

    public int dotCount = 12;
    public static bool active;
    private void Awake()
    {
        pointList = new List<Vector3>();
        TryGetComponent(out lineRenderer);
        Clear();
    }

    private void Start()
    {
        lineRenderer.widthMultiplier = 0;
        invisible = new Color(0, 0, 0, 0);
    }

    void Update()
    {
        if (active)
        {
            lineRenderer.widthMultiplier = 1;
            point2 = Vector3.Lerp(frogTransform.position, highestPoint, 1);
            point3 = Vector3.Lerp(frogTransform.position, endPosition, 1);
            pointList.Clear();
            int count = 0;
            for (float ratio = 0; ratio <= 1; ratio += 1.0f / dotCount)
            {
                var tangentLineVertex1 = Vector3.Lerp(frogTransform.position, point2, ratio); 
                var tangentLineVertex2 = Vector3.Lerp(point2, point3, ratio);
                var bezierpoint = Vector3.Lerp(tangentLineVertex1, tangentLineVertex2, ratio); // interpolacion entre ambas lineas
                pointList.Add(bezierpoint);
                count++;
            }
            lineRenderer.positionCount = pointList.Count;
            lineRenderer.SetPositions(pointList.ToArray());

        }
        else
        {
            Clear();
        }
    }
    public static void Clear()
    {
        lineRenderer.widthMultiplier = 0;


    }


}
