using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using IndieMarc.TopDown;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Canvas m_PauseCanvas;
    private bool m_Paused;

    void Start()
    {
        m_Paused = false;
        m_PauseCanvas.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) TogglePause();
    }

    public void TogglePause()
    {
        m_Paused = !m_Paused;
        m_PauseCanvas.gameObject.SetActive(m_Paused);
        Time.timeScale = m_Paused ? 0 : 1;
    }

    public void ReturnToMenu()
    {
        Destroy(FindFirstObjectByType<Player>().gameObject);
        Time.timeScale = 1;
        SceneManager.LoadScene("Main_Menu");
    }
}
