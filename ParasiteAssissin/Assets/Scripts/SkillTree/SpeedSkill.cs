using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpeedSkill", menuName = "SkillTree/SpeedSkill")]
public class SpeedSkill : Skill {

    public float speed;

    public override void Apply (Parasite p) {
        p.controlSpeed = speed;
    }

}
