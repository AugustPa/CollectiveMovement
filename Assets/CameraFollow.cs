using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public FishSchoolAnalysis fishSchoolAnalysis;
    private Vector3 offset; // Set this offset in the editor or in Start()

    void Update()
{
    if (fishSchoolAnalysis != null)
    {
        Vector3 newCameraPosition = fishSchoolAnalysis.CenterOfMass + offset;
        
        // Check for NaN
        if (!float.IsNaN(newCameraPosition.x) && !float.IsNaN(newCameraPosition.y))
        {
            transform.position = new Vector3(newCameraPosition.x, newCameraPosition.y, transform.position.z);
        }
    }
}

}
