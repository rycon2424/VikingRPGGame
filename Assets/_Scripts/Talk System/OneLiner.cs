using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneLiner : TalkAble
{
    public override void Talk()
    {
        base.Talk();
        Debug.Log("Say one Liner");
    }
}
