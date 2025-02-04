using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IndieMarc.TopDown
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private Vector3 m_PlayerSpawnPoint;
        private Player m_Player;

        private void Start()
        {
            m_Player = FindFirstObjectByType<Player>();
            if (m_Player) m_Player.transform.position = m_PlayerSpawnPoint;
        }
    }
}
