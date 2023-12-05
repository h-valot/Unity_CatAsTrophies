using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider volumeSlider;

    private void Start()
    {
        float valuetochange = DataManager.data.MusicVolumeValue;
        volumeSlider.value = valuetochange;
    }
    public void SetLevel(float sliderValue)
    {
        mixer.SetFloat("MusicVol", Mathf.Log10(sliderValue) * 20);
        DataManager.data.MusicVolumeValue = sliderValue;
    }
}
