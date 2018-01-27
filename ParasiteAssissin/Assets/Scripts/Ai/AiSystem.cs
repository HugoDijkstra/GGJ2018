using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiSystem : MonoBehaviour
{

    private TileObject[][] _grid;

    private int width, height;

    void Start()
    {
        width = RoomManager.instance.worldSize.x;
        height = RoomManager.instance.worldSize.y;

        _grid = RoomManager.instance.FullWorld;
    }

    public Queue<TileObject> CalulatePathTo(Vector2 s, Vector2 e)
    {
        return this.ArraylistToQueue(this.CalculatePath(s, e));
    }

    public Queue<TileObject> CalulatePathToByTier(Vector2 s, Vector2 e, int tier)
    {
        if (tier <= _grid[(int)e.y][(int)e.x].tileTier)
        {
            return this.ArraylistToQueue(this.CalculatePath(s, e));
        }
        else
        {
            return new Queue<TileObject>();
        }
    }

    public Queue<TileObject> CalculateRandomPath(Vector2 s)
    { // <-- calculate random path
        this.reset();

        ArrayList walkable = new ArrayList();
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (_grid[y][x].walkable)
                {
                    walkable.Add(_grid[y][x]);
                }
            }
        }

        TileObject t = walkable[UnityEngine.Random.Range(0, walkable.Count)] as TileObject;

        return this.ArraylistToQueue(this.CalculatePath(s, new Vector2(t.x, t.y)));
    }

    public Queue<TileObject> CalculateRandomPathByTier(Vector2 s, int tier)
    { // <-- calculate random path
        this.reset();

        ArrayList walkable = GetTilesByTier(tier);

        // TODO check for no tiles then destroy

        TileObject t = walkable[UnityEngine.Random.Range(0, walkable.Count)] as TileObject;

        return this.ArraylistToQueue(this.CalculatePath(s, new Vector2(t.x, t.y)));
    }

    public ArrayList GetTilesByTier(int tier)
    {
        ArrayList list = new ArrayList();

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (_grid[y][x].walkable && _grid[y][x].tileTier <= tier)
                {
                    list.Add(_grid[y][x]);
                }
            }
        }

        return list;
    }
    public ArrayList GetTilesByTierHigher(int tier)
    {
        ArrayList list = new ArrayList();

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (_grid[y][x].walkable && _grid[y][x].tileTier > tier)
                {
                    list.Add(_grid[y][x]);
                }
            }
        }

        return list;
    }
    private ArrayList getNeighbour(TileObject tile)
    { // <<-- get the neighbours of the tile
        ArrayList neighbour = new ArrayList();

        for (int y = -1; y < 2; y++)
        {
            for (int x = -1; x < 2; x++)
            {
                int _y = tile.y + y;
                int _x = tile.x + x;

                if (_y < 0 || _y > height - 1 || _x < 0 || _x > width - 1 || x == 0 && y == 0)
                {
                    continue;
                }

                neighbour.Add(_grid[_y][_x]);
            }
        }

        return neighbour;
    }

    private int GetDistance(TileObject tileA, TileObject tileB)
    { // <-- get the distance
        int x = tileA.x - tileB.x;
        int y = tileA.y - tileB.y;

        if (x < 0)
            x *= -1;

        if (y < 0)
            y *= -1;

        return 14 * y + 10 * (x - y);
    }

    private void reset()
    { // <-- reset grid
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                _grid[y][x].visited = false;
                _grid[y][x].parent = null;
                _grid[y][x].gCost = 0;
                _grid[y][x].hCost = 0;
                _grid[y][x].fCost = 0;
            }
        }
    }

    public Queue<TileObject> ArraylistToQueue(ArrayList list)
    {
        Queue<TileObject> queue = new Queue<TileObject>();
        for (int i = 0; i < list.Count; i++)
        {
            queue.Enqueue(list[i] as TileObject);
        }
        return queue;
    }

    public ArrayList CalculatePath(Vector2 s, Vector2 e)
    { // <-- A*
        TileObject start = _grid[(int)s.y][(int)s.x];
        TileObject end = _grid[(int)e.y][(int)e.x];

        ArrayList open = new ArrayList();
        ArrayList closed = new ArrayList();

        open.Add(start);
        while (open.Count > 0)
        {
            // get the best tile you can chose
            TileObject currentTile = null;
            float fCost = Mathf.Infinity;
            foreach (TileObject t in open)
            {
                if (t.fCost <= fCost)
                {
                    fCost = t.fCost;
                    currentTile = t;
                }
            }

            currentTile.visited = true;

            // remove current tile from open and add it to closed
            open.Remove(currentTile);
            closed.Add(currentTile);

            // check if you are at the end
            if (currentTile.x == end.x && currentTile.y == end.y)
            {
                break;
            }

            // search new tiles
            foreach (TileObject neighbour in getNeighbour(currentTile))
            {

                if (neighbour.visited || !neighbour.walkable)
                {
                    continue;
                }

                int costToNeighbour = currentTile.gCost + GetDistance(currentTile, neighbour);
                if (currentTile.gCost < costToNeighbour || !open.Contains(neighbour))
                {
                    neighbour.gCost = costToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, end);
                    neighbour.fCost = costToNeighbour + GetDistance(neighbour, end);
                    neighbour.parent = currentTile;

                    if (!open.Contains(neighbour))
                    {
                        open.Add(neighbour);
                    }
                }


            }
        }

        // create path
        ArrayList path = new ArrayList();
        TileObject currentNode = end;

        while (currentNode != start)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();

        return path;
    }
}

//public class TileObject {

//    public GameObject me;

//    public bool walkable;
//    public int tileTier;

//    public int x;
//    public int y;

//    // A* variables
//    public int gCost;
//    public int hCost;
//    public int fCost;
//    public TileObject parent;
//    public bool visited;
//    public AiEntity heading;

//    public TileObject(int y, int x) {
//        this.y = y;
//        this.x = x;

//        this.visited = false;
//        this.walkable = true;
//        this.tileTier = 0;
//    }

//    public bool isMe(TileObject t) {
//        if (t.x == x && t.y == y) {
//            return true;
//        }
//        return false;
//    }

//    public Vector2 GetPosition() {
//        return new Vector2(x,y);
//    }
//}
