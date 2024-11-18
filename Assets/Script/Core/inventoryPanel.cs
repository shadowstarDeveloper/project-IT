using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UnityEditor.Progress;

public class inventoryPanel : MonoBehaviour
{
    public itemClass[,] inventory;
    private slotClass[,] slots;
    public int row;
    public int col;
    public float size;
    public List<itemClass> itemList = new List<itemClass>();
    public Transform pos;
    [SerializeField] private Image background;
    [SerializeField] private Image weightbar;

    //
    [SerializeField] GameObject invenSlot;
    // Start is called before the first frame update
    void Start()
    {
        //�ʱ�ȭ
        inventory = new itemClass[row, col];
        slots = new slotClass[row, col];
        background.rectTransform.sizeDelta = new Vector2(size * row, size * col);
        weightbar.rectTransform.sizeDelta = new Vector2(size*row, 48f);
        weightbar.rectTransform.localPosition = new Vector2(0, -size*col - size/4);
        for (int a = 0; a < row; a++) {
            for (int b = 0; b < col; b++) {
               slots[a,b] = fmakeSlot(a, b) ;
            }
        }
        
        
        printInventory();
        //
    }
    public void printInventory() {
        //�κ��丮 ǥ��
        for (int r = 0; r < row; r++) {
            for (int c = 0; c < col; c++) {
                slots[r, c].setTile(inventory[r,c]);
            }
        }
        //���� �� ����
        weightbar.fillAmount = Random.Range(0f,1f);
    }
    
    public bool AddItem(itemClass item, int startRow, int startCol) {
        if (canPlaceItem(item, startRow, startCol)) {
            //������ ������ ������ ���
            for (int r = 0; r < item.row; r++) {
                for (int c = 0; c < item.col; c++) {
                    inventory[startRow + r, startCol + c] = item; // ���� ä���

                }
            }
                printInventory();
                return true;
        } else {
            //�κ��� ���� �ְų� ������ ���
            printInventory();
            return false;
        }
    }
    public bool RemoveItem(itemClass item, int startRow, int startCol) {
        if (startRow < 0) startRow = 0;
        if (startCol < 0) startCol = 0;
        int _row = startRow + item.row;
        int _col = startCol + item.col;
        if (_row > row) _row = row;
        if( _col > col) _col = col;

        //������ ������ ������ ���
        if (hasPlaceItem(item, startRow, startCol)) {
            //������ ������ ������ ���
            for (int r = 0; r < item.row; r++) {
                for (int c = 0; c < item.col; c++) {
                    inventory[startRow + r, startCol + c] = null; // ���� ä���

                }
            }
            printInventory();
            return true;
        } else {
            //�κ��� ���� �ְų� ������ ���
            printInventory();
            return false;
        }

    }
    
    public bool canPlaceItem(itemClass item, int startRow, int startCol) {
        //������ ��ġ ���� ���� Ȯ��
        if(startRow <0 || startCol <0) return false;

        for(int r = 0; r <item.row; r++) {
            for (int c = 0; c < item.col; c++) {
                int _row = startRow + r;
                int _col = startCol + c;
                //Debug.Log($"{startRow},{startCol} to {_row},{_col}");
                if (_row >= row || _col >= col ) {
                    return false; // ������ ����ų� �̹� ��� ���� ������ ���� ���

                } else if( inventory[_row, _col] != null) {
                    if (inventory[_row, _col] == item) {
                        return true;
                    } else {
                        return false;
                    }
                }
            }
        }
        return true;
    }
    private bool hasPlaceItem(itemClass item, int startRow, int startCol) {
        //������ ��ġ ���� ���� Ȯ��
        if (startRow < 0 || startCol < 0) return false;

        for (int r = 0; r < item.row; r++) {
            for (int c = 0; c < item.col; c++) {
                int _row = startRow + r;
                int _col = startCol + c;
                //Debug.Log($"{startRow},{startCol} to {_row},{_col}");
                if (_row >= row || _col >= col ) {
                    return false; // ������ ����ų� �̹� ��� ���� ������ ���� ���
                } else {
                    if (inventory[_row, _col] == item) {
                        return true;
                    } else {
                        return false; // ������ ����ų� �̹� ��� ���� ������ ���� ���
                    }
                }
            }
        }
        return true;
    }
    public Vector2 slotPosition(int _row, int _col) {
        Vector2 pos = slots[_row, _col].transform.localPosition;
        return new Vector2(pos.x-size/2, pos.y+size/2);
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
