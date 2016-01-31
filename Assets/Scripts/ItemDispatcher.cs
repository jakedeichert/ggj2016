using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemDispatcher : MonoBehaviour {
    public void CallItemAction(int id, Transform callee) {
        switch (id) {
            case 0:
                callee.SendMessage("AddHealth", 10);
                break;
            case 1:
                callee.SendMessage("AddHealth", 50);
                break;
            case 2:
                BuffInfo speedBuff;
                speedBuff.buffType = "Speed";
                speedBuff.buffAmount = 5;
                callee.SendMessage("ApplyBuff", speedBuff);
                break;
            default:
                break;
        }
    }
}
