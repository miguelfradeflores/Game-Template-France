using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Camera))]
public class CameraVFX : MonoBehaviour
{
    [SerializeField] private Animator m_Animator;
    [SerializeField] private Image m_Vignette;
    private Camera m_Camera;

    private void Awake()
    {
        m_Camera = GetComponent<Camera>();
    }

    public void TurnOnParanoiaMode()
    {
        //m_Animator.SetBool("paranoia", true);
        LeanTween.color(m_Vignette.rectTransform, new(1, 1, 1, 0.45f), 0.25f);
        LeanTween.value(gameObject, m_Camera.orthographicSize, m_Camera.orthographicSize - 0.5f, 0.25f)
            .setOnUpdate((float value) =>
            {
                m_Camera.orthographicSize = value;
            });
    }

    public void TurnOffParanoiaMode()
    {
        //m_Animator.SetBool("paranoia", false);
        LeanTween.color(m_Vignette.rectTransform, new(1, 1, 1, 0), 0.25f);
        LeanTween.value(gameObject, m_Camera.orthographicSize, m_Camera.orthographicSize + 0.5f, 0.25f)
            .setOnUpdate((float value) =>
            {
                m_Camera.orthographicSize = value;
            });
    }

    public void Shake(float duration, float magnitude)
    {
        StartCoroutine(ShakeCoroutine(duration, magnitude));
    }

    private IEnumerator ShakeCoroutine(float duration, float magnitude)
    {
        float time = 0;
        while(time < duration)
        {
            time += Time.deltaTime;
            Vector3 newPosition = Vector3.forward * -10;
            newPosition.x = Random.Range(-1.0f, 1.0f) * magnitude;
            newPosition.y = Random.Range(-1.0f, 1.0f) * magnitude;
            
            transform.localPosition = newPosition;

            yield return null;
        }

        transform.localPosition = Vector3.forward * -10;
    }
}
