using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererController : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public PolygonCollider2D polygonCollider2D;
    public Transform leftPoint;
    public Transform rightPoint;

    private void Start()
    {
        lineRenderer.positionCount = 2;
    }

    private void Update()
    {
        lineRenderer.SetPosition(0, leftPoint.position);
        lineRenderer.SetPosition(1, rightPoint.position);
    }

    public float checkDistanceBetweenPoints(bool checkLeftPoint)
    {
        float yDistance = 0;

        if (checkLeftPoint)
        {
            yDistance = leftPoint.position.y - rightPoint.position.y;
        }
        else
        {
            yDistance = rightPoint.position.y - leftPoint.position.y;
        }

        return yDistance;
    }

    public bool CanMoveHigher(bool checkLeftPoint)
    {
        if (checkLeftPoint)
        {
            if (leftPoint.transform.position.y >= 5)
            {
                return false;
            }
        }
        else
        {
            if (rightPoint.transform.position.y >= 5)
            {
                return false;
            }
        }

        return true;
    }

    public Vector3[] GetPositions()
    {
        Vector3[] positions = new Vector3[lineRenderer.positionCount];
        lineRenderer.GetPositions(positions);
        return positions;
    }
}
