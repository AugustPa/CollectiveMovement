using UnityEngine;
using System.Collections.Generic;

public class FishSchoolAnalysis : MonoBehaviour
{
    public List<Fish> fishSchool; // Reference to the fish school

    private Vector3 centerOfMass;
    private float rotation;
    private float polarization;
    private float plotSize = 250;

    public Vector3 CenterOfMass => centerOfMass;
    public float Rotation => rotation;
    public float Polarization => polarization;
    public RectTransform pointMarker;


    void Update()
    {
        centerOfMass = CalculateCenterOfMass();
        rotation = CalculateRotation(centerOfMass);
        polarization = CalculatePolarization();

        // Optional: Debug logs or handling the calculated values
        Debug.Log("Center of Mass: " + centerOfMass);
        Debug.Log("Rotation: " + rotation);
        Debug.Log("Polarization: " + polarization);
        // Map rotation and polarization to graph positions

        // Map rotation and polarization to graph positions
        Vector2 markerPos = MapToGraph(rotation, polarization);
        pointMarker.anchoredPosition = markerPos;
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


    private float CalculateRotation(Vector3 centerOfMass)
    {
        Vector3 sumRotation = Vector3.zero;
        int fishCount = fishSchool.Count;

        foreach (Fish fish in fishSchool)
        {
            Vector3 directionToCenterOfMass = (fish.transform.position - centerOfMass).normalized;
            Vector3 fishDirection = fish.velocity.normalized;
            
            Vector3 crossProduct = Vector3.Cross(fishDirection, directionToCenterOfMass);
            sumRotation += crossProduct;
        }

        // Calculate the average of the sum of the absolute values of the cross products
        float averageRotation = sumRotation.magnitude / fishCount;

        return averageRotation; // This value should already be between 0 and 1
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
    private Vector2 MapToGraph(float rotationValue, float polarizationValue)
    {
        // Assume graphSize and maxValue are defined to represent the size of your graph and the max values you expect
        float x = (polarizationValue) * plotSize - (plotSize / 2); // Centering the graph
        float y = (rotationValue) * plotSize - (plotSize / 2);

        return new Vector2(x, y);
    }

}
