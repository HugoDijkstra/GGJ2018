using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButtons : MonoBehaviour {

    [SerializeField] private GameObject main;
    [SerializeField] private GameObject howTo;
    [SerializeField] private GameObject settings;
    [SerializeField] private GameObject credits;
    [SerializeField] private GameObject resolution;
    public Text currentRes;
    public Text currentQuality;
    bool isFullscreen;

    // Use this for initialization
    void Start () {
        isFullscreen = Screen.fullScreen;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void nextScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void gotoTab(GameObject target) {
        if (target == resolution) {
            currentRes.text = (Screen.width + "X" + Screen.height);
        }
        else if (target == settings) {
            setQuality(0);
        }

            main.gameObject.SetActive(false);
            howTo.gameObject.SetActive(false);
            settings.gameObject.SetActive(false);
            credits.gameObject.SetActive(false);
            resolution.gameObject.SetActive(false);
            target.gameObject.SetActive(true);
        }

    public void toggleFullscreen() {
        isFullscreen =! isFullscreen;
        Screen.fullScreen = isFullscreen;
    }

    public void setQuality(int i) {
        if (QualitySettings.GetQualityLevel() + i > 5 || QualitySettings.GetQualityLevel() + i < 0) {
            i = 0;
        }
        QualitySettings.SetQualityLevel(QualitySettings.GetQualityLevel() + i);

        switch (QualitySettings.GetQualityLevel()) {
            case 0:
                currentQuality.text = ("Very Low");
                break;
            case 1:
                currentQuality.text = ("Low");
                break;
            case 2:
                currentQuality.text = ("Medium");
                break;
            case 3:
                currentQuality.text = ("High");
                break;
            case 4:
                currentQuality.text = ("Very High");
                break;
            case 5:
                currentQuality.text = ("Ultra");
                break;

        }
    }

    public void setResolution(int i) {
        switch (i) {
            case 0:
                Screen.SetResolution(640, 480, isFullscreen);
                break;
            case 1:
                Screen.SetResolution(800, 600, isFullscreen);
                break;
            case 2:
                Screen.SetResolution(1280, 1024, isFullscreen);
                break;
            case 3:
                Screen.SetResolution(1360, 768, isFullscreen);
                break;
            case 4:
                Screen.SetResolution(1680, 1050, isFullscreen);
                break;
            case 5:
                Screen.SetResolution(1920, 1080, isFullscreen);
                break;
            case 6:
                Screen.SetResolution(1920, 1200, isFullscreen);
                break;
        }
        gotoTab(settings);
    }      


    public void quitGame() {
        Application.Quit();
    }
}
