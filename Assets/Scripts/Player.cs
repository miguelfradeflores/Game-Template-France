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
        public float max_hp = 5f;

        public float speed = 200f;
        private float horizontal_input;
        private float vertical_input;
        private float hp;
        private float hit_timer = 0f;

        public UnityAction onDeath;
        public UnityAction onHit;

        private Rigidbody2D rigid;
        private Collider2D collide;
        private Animator animator;
        private PlayerCharacterState state;


        private static Player instance;

        private void Awake()
        {
            if (instance != null) Destroy(gameObject);
            instance = this;

            rigid = GetComponent<Rigidbody2D>();
            collide = GetComponent<Collider2D>();
            animator = GetComponent<Animator>();

            hp = max_hp;
        }

        void Update()
        {
            hit_timer += Time.deltaTime;

            animator.SetBool("walking", horizontal_input != 0 || vertical_input != 0);

            if (Input.GetMouseButtonDown(0)) CalculateAttack();
        }

        private void FixedUpdate()
        {
            horizontal_input = Input.GetAxisRaw("Horizontal");
            vertical_input = Input.GetAxisRaw("Vertical");

            rigid.velocity = speed * Time.deltaTime * new Vector2(horizontal_input, vertical_input).normalized;
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
            if (!IsDead())
            {
                hp += heal;
                hp = Mathf.Min(hp, max_hp);
            }
        }

        public void TakeDamage(float damage)
        {
            if (!IsDead() && hit_timer > 0f)
            {
                hp -= damage;
                hit_timer = -1f;

                PlayerData pdata = PlayerData.Get();
                if (pdata != null)
                    pdata.hp = hp;

                if (hp <= 0f)
                {
                    Kill();
                }
                else
                {
                    if (onHit != null)
                        onHit.Invoke();
                }
            }
        }

        private void TouchEnemy(Enemy enemy)
        {
            TakeDamage(enemy.damage);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Debug.Log("Colision");
            if (IsDead())
                return;

            if (collision.gameObject.TryGetComponent(out Enemy enemy))
            {
                Debug.Log("Enemy");
                TouchEnemy(enemy);
            }
        }

        public bool IsDead()
        {
            return state == PlayerCharacterState.Dead;
        }

        public float GetHP() => hp;

        public static Player Get()
        {
            return instance;
        }

        public void Kill()
        {
            if (!IsDead())
            {
                state = PlayerCharacterState.Dead;
                rigid.velocity = Vector2.zero;
                //move = Vector2.zero;
                //state_timer = 0f;
                collide.enabled = false;

                if (onDeath != null)
                    onDeath.Invoke();
            }
        }
    }
}
