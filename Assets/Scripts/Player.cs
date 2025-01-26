using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
        [SerializeField] private CameraVFX m_CameraVFX;

        [SerializeField] private Material m_NormalMaterial;
        [SerializeField] private Material m_WhiteMaterial;

        public float max_hp = 5f;
        public float attack_damage = 1f;

        public float m_Speed;
        private float horizontal_input;
        private float vertical_input;
        private float m_HealthPoints;

        public UnityAction onDeath;
        public UnityAction onHit;
        public UnityAction<GameObject> onAttackHit;

        private Rigidbody2D m_RigidBody;
        private Collider2D m_Collider;
        private Animator m_Animator;
        private SpriteRenderer m_Sprite;
        private SpriteRenderer m_Sword;
        private PlayerCharacterState m_State;
        private bool m_CanMove;
        private int m_Paranoia;

        private static Player instance;

        private void Awake()
        {
            if (instance != null) Destroy(gameObject);
            instance = this;

            m_RigidBody = GetComponent<Rigidbody2D>();
            m_Collider = GetComponent<Collider2D>();
            m_Animator = GetComponent<Animator>();
            m_Sprite = GetComponent<SpriteRenderer>();
            m_Sword = transform.GetChild(0).GetComponent<SpriteRenderer>();

            m_HealthPoints = max_hp;
            m_Paranoia = 0;
            m_CanMove = true;
        }

        void Update()
        {
            if (!IsAlive()) return;

            m_Animator.SetBool("walking", horizontal_input != 0 || vertical_input != 0);

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

            transform.Translate(m_Speed * Time.deltaTime * new Vector2(horizontal_input, vertical_input).normalized);
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
            m_Animator.SetTrigger(direction);
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

            m_Sprite.material = m_WhiteMaterial;
            m_Sword.material = m_WhiteMaterial;

            m_CameraVFX.Shake(0.3f, 0.1f);

            LeanTween.delayedCall(0.15f, () =>
            {
                m_Sprite.material = m_NormalMaterial;
                m_Sword.material = m_NormalMaterial;
            });

            LeanTween.move(gameObject, transform.position + (transform.position - from).normalized * 1.5f, 1.5f / (m_Speed + 5))
            .setEaseOutQuad()
            .setOnComplete(() =>
            {
                if (m_HealthPoints <= 0)
                {
                    Debug.Log("Dying");
                    m_Animator.SetTrigger("die");
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

        public static Player Get()
        {
            return instance;
        }

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
            m_CameraVFX.TurnOnParanoiaMode();
        }

        private void TurnOffParanoiaMode()
        {
            m_CameraVFX.TurnOffParanoiaMode();
        }
    }
}
