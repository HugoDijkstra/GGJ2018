using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{

    public bool mouseOver;
    // Use this for initialization
    void Start()
    {
        mouseOver = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseEnter()
    {
        Debug.Log("I am over something");
        mouseOver = true;
    }

    void OnMouseExit()
    {
        mouseOver = false;
    }
}
