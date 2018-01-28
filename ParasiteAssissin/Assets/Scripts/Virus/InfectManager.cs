using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfectManager : MonoBehaviour
{
    static public InfectManager instance;
    //int die bijhoud hoeveel entity's zijn geinfect
    private int totalInfected;
    ArrayList entityViruslist;
    public Parasite player;
    //de minimale disctance waar hij de virusbar moet laten zien
    public float minDisplayDist;

    // Use this for initialization
    void Awake()
    {
        // Make a static  InfectManager, make sure there's no duplicate
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }
        // Inititalize int
        totalInfected = 0;
        // Initialize ArrayList
        instance.entityViruslist = new ArrayList();
    }

    private void Start()
    {
        // Get all the entities
        entityViruslist = new ArrayList(AiManager.instance._aiEntitys);
    }

    // Update is called once per frame
    void Update()
    {
        //for (int i = 0; i < entityViruslist.Count; i++) {
        //    //distance between player and entity
        //    float dis = Vector2.Distance (((AiEntity)entityViruslist[i]).gameObject.transform.position, player.transform.position);
        //    if (dis <= minDisplayDist) {
        //        //call for the function setAlfa and put the distance in it
        //        ((AiEntity)entityViruslist[i]).GetComponent<AIVirus> ().setAlfa (dis / 12);
        //    }
        //}
    }

    public static void addEntity(AIVirus e)
    {
        // Add a entity to the arraylist
        instance.entityViruslist.Add(e);
    }

    public static void updateInfected()
    {

    }
    public int getInfectedAmount()
    {
        //Initialize int
        int totalInfected = 0;
        // Get all the entities and check if it is infected
        foreach (AIVirus e in instance.entityViruslist)
        {
            if (e.getInfectPercent() > 0)
            {
                totalInfected += 1;
            }
        }
        // Return the total infected amount
        return totalInfected;
    }

    public int GetInfectedEntities(Vector2 pos, float range, int percent)
    {
        int amount = 0;
        foreach (AIVirus v in entityViruslist)
        {
            if (v.infectPercent <= percent)
                if (Vector2.Distance(v.transform.position, player.transform.position) <= range)
                {
                    amount++;
                }
        }
        return amount;
    }
}

