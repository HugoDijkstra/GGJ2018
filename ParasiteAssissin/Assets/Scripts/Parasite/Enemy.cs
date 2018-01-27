using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public bool visited;
    public AiEntity ai;
    public bool mouseOver;

    private void Awake()
    {
        ai = GetComponent<AiEntity>();
    }
    // Use this for initialization
    void Start()
    {
        mouseOver = false;
        PlayerLevelManager.instance.onLevelUp += () => disableAi();
        PlayerLevelManager.instance.doneLeveling += () => enableAi();
    }

    private void OnDestroy()
    {
        PlayerLevelManager.instance.onLevelUp -= () => disableAi();
        PlayerLevelManager.instance.doneLeveling -= () => enableAi();
    }

    private void enableAi()
    {
        ai.enabled = true;
    }

    private void disableAi()
    {
        ai.enabled = false;
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
