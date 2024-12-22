using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class Character_db : ScriptableObject
{
    public Character_Selector [] characters;

    
    public int Character_count
    {
        get
        {
            return characters.Length;
        }
    }


    public Character_Selector getCharacter(int index)
    {
        return characters[index];
    }

}
