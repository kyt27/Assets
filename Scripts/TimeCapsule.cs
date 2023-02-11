using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeCapsule : Collectable {
    public Sprite openedTimeCapsule;

    protected override void OnCollect() {
        if (!collected) {
            collected = true;
            GetComponent<SpriteRenderer>().sprite = openedTimeCapsule;
            GameManager.instance.showInstall.SetBool("Show", true);
        }
    }
}
