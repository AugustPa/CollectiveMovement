using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Required for UI elements

public class Simulation : MonoBehaviour
{
    public int numberOfFish = 100;
    public GameObject fishPrefab;
    public List<Fish> fishSchool = new List<Fish>();

    // UI Sliders
    public Slider repulsionSlider;
    public Slider alignmentSlider;
    public Slider attractionSlider;

    private void Start()
    {
        InitializeFishSchool();
        UpdateFishWeights(); // Update weights at the start
    }

    private void Update()
    {
        UpdateFishWeights(); // Continuously update weights
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

    private void UpdateFishWeights()
    {
        // Get values from sliders
        float repulsion = repulsionSlider.value;
        float alignment = alignmentSlider.value;
        float attraction = attractionSlider.value;

        // Update each fish with new weights
        foreach (Fish fish in fishSchool)
        {
            fish.SetBehaviorWeights(repulsion, alignment, attraction);
        }
    }

}
