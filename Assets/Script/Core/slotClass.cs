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
    public void hitOutTile(string _tag) {
        if (tagTxt.text.Length > 0) {
            if(tagTxt.text == _tag) {
                //원래 충돌하던 것이 나갈 경우
                resetTile();
            } else {
                setTileColor(0);
            }
            
        } else {
            resetTile();
        }
    }
    public bool setTile(string _tag) {
        image.rectTransform.sizeDelta = new Vector2(size, size);
        setTileColor(0);
        if (_tag != null) {
            tagTxt.text = _tag;
            return false;
        } else {
            tagTxt.text = "";
            return true;
        }
    }
    public bool hitTile(string _tag) {
        image.rectTransform.sizeDelta = new Vector2(size, size);
        if (tagTxt.text.Length>0) {
            if (tagTxt.text == _tag) {
                //같은 타겟과 부딛힌 경우
                setTileColor(1);
                return false;
            } else {
                setTileColor(2);
                //다른 타겟과 부딛힌 경우
                return true;
            }
        } else {
            setTileColor(1);
            return false;
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

