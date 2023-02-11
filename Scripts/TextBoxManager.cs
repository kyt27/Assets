using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;
using System;

public class TextBoxManager : MonoBehaviour {
    public GameObject textContainer;
    public GameObject textPrefab;

    private List<TextBox> floatingTexts = new List<TextBox>();

    int curLine;
    private List<Tuple<String, int, String>> dialogue;

    public void readFile(String filePath) {
        StreamReader file = new StreamReader(filePath);

        string ln;

        while ((ln = file.ReadLine()) != null) {
            string[] splitted = ln.Split('+');
            dialogue.Add(Tuple.Create(splitted[0], Int32.Parse(splitted[1]), splitted[2]));
        }

        file.Close();

        curLine = 0;
    }

    public void OnArrowClick() {
        if (curLine + 1 < dialogue.Count) {
            curLine = ++curLine;
        }
    }

    public void Show(string name, int expression, string msg, int fontSize, Color color) {
        TextBox textBox = GetTextBox();

        textBox.txt.text = msg;
        textBox.txt.fontSize = fontSize;
        textBox.txt.color = color;

        //textBox.Show();
    }

    private void Update() {
        //foreach (TextBox txt in floatingTexts) txt.UpdateTextBox();
    }

    private TextBox GetTextBox() {
        TextBox txt = floatingTexts.Find(t => !t.active);
        if (txt == null) {
            txt = new TextBox();
            txt.go = Instantiate(textPrefab);
            txt.go.transform.SetParent(textContainer.transform);
            txt.txt = txt.go.GetComponent<TextMeshPro>();

            floatingTexts.Add(txt);
        }
        return txt;
    }

}
