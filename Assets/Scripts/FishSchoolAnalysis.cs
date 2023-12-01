using UnityEngine;
using System.Collections.Generic;

public class FishSchoolAnalysis : MonoBehaviour
{
    public List<Fish> fishSchool; // Reference to the fish school

    private Vector3 centerOfMass;
    private Vector3 rotation;
    private float polarization;

    public Vector3 CenterOfMass => centerOfMass;
    public Vector3 Rotation => rotation;
    public float Polarization => polarization;

    void Update()
    {
        centerOfMass = CalculateCenterOfMass();
        rotation = CalculateRotation(centerOfMass);
        polarization = CalculatePolarization();

        // Optional: Debug logs or handling the calculated values
        Debug.Log("Center of Mass: " + centerOfMass);
        Debug.Log("Rotation: " + rotation);
        Debug.Log("Polarization: " + polarization);
    }

    private Vector3 CalculateCenterOfMass()
{
    if (fishSchool == null || fishSchool.Count == 0)
    {
        return Vector3.zero; // Or some default value
    }

    Vector3 sumPositions = Vector3.zero;
    foreach (Fish fish in fishSchool)
    {
        sumPositions += fish.transform.position;
    }
    return sumPositions / fishSchool.Count;
}


    private Vector3 CalculateRotation(Vector3 centerOfMass)
    {
        Vector3 sumRotation = Vector3.zero;
        foreach (Fish fish in fishSchool)
        {
            Vector3 directionVector = (fish.transform.position - centerOfMass).normalized;
            Vector3 velocityUnitVector = fish.velocity.normalized;
            sumRotation += Vector3.Cross(velocityUnitVector, directionVector);
        }
        return sumRotation;
    }

    private float CalculatePolarization()
    {
        Vector3 sumVelocities = Vector3.zero;
        foreach (Fish fish in fishSchool)
        {
            sumVelocities += fish.velocity.normalized;
        }
        return (sumVelocities / fishSchool.Count).magnitude;
    }
}
