using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Modifiers/CharacterStatsHealthModifierSO")]
public class CharacterStatsHealthModifierSO : CharacterStatsModifierSO
{
    public override void AffectCharacter(GameObject character, float val)
    {
        PlayerHP health = character.GetComponent<PlayerHP>();

        if (health != null)
        {
            health.AddHealth((int)val);
        }
    }
}
