using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class Tile : ScriptableObject
{
    public bool walkable;
    public Sprite texture;
    public delegate void OnCollision();
    public OnCollision onCollision;
}
