using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredictionManager : MonoBehaviour
{
    Plane plane;
    public LineRenderer lineRenderer;
    public Vector3 endPoint;
    // Number of points on the line
    public int numPoints = 50;

    // distance between those points on the line
    public float timeBetweenPoints = 0.1f;
    Rigidbody rigid;
    // The physics layers that will cause the line to stop being drawn
    public LayerMask CollidableLayers;
    public bool predict;
    void Start()
    {
        plane = GetComponent<Plane>();
        predict = false;
        rigid = GetComponent<Rigidbody>();
    }


    void Update()
    {
        if (predict)
        {
            lineRenderer.positionCount = (int)numPoints;
            List<Vector3> points = new List<Vector3>();
            Vector3 startingPosition = plane.ballHolderTransform.position;
            Vector3 startingVelocity = rigid.velocity + (plane.ballHolderTransform.up * plane.ballPower);

            for (float t = 0; t < numPoints; t += timeBetweenPoints)
            {
                Vector3 newPoint = startingPosition + t * startingVelocity;
                newPoint.y = startingPosition.y + startingVelocity.y * t + Physics.gravity.y / 2f * t * t;
                points.Add(newPoint);

                if (Physics.OverlapSphere(newPoint, 2, CollidableLayers).Length > 0)
                {
                    lineRenderer.positionCount = points.Count;
                    endPoint = points[points.Count - 1];
                    break;
                }
            }

            lineRenderer.SetPositions(points.ToArray());
        }
        else
        {
            lineRenderer.positionCount = 0;
        }
       
    }
}