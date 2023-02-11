using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterMenu : MonoBehaviour {
    //Text fields
    public TextMeshPro healthText;

    //Logic
    private int currentCharSelect = 0;
    public Image charSprite;

    //Char selection
    public void OnArrowClick(bool right) {
        int spriteCount = GameManager.instance.playerSprites.Count;
        if (right) {
            currentCharSelect = (++currentCharSelect + spriteCount) % spriteCount;
            OnSelectionChanged();
        } else {
            currentCharSelect = (--currentCharSelect + spriteCount) % spriteCount;
            OnSelectionChanged();
        }
    }

    private void OnSelectionChanged() {
        charSprite.sprite = GameManager.instance.playerSprites[currentCharSelect];
        GameManager.instance.player.SwapSprite(currentCharSelect);
    }

    //Update character info
    public void UpdateMenu() {
        // Stats/Metadata
        healthText.text = GameManager.instance.player.hitpoint.ToString() + " / " + GameManager.instance.player.maxHitpoint.ToString();
    }
}
