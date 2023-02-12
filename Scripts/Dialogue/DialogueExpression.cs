using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class DialogueExpression
{
    // Start is called before the first frame update
    public string character;
    public Sprite spriteExpr;
    public AudioSource textsound;

    [TextArea(3, 10)]
    public string[] sentences;

    public DialogueExpression(string character, Sprite spriteExpr, AudioSource textsound, string dialogue) {
        this.character = character;
        this.spriteExpr = spriteExpr;
        this.textsound = textsound;
    }
}
