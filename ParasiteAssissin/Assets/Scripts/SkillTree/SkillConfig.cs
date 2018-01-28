using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "SkillTree/Skill")]
public class SkillConfig : ScriptableObject{
    public string skillName;
    public Skill skill;

    public SkillConfig[] skillConfigs;
}
