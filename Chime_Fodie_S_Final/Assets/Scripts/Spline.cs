using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spline : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3[] splinePoint;
    private int splineCount;

    public bool debugDrawSpline = true;
    void Start()
    {
        splineCount = transform.childCount;
        splinePoint = new Vector3[splineCount];

        for(int i = 0; i < splineCount; i++)
        {
            splinePoint[i] = transform.GetChild(i).position;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(splineCount > 1)
        {
            for (int i = 0; i < splineCount; i++)
            {
                Debug.DrawLine(splinePoint[i], splinePoint[i + 1], Color.red);
            }
        }
    }
}
