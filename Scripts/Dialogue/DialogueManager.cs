using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;
using System;

public class DialogueManager : MonoBehaviour {
    public TextMeshProUGUI namebox;
    public TextMeshProUGUI textbox;

    String[] expressions = {"neutral", "dead", "smug", "cry", "sad", "anger", "hurt", "determined"};
    private Queue<Tuple<string, string>> charas;
    private Queue<String> dialogue;

    void Start() {
        dialogue = new Queue<String>();
    }

    public void StartDialogue(DialogueExpression[] dialogues) {
        makeDialogues(dialogues);
        NextDialogue();
    }

    public void makeDialogues(DialogueExpression[] dialogues) {
        dialogue.Clear();
        
        foreach (DialogueExpression expr in dialogues) {
            foreach(string sentence in expr.sentences) {
                dialogue.Enqueue(sentence);
                charas.Enqueue(new Tuple<string, string>(expr.character, expr.spriteExpr));
            }
        }
    }


    public void NextDialogue() {
        if (dialogue.Count == 0) {
            // EndDialogue();
            return;
        }
        Tuple<string, string> chara = charas.Dequeue();
        namebox.text = chara.Item1;
        textbox.text = dialogue.Dequeue();
    }

    private void Update() {
        if (Input.GetKeyDown("enter")) {
            NextDialogue();
        }
        //foreach (TextBox txt in floatingTexts) txt.UpdateTextBox();
    }


}
