using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiTile : MonoBehaviour
{
    public Tile represents;

    private void Start()
    {
        GetComponent<Image>().sprite = represents.texture;
        GetComponent<Button>().onClick.AddListener(setActiveTileToThis);
    }
    public void setActiveTileToThis()
    {
        RoomEditor.instance.setActiveTile(represents);
    }
}
