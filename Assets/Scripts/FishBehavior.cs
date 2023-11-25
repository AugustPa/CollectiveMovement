using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    private List<Fish> fishSchool;

    public void SetFishSchool(List<Fish> school)
    {
        fishSchool = school;
    }

    public Vector3 position => transform.position;
    public Vector3 velocity;
    public Vector3 acceleration;

    private void Update()
    {
        ApplyBehaviors();
        velocity += acceleration * Time.deltaTime;
        transform.position += velocity * Time.deltaTime;
        LimitSpeed();
        acceleration = Vector3.zero;

        // Debug Log to check update call and final velocity
        Debug.Log("Fish Update - Position: " + position + ", Velocity: " + velocity);
    }

    private void ApplyBehaviors()
    {
        Vector3 separationForce = CalculateSeparation();
        Vector3 alignmentForce = CalculateAlignment();
        Vector3 cohesionForce = CalculateCohesion();

        float separationWeight = 1.5f;
        float alignmentWeight = 1.0f;
        float cohesionWeight = 1.0f;

        acceleration += separationForce * separationWeight;
        acceleration += alignmentForce * alignmentWeight;
        acceleration += cohesionForce * cohesionWeight;

        // Debug Log to check forces
        Debug.Log("Behaviors - Separation: " + separationForce + ", Alignment: " + alignmentForce + ", Cohesion: " + cohesionForce);
    }

    private Vector3 CalculateSeparation()
    {
        Vector3 separationVector = Vector3.zero;
        int neighborsCount = 0;
        float desiredSeparation = 1.0f; // Adjust as needed

        foreach (Fish otherFish in fishSchool)
        {
            float distance = Vector3.Distance(position, otherFish.position);
            if (distance > 0 && distance < desiredSeparation)
            {
                Vector3 diff = position - otherFish.position;
                diff.Normalize();
                diff /= distance; // Weight by distance
                separationVector += diff;
                neighborsCount++;
            }
        }

        if (neighborsCount > 0)
        {
            separationVector /= neighborsCount;
        }

        // Debug Log for Separation Force
        Debug.Log("Separation - Count: " + neighborsCount + ", Force: " + separationVector);
        return separationVector;
    }

    private Vector3 CalculateAlignment()
    {
        Vector3 averageVelocity = Vector3.zero;
        int neighborsCount = 0;
        float neighborRadius = 3.0f; // Adjust as needed

        foreach (Fish otherFish in fishSchool)
        {
            float distance = Vector3.Distance(position, otherFish.position);
            if (distance > 0 && distance < neighborRadius)
            {
                averageVelocity += otherFish.velocity;
                neighborsCount++;
            }
        }

        if (neighborsCount > 0)
        {
            averageVelocity /= neighborsCount;
            averageVelocity.Normalize();
        }

        // Debug Log for Alignment Force
        Debug.Log("Alignment - Count: " + neighborsCount + ", Force: " + averageVelocity);
        return averageVelocity;
    }

    private Vector3 CalculateCohesion()
    {
        Vector3 centerOfMass = Vector3.zero;
        Vector3 cohesionVector = Vector3.zero;
        int neighborsCount = 0;
        float neighborRadius = 3.0f; // Adjust as needed

        foreach (Fish otherFish in fishSchool)
        {
            float distance = Vector3.Distance(position, otherFish.position);
            if (distance > 0 && distance < neighborRadius)
            {
                centerOfMass += otherFish.position;
                neighborsCount++;
            }
        }

        if (neighborsCount > 0)
        {
            centerOfMass /= neighborsCount;
            cohesionVector = centerOfMass - position;
            cohesionVector.Normalize();
            return cohesionVector;
        }

        // Debug Log for Cohesion Force
        Debug.Log("Cohesion - Count: " + neighborsCount + ", Force: " + cohesionVector);
        return Vector3.zero;
    }

    private void LimitSpeed()
    {
        float maxSpeed = 5.0f;
        if (velocity.magnitude > maxSpeed)
        {
            velocity = velocity.normalized * maxSpeed;
        }
    }
}
