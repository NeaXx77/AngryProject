using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryDraw : MonoBehaviour
{
    LineRenderer trajectory;

    public float velocityForce;
    public float angle;
    public int resolution;

    //Gravity with gravity scale on y axis
    float g;
    float radianAngle;
    void Awake()
    {
        g = Mathf.Abs(Physics2D.gravity.y /* * GetComponent<Rigidbody2D>().gravityScale*/);
        trajectory = GetComponent<LineRenderer>();
    }

    private void Start() {
    }
    void Update()
    {
        RenderArc();        
    }
    void RenderArc(){
        trajectory.positionCount = (resolution + 1);
        trajectory.SetPositions(CalculateArcArray());
    }
    Vector3[] CalculateArcArray(){
        Vector3[] arcArray = new Vector3[resolution + 1];

        radianAngle = Mathf.Deg2Rad * angle;
        float maxDistance = (velocityForce * velocityForce * Mathf.Sin(2*radianAngle)) / g;

        for (int i = 0; i <= resolution; i++)
        {
            float t = (float)i / (float)resolution;
            arcArray[i] = CalculateArcPoint(t, maxDistance);
        }
        return arcArray;
    }
    Vector3 CalculateArcPoint(float t, float maxDistance){
        float x = t * maxDistance;
        float y = x * Mathf.Tan(radianAngle) - ((g*x*x) / (2 * velocityForce * velocityForce * Mathf.Cos(radianAngle) * Mathf.Cos(radianAngle)));
    
        return new Vector3(x,y);
    }
}
