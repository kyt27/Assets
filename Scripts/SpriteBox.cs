using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteBox : MonoBehaviour
{
    private Image boxImage;
    private Image spriteImage;

    // Start is called before the first frame update
    void Start()
    {
        boxImage = this.GetComponent<Image>();
        spriteImage = this.transform.GetChild(0).gameObject.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetBackgroundColor(Color color)
    {
        boxImage.color = color;
    }

    public void SetSprite(Sprite sprite)
    {
        spriteImage.sprite = sprite;
    }
}
