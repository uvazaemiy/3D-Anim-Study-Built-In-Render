using System;
using UnityEngine;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    
    private Slider slider;


    private void Start()
    {
        slider = GetComponent<Slider>();
        
        audioSource.volume = slider.value;
    }

    public void ChangeVolume()
    {
        audioSource.volume = slider.value;
    }
}
