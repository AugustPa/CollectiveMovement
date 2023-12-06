using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Required for UI elements
public enum NoiseType { None, Uniform, Gaussian, Perlin }
public class Simulation : MonoBehaviour
{
    public FishSchoolAnalysis fishSchoolAnalysis;

    public int numberOfFish = 100;
    public GameObject fishPrefab;
    public List<Fish> fishSchool = new List<Fish>();
    public bool initializeInCircularMotion = false; // New boolean to toggle initial conditions
    public float initialSpeed = 1.0f; // Speed for initial circular motion

    // UI Sliders
    public Slider repulsionSlider;
    public Slider alignmentSlider;
    public Slider attractionSlider;

    // Additional UI Sliders for radii
    public Slider repulsionRadiusSlider;
    public Slider neighborRadiusSlider;
    public NoiseType noiseType = NoiseType.None;
    public float noiseStrength = 0.1f;

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
            // Check for key presses
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit(); // Quit the application
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            ResetFishPositionsAndVelocities();
        }
    }

    private void InitializeFishSchool()
    {
        for (int i = 0; i < numberOfFish; i++)
        {
            GameObject fishObject;
            if (initializeInCircularMotion)
            {
                // Circular motion initialization
                Vector3 position = Random.insideUnitCircle.normalized * 1.0f; // Adjust 5.0f to set the radius
                fishObject = Instantiate(fishPrefab, position, Quaternion.identity);
                Fish fish = fishObject.GetComponent<Fish>();
                Vector3 perpVelocity = new Vector3(-position.y, position.x, 0).normalized * initialSpeed;
                fish.velocity = perpVelocity;
            }
            else
            {
                // Random position initialization
                Vector3 randomPosition = new Vector3(Random.value, Random.value, 0);
                fishObject = Instantiate(fishPrefab, randomPosition, Quaternion.identity);
            }

            Fish fishComponent = fishObject.GetComponent<Fish>();
            if (fishComponent != null)
            {
                fishSchool.Add(fishComponent);
                fishComponent.SetFishSchool(fishSchool);
            }
        }
    }

    private void UpdateFishBehavior()
    {
        float repulsion = repulsionSlider.value;
        float alignment = alignmentSlider.value;
        float attraction = attractionSlider.value;
        float repulsionRadius = repulsionRadiusSlider.value;
        float neighborRadius = neighborRadiusSlider.value;

        foreach (Fish fish in fishSchool)
        {
            fish.SetBehaviorWeights(repulsion, alignment, attraction);
            fish.SetBehaviorRadii(repulsionRadius, neighborRadius);
            fish.SetNoiseParameters(noiseType, noiseStrength);
        }
    }

    public void ResetFishPositionsAndVelocities()
    {
        foreach (Fish fish in fishSchool)
        {
            if (initializeInCircularMotion)
            {
                // Reset position and velocity for circular motion
                Vector3 position = Random.insideUnitCircle.normalized * 1.0f; // Adjust 5.0f to set the radius
                fish.transform.position = position;
                Vector3 perpVelocity = new Vector3(-position.y, position.x, 0).normalized * initialSpeed;
                fish.velocity = perpVelocity;
            }
            else
            {
                // Reset position and velocity for random initialization
                Vector3 randomPosition = new Vector3(Random.value, Random.value, 0);
                fish.transform.position = randomPosition;
                fish.ResetVelocity();
            }
        }
    }
      public void OnNoiseTypeChanged(int noiseTypeIndex)
    {
        noiseType = (NoiseType)noiseTypeIndex;
        // Update the noise type for all fish
        foreach (Fish fish in fishSchool)
        {
            fish.SetNoiseParameters(noiseType, noiseStrength);
        }
    }

    public void OnNoiseStrengthChanged(float strength)
    {
        noiseStrength = strength;
        // Update the noise strength for all fish
        foreach (Fish fish in fishSchool)
        {
            fish.SetNoiseParameters(noiseType, noiseStrength);
        }
    }

}
