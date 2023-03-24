using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notification
{
    public string NotifText {get; set;}
    public bool IsRead { get; set;}

    public Notification (string NotifText, bool IsRead)
    {
        this.NotifText = NotifText;
        this.IsRead = IsRead;
    }
}
