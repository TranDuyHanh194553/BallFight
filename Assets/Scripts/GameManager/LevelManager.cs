using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public LevelData[] levels;

    public LevelData GetLevelData(int levelID)
    {
        foreach(LevelData levelData in levels)
        {
            if(levelData.levelID == levelID)
            {
                return levelData;
            }
        }

        return null;
    }


    public StageData GetStageData(LevelData levelData, int stageID)
    {
        if (levelData != null && levelData.stages != null)
        {
            foreach (StageData stageData in levelData.stages)
            {
                if (stageData.stageID == stageID)
                {
                    return stageData;
                }
            }
        }
        return null;
    }
}
