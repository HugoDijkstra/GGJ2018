using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : ScriptableObject {

    public bool upgraded = false;

    public abstract void Apply(Parasite p);

    private void Awake()
    {
        upgraded = false;
    }

    public void Upgrade()
    {
        upgraded = true;
    }
}
