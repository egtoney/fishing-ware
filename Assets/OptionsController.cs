using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsController : MonoBehaviour
{
	public AudioMixer mixer;
	public Slider volumeSlider;

    // Start is called before the first frame update
    void Start()
    {
		if (PlayerPrefs.HasKey("volume")) {
			var volume = PlayerPrefs.GetFloat("volume");
			mixer.SetFloat("masterVolume", linearToLogVolume(volume));
			volumeSlider.value = volume;
		}
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private float linearToLogVolume(float volume) {
		return volume == 0 ? -80 : Mathf.Log10(volume) * 20;
	}

	public void OnVolume(float volume) {
		PlayerPrefs.SetFloat("volume", volume);

		mixer.SetFloat("masterVolume", linearToLogVolume(volume));
	}
}
