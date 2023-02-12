using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    public DialogueExpression[] dialogue;

    public void TriggerDialogue () {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }
}
