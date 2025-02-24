using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverAnim : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    enum HoverAnimType
    {
        None,
        MoveRight,
        ScaleUp
    };

    enum ClickAnimType
    {
        MoveRightOffScreen,
        Pop
    };

    private RectTransform m_RectTransform;
    private SwitchScene m_SwitchScene;
    private bool m_HoverEnabled;

    [SerializeField] private HoverAnimType m_HoverAnimationType;
    [SerializeField] private ClickAnimType m_ClickAnimationType;
    [SerializeField] private bool m_ChangeScene;
    [SerializeField] private string m_SceneToChange;

    private void Awake()
    {
        m_RectTransform = GetComponent<RectTransform>();
        m_HoverEnabled = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!m_HoverEnabled) return;

        LeanTween.reset();

        if (m_HoverAnimationType == HoverAnimType.MoveRight)
        {
            LeanTween.moveX(m_RectTransform, 125, 0.1f);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!m_HoverEnabled) return;

        LeanTween.reset();

        if (m_HoverAnimationType == HoverAnimType.MoveRight) LeanTween.moveX(m_RectTransform, 75, 0.1f);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        LeanTween.reset();

        if (m_ClickAnimationType == ClickAnimType.MoveRightOffScreen)
            LeanTween.moveX(m_RectTransform, 1920, 0.5f).setEaseInCubic();

        else if (m_ClickAnimationType == ClickAnimType.Pop)
            LeanTween.scale(m_RectTransform, Vector3.one * 1.25f, 0.1f)
                .setEaseOutCubic()
                .setOnComplete(() =>
                {
                    LeanTween.scale(m_RectTransform, Vector3.one, 0.1f)
                        .setEaseInCubic();
                });

        m_SwitchScene = FindFirstObjectByType<SwitchScene>();

        if (m_ChangeScene && m_SwitchScene)
            m_SwitchScene.SwitchSceneLeftToRight(m_SceneToChange);

        m_HoverEnabled = false;
    }
}
