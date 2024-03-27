using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "StageData", menuName = "ScriptableObject/StageData")]
public class StageData : ScriptableObject
{
    public int stageID;
    public string stageName;
    public int stageTime;
    public string balls;
    public int targets;
    public int enemyRobots;
    public int bonusCoins;
    public int objects;
    public int reward;
}
