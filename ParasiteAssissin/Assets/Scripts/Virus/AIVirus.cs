using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIVirus : MonoBehaviour {
    //Percent how much the entity is infected
    public float infectPercent { get; private set; }
    //The bar to show how much the entity is infected
    public GameObject virusMeter;

    // Use this for initialization
    void Start () {
        infectPercent = 0.0f;
        //Adding this entity to the manager his list of infected entity's
        InfectManager.addEntity (this);
        infectEntity (0.15f);
        virusMeter = transform.GetChild (0).GetChild (0).gameObject;
    }

    // Update is called once per frame
    void Update () {
        
    }

    void infectEntity (float ia) {
        //Wanneer aangeroepen, total infect count up, this infectPercentage up
        infectPercent += ia;
        if(virusMeter != null) {
            //scaling the bar to show in the game how much he is infected
            virusMeter.transform.localScale += new Vector3 (infectPercent, 0, 0);
        }
    }

    //Getter for infectPercent
    public float getInfectPercent () {
        return infectPercent;
    }

    //void to set the alfa of the bar to let in show smooth when getting closer
    public void setAlfa(float a) {
        Color c = virusMeter.transform.parent.GetComponent<SpriteRenderer> ().color;
        //setting the alfa of the virus meter
        c.a = 1 - a;
        virusMeter.transform.parent.GetComponent<SpriteRenderer>().color = c;
    }
}