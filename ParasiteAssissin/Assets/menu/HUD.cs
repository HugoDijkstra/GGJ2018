using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {
    public GameObject awareness, infected, tier;
    public Text textAwareness, textInfected, textTier;

    public Parasite p;
    

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //textAwareness.text = ("awareness: " + awareness.GetComponent<Cheekidemuderfkingbreeki>().awareness);
        //textInfected.text = ("infected: " + infected.GetComponent<Cheekidemuderfkingbreeki>().infected);
        if (p.stuckOn == null)
        {
            textTier.text = ("Tier: empty");
        }
        else {
            
            textTier.text = ("Tier: " + p.stuckOn.GetComponent<AiEntity>().getEntityInfo().Tier);
        }
        

    }
}
