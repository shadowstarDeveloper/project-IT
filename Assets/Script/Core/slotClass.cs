using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class slotClass : MonoBehaviour
{
    [SerializeField] private Color color;
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text txt;
    [SerializeField] private TMP_Text tagTxt;
    public int a { get; set; }
    public int b { get; set; }
    private float size;

    
    public void defalutSet(int _a, int _b, float _size) {
         a = _a; b = _b; size = _size;
        txt.text = $"{a},{b}";
        resetTile();
    }
    public void resetTile() {
        image.rectTransform.sizeDelta = new Vector2(size - 10f, size - 10f);
        setTileColor(0);
        tagTxt.text = "";     
    }
   
    public bool setTile(itemClass item) {
       
        if (item != null) {
            image.rectTransform.sizeDelta = new Vector2(size, size);
            setTileColor(0);
            tagTxt.text = item.Tag;
            return false;
        } else {
            resetTile();
            tagTxt.text = "";
            return true;
        }
    }
    
    public void setTileColor(int value) {

        if(value == 1) {
            image.color = Color.green;
        }else if (value == 2) {
            image.color = Color.red;
        } else {
            image.color = color;
        }
    }
}

