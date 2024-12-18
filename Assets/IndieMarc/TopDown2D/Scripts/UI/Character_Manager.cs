using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Character_Manager : MonoBehaviour
{

    public Character_db CharacterDB;

    public string text;
    public SpriteRenderer player;

    private int option = 0;

    void Start()
    {
        UpdateCharacter(option);
    }


    public void nextOption()
    {
        option++;
        if (option >= CharacterDB.Character_count)
        {
            option = 0;
        }
        UpdateCharacter(option);
    }

    public void previousOption()
    {
        option--;
        if (option < 0)
        {
            option = CharacterDB.Character_count -1;
        }
        UpdateCharacter(option);
    }

    private void UpdateCharacter(int opt)
    {
        Character_Selector character = CharacterDB.getCharacter(opt);
        player.sprite = character.character_sprite;
        text = character.character_name;
    }

    public void changeScene()
    {
        SceneManager.LoadScene(5);
    }
}
