using UnityEngine;
using UnityEngine.Events;
using System.Collections;


public enum PlayerCharacterState
{
    Normal = 0,
    Climb = 10,
    Jump = 20,
    Dead = 50,
}

namespace IndieMarc.TopDown
{
    public class Player : MonoBehaviour
    {
        public Material normalMaterial;
        public Material whiteMaterial;

        public float max_hp = 5f;
        public float attack_damage = 1f;

        public float speed;
        private float horizontal_input;
        private float vertical_input;
        private float m_HealthPoints;
        private Vector3 spawning_point;

        public UnityAction onDeath;
        public UnityAction onHit;
        public UnityAction<GameObject> onAttackHit;

        private CameraVFX m_CameraVFX;
        public Animator animator;
        public SpriteRenderer spriteRenderer;
        public SpriteRenderer swordSpriteRenderer;
        private bool m_CanMove;
        private int m_Paranoia;

        private void Awake()
        {
            if (FindObjectsByType<Player>(FindObjectsSortMode.None).Length > 1) Destroy(gameObject);
            DontDestroyOnLoad(gameObject);

            m_HealthPoints = max_hp;
            m_Paranoia = 0;
            m_CanMove = true;
        }

        private void Start()
        {
            m_CameraVFX = FindFirstObjectByType<CameraVFX>();
        }

        void Update()
        {
            if (!IsAlive()) return;

            animator.SetBool("walking", horizontal_input != 0 || vertical_input != 0);

            if (Input.GetMouseButtonDown(0)) CalculateAttack();
        }



        private void FixedUpdate()
        {
            if (IsAlive() && m_CanMove)
            {
                horizontal_input = Input.GetAxisRaw("Horizontal");
                vertical_input = Input.GetAxisRaw("Vertical");
            }
            else
            {
                horizontal_input = 0;
                vertical_input = 0;
            }

            transform.Translate(speed * Time.deltaTime * new Vector2(horizontal_input, vertical_input).normalized);
        }

        private void CalculateAttack()
        {
            Vector2 mouse_position = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            float angle = Vector2.SignedAngle(Vector2.right, mouse_position);

            if (angle > -45f && angle <= 45) Attack("right");
            else if (angle > 45 && angle <= 135) Attack("up");
            else if (angle <= -45 && angle >= -135) Attack("down");
            else Attack("left");
        }

        private void Attack(string direction)
        {
            animator.SetTrigger(direction);
        }

        public void HealDamage(float heal)
        {
            if (IsAlive())
            {
                m_HealthPoints += heal;
                m_HealthPoints = Mathf.Min(m_HealthPoints, max_hp);
            }
        }

        public void TakeDamage(Vector3 from, float damage)
        {
            m_HealthPoints -= damage;

            PlayerData pdata = PlayerData.Get();
            if (pdata != null)
                pdata.hp = m_HealthPoints;

            m_CanMove = false;

            spriteRenderer.material = whiteMaterial;
            swordSpriteRenderer.material = whiteMaterial;

            if (!m_CameraVFX) m_CameraVFX = FindFirstObjectByType<CameraVFX>();
            if(m_CameraVFX) m_CameraVFX.Shake(0.3f, 0.1f);

            LeanTween.delayedCall(0.15f, () =>
            {
                spriteRenderer.material = normalMaterial;
                swordSpriteRenderer.material = normalMaterial;
            });

            LeanTween.move(gameObject, transform.position + (transform.position - from).normalized * 1.5f, 1.5f / (speed + 5))
            .setEaseOutQuad()
            .setOnComplete(() =>
            {
                if (m_HealthPoints <= 0)
                {
                    Debug.Log("Dying");
                    animator.SetTrigger("die");
                    new WaitForSeconds(1000);
                    Revive();
                }
                else
                {
                    LeanTween.delayedCall(0.25f, () =>
                    {
                        m_CanMove = true;
                    });
                }
            });
        }

        public float GetHP() => m_HealthPoints;

        public void OnSwordHit(Collider2D other)
        {
            if (other.TryGetComponent(out Enemy2 enemy))
            {
                enemy.TakeDamage(attack_damage);
            }
        }

        public bool IsAlive() => m_HealthPoints > 0;

        public void IncreaseParanoia()
        {
            if (m_Paranoia == 0) TurnOnParanoiaMode();
            m_Paranoia++;
        }
        public void DecreaseParanoia()
        {
            m_Paranoia--;
            if (m_Paranoia == 0) TurnOffParanoiaMode();
        }

        private void TurnOnParanoiaMode()
        {
            if (!m_CameraVFX) m_CameraVFX = FindFirstObjectByType<CameraVFX>();
            if (m_CameraVFX) m_CameraVFX.TurnOnParanoiaMode();
        }

        private void TurnOffParanoiaMode()
        {
            if (!m_CameraVFX) m_CameraVFX = FindFirstObjectByType<CameraVFX>();
            if (m_CameraVFX) m_CameraVFX.TurnOffParanoiaMode();
        }

        public void SetPlayerParameters(Character_Selector properties)
        {
            spriteRenderer.sprite = properties.character_sprite;
            swordSpriteRenderer.sprite = properties.character_sword_sprite;
            animator.runtimeAnimatorController = properties.animator_controller;
        }

        public void setSpawnPoint(Vector3 spawnPoint)
        {
            spawning_point = spawnPoint;
        }


        private void Revive()
        {
            transform.localPosition = spawning_point;
            animator.SetBool("walking", true);
            HealDamage(3);
            m_CanMove = true;

        }

    }
}
