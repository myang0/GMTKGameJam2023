using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIButtons : MonoBehaviour
{
    public GameObject playButton;
    public GameObject pauseButton;
    public GameObject resetButton;

    public void OnPlayButtonClicked()
    {
        if (ButtonsNull() || GridMovementManager.Instance == null)
            return;
        
        if (GridMovementManager.Instance.phase == Phase.Planning)
        {
            resetButton.SetActive(true);
        }
        
        GridMovementManager.Instance.HandlePlayButtonClicked();
        playButton.SetActive(false);
        pauseButton.SetActive(true);
    }

    public void OnPauseButtonClicked()
    {
        if (ButtonsNull() || GridMovementManager.Instance == null)
            return;

        GridMovementManager.Instance.HandlePauseButtonClicked();
        playButton.SetActive(true);
        pauseButton.SetActive(false);
    }

    public void OnResetButtonClicked()
    {
        if (ButtonsNull() || GridMovementManager.Instance == null)
            return;

        GridMovementManager.Instance.HandleResetButtonClicked();
        playButton.SetActive(true);
        pauseButton.SetActive(false);
        resetButton.SetActive(false);
    }

    bool ButtonsNull()
    {
        return (playButton == null || pauseButton == null || resetButton == null);
    }
}
