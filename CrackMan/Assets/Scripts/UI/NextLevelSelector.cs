using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelSelector : MonoBehaviour
{
    public int totalPacmen;
    public int currentPacmen;

    public GameObject displayPanel;

    void Awake()
    {
        totalPacmen = GameObject.FindGameObjectsWithTag("Player").Length;
        currentPacmen = totalPacmen;

        GridMovementManager.Instance.onGridReset += HandleGridReset;
    }

    void HandleGridReset()
    {
        currentPacmen = totalPacmen;
        displayPanel.SetActive(false);
    }

    public void HandleKill()
    {
        currentPacmen -= 1;
        if (currentPacmen <= 0)
        {
            displayPanel.SetActive(true);
        }
    }

    public void OnNextLevelButtonPress()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
