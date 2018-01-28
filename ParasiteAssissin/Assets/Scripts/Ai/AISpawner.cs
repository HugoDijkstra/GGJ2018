using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject aiEntity;
    ArrayList usedTiles;
    [SerializeField]
    GameObject[] enemyTiers;
    int[] enemySpawns;
    bool hasgonethrough;

    // Use this for initialization
    void Start()
    {
        hasgonethrough = false;
        usedTiles = new ArrayList();
        enemySpawns = new int[] { 3, 2, 1 };
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasgonethrough)
        {
            hasgonethrough = true;
            for (int k = 0; k < enemySpawns.Length; k++)
            {
                int rand = Random.Range(0, enemySpawns[k]);
                for (int i = 0; i < rand; i++)
                {
                    ArrayList spawnableTiles = AiManager.instance._aiSystem.GetTilesByTier(k + 1);
                    int randomTile = Random.Range(0, spawnableTiles.Count);
                    TileObject targetPosition = (TileObject)spawnableTiles[randomTile];
                    for (int j = 0; j < usedTiles.Count; j++)
                    {
                        if (targetPosition.GetPosition() == (Vector2)usedTiles[j])
                        {
                            randomTile = Random.Range(0, spawnableTiles.Count);
                            targetPosition = (TileObject)spawnableTiles[randomTile];
                            j = 0;
                        }
                    }
                    GameObject newEntity = Instantiate(enemyTiers[k], targetPosition.GetPosition(), Quaternion.identity);
                    AiManager.AddAiEntity(newEntity.GetComponent<AiEntity>());
                    AiEntity ae = newEntity.GetComponent<AiEntity>();
                    ae.SetCurrentTile(targetPosition);
                    ae.SetPath(AiManager.instance._aiSystem.CalculateRandomPathByTier(targetPosition.GetPosition(), k + 1));
                    usedTiles.Add(targetPosition.GetPosition());
                }
            }
        }
    }

}
