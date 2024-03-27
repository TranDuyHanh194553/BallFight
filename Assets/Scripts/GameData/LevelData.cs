using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObject/LevelData")]
public class LevelData : ScriptableObject
{
    public int levelID;
    public string levelName;
    public StageData[] stages;
}
