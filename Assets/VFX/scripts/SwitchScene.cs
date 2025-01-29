using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SwitchScene : MonoBehaviour
{
    [SerializeField] private Image m_CoverImage;

    private void Start()
    {
        if (FindObjectsByType<SwitchScene>(FindObjectsSortMode.None).Length > 1) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    public void SwitchSceneLeftToRight(string sceneName)
    {
        m_CoverImage.rectTransform.anchoredPosition = new(-2260, 0);
        LeanTween.moveX(m_CoverImage.rectTransform, 0, 0.5f)
            .setEaseInCubic()
            .setOnComplete(() =>
            {
                SceneManager.LoadScene(sceneName);
                LeanTween.moveX(m_CoverImage.rectTransform, 2260, 0.5f)
                    .setEaseOutCubic();
            });
    }

    public void SwitchSceneRightToLeft(string sceneName)
    {
        m_CoverImage.rectTransform.anchoredPosition = new(2260, 0);
        LeanTween.moveX(m_CoverImage.rectTransform, 0, 0.5f)
            .setEaseInCubic()
            .setOnComplete(() =>
            {
                SceneManager.LoadScene(sceneName);
                LeanTween.moveX(m_CoverImage.rectTransform, -2260, 0.5f)
                    .setEaseOutCubic();
            });
    }
}
