using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMovement : MonoBehaviour
{
    void Start(){
        AbstractDataBattle.MonsterAnimate = AbstractDataBattle.monster.GetComponent<Animator>() as Animator;
        AbstractDataBattle.MonsterAnimate.SetTrigger("Idle");
        AbstractDataBattle.clips = AbstractDataBattle.MonsterAnimate.runtimeAnimatorController.animationClips;

        if(AbstractDataBattle.mob.hp<AbstractDataBattle.mob.max_hp) AbstractDataBattle.MonsterAnimate.SetTrigger("IdleCombat");
    }

    
    private void Update()
    {
        if (AbstractDataBattle.mob.hp<AbstractDataBattle.mob.max_hp && AbstractDataBattle.isEnteredCombat == false && AbstractDataBattle.isBattleDone == false)
        {
            AbstractDataBattle.MonsterAnimate.SetTrigger("IdleCombat");
            AbstractDataBattle.isEnteredCombat = true;
        }

        if (AbstractDataBattle.isEnteredCombat == true && AbstractDataBattle.isBattleDone == false) {
            if(AbstractDataBattle.mob.energy == AbstractDataBattle.mob.energy_cap && AbstractDataBattle.isUltiDone == true)
                StartCoroutine(MonsterUlti());
            else 
                StartCoroutine(countdownMonsterAttack());
        }
    }

    IEnumerator MonsterUlti(){
        AbstractDataBattle.isUltiDone = false;
        AbstractDataBattle.MonsterAnimate.SetTrigger("Ulti");
        AbstractPlayerData.player.Status.HealthPoint -= (AbstractDataBattle.mob.attack * AbstractDataBattle.mob.ultimate);
        yield return new WaitForSecondsRealtime(AbstractDataBattle.GetClipLength("Ulti"));
        AbstractDataBattle.monster.transform.position = new Vector3(0,0,2);
        AbstractDataBattle.monster.transform.rotation = Quaternion.Euler(0f, -180f, 0);
        AbstractDataBattle.timerPlayerIdle = 0;
        AbstractDataBattle.mob.energy = 0;
        AbstractDataBattle.isUltiDone = true;
    }

    IEnumerator countdownMonsterAttack(){
        if (AbstractDataBattle.timerPlayerIdle >= AbstractDataBattle.maxPlayerIdle)
        {
            AbstractDataBattle.MonsterAnimate.SetTrigger("Punching");
            AbstractPlayerData.player.Status.HealthPoint -= AbstractDataBattle.mob.attack;
            AbstractDataBattle.monster.transform.position = new Vector3(0,0,2);
            AbstractDataBattle.monster.transform.rotation = Quaternion.Euler(0f, -180f, 0);
            AbstractDataBattle.timerPlayerIdle = 0;
        }
        AbstractDataBattle.timerPlayerIdle += Time.deltaTime;
        yield return null;
    }
}