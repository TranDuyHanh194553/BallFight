using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Common
{
    public const string targetTriggerTag = "TargetTrigger";
    public const string ballTag = "Ball";


    public const int AVAIABLE_BALL = 10;


    // Timer 120 seconds
    public const int TIME_COUNT = 120;

    //Limit stage of level
    public const int LIMIT_STAGE = 5;
    public const int LIMIT_LEVEL = 2;
    public const string UNLIMIT_AVAIABLE = "∞";

    public enum SoundState
    {
        ShootBall,
        HitTarget,
        HitBarrier,
        GameOver,
        EffectNext,
        GetCoin,
        ButtonClick,
    }
}