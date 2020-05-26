using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SketchController : MonoBehaviour
{
    public XRController applauseAndLaughterXRController;
    public InputHelpers.Button applauseActivationButton;
    public InputHelpers.Button laughterActivationButton;


    public XRController weatherXRController;
    public InputHelpers.Button weatherActivationButton;


    public AudioClip[] applauseSounds;
    public AudioClip[] laughterSounds;
    public Material[] skyBoxMaterials;

    public float activationThreshold = 0.1f;
    public AudioSource audio;
    private int applauseIndex = 0;
    private int laughterIndex = 0;
    private int weatherIndex = 0;


    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(CheckIfActivated(applauseAndLaughterXRController, applauseActivationButton))
        {
            // APPLAUSE

            audio.clip = applauseSounds[applauseIndex];
            audio.Play();
            applauseIndex++;
            if (applauseIndex > applauseSounds.Length + 1) applauseIndex = 0;

        }

        if (CheckIfActivated(applauseAndLaughterXRController, laughterActivationButton))
        {
            // LAUGHTER
            audio.clip = laughterSounds[laughterIndex];
            audio.Play();
            laughterIndex++;
            if (laughterIndex > laughterSounds.Length + 1) laughterIndex = 0;

        }

        if (CheckIfActivated(weatherXRController, weatherActivationButton))
        {
            // WEATHER
            RenderSettings.skybox = skyBoxMaterials[weatherIndex];
            weatherIndex++;
            if (weatherIndex > skyBoxMaterials.Length + 1) weatherIndex = 0;
        }

    }

    public bool CheckIfActivated(XRController controller, InputHelpers.Button activationButton)
    {
        InputHelpers.IsPressed(controller.inputDevice, activationButton, out bool isActivated, activationThreshold);
        return isActivated;
    }

}
