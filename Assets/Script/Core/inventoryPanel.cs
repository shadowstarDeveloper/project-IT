using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class inventoryPanel : MonoBehaviour
{
    public slotClass[,] matrix;
    public int row;
    public int col;
    public float size;
    public List<itemClass> itemList = new List<itemClass>();
    [SerializeField] private Transform pos;

    //
    [SerializeField] GameObject invenSlot;
    // Start is called before the first frame update
    void Start()
    {
        //√ ±‚»≠
        matrix = new slotClass[row, col];
        for (int a = 0; a < row; a++) {
            for (int b = 0; b < col; b++) {
                matrix[a, b] = fmakeSlot(a, b);

            }
        }
        //
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private slotClass fmakeSlot(int a, int b) {
        GameObject slot= Instantiate(invenSlot, pos);
        slot.transform.localPosition = new Vector2(a * size, -b * size);

        slotClass clas = slot.GetComponent<slotClass>();
        clas.defalutSet(a,b, size);
        return clas;
    }
}
