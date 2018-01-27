using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomFromJson : MonoBehaviour {

    [SerializeField]
    string filePath;
    [SerializeField]
    Room r;
	// Use this for initialization
	void Start () {
        JsonUtility.FromJsonOverwrite(File.ReadAllText(filePath),r);
	}
}
