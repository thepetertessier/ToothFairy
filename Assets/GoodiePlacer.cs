using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodiePlacer : MonoBehaviour {
    [SerializeField] private int teethToPlace = 5;

    private readonly HashSet<string> bedsWithKey = new();
    private readonly HashSet<string> bedsWithTeeth = new();

    private void Awake() {
        Setup();
    }

    private void Setup() {
        GameObject[] beds = GameObject.FindGameObjectsWithTag("Bed");
        List<string> bedNames = new();
        foreach (GameObject bed in beds) {
            bedNames.Add(bed.name);
        }
        // If there are no beds, we can't place anything
        if (bedNames.Count == 0) return;

        // Choose a random bed to have the key
        string keyBed = PopRandom(bedNames);
        bedsWithKey.Add(keyBed);
        Debug.Log($"Put key in bed: {keyBed}");

        for (int i = 0; i < teethToPlace; i++) {
            if (bedNames.Count == 0) break;
            string bed = PopRandom(bedNames);
            bedsWithTeeth.Add(bed);
            Debug.Log($"Put tooth in bed: {bed}");
        }
    }

    private string PopRandom(List<string> strings) {
        // assume strings is nonempty
        int index = Random.Range(0, strings.Count);
        string element = strings[index];
        strings.RemoveAt(index);
        Debug.Log($"Chose index {index} from {strings} with count {strings.Count}");
        return element;
    }

    // Function to check if the specified bed has the key
    public bool HasKey(string bedName) {
        return bedsWithKey.Contains(bedName);
    }

    // Function to check if the specified bed has a tooth
    public bool HasTooth(string bedName) {
        return bedsWithTeeth.Contains(bedName);
    }
}
