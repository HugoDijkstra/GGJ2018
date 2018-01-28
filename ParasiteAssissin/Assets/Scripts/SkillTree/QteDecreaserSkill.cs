using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QteDecreaserSkill", menuName = "SkillTree/QteDecreaserSkill")]
public class QteDecreaserSkill : Skill {

    public int QteDecreasment;

    public override void Apply(Parasite p)
    {
        p.qteDecreaser = QteDecreasment;
    }

}
