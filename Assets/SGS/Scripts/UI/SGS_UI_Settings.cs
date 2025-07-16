//----------------------------------------------
//          Simple Garage System
//
// Copyright © 2014 - 2024 BoneCracker Games
// https://www.bonecrackergames.com
// Ekrem Bugra Ozdoganlar
//
//----------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if BCG_RCCP

/// <summary>
/// Options panel used in UI.
/// </summary>
[AddComponentMenu("BoneCracker Games/SGS/UI/SGS UI Settings")]
public class SGS_UI_Settings : MonoBehaviour {

    /// <summary>
    /// Quality buttons in the options / settings menu.
    /// </summary>
    public Button lowQualityButton, medQualityButton, highQualityButton, ultraQualityButton;

    /// <summary>
    /// Audio and music sliders.
    /// </summary>
    public Slider audioSlider;

    private void OnEnable() {

        //  Checking quality buttons on enable.
        CheckQualityButtons();

        //  Checking audio sliders on enable.
        CheckAudioSliders();

    }

    /// <summary>
    /// Checks the audio and music sliders. Sets their values without notfying them.
    /// </summary>
    public void CheckAudioSliders() {

        if (audioSlider)
            audioSlider.SetValueWithoutNotify(SGS.GetAudioVolume());

    }

    /// <summary>
    /// Checks the quality buttons. Sets their states depending on the quality settings.
    /// </summary>
    public void CheckQualityButtons() {

        //  Changing the color of the target graphic.
        switch (QualitySettings.GetQualityLevel()) {

            case 0:

                if (lowQualityButton)
                    lowQualityButton.targetGraphic.color = lowQualityButton.colors.highlightedColor;

                if (medQualityButton)
                    medQualityButton.targetGraphic.color = medQualityButton.colors.disabledColor;

                if (highQualityButton)
                    highQualityButton.targetGraphic.color = highQualityButton.colors.disabledColor;

                if (ultraQualityButton)
                    ultraQualityButton.targetGraphic.color = ultraQualityButton.colors.disabledColor;

                break;

            case 1:

                if (lowQualityButton)
                    lowQualityButton.targetGraphic.color = lowQualityButton.colors.disabledColor;

                if (medQualityButton)
                    medQualityButton.targetGraphic.color = medQualityButton.colors.highlightedColor;

                if (highQualityButton)
                    highQualityButton.targetGraphic.color = highQualityButton.colors.disabledColor;

                if (ultraQualityButton)
                    ultraQualityButton.targetGraphic.color = ultraQualityButton.colors.disabledColor;

                break;

            case 2:

                if (lowQualityButton)
                    lowQualityButton.targetGraphic.color = lowQualityButton.colors.disabledColor;

                if (medQualityButton)
                    medQualityButton.targetGraphic.color = medQualityButton.colors.disabledColor;

                if (highQualityButton)
                    highQualityButton.targetGraphic.color = highQualityButton.colors.highlightedColor;

                if (ultraQualityButton)
                    ultraQualityButton.targetGraphic.color = ultraQualityButton.colors.disabledColor;

                break;

            case 3:

                if (lowQualityButton)
                    lowQualityButton.targetGraphic.color = lowQualityButton.colors.disabledColor;

                if (medQualityButton)
                    medQualityButton.targetGraphic.color = medQualityButton.colors.disabledColor;

                if (highQualityButton)
                    highQualityButton.targetGraphic.color = highQualityButton.colors.disabledColor;

                if (ultraQualityButton)
                    ultraQualityButton.targetGraphic.color = ultraQualityButton.colors.highlightedColor;

                break;

        }

    }

    /// <summary>
    /// Sets the volume of the audio.
    /// </summary>
    /// <param name="slider"></param>
    public void SetAudioVolume(Slider slider) {

        SGS.SetAudioVolume(slider.value);

    }

    /// <summary>
    /// Sets the volume of the music.
    /// </summary>
    /// <param name="slider"></param>
    public void SetMusicVolume(Slider slider) {

        SGS.SetMusicVolume(slider.value);

    }

    /// <summary>
    /// Sets the level of the quality.
    /// </summary>
    /// <param name="level"></param>
    public void SetQualityLevel(int level) {

        //  Sets the level of the quality.
        QualitySettings.SetQualityLevel(level);

        //  Calling an event on quality changed.
        CheckQualityButtons();

    }

}

#else

/// <summary>
/// Options panel used in UI.
/// </summary>
[AddComponentMenu("BoneCracker Games/SGS/UI/SGS UI Settings")]
public class SGS_UI_Settings : MonoBehaviour {
}

#endif