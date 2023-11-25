using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulation : MonoBehaviour
{
    public int numberOfFish = 100;
    public GameObject fishPrefab;
    public List<Fish> fishSchool = new List<Fish>();

    private void Start()
    {
        InitializeFishSchool();
    }

    private void InitializeFishSchool()
    {
        for (int i = 0; i < numberOfFish; i++)
        {
            Vector3 randomPosition = new Vector3(Random.value, Random.value, Random.value);
            GameObject fishObject = Instantiate(fishPrefab, randomPosition, Quaternion.identity);
            Fish fish = fishObject.GetComponent<Fish>();
            fishSchool.Add(fish);
        }
    }
}
