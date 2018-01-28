using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RangeSkill", menuName = "SkillTree/RangeSkill")]
public class RangeSkill : Skill {

    public int range;

    public override void Apply (Parasite p) {
        p.range = range;
    }
}
