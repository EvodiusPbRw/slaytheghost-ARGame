using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerModel;
using static MonsterModel;

public abstract class AbstractDataBattle : MonoBehaviour
{
    public static Monster mob;
    public static GameObject monster;
    public static Animator MonsterAnimate;
    public static AnimationClip[] clips;

    public static bool isBattleDone = false;
    public static bool isEnteredCombat = false;
    public static bool isUltiDone = true;

    public static float maxPlayerIdle = 4f;
    public static float timerPlayerIdle = 0f;

    public static void MonsterHitState(string _state){        
        AbstractDataBattle.MonsterAnimate.SetTrigger(_state);
        AbstractDataBattle.timerPlayerIdle = 0;
        AbstractDataBattle.monster.transform.position = new Vector3(0,0,2);
        AbstractDataBattle.monster.transform.rotation = Quaternion.Euler(0f, -180f, 0);
    }

    public static float GetClipLength(string _state){
        float clipLength = 0;
        foreach(AnimationClip clip in clips)
        {
            if (clip.name == _state){

                clipLength = clip.length;
            }
        }

        return clipLength;
    }
}
