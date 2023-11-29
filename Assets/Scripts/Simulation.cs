using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Required for UI elements

public class Simulation : MonoBehaviour
{
     public FishSchoolAnalysis fishSchoolAnalysis;

    public int numberOfFish = 100;
    public GameObject fishPrefab;
    public List<Fish> fishSchool = new List<Fish>();

    // UI Sliders
    public Slider repulsionSlider;
    public Slider alignmentSlider;
    public Slider attractionSlider;

     // Additional UI Sliders for radii
    public Slider repulsionRadiusSlider;
    public Slider neighborRadiusSlider;

    private void Start()
    {
        InitializeFishSchool();
        UpdateFishBehavior(); // Update weights and radii at the start
        // Pass the fishSchool list to the FishSchoolAnalysis script
        if (fishSchoolAnalysis != null)
        {
            fishSchoolAnalysis.fishSchool = fishSchool;
        }
    }

    private void Update()
    {
        UpdateFishBehavior(); // Continuously update weights and radii
    }

    private void InitializeFishSchool()
    {
        for (int i = 0; i < numberOfFish; i++)
        {
            Vector3 randomPosition = new Vector3(Random.value, Random.value, Random.value);
            GameObject fishObject = Instantiate(fishPrefab, randomPosition, Quaternion.identity);
            Fish fish = fishObject.GetComponent<Fish>();

            if (fish != null)
            {
                fishSchool.Add(fish);
            }
        }

        foreach (Fish fish in fishSchool)
        {
            fish.SetFishSchool(fishSchool);
        }
    }

     private void UpdateFishBehavior()
    {
        // Get values from all sliders
        float repulsion = repulsionSlider.value;
        float alignment = alignmentSlider.value;
        float attraction = attractionSlider.value;
        float repulsionRadius = repulsionRadiusSlider.value;
        float neighborRadius = neighborRadiusSlider.value;
        // Log the current slider values
        // Debug.Log("Updating Fish Behavior - Repulsion: " + repulsion 
            //   + ", Alignment: " + alignment 
            //   + ", Attraction: " + attraction 
            //   + ", Repulsion Radius: " + repulsionRadius 
            //   + ", Neighbor Radius: " + neighborRadiues);
        // Update each fish with new weights and radii
        foreach (Fish fish in fishSchool)
        {
            fish.SetBehaviorWeights(repulsion, alignment, attraction);
            fish.SetBehaviorRadii(repulsionRadius, neighborRadius);
        }
    }

}
