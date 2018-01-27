using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomEditor : MonoBehaviour
{
    public static RoomEditor instance;

    public Room currentRoom;
    private Room lastRoom;

    [SerializeField]
    GameObject tilePrefab;

    public TileObject[][] editing;

    [SerializeField]
    InputField sizeField;

    Tile activeTile;

    [SerializeField]
    List<Tile> tiles;

    [SerializeField]
    GameObject uiTileObject;
    GameObject uiTileBlock;
    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }
        uiTileBlock = GameObject.Find("Canvas").transform.Find("UiTileBlock").gameObject;
        activeTile = tiles[0];
        for (int i = 0; i < tiles.Count; i++)
        {
            GameObject newTile = Instantiate(uiTileObject, uiTileBlock.transform);
            newTile.transform.position = new Vector3(100 * i, 50);
            newTile.GetComponent<UiTile>().represents = tiles[i];
        }
    }

    void Update()
    {
        if (currentRoom != lastRoom)
        {
            SetupRoom(currentRoom);
        }
        lastRoom = currentRoom;
        Camera.main.transform.position += new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * Time.deltaTime * 4;
        uiTileBlock.transform.localPosition -= new Vector3(Input.GetAxisRaw("Mouse ScrollWheel"), 0) * 60f;
        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                print(hit.transform);
                TileObject t = hit.transform.GetComponent<TileObject>();
                if (t != null)
                {
                    t.incrementTier();
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                print(hit.transform);
                TileObject t = hit.transform.GetComponent<TileObject>();
                if (t != null)
                {
                    t.decrementTier();
                }
            }
        }
        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                TileObject t = hit.transform.GetComponent<TileObject>();
                if (t != null)
                {
                    t.setTile(activeTile);
                }
            }
        }
    }

    public void applyRoom()
    {
        for (int i = 0; i < currentRoom.room.Length; i++)
            for (int j = 0; j < currentRoom.room[i].Length; j++)
            {
                currentRoom.room[i][j] = editing[i][j].represents;
                currentRoom.tileTiers[i][j] = editing[i][j].tileTier;
            }
        if (!Directory.Exists("Rooms"))
            Directory.CreateDirectory("Rooms");
        File.WriteAllText("Rooms/" + currentRoom.name, JsonUtility.ToJson(currentRoom));
    }

    public void checkSize()
    {
        if (currentRoom == null)
        {
            Debug.LogWarning("No Room set");
            return;
        }
        Vector2Int parsed;
        if (ParseVector2(sizeField.text, ',', out parsed))
        {
            if (parsed != currentRoom.size)
            {
                currentRoom.changeSize(parsed);
                SetupRoom(currentRoom);
            }
        }
    }

    public void setTile(Vector2Int at, Tile t)
    {
        editing[at.y][at.x].represents = t;
    }


    public void setActiveTile(Tile t)
    {
        activeTile = t;
    }

    public static bool ParseVector2(string parse, char splitAt, out Vector2 Output)
    {
        string[] split = parse.Split(splitAt);
        float x, y;
        if (!float.TryParse(split[0], out x))
        {
            Output = Vector2.zero;
            return false;
        }
        if (!float.TryParse(split[1], out y))
        {
            Output = Vector2.zero;
            return false;
        }
        Output = new Vector2(x, y);
        return true;
    }

    public static bool ParseVector2(string parse, char splitAt, out Vector2Int Output)
    {
        string[] split = parse.Split(splitAt);
        int x, y;
        if (!int.TryParse(split[0], out x))
        {
            Output = Vector2Int.zero;
            return false;
        }
        if (!int.TryParse(split[1], out y))
        {
            Output = Vector2Int.zero;
            return false;
        }
        Output = new Vector2Int(x, y);
        return true;
    }

    void SetupRoom(Room room)
    {
        if (editing != null)
            for (int i = 0; i < editing.Length; i++)
            {
                for (int j = 0; j < editing[i].Length; j++)
                {
                    if (editing[i][j] != null)
                        Destroy(editing[i][j].gameObject);
                }
            }

        editing = new TileObject[(int)room.size.y][];
        for (int i = 0; i < editing.Length; i++)
        {
            editing[i] = new TileObject[(int)room.size.x];
        }

        for (int i = 0; i < editing.Length; i++)
        {
            for (int j = 0; j < editing[i].Length; j++)
            {
                GameObject newTile = Instantiate(tilePrefab, transform.position + new Vector3(j, -i), Quaternion.identity);
                TileObject tObj = newTile.GetComponent<TileObject>();
                tObj.positionInRoom = new Vector2Int(j, i);
                tObj.represents = currentRoom.room[i][j];
                tObj.tileTier = currentRoom.tileTiers[i][j];
                editing[i][j] = tObj;
            }
        }

    }
}
