using UnityEngine;
using TMPro; // Add this to use TextMeshPro components

public class Speedometer : MonoBehaviour
{
    public Rigidbody carRigidbody; // Assign this in the inspector
    public TextMeshProUGUI speedText; // Assign this in the inspector

    void Update()
    {
        float speed = carRigidbody.velocity.magnitude * 3.6f; // Convert meters per second to KM/h
        speedText.text = speed.ToString("F0") + " KM/h"; // Update the text, "F0" formats the float to no decimal places
    }
}
