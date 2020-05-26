using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SketchController : MonoBehaviour
{
    public InputHelpers.Button applauseActivationButton;
    public InputHelpers.Button laughterActivationButton;
    public InputHelpers.Button weatherActivationButton;

    public XRController applauseAndLaughterXRController;
    public XRController weatherXRController;

    public AudioClip[] applauseSound;
    public AudioClip[] laughterSound;

    public float activationThreshold = 0.1f;
    public AudioSource audio;


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

            audio.clip = applauseSound[0];
            audio.Play();

        }

        if (CheckIfActivated(applauseAndLaughterXRController, laughterActivationButton))
        {
            // LAUGHTER
            audio.clip = laughterSound[0];
            audio.Play();

        }

        if (CheckIfActivated(weatherXRController, weatherActivationButton))
        {
            // WEATHER
        }

    }

    public bool CheckIfActivated(XRController controller, InputHelpers.Button activationButton)
    {
        InputHelpers.IsPressed(controller.inputDevice, activationButton, out bool isActivated, activationThreshold);
        return isActivated;
    }

}
