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
    public Image sprite;
    public Animator animator;

    private Queue<Tuple<string, Sprite>> charas;
    private Queue<String> dialogue;
    private bool hasFinished = false;
    private string toDisplay = "";

    void Start() {
        dialogue = new Queue<String>();
        charas = new Queue<Tuple<string, Sprite>>();
    }

    public void StartDialogue(DialogueExpression[] dialogues) {
        animator.SetBool("isOpen", true);
        makeDialogues(dialogues);
        NextDialogue();
    }

    public void makeDialogues(DialogueExpression[] dialogues) {
        dialogue.Clear();
        charas.Clear();
        
        foreach (DialogueExpression expr in dialogues) {
            foreach(string sentence in expr.sentences) {
                dialogue.Enqueue(sentence);
                charas.Enqueue(new Tuple<string, Sprite>(expr.character, expr.spriteExpr));
            }
        }
    }


    public void NextDialogue() {
        if (dialogue.Count == 0) {
            EndDialogue();
            return;
        }
        hasFinished = false;
        Tuple<string, Sprite> chara = charas.Dequeue();
        namebox.text = chara.Item1;
        sprite.sprite = chara.Item2;
        toDisplay = dialogue.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(toDisplay));
    }

    private void Update() {
        if (Input.GetKeyDown("r")) {
            if (!hasFinished) {
                StopAllCoroutines();
                textbox.text = toDisplay;
                hasFinished = true;
                return;
            }
            NextDialogue();
        }
        
        //foreach (TextBox txt in floatingTexts) txt.UpdateTextBox();
    }

    IEnumerator TypeSentence(string sentence) {
        textbox.text = "";
        foreach (char letter in sentence.ToCharArray()) {
            textbox.text += letter;
            yield return new WaitForSeconds(0.06f);
        }
        hasFinished = true;
    }

    public void EndDialogue() {
        animator.SetBool("isOpen", false);

    }

}
