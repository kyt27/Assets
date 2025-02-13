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
    public AudioSource m_MyAudioSource;

    private Queue<Tuple<string, Sprite>> charas;
    private Queue<String> dialogue;
    private Queue<AudioSource> audios;
    private bool hasFinished = false;
    private string toDisplay = "";

    private bool loadingNext = false;

    void Start() {
        dialogue = new Queue<String>();
        charas = new Queue<Tuple<string, Sprite>>();
        audios = new Queue<AudioSource>();
    }

    public void StartDialogue(DialogueExpression[] dialogues) {
        IEnumerator WaitUntilReady()
        {
            while (dialogue == null || charas == null || audios == null)
            {
                yield return null;
            }
            animator.SetBool("isOpen", true);
            makeDialogues(dialogues);
            NextDialogue();
        }
        StartCoroutine(WaitUntilReady());
    }

    public void makeDialogues(DialogueExpression[] dialogues) {
        dialogue.Clear();
        charas.Clear();
        
        foreach (DialogueExpression expr in dialogues) {
            foreach(string sentence in expr.sentences) {
                dialogue.Enqueue(sentence);
                charas.Enqueue(new Tuple<string, Sprite>(expr.character, expr.spriteExpr));
                audios.Enqueue(expr.textsound);
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
        m_MyAudioSource = audios.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(toDisplay));
    }

    private void Update() {
        if (Input.GetKeyDown("z")) {
            if (!hasFinished) {
                StopAllCoroutines();
                textbox.text = toDisplay;
                hasFinished = true;
                return;
            }
            NextDialogue();

            if (hasFinished && !loadingNext)
            {
                GameManager.instance.LoadNextScene();
                loadingNext = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !loadingNext)
        {
            GameManager.instance.LoadNextScene();
            loadingNext = true;
        }
        
        //foreach (TextBox txt in floatingTexts) txt.UpdateTextBox();
    }

    IEnumerator TypeSentence(string sentence) {
        textbox.text = "";
        foreach (char letter in sentence.ToCharArray()) {
            textbox.text += letter;
            m_MyAudioSource.Play();
            yield return new WaitForSeconds(0.03f);
        }
        hasFinished = true;
    }

    public void EndDialogue() {
        animator.SetBool("isOpen", false);
    }

}
