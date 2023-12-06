using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour

{
    // Add weight variables
    public float repulsionWeight = 1.0f;
    public float alignmentWeight = 1.0f;
    public float attractionWeight = 1.0f;
    // Add radius variables
    public float repulsionRadius = 1.0f;
    public float neighborRadius = 3.0f;
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
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        // Update Z rotation based on velocity
        UpdateRotation();
        LimitSpeed();
        acceleration = Vector3.zero;

        // Debug Log to check update call and final velocity
        //Debug.Log("Fish Update - Position: " + position + ", Velocity: " + velocity);
    }

     private void ApplyBehaviors()
    {
        // Use the variables instead of fixed values
        Vector3 repulsionForce = Calculaterepulsion() * repulsionWeight;
        Vector3 alignmentForce = CalculateAlignment() * alignmentWeight;
        Vector3 attractionForce = Calculateattraction() * attractionWeight;

        acceleration += repulsionForce;
        acceleration += alignmentForce;
        acceleration += attractionForce;

        // Debug Log to check forces
        //Debug.Log("Behaviors - repulsion: " + repulsionForce + ", Alignment: " + alignmentForce + ", attraction: " + attractionForce);
    }
    private void UpdateRotation()
    {
        if (velocity != Vector3.zero)
        {
            // Calculate the angle in degrees
            float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg + 90;
            // Apply the rotation around the Z axis
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }

    private Vector3 Calculaterepulsion()
    {
        Vector3 repulsionVector = Vector3.zero;
        int neighborsCount = 0;

        foreach (Fish otherFish in fishSchool)
        {
            float distance = Vector3.Distance(position, otherFish.position);
            if (distance > 0 && distance < repulsionRadius)
            {
                Vector3 diff = position - otherFish.position;
                diff.Normalize();
                diff /= distance; // Weight by distance
                repulsionVector += diff;
                neighborsCount++;
            }
        }

        if (neighborsCount > 0)
        {
            repulsionVector /= neighborsCount;
        }

        repulsionVector = new Vector3(repulsionVector.x, repulsionVector.y, 0);

        // Debug Log for repulsion Force
        //Debug.Log("repulsion - Count: " + neighborsCount + ", Force: " + repulsionVector);
        return repulsionVector;
    }

    private Vector3 CalculateAlignment()
    {
        Vector3 averageVelocity = Vector3.zero;
        int neighborsCount = 0;

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

        averageVelocity = new Vector3(averageVelocity.x, averageVelocity.y, 0);
        // Debug Log for Alignment Force
        //Debug.Log("Alignment - Count: " + neighborsCount + ", Force: " + averageVelocity);
        return averageVelocity;
    }

    private Vector3 Calculateattraction()
    {
        Vector3 centerOfMass = Vector3.zero;
        Vector3 attractionVector = Vector3.zero;
        int neighborsCount = 0;

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
            attractionVector = centerOfMass - position;
            attractionVector.Normalize();
            return attractionVector;
        }
        attractionVector = new Vector3(attractionVector.x, attractionVector.y, 0);
        // Debug Log for attraction Force
        //Debug.Log("attraction - Count: " + neighborsCount + ", Force: " + attractionVector);
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

    public void SetBehaviorWeights(float repulsion, float alignment, float attraction)
    {
        repulsionWeight = repulsion;
        alignmentWeight = alignment;
        attractionWeight = attraction;
    }

    // Method to set behavior radii
    public void SetBehaviorRadii(float repulsionRad, float neighborRad)
    {
        repulsionRadius = repulsionRad;
        neighborRadius = neighborRad;
    }
    public void ResetVelocity()
    {
        velocity = Vector3.zero;
    }

}
