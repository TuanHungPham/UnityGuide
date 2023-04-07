using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Modifiers/CharacterAtkDmgModifierSO")]
public class CharacterAtkDmgModifierSO : CharacterStatsModifierSO
{
    public override void AffectCharacter(GameObject character, float val)
    {
        PlayerStats atkDmg = character.GetComponent<PlayerStats>();

        if (atkDmg != null)
        {
            atkDmg.ChangeAtkDmg(val);
        }
    }
}
