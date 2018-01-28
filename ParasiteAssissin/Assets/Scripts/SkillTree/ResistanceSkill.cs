using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ResistanceSkill", menuName = "SkillTree/ResistanceSkill")]
public class ResistanceSkill : Skill {

    public float resistance;

    public override void Apply(Parasite p)
    {
        p.resistance = resistance;
    }
}
