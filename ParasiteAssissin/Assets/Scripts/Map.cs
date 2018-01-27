using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map
{
    Vector2Int size;
    Tile[] tiles;
    Tile[][] map;
    int[] savedTileTiers;
    int[][] tileTiers;
    public Map()
    {
        int index = 0;
        map = new Tile[size.y][];
        for (int x = 0; x < size.y; x++)
        {
            map[x] = new Tile[size.x];
            for (int y = 0; y < size.x; y++)
            {
                map[x][y] = tiles[index];
                index++;
            }
        }
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

    public void setup()
    {
        int index = 0;
       // for(int i = 0; i < )
    }
}
