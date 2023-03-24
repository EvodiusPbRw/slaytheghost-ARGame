using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerModel;
using static QuestModel;
using static ItemModel;
using static CheckpointData;
using static Notification;
using static UserSession;

public abstract class AbstractPlayerData : MonoBehaviour
{
    public static string username;
    public static PlayerModel playermodel;
    public static QuestModel questmodel;
    public static Player player;
    public static CheckpointData checkpoint = new CheckpointData();
    public static UserSession userSession = new UserSession();
    public static List<MainQuest> mainQuest = new List<MainQuest>();
    public static List<SideQuest> sideQuest = new List<SideQuest>();
    public static List<Item> items = new List<Item>();
    public static Sprite itemicon;
    public static List<Sprite> skillicons = new List<Sprite>();
    public static List<Sprite> bursticons = new List<Sprite>();

    public static List<Notification> notification = new List<Notification>();

    public static Sprite GetSkillUsedSprite() {
        foreach(Sprite sprite in skillicons)
        {
            for(int i = 0;i < AbstractPlayerData.player.Talent.Skill.Count;i++)
            {
                if(sprite.name == AbstractPlayerData.player.Talent.Skill[i].SourceIcon && AbstractPlayerData.player.Talent.Skill[i].IsUsed == true)
                {
                    return sprite;
                }
            }
        }

        return null;
    }

    public static Sprite GetBurstUsedSprite() {
        foreach(Sprite sprite in bursticons)
        {
            for(int i = 0; i < AbstractPlayerData.player.Talent.Burst.Count;i++)
            {
                if(sprite.name == AbstractPlayerData.player.Talent.Burst[i].SourceIcon && AbstractPlayerData.player.Talent.Burst[i].IsUsed == true)
                {
                    return sprite;
                }
            }
        }
        return null;
    }

    public static Skill GetSkillUsed() {
        for(int i = 0; i < AbstractPlayerData.player.Talent.Skill.Count;i++)
        {
            if (AbstractPlayerData.player.Talent.Skill[i].IsUsed == true)
            {
                return AbstractPlayerData.player.Talent.Skill[i];
            }
        }
        return null;
    } 

    public static Burst GetBurstUsed() {
        for(int i = 0; i < AbstractPlayerData.player.Talent.Burst.Count;i++)
        {
            if (AbstractPlayerData.player.Talent.Burst[i].IsUsed == true)
            {
                return AbstractPlayerData.player.Talent.Burst[i];
            }
        }
        return null;
    }

    public static MainQuest GetCurrentMainQuest(){
        foreach(MainQuest mq in AbstractPlayerData.mainQuest) {
            if(mq.IsFinish == false) 
            {
                return mq;       
            }
        }
        return null;
    }

    public static SideQuest GetCurrentSecretQuest()
    {
        foreach(SideQuest sq in AbstractPlayerData.sideQuest){
            if(sq.IsFinish == false)
            {
                return sq;
            }
        }

        return null;
    }

    public static Item GetItemUsed() {
        for(int i = 0; i < AbstractPlayerData.player.Slots.Count;i++)
        {
            if (AbstractPlayerData.player.Slots[i].IsUsed == true)
            {
                foreach(Item item in items)
                {
                    if(item.ItemID == AbstractPlayerData.player.Slots[i].ItemID)
                    {
                        return item;
                    }
                }
            }
        }

        return null;
    }

    public static int GetQtyItemUsed()
    {
        for(int i = 0; i < AbstractPlayerData.player.Slots.Count;i++)
        {
            if (AbstractPlayerData.player.Slots[i].IsUsed == true)
            {
                return AbstractPlayerData.player.Slots[i].Quantity;
            }
        }

        return 0;
    }

    public static void LevelUp()
    {
        if(AbstractPlayerData.player.Level + 1 <= 5)
        {
            AbstractPlayerData.player.Status.ExperiencePoint -= AbstractPlayerData.player.Status.ExperienceCap;
            AbstractPlayerData.player.Level += 1;

            AbstractPlayerData.player.Status.MaxHealthPoint *= 1.5f ;
            AbstractPlayerData.player.Status.MaxHealthPoint = (double)(Mathf.Ceil((float)AbstractPlayerData.player.Status.MaxHealthPoint));
            AbstractPlayerData.player.Status.HealthPoint = AbstractPlayerData.player.Status.MaxHealthPoint;
            AbstractPlayerData.player.Status.MaxChargePoint *= 1.5f ;
            AbstractPlayerData.player.Status.MaxChargePoint = (double)(Mathf.Ceil((float)AbstractPlayerData.player.Status.MaxChargePoint));
            AbstractPlayerData.player.Status.ChargePoint = AbstractPlayerData.player.Status.MaxChargePoint;
            AbstractPlayerData.player.Status.BaseRechargePoint *= 1.5f ;
            AbstractPlayerData.player.Status.BaseRechargePoint = (double)(Mathf.Ceil((float)AbstractPlayerData.player.Status.BaseRechargePoint));
            AbstractPlayerData.player.Status.BaseAttack *= 1.5f ;
            AbstractPlayerData.player.Status.BaseAttack = (double)(Mathf.Ceil((float)AbstractPlayerData.player.Status.BaseAttack));
            AbstractPlayerData.player.Status.BaseDefense *= 1.5f ;
            AbstractPlayerData.player.Status.BaseDefense = (double)(Mathf.Ceil((float)AbstractPlayerData.player.Status.BaseDefense));
        }  
        
    }
}
