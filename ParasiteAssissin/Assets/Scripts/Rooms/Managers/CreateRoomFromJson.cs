using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateRoomFromJson : MonoBehaviour
{
    [SerializeField]
    string filePath;

    [SerializeField]
    Room room;
    private void Start()
    {
        try
        {
            room = JsonUtility.FromJson<Room>(File.ReadAllText(filePath));
        }
        catch (System.Exception e)
        {
            print(e.ToString());
        }
    }
}
