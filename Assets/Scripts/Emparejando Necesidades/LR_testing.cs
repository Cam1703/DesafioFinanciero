using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LR_testing : MonoBehaviour
{
    [SerializeField] private LineController lineController;
    [SerializeField] private Transform startPoint;
    private Transform endPoint;

    public void SetStartPoint(Transform point)
    {
        startPoint = point;
    }

    public void SetEndPoint(Transform point)
    {
        endPoint = point;
        if (startPoint != null && endPoint != null)
        {
            //lineController.SetUpLine(new Transform[] { startPoint, endPoint });
        }
    }
}
