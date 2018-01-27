using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    Vector2Int worldSize = new Vector2Int(50, 50);
    TileObject[][] FullWorld;
    [SerializeField]
    List<Tile> tiles;
    [SerializeField]
    List<Room> rooms;

    Room[][] roomByTile;

    [SerializeField]
    GameObject spawnPrefab;

    [SerializeField]
    Room lastroom;

    public static RoomManager instance;
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
        FullWorld = new TileObject[worldSize.y][];
        for (int y = 0; y < worldSize.y; y++)
        {
            FullWorld[y] = new TileObject[worldSize.x];
        }

        StartCoroutine(CreateWorld());
    }

    IEnumerator CreateWorld()
    {
        Room ogRoom = Instantiate(rooms[0]);
        SpawnRoom(0, 0, rooms[0]);
        Vector2Int roomPos = Vector2Int.zero;
        for (int i = 0; i < ogRoom.doors.Count; i++)
        {
            Room r = Instantiate(randomRoom());
            if (ogRoom.doors[i].y == ogRoom.size.y - 1)
            {
                print("Under");
                bool fittingAbove = checkFitting(ogRoom.doors[i].x, ogRoom.doors[i].y + 1 + r.size.y, r);
                print("Fitting Under " + fittingAbove);
                if (fittingAbove)
                {
                    SpawnRoom(ogRoom.doors[i].x, ogRoom.doors[i].y + 1, r);
                    lastroom = r;
                }
            }
            ogRoom.doors.Remove(ogRoom.doors[i]);
            i--;
        }
        yield return null;
    }

    Room randomRoom()
    {
        return rooms[Random.Range(0, rooms.Count)];
    }
    Vector2Int[] checkCompatibleDoor(Room a, Room b)
    {
        for (int i = 0; i < a.doors.Count; i++)
        {
            for (int j = 0; j < b.doors.Count; j++)
            {
                Vector2Int doorA = a.doors[i];
                Vector2Int doorB = b.doors[i];

                if (doorA.x == doorB.x)
                {
                    if (doorA.y == a.size.y - 1 && doorB.y == 0)
                    {
                        return new Vector2Int[] { doorA, doorB };
                    }
                }
                if (doorA.y == doorB.y)
                {
                    if (doorA.y == a.size.y - 1 && doorB.y == 0)
                    {
                        return new Vector2Int[] { doorA, doorB };
                    }
                }
            }
        }
        return new Vector2Int[0];
    }
    bool checkFitting(int x, int y, Room r)
    {
        if (x > 0 && x < worldSize.x + r.size.x && y > 0 && y < worldSize.y + r.size.y)
        {
            for (int _x = x; _x < x + r.size.x; _x++)
            {
                for (int _y = y; _y < y + r.size.y; _y++)
                {
                    if (FullWorld[y][x] != null)
                        return false;
                }
            }
        }
        else
        {
            return false;
        }
        return true;
    }

    void SpawnRoom(int x, int y, Room r)
    {
        for (int _x = 0; _x < r.size.x; _x++)
        {
            for (int _y = 0; _y < r.size.y; _y++)
            {
                //if(_x < 0)
                try
                {
                    TileObject spawnedTile = Instantiate(spawnPrefab, transform.position + new Vector3(x + _x, y + -_y), Quaternion.identity).GetComponent<TileObject>();
                    spawnedTile.represents = r.room[_y][_x];
                    spawnedTile.positionInWorld = new Vector2Int(_x + x, y + _y);
                    FullWorld[y + _y][_x + x] = spawnedTile;
                }
                catch (System.Exception e)
                {
                    print(e.ToString() + " | " + _x + " " + _y);
                }
            }
        }
    }

    IEnumerator CreateRoom()
    {
        yield return null;
    }
}
