using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodiePlacer : MonoBehaviour {
    [SerializeField] private int numberOfTeeth = 5;

    private readonly HashSet<string> bedsWithKey = new();
    private readonly HashSet<string> bedsWithTeeth = new();

    private readonly List<string> bedNames = new();

    private void Awake() {
        Setup();
    }

    private void Setup() {
        // Get all the bed GameObjects in the scene
        GameObject[] beds = GameObject.FindGameObjectsWithTag("Bed");

        // Populate bedNames list
        foreach (GameObject bed in beds) {
            bedNames.Add(bed.name);
        }

        // If there are no beds, we can't place anything
        if (bedNames.Count == 0) return;

        // Choose a random bed to have the key
        string keyBed = bedNames[Random.Range(0, bedNames.Count)];
        bedsWithKey.Add(keyBed);
        Debug.Log($"Put key in bed: {keyBed}");

        // Shuffle the bedNames list to randomly select beds for teeth
        List<string> shuffledBedNames = new(bedNames);
        ShuffleList(shuffledBedNames);

        // Choose numberOfTeeth beds to have a tooth, excluding the key bed
        int teethPlaced = 0;
        foreach (string bedName in shuffledBedNames) {
            if (bedName != keyBed && teethPlaced < numberOfTeeth) {
                bedsWithTeeth.Add(bedName);
                teethPlaced++;
            }
        }
    }

    // Function to check if the specified bed has the key
    public bool HasKey(string bedName) {
        return bedsWithKey.Contains(bedName);
    }

    // Function to check if the specified bed has a tooth
    public bool HasTooth(string bedName) {
        return bedsWithTeeth.Contains(bedName);
    }

    // Utility function to shuffle a list (Fisher-Yates shuffle)
    private void ShuffleList(List<string> list) {
        for (int i = list.Count - 1; i > 0; i--) {
            int randomIndex = Random.Range(0, i + 1);
            // Swap the elements
            (list[randomIndex], list[i]) = (list[i], list[randomIndex]);
        }
    }
}
