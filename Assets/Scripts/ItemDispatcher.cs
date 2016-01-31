using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemDispatcher : MonoBehaviour {
    private HittableBehaviour hitBehav;
    public int CallItemAction(int id, Transform callee) {
        int itemSuccess = 0;
        switch (id) {
            case 0:
                hitBehav = callee.GetComponent<HittableBehaviour>();
                if (hitBehav == null) {
                    Debug.LogWarning("Callee does not contain a HittableBehaviour.");
                    break;
                }
                if (hitBehav.health < hitBehav.maxHealth) {
                    hitBehav.AddHealth(10);
                    itemSuccess = 1;
                }
                break;
            case 1:
                hitBehav = callee.GetComponent<HittableBehaviour>();
                if (hitBehav == null) {
                    Debug.LogWarning("Callee does not contain a HittableBehaviour.");
                    break;
                }
                if (hitBehav.health < hitBehav.maxHealth) {
                    hitBehav.AddHealth(50);
                    itemSuccess = 1;
                }
                break;
            case 2:
                BuffInfo speedBuff;
                speedBuff.buffType = "Speed";
                speedBuff.buffAmount = 5;
                callee.SendMessage("ApplyBuff", speedBuff);
                itemSuccess = 1;
                break;
            case 3:
                BuffInfo healthBuff;
                healthBuff.buffType = "Health";
                healthBuff.buffAmount = 10;
                callee.SendMessage("ApplyBuff", healthBuff);
                itemSuccess = 1;
                break;
            case 4:
                BuffInfo attackBuff;
                attackBuff.buffType = "Attack";
                attackBuff.buffAmount = 1;
                callee.SendMessage("ApplyBuff", attackBuff);
                itemSuccess = 1;
                break;
            default:
                break;
        }
        return itemSuccess;
    }
}
