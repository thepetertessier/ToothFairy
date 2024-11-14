using UnityEngine;

public class ToothstalkerAudio : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public float maxVolume = 1.0f; // The max volume when the Toothstalker is closest
    public float minVolume = 0.0f; // The min volume when the Toothstalker is far away
    public float maxDistance = 1.0f; // The distance at which the sound is at its max volume
    public float minDistance = 0.1f; // The distance at which the sound starts to fade in

    private void Awake() {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    public float GetVolume() {
        // Check if player is assigned
        if (player == null) return 0f;

        // Calculate the distance between the player and the Toothstalker
        float distance = Vector3.Distance(transform.position, player.position);

        // Map the distance to a volume level
        float volume = Mathf.Clamp01((maxDistance - distance) / (maxDistance - minDistance));

        // Adjust the volume of the AudioSource based on the proximity
        return Mathf.Lerp(minVolume, maxVolume, volume);
    }
}
