using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Entity", menuName = "AI/Entity")]
public class Entity : ScriptableObject  {
    [Range(1,4)]
    [SerializeField]
    private int tier;

    [SerializeField]
    private float speed;

    [SerializeField]
    private bool isDoctor;
    public int minVindValue;
    public int searchRange;
    public int hasToFind;

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
