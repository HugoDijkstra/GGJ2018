using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour {

    public static SkillManager instance;

    public List<Skill> skills;

    void Awake () {
        if (instance == null) {
            instance = this;
        } else {
            Destroy (this);
            return;
        }
    }

    // Use this for initialization
    void Start () {

    }

    void AddSkill(Skill skill) {
        skills.Add (skill);
    }

    // Update is called once per frame
    void Update () {

    }
}
