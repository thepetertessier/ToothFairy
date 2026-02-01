using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodiePlacer : MonoBehaviour, IInitializable {
    [SerializeField] private int teethToPlace = 4;

    private HashSet<string> bedsWithKey = new();
    private HashSet<string> bedsWithTeeth = new();
    private readonly List<string> bedNames = new();

    private void Awake() {
        GameObject[] beds = GameObject.FindGameObjectsWithTag("Bed");
        foreach (GameObject bed in beds) {
            bedNames.Add(bed.name);
        }
    }

    private void PlaceGoodies() {
        if (bedNames.Count == 0) return;
        // Choose a random bed to have the key
        string keyBed = PopRandom(bedNames);
        bedsWithKey.Add(keyBed);
        Debug.Log($"Put key in bed: {keyBed}");

        for (int i = 0; i < teethToPlace; i++) {
            if (bedNames.Count == 0) break;
            string bed = PopRandom(bedNames);
            bedsWithTeeth.Add(bed);
        }
    }

    public void Initialize() {
        // Re-populate bed names in case something changed between Awake and Initialize
        bedNames.Clear();
        GameObject[] beds = GameObject.FindGameObjectsWithTag("Bed");
        foreach (GameObject bed in beds) {
            bedNames.Add(bed.name);
        }

        bedsWithKey = new HashSet<string>();
        bedsWithTeeth = new HashSet<string>();
        PlaceGoodies();
    }

    private string PopRandom(List<string> strings) {
        // assume strings is nonempty
        int index = Random.Range(0, strings.Count);
        string element = strings[index];
        strings.RemoveAt(index);
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
