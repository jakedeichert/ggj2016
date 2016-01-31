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
            default:
                break;
        }
    }
}
