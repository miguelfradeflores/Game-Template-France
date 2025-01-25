using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IndieMarc.TopDown
{
    [RequireComponent(typeof(Animator))]
    public class Enemy2 : MonoBehaviour
    {
        private enum State { Moving, Idle, Chasing, Attacking, TakingDamage };

        [SerializeField] private Player m_Player;
        [SerializeField] private List<Transform> m_PatrolPoints = new();
        [SerializeField] private Material m_NormalMaterial;
        [SerializeField] private Material m_WhiteMaterial;
        [SerializeField] private float m_Speed;
        [SerializeField] private float m_ViewDistance;
        [SerializeField] private float m_IdleTime;
        [SerializeField] private float m_AttackDistance;
        [SerializeField] private float m_AttackDelay;
        [SerializeField] private float m_AttackDamage;
        [SerializeField] private float m_HealthPoints;

        private State m_CurrentState = State.Idle;
        private Animator m_Animator;
        private SpriteRenderer m_Sprite;
        private SpriteRenderer m_Sword;
        private int m_CurrentPatrolPoint;
        private int m_NextPatrolPoint;
        private float m_IdleTimer = 0f;
        private float m_AttackTimer = 0f;
        private bool m_ParanoiaSentToPlayer = false;

        private void Awake()
        {
            m_Animator = GetComponent<Animator>();
            m_Sprite = GetComponent<SpriteRenderer>();
            m_Sword = transform.GetChild(0).GetComponent<SpriteRenderer>();

            m_CurrentPatrolPoint = 0;
            m_NextPatrolPoint = (m_CurrentPatrolPoint + 1) % m_PatrolPoints.Count;
        }

        void Start()
        {
            if (m_PatrolPoints.Count == 0) m_PatrolPoints.Add(transform);
            transform.position = m_PatrolPoints[0].position;
        }

        void Update()
        {
            if (m_CurrentState == State.TakingDamage) return;

            if (m_CurrentState == State.Idle)
            {
                m_IdleTimer += Time.deltaTime;

                if (m_IdleTimer > m_IdleTime)
                {
                    m_CurrentState = State.Moving;
                    m_Animator.SetBool("walking", true);

                    Vector3 nextPoint = m_PatrolPoints[m_NextPatrolPoint].position;

                    LeanTween.move(gameObject, nextPoint, Vector3.Distance(transform.position, nextPoint) / m_Speed)
                        .setOnComplete(() =>
                        {
                            m_CurrentState = State.Idle;
                            m_Animator.SetBool("walking", false);
                            m_IdleTimer = 0f;
                            m_CurrentPatrolPoint = m_NextPatrolPoint;
                            m_NextPatrolPoint = (m_CurrentPatrolPoint + 1) % m_PatrolPoints.Count;
                        });
                }
            }

            if (Vector3.Distance(transform.position, m_Player.transform.position) <= m_ViewDistance && m_CurrentState != State.Attacking)
            {
                LeanTween.cancel(gameObject);
                m_CurrentState = State.Chasing;
                if (!m_ParanoiaSentToPlayer)
                {
                    m_Player.IncreaseParanoia();
                    m_ParanoiaSentToPlayer = true;
                }
            }

            m_AttackTimer += Time.deltaTime;

            if (m_CurrentState == State.Chasing && m_Player.IsAlive())
            {
                if (Vector3.Distance(transform.position, m_Player.transform.position) <= m_AttackDistance)
                {
                    if (m_AttackTimer >= m_AttackDelay)
                    {
                        m_Animator.SetBool("walking", false);
                        m_CurrentState = State.Attacking;
                        m_AttackTimer = 0;
                        CalculateAttack();
                    }
                }
                else
                {
                    transform.Translate(m_Speed * Time.deltaTime * (m_Player.transform.position - transform.position).normalized);
                    m_Animator.SetBool("walking", true);
                }
            }
        }

        private void CalculateAttack()
        {
            Vector2 playerPosition = m_Player.transform.position - transform.position;
            float angle = Vector2.SignedAngle(Vector2.right, playerPosition);

            if (angle > -45f && angle <= 45) Attack("right");
            else if (angle > 45 && angle <= 135) Attack("up");
            else if (angle <= -45 && angle >= -135) Attack("down");
            else Attack("left");
        }

        private void Attack(string direction)
        {
            m_Player.TakeDamage(transform.position, m_AttackDamage);
            m_Animator.SetTrigger(direction);
        }

        public void FinishAttack()
        {
            m_CurrentState = State.Chasing;
        }

        public void TakeDamage(float damage)
        {
            m_HealthPoints -= damage;

            m_CurrentState = State.TakingDamage;
            m_Animator.SetBool("walking", false);

            m_Sprite.material = m_WhiteMaterial;
            m_Sword.material = m_WhiteMaterial;

            LeanTween.delayedCall(0.15f, () =>
            {
                m_Sprite.material = m_NormalMaterial;
                m_Sword.material = m_NormalMaterial;
            });

            LeanTween.move(gameObject, transform.position + (transform.position - m_Player.transform.position).normalized * 1.5f, 1.5f / (m_Speed + 5))
                .setEaseOutQuad()
                .setOnComplete(() =>
                {
                    if (m_HealthPoints <= 0)
                    {
                        m_Animator.SetTrigger("die");

                        if (m_ParanoiaSentToPlayer)
                        {
                            m_Player.DecreaseParanoia();
                            m_ParanoiaSentToPlayer = false;
                        }
                    }
                    else
                    {
                        LeanTween.delayedCall(0.25f, () =>
                        {
                            m_CurrentState = State.Chasing;
                        });
                    }
                });
        }

        public void Die()
        {
            Destroy(gameObject);
        }

        [ExecuteInEditMode]
        private void OnDrawGizmos()
        {
            if (m_PatrolPoints.Count == 0) return;

            Vector3 currentPoint = m_PatrolPoints[0].position;
            for (int i = 1; i < m_PatrolPoints.Count + 1; i++)
            {
                Gizmos.DrawLine(currentPoint, m_PatrolPoints[i % m_PatrolPoints.Count].position);
                currentPoint = m_PatrolPoints[i % m_PatrolPoints.Count].position;
            }
        }
    }
}
