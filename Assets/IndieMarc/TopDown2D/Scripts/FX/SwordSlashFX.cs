using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSlashFX : MonoBehaviour
{
    void Start()
    {
        transform.localScale = Vector3.zero;

        LeanTween.scale(gameObject, Vector3.one, 0.15f);
        LeanTween.color(gameObject, new Color(1, 1, 1, 0), 0.25f);
        Destroy(gameObject, 0.5f);
    }
}
