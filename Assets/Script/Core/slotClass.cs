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
    public int a { get; set; }
    public int b { get; set; }


    public void defalutSet(int _a, int _b, float size) {
         a = _a; b = _b;
        image.rectTransform.sizeDelta = new Vector2(size-10f, size - 10f);
        image.color = color;
        txt.text = $"{a},{b}";
    }
}
