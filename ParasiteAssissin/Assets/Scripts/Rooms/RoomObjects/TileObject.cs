using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileObject : MonoBehaviour
{
    public Vector2Int positionInWorld;
    public Vector2Int positionInRoom;
    public Tile represents;
    SpriteRenderer sprRenderer;
    public int tileTier;

    GameObject tierVisualizer;
    TextMesh vizualizerMesh;

    public GameObject me;

    public bool walkable;

    public int x;
    public int y;

    // A* variables
    public int gCost;
    public int hCost;
    public int fCost;
    public Tile parent;
    public bool visited;


    public Vector2 GetPosition()
    {
        return positionInWorld;
    }

    // Use this for initialization
    void Start()
    {
        sprRenderer = GetComponent<SpriteRenderer>();
        sprRenderer.sprite = represents.texture;
        sprRenderer.sortingLayerName = "Default";
        sprRenderer.sortingOrder = -10;
        tierVisualizer = new GameObject();
        tierVisualizer.transform.parent = transform;
        tierVisualizer.transform.localPosition = new Vector3(-0.5f,0.5f);
        vizualizerMesh = tierVisualizer.AddComponent<TextMesh>();
        vizualizerMesh.text = tileTier.ToString();
        vizualizerMesh.color = Color.green;


        this.y = y;
        this.x = x;

        this.visited = false;
        this.walkable = true;

    }
    public void setTile(Tile t)
    {
        represents = t;
        sprRenderer.sprite = represents.texture;
    }

    public void incrementTier()
    {
        tileTier++;
    }

    public void decrementTier()
    {
        tileTier--;
    }

    // Update is called once per frame
    void Update()
    {
         vizualizerMesh.text = tileTier.ToString();
    }
}
