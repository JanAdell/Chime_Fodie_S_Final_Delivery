using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineMover : MonoBehaviour
{
    // Start is called before the first frame update
    public Spline spline;
    public Transform followObj;

    private Transform thisTransform;

    void Start()
    {
        thisTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        thisTransform.position = spline.WhereOnSpline(followObj.position);
    }
}
