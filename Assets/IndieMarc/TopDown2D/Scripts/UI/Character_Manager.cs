using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Character_Manager : MonoBehaviour
{

    public Character_db CharacterDB;
    public SpriteRenderer player;

    private string character_name;
    private bool canChangePlayer = true;
    private int option = 0;

    void Start()
    {
        Character_Selector character = CharacterDB.getCharacter(option);
        player.sprite = character.character_sprite;
    }


    public void nextOption()
    {
        if (!canChangePlayer) return;

        option = (option + 1) % CharacterDB.Character_count;
        Character_Selector character = CharacterDB.getCharacter(option);
        canChangePlayer = false;

        MoveCharacter(true, character.character_sprite);
        FadeMotion();
        
    }

    public void previousOption()
    {
        if (!canChangePlayer) return;

        option--;
        if (option < 0) option = CharacterDB.Character_count - 1;
        Character_Selector character = CharacterDB.getCharacter(option);
        canChangePlayer = false;

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
                player.sprite = newSprite;
                LeanTween.moveX(gameObject, 0, 0.25f)
                    .setEaseOutBack()
                    .setOnComplete(() =>
                    {
                        canChangePlayer = true;
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

    public void changeScene()
    {
        FindFirstObjectByType<SwitchScene>().SwitchSceneLeftToRight("Level1");
    }
}
