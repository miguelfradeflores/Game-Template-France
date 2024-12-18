using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IndieMarc.TopDown
{
    public class HealthBar : MonoBehaviour
    {
        private IconBar icon_bar;
        private ProgressBar health_bar;

        void Start()
        {
            icon_bar = GetComponent<IconBar>();
            health_bar = GetComponent<ProgressBar>();

            if (icon_bar == null && health_bar == null)
                Debug.LogError("HealthBar requires IconBar.cs or ProgressBar.cs");
        }
        
        void Update()
        {
            if (icon_bar != null)
            {
                Player character = Player.Get();
                icon_bar.value = Mathf.RoundToInt(character.GetHP());
                icon_bar.value_max = Mathf.RoundToInt(character.max_hp);
            }

            if (health_bar != null)
            {
                Player character = Player.Get();
                health_bar.value = character.GetHP();
                health_bar.value_max = character.max_hp;
            }
        }
    }
}
