using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScene : MonoBehaviour {
	// Use this for initialization
	void Start () {
        this.GetComponent<Button>().onClick.AddListener(loladScene);
    }

    // Update is called once per frame
    void Update () {
	}

    public void loladScene() {
        SceneManager.LoadScene(0);
    }
}
