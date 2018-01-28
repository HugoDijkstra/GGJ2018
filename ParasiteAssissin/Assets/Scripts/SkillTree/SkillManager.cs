using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour {

    public static SkillManager instance;

    [SerializeField]
    private SkillConfig skillConfig;
    [SerializeField]
    private Transform buttonPrefab;
    [SerializeField]
    private Transform panel;
    [SerializeField]
    private Parasite p;

    private Animator anim;
    private List<Transform> buttons;
    private bool selectingSkill;
    public bool SelectingSkill
    {
        get { return selectingSkill; }
    }

    //private int level = 0;

    void Awake () {
        if (instance == null) {
            instance = this;
        } else {
            Destroy (this);
            return;
        }
        // Get the animator from the panel
        anim = panel.GetComponent<Animator>();
    }

    // Use this for initialization
    void Start () {
        // Initialize variables
        instance.selectingSkill = false;
        instance.buttons = new List<Transform>();

        PlayerLevelManager.instance.onLevelUp = instance.LevelUp;
    }

    private void Update()
    {
        if (instance.selectingSkill)
        {
            // Loop through the skills
            foreach (SkillConfig sc in instance.skillConfig.skillConfigs)
            {
                // If one of the skills has been upgraded, apply the upgrade to the parasite
                if (sc.skill.upgraded)
                {
                    instance.selectingSkill = false;
                    sc.skill.Apply(p);
                    // Remove the button listeners otherwise the user can still upgrade in the animation
                    RemoveAllButtonListeners();
                    // Set upgraded of the currently selected skill to false
                    sc.skill.upgraded = false;
                    // Set the next skillConfig
                    instance.skillConfig = sc;
                    // Play the animation out
                    anim.SetTrigger("OnOut");

                    //instance.LevelUp();
                }
            }
        }
    }

    private Button InstantiateButton(string skillName)
    {
        // InstantiatieButtons through a prefab
        Transform newButtonPrefab = Instantiate(buttonPrefab, panel.transform);
        Button b = newButtonPrefab.GetComponent<Button>();
        instance.buttons.Add(b.transform);
        // Set the the text of the button to be the name of the skill
        b.GetComponentInChildren<Text>().text = skillName;
        return b;
    }

    private void RemoveAllButtons()
    {
        // Remove all buttons from the panel
        for (int i=0;i<buttons.Count;i++)
        {
            Destroy(buttons[i].gameObject);
            buttons.RemoveAt(i);
            i--;
        }
    }

    private void RemoveAllButtonListeners()
    {
        // Remove all listenerers from the buttons
        for (int i=0;i<buttons.Count;i++)
        {
            buttons[i].gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
        }
    }

    public void LevelUp()
    {
        // Remvove the buttons before instating new ones
        instance.RemoveAllButtons();

        foreach (SkillConfig sc in instance.skillConfig.skillConfigs)
        {
            Button b = InstantiateButton(sc.skillName);
            // Set the listener of the button to call the upgrade function of the skill
            b.onClick.AddListener(sc.skill.Upgrade);
        }
        // Selecting skill is true
        instance.selectingSkill = true;
        // Play on in animation
        anim.SetTrigger("OnIn");
    }
}
