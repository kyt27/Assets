using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueExpression
{
    // Start is called before the first frame update
    public string character;
    public string spriteExpr;

    [TextArea(3, 10)]
    public string[] sentences;

    public DialogueExpression(string character, string spriteExpr, string dialogue) {
        this.character = character;
        this.spriteExpr = spriteExpr;
    }
}
