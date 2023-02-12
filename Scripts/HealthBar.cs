using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    public float maxVal = 1;
    public float val = 0;
    private RectTransform bar;
    private TextMeshProUGUI text;
    private float ratio;

    // Start is called before the first frame update
    void Start()
    {
        bar = this.transform.GetChild(0).gameObject.GetComponent<RectTransform>();
        text = this.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        ratio = val / maxVal;
        text.text = ((int)val).ToString() + "/" + ((int)maxVal).ToString();
        bar.localScale = new Vector3(ratio, 1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMaxVal(float value)
    {
        maxVal = value;
        ratio = val / maxVal;
        text.text = ((int)val).ToString() + "/" + ((int)maxVal).ToString();
        bar.localScale = new Vector3(ratio, 1, 1);
    }

    public void SetVal(float value)
    {
        val = value;
        ratio = val / maxVal;
        text.text = ((int)val).ToString() + "/" + ((int)maxVal).ToString();
        bar.localScale = new Vector3(ratio, 1, 1);
    }

    public void SetValAndMaxVal(float value, float maxValue)
    {
        val = value;
        maxVal = maxValue;
        ratio = val / maxVal;
        text.text = ((int)val).ToString() + "/" + ((int)maxVal).ToString();
        bar.localScale = new Vector3(ratio, 1, 1);
    }

    public float GetRatio() {
        return ratio;
    }

}
