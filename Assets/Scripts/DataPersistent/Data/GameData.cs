using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public SerializableDictionary<string, Vector3> objectPos;

	public GameData()
    {
        objectPos = new SerializableDictionary<string, Vector3>();
    }

}
