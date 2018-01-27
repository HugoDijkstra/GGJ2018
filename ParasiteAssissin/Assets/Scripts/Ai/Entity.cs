using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Entity", menuName = "AI/Entity")]
public class Entity : ScriptableObject  {
    [SerializeField]
    private int tier;

    [SerializeField]
    private bool isDoctor;

    [SerializeField]
    private float speed;

    public Color color;

    public int Tier
    {
        get
        {
            return tier;
        }
    }
    public bool IsDoctor
    {
        get
        {
            return isDoctor;
        }
    }
    public float Speed
    {
        get
        {
            return speed;
        }
    }
}
