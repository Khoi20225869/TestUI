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

/// <summary>
/// Music player across scenes.
/// </summary>
[AddComponentMenu("BoneCracker Games/SGS/SGS Soundtrack")]
public class SGS_Soundtrack : MonoBehaviour {

    private static SGS_Soundtrack Instance;
    public AudioSource audioSource;

    private void Awake() {

        if (Instance == null) {

            Instance = this;
            DontDestroyOnLoad(gameObject);

        } else {

            Destroy(gameObject);
            return;

        }

    }

    private void OnEnable() {

        if (!audioSource.isPlaying)
            audioSource.Play();

    }

    private void Reset() {

        audioSource = GetComponent<AudioSource>();

        if (!audioSource)
            return;

        audioSource.bypassEffects = true;
        audioSource.loop = true;
        audioSource.priority = 0;
        audioSource.spatialBlend = 0f;

    }

}
