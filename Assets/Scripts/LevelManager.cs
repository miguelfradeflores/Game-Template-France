using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IndieMarc.TopDown
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private Vector3 m_PlayerSpawnPoint;
        [SerializeField] private Vector3 m_PlayerScale = new Vector3(1f,1f,1f);
        private Player m_Player;

        private void Start()
        {
            m_Player = FindFirstObjectByType<Player>();
            if (m_Player) {
                m_Player.transform.position = m_PlayerSpawnPoint;
                m_Player.transform.localScale = m_PlayerScale;
                m_Player.setSpawnPoint(m_PlayerSpawnPoint);
            }
        }
    }
}
