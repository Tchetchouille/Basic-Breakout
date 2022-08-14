using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawBrick : MonoBehaviour
{
    //Allows us to acces the LineRenderer of the Brick
    [SerializeField]
    LineRenderer brickRenderer;

    //Allows us to acces the EdgeCollider2D of the Brick
    [SerializeField]
    EdgeCollider2D brickCollider;

    //We choose an angle for our brick, as well as its speed
    public int angle;
    [SerializeField]
    float speed;

    //The number of steps is determined by the number of points in the collider
    int steps;

    // Start is called before the first frame update
    void Start()
    {
        //We use the numbers of points in our collider to determine the number of vertices in our line
        steps = brickCollider.points.Length;
    }

    // Update is called once per frame
    void Update()
    {
        //We make the brick expand (actually here we just move )
        transform.Translate(Vector3.up * speed * Time.deltaTime);

        //Each frame, we adjust the size of the circle arc
        ShapeBrick(steps, getDistance(), angle);

        brickCollider.offset = -transform.position;
    }

    void ShapeBrick(int steps, float radius, int angle)
    {
        //Adding the proper number of vertices
        brickRenderer.positionCount = steps + 1;

        //We create an array to store the new position for our collider's points
        Vector2[] colliderpoints = new Vector2[steps + 1];

        //for each vertex, determine its position
        for(int currentStep = 0; currentStep < steps + 1; currentStep++)
        {
            
            //From 0 to 1, how far along the circle we are
            float stepProgress = (float)currentStep / steps / (360f / angle);

            //To get the progress in actual length (on a circle of radius 1) and not steps percentage, we multiply it by 2 * PI.
            float lengthProgress = stepProgress * 2 * Mathf.PI;

            //Using that length, we can calculate the coordinates using cos and sin and then we multiply by the radius to get the actual position
            float vertexX = Mathf.Cos(lengthProgress) * radius;
            float vertexY = Mathf.Sin(lengthProgress) * radius;

            //The we store the coordinates in a Vector3
            Vector3 vertexPos = new Vector3(vertexX, vertexY, 0);

            //Finally we attribute this coordinates to each vertex
            brickRenderer.SetPosition(currentStep, vertexPos);

            //We attribute the vertex position to its corresponding collider point
            colliderpoints[currentStep] = new Vector2(vertexX, vertexY);
        }
        //Finally we attribute its position to each point of the collider
        brickCollider.points = colliderpoints;
    }
    float getDistance()
    {
        return Mathf.Sqrt(transform.position.x * transform.position.x + transform.position.y * transform.position.y);
    }

}
