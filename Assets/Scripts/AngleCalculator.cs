using UnityEngine;

public class AngleCalculator : MonoBehaviour
{
    public Transform pointA;

    void Update()
    {
        if (pointA != null)
        {
            
            float angle = CalculateAngleBetweenPoints(pointA.position, Input.mousePosition);
            transform.rotation = new Quaternion(0, 0, angle, 0);
        }
    }

    public static float CalculateAngleBetweenPoints(Vector2 pointA, Vector2 pointB)
    {
        // Calculate the difference vector between the two points
        Vector2 difference = pointB - pointA;

        // Calculate the angle in radians between the x-axis and the difference vector
        float angleRadians = Mathf.Atan2(difference.y, difference.x);

        // Convert radians to degrees
        float angleDegrees = angleRadians * Mathf.Rad2Deg;

        // Return the angle (range: -180 to 180)
        //return angleDegrees;

        // If you want the angle in range 0-360, use:
        return (angleDegrees + 360) % 360;
    }
}

