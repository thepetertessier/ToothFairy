using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelText : MonoBehaviour
{
    [SerializeField] private TMP_Text tmp;


    public void SetText(string text)
    {
        tmp.text = text;
    }
}
