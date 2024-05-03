using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ButtonRef : MonoBehaviour
{
    public GameObject selectIndicator;

    public bool selected;
    void Start()
    {
        selectIndicator.SetActive(false);
    }

    
    void Update()
    {
        selectIndicator.SetActive(selected);
    }
}
