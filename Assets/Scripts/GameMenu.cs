using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    static bool paused = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) PauseButton(transform);
    }

    public void ChangePanelVisibilityButton(Animator panel)
    {
        panel.SetBool("Visible", !panel.GetBool("Visible"));
    }

    public void SpeedUp(TextMeshProUGUI text)
    {
        if(Time.timeScale < 3f)
            Time.timeScale += 1f;
        else Time.timeScale = 1f;
        text.text = Time.timeScale + "x";
    }

    public void PauseButton(Transform images)
    {
        paused = !paused;
        Time.timeScale = paused ? 0 : 1;
        SwitchObject(images);
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void SwitchObject(Transform elements)
    {
        foreach (Transform element in elements)
            element.gameObject.SetActive(!element.gameObject.active);
    }
}