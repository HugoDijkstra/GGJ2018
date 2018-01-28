using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevelManager : MonoBehaviour
{
    public static PlayerLevelManager instance;

    public float exp;
    public int playerLevel;

    public delegate void OnPlayerLevelUp();
    public delegate void DoneLeveling();
    public OnPlayerLevelUp onLevelUp;
    public DoneLeveling doneLeveling;

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

        onLevelUp += () => print("Level up");
    }

    public void addExp(float xp)
    {
        exp += xp;
    }

    // Update is called once per frame
    void Update()
    {
        if (exp >= 200 + (50 * playerLevel))
        {
            playerLevel++;
            onLevelUp.Invoke();
        }
    }
}
