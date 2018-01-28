using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class QuickMenu : MonoBehaviour {

    [SerializeField] private GameObject quickMenu;

    // Use this for initialization
    void Start () {
        Time.timeScale = 1.0f;
	}
	
	// Update is called once per frame
	void Update () {
    }

    void OnGUI() {
        Event e = Event.current;
        if (e.isKey && e.keyCode == KeyCode.Escape) {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                Debug.Log("Detected key code: " + e.keyCode);
                toggle();
            }
            
        }
    }

    public void gotoMain() {
        SceneManager.LoadScene(0);
    }

    public void toggle() {
        quickMenu.gameObject.SetActive(!quickMenu.gameObject.activeSelf);
        if (quickMenu.gameObject.activeSelf) {
            Time.timeScale = 0.0f;
        }
        else {
            Time.timeScale = 1.0f;
        }
    }
}
////