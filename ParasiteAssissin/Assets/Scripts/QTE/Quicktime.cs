using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Quicktime : MonoBehaviour
{
    static public Quicktime instance;

    [SerializeField]
    Sprite[] quicktimeButtons;
    KeyCode[] quickTimeButtonCodes;
    [SerializeField]
    private int tier;
    [SerializeField]
    bool startSequence;
    int currentKey;
    float timer;
    public float maxTime;
    bool death;

    // Use this for initialization
    void Start()
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
        timer = 0.0f;
        tier = 0;
        startSequence = false;
        quickTimeButtonCodes = new KeyCode[4] { KeyCode.W, KeyCode.S, KeyCode.D, KeyCode.A };

    }

    // Update is called once per frame
    void Update()
    {
        if (startSequence)
        {
            if (Input.GetKeyDown(quickTimeButtonCodes[0]) || Input.GetKeyDown(quickTimeButtonCodes[1]) || Input.GetKeyDown(quickTimeButtonCodes[2]) || Input.GetKeyDown(quickTimeButtonCodes[3]))
            {
                if (Input.GetKeyDown(quickTimeButtonCodes[currentKey]))
                {
                    startSequence = false;
                }
                else
                {
                    death = true;
                }
            }
        }
    }

    static public bool isDeath()
    {
        return instance.death;
    }

    static public bool GetQuicktimeSucces(float ti)
    {

        instance.gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<Image>().fillAmount = 1f - (ti / instance.maxTime);
        if (instance.tier != 0 && !instance.startSequence)
        {
            return true;
        }
        return false;
    }

    static public void EnableQuicktime(int t, int ic)
    {
        instance.currentKey = Random.Range(0, 4);
        instance.gameObject.transform.GetChild(0).GetComponent<Image>().sprite = instance.quicktimeButtons[instance.currentKey];
        instance.gameObject.transform.GetChild(0).transform.position = new Vector3(Random.Range(instance.gameObject.transform.GetChild(0).GetComponent<RectTransform>().rect.width, Screen.width - instance.gameObject.transform.GetChild(0).GetComponent<RectTransform>().rect.width), Random.Range(instance.gameObject.transform.GetChild(0).GetComponent<RectTransform>().rect.height, Screen.height - instance.gameObject.transform.GetChild(0).GetComponent<RectTransform>().rect.height), 0);
        instance.startSequence = true;
        instance.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        instance.timer = 0.0f;
        instance.tier = t;
        instance.maxTime = (1.0f / (float)instance.tier) + (0.3f * (float)ic);
    }

    static public void DisableQuicktime()
    {
        instance.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        instance.startSequence = false;
    }
}
