using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace IndieMarc.TopDown
{
    public class Character_Manager : MonoBehaviour
    {
        [SerializeField] private Character_db m_CharacterDB;
        [SerializeField] private SpriteRenderer m_PlayerSprite;
        [SerializeField] private Material m_NormalMaterial;
        [SerializeField] private Material m_WhiteMaterial;

        private bool m_CanChangePlayer = true;
        private int m_SelectedOption = 0;

        void Start()
        {
            Character_Selector character = m_CharacterDB.getCharacter(m_SelectedOption);
            m_PlayerSprite.sprite = character.character_sprite;
        }


        public void nextOption()
        {
            if (!m_CanChangePlayer) return;

            m_SelectedOption = (m_SelectedOption + 1) % m_CharacterDB.Character_count;
            Character_Selector character = m_CharacterDB.getCharacter(m_SelectedOption);
            m_CanChangePlayer = false;

            MoveCharacter(true, character.character_sprite);
            FadeMotion();

        }

        public void previousOption()
        {
            if (!m_CanChangePlayer) return;

            m_SelectedOption--;
            if (m_SelectedOption < 0) m_SelectedOption = m_CharacterDB.Character_count - 1;
            Character_Selector character = m_CharacterDB.getCharacter(m_SelectedOption);
            m_CanChangePlayer = false;

            MoveCharacter(false, character.character_sprite);
            FadeMotion();
        }

        private void MoveCharacter(bool left, Sprite newSprite)
        {
            float firstPosition = -1.5f, secondPosition = 1.5f;

            if (!left)
            {
                firstPosition *= -1;
                secondPosition *= -1;
            }

            LeanTween.moveX(gameObject, firstPosition, 0.25f)
                .setEaseInBack()
                .setOnComplete(() =>
                {
                    transform.position = Vector3.right * secondPosition;
                    m_PlayerSprite.sprite = newSprite;
                    LeanTween.moveX(gameObject, 0, 0.25f)
                        .setEaseOutBack()
                        .setOnComplete(() =>
                        {
                            m_CanChangePlayer = true;
                        });
                });
        }

        private void FadeMotion()
        {
            LeanTween.color(gameObject, new(1, 1, 1, 0), 0.25f)
                .setEaseInCubic()
                .setOnComplete(() =>
                {
                    LeanTween.color(gameObject, Color.white, 0.25f)
                        .setEaseOutCubic();
                });
        }

        public void ConfirmPlayerAndPlay()
        {
            Character_Selector properties = m_CharacterDB.getCharacter(m_SelectedOption);

            GameObject player = new("Player");
            player.transform.position = new Vector3(-12, 0, 0);
            player.tag = "Player";

            GameObject sword = new("Sword");
            sword.transform.parent = player.transform;

            GameObject swordSwing = new("Sword-Swing");
            swordSwing.transform.parent = player.transform;

            GameObject attackZone = new("Attack Zone");
            attackZone.transform.parent = player.transform;

            SpriteRenderer playerSprite = player.AddComponent<SpriteRenderer>();
            playerSprite.sprite = properties.character_sprite;
            playerSprite.sortingOrder = 10;

            Rigidbody2D playerRB = player.AddComponent<Rigidbody2D>();
            playerRB.mass = 20;
            playerRB.drag = 10;
            playerRB.angularDrag = 10;
            playerRB.gravityScale = 0;
            playerRB.freezeRotation = true;

            CapsuleCollider2D playerCollider = player.AddComponent<CapsuleCollider2D>();
            playerCollider.offset = new Vector2(0, -0.25f);
            playerCollider.size = new Vector2(0.5f, 1);

            sword.transform.localPosition = new Vector3(0.413f, -0.418f, 0);
            sword.transform.localEulerAngles = new(0, 0, -149.19f);
            sword.transform.localScale = Vector3.one * 0.5f;

            swordSwing.transform.localScale = Vector3.one * 0.5f;

            SpriteRenderer swordSprite = sword.AddComponent<SpriteRenderer>();
            swordSprite.sprite = properties.character_sword_sprite;
            swordSprite.sortingOrder = 10;

            SpriteRenderer swordSwingSprite = swordSwing.AddComponent<SpriteRenderer>();
            swordSwingSprite.sprite = properties.character_sword_swing_sprite;
            swordSwingSprite.sortingOrder = 10;
            swordSwing.SetActive(false);

            CircleCollider2D attackZoneCollider = attackZone.AddComponent<CircleCollider2D>();
            attackZoneCollider.isTrigger = true;
            attackZoneCollider.radius = 0.5f;

            Player playerController = player.AddComponent<Player>();
            playerController.normalMaterial = m_NormalMaterial;
            playerController.whiteMaterial = m_WhiteMaterial;
            playerController.max_hp = 5;
            playerController.attack_damage = 1;
            playerController.speed = 3;

            AttackZone attackZoneController = attackZone.AddComponent<AttackZone>();
            attackZoneController.character = playerController;

            attackZone.transform.position = Vector3.zero;
            attackZone.SetActive(false);

            Animator playerAnimator = player.AddComponent<Animator>();
            playerAnimator.runtimeAnimatorController = properties.animator_controller;
            playerAnimator.fireEvents = false;

            playerController.animator = playerAnimator;
            playerController.spriteRenderer = playerSprite;
            playerController.swordSpriteRenderer = swordSprite;

            FindFirstObjectByType<SwitchScene>().SwitchSceneLeftToRight("Level1");
        }
    }
}
