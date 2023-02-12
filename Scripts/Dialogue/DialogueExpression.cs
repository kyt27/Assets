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

    [TextArea(3, 10)]
    public string[] sentences;

    public DialogueExpression(string character, Sprite spriteExpr, string dialogue) {
        this.character = character;
        this.spriteExpr = spriteExpr;
    }
}
