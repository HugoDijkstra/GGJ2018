using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevelManager : MonoBehaviour
{
    public static PlayerLevelManager instance;

    float exp = 0;
    int playerLevel = 0;

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
        if (exp >= 200 + (50 * playerLevel * 1.4f))
        {
            playerLevel++;
            onLevelUp.Invoke();
        }
    }
}
