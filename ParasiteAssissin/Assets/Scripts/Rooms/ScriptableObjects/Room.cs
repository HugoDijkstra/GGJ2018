using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu()]
public class Room
{
    public string name;
    public Vector2Int size;
    [SerializeField]
    public Tile[][] room;
    public int[][] tileTiers;
    public int[] savedTileTiers;
    public Tile[] tiles;

    public List<Vector2Int> doors;
    private Room()
    {
        if (tiles == null || tiles.Length != size.x * size.y)
        {
            tiles = new Tile[size.x * size.y];
            room = new Tile[size.y][];
            int index = 0;
            for (int i = 0; i < room.Length; i++)
            {
                room[i] = new Tile[size.x];
                for (int j = 0; j < room[i].Length; j++)
                {
                    Debug.Log(((Tile)Resources.Load("TileInstances/PlaceHolder")).name);
                    room[i][j] = Resources.Load("TileInstances/PlaceHolder") as Tile;
                    tiles[index] = room[i][j];
                    index++;
                }
            }
        }
        else
        {
            int index = 0;
            room = new Tile[size.y][];
            for (int x = 0; x < size.y; x++)
            {
                room[x] = new Tile[size.x];
                for (int y = 0; y < size.x; y++)
                {
                    room[x][y] = tiles[index];
                    if (tiles[index].name.Contains("Door"))
                        doors.Add(new Vector2Int(y, x));
                    index++;
                }
            }
        }
        if (savedTileTiers == null || savedTileTiers.Length != size.x * size.y)
        {
            savedTileTiers = new int[size.y * size.x];
            tileTiers = new int[size.y][];
            for (int x = 0; x < size.y; x++)
            {
                tileTiers[x] = new int[size.x];
                for (int y = 0; y < size.x; y++)
                {
                    tileTiers[x][y] = 1;
                }
            }
        }
        else
        {
            int index = 0;
            tileTiers = new int[size.y][];
            for (int x = 0; x < size.y; x++)
            {
                tileTiers[x] = new int[size.x];
                for (int y = 0; y < size.x; y++)
                {
                    tileTiers[x][y] = savedTileTiers[index];
                    index++;
                }
            }
        }
    }

    ~Room()
    {
        int index = 0;
        if (room != null)
            for (int i = 0; i < room.Length; i++)
            {
                for (int j = 0; j < room[i].Length; j++)
                {
                    tiles[index] = room[i][j];
                    index++;
                }
            }
        index = 0;
        for (int i = 0; i < tileTiers.Length; i++)
        {
            for (int j = 0; j < tileTiers[i].Length; j++)
            {
                savedTileTiers[index] = tileTiers[i][j];
                index++;
            }
        }
    }

    public void changeSize(Vector2Int newSize)
    {
        size = newSize;
        int index = 0;
        tiles = new Tile[size.x * size.y];
        room = new Tile[(int)size.y][];
        for (int i = 0; i < room.Length; i++)
        {
            room[i] = new Tile[size.x];
            for (int j = 0; j < room[i].Length; j++)
            {
                Debug.Log(((Tile)Resources.Load("TileInstances/PlaceHolder")).name);
                room[i][j] = Resources.Load("TileInstances/PlaceHolder") as Tile;
                tiles[index] = room[i][j];
                index++;
            }
        }
    }
}
