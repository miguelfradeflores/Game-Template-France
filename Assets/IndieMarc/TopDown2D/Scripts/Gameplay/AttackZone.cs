using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IndieMarc.TopDown
{
    public class AttackZone : MonoBehaviour
    {
        public Player character;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            character.OnSwordHit(collision);
            Debug.Log($"Hit {collision.name}");
        }
    }
}
