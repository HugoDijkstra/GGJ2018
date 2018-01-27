using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeSkill : Skill {

    public override void Apply (Parasite p) {
        p.range = 2 * level;
    }
}
