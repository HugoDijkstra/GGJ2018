using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : ScriptableObject {

    [SerializeField]
    protected int level = 1;

    public abstract void Apply (Parasite p);
    public void Upgrade () { level++; }
}
