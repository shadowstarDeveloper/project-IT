using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class invenItemController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    
    private Image image;
    public Color color;
    public float size;
    private List<GameObject> items = new List<GameObject>();

    public itemClass itemclass { get; set; }

    private Canvas canvas;
    private RectTransform rectTransform { get; set; }
    private CanvasGroup canvasGroup;
    private Transform originalParent;
    [SerializeField] private inventoryPanel invenPanel;
    private Vector2 resetPosition;
    private Vector2 startPosition;

    private void Awake() {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
        itemclass = GetComponent<itemClass>();
        setSize();
    }
    private void Start() {
        resetPosition = transform.localPosition;
        Debug.Log(resetPosition);
    }
    private void setSize() {
        if (itemclass.row < 1) itemclass.row = 1;
        if (itemclass.col < 1) itemclass.col = 1;
        image = GetComponent<Image>();
        image.rectTransform.sizeDelta = new Vector2((size * itemclass.row) - 0f, (size * itemclass.col) - 0f);
    }
    //ȸ���Լ�
    public void setRotation() {
        //Debug.Log($"{row}, {col}");
        int a = (int)itemclass.row;
        int b = (int)itemclass.col;
        itemclass.col = (int)a;
        itemclass.row = (int)b;
        setSize();
       // Debug.Log($"{row}, {col}");
    }

    IEnumerator rotation() {
        while (true) {

            yield return new WaitForFixedUpdate();
            
            if (Input.GetKeyDown(KeyCode.R)) {
                setRotation();
            }
         }
    }


    public void OnBeginDrag(PointerEventData eventData) {
        StartCoroutine("rotation");
        resetTileTarget();
        // �巡�� ���� ��
        originalParent = transform.parent; // ���� �θ� ����
                                           // transform.SetParent(canvas.transform); // Canvas�� �θ� ����
        transform.SetParent(invenPanel.pos.gameObject.transform);
        startPosition = transform.localPosition;
        //canvasGroup.blocksRaycasts = false; // ��� ������ ���� Raycast ��Ȱ��ȭ
    }
    public void OnDrag(PointerEventData eventData) {
       // transform.localPosition = new Vector2(Input.mousePosition.x-mousePos.x, Input.mousePosition.y-mousePos.y);
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor; // ���콺 ���� �̵�

        //�浹�˻�
        

    }
    public void OnEndDrag(PointerEventData eventData) {
        // �巡�� ���� ��
        //canvasGroup.blocksRaycasts = true; // Raycast �ٽ� Ȱ��ȭ

        
        StopCoroutine("rotation");
        setTileTarget();
        
    }

    public void setTileTarget( ) {
        int row = returnRow();
        int col = returnCol();
       
        if(invenPanel.AddItem(itemclass, row, col)) {
            Debug.Log($"{row},{col}");
            transform.localPosition = new Vector2(row * size - size / 2, -col * size + size / 2);
        } else {
            Debug.Log($"out of range");
            if(originalParent == canvas) {
                //�������� �θ� ĵ������ ���, �ٱ��� ���
                transform.SetParent(canvas.transform);
                transform.localPosition = resetPosition;
            } else {
                transform.localPosition = startPosition;
                invenPanel.AddItem(itemclass, returnRow(), returnCol());
            }
            
        }
        
        invenPanel.printInventory();
    }
    public void resetTileTarget() {
        int row = returnRow();
        int col = returnCol();
        Debug.Log($"{row},{col}");
        if (invenPanel.RemoveItem(itemclass, row, col)) {

        } else {
            //����� ����
        }
    }

    public int returnRow() {
        int row = (int)Mathf.Ceil(( transform.localPosition.x ) / size);
        // int row = (int)Mathf.Ceil(( transform.localPosition.x - invenPanel.pos.parent.localPosition.x) / size);
        return row;
    }
    public int returnCol() {
        int col = (int)Mathf.Ceil(-(transform.localPosition.y ) / size);
        //int col = (int)Mathf.Ceil(-(transform.localPosition.y - invenPanel.pos.parent.localPosition.y) / size);
        return col;
    }
    /*

        IEnumerator resetTileHit() {
            while( items.Count < col * row) {
                yield return new WaitForFixedUpdate();
                setRotation();
            }
            for (int i = 0; i < items.Count; i++) {
                if (items[i].tag == "invenTile") {
                    items[i].GetComponent<slotClass>().setTile(itemclass.Tag);
                }
            }

            StopCoroutine("resetTileHit");
        }
        private void setTileTarget() {
            bool hit = setTileTargetCheck();

            if (hit) {


                for (int i = 0; i < items.Count; i++) {
                    if (items[i].tag == "invenTile") {
                        items[i].GetComponent<slotClass>().hitOutTile(itemclass.Tag);
                    }
                }
                //���� �浹���� �� ������ġ�� ���ư���
                gameObject.transform.localPosition = startPos;
                StartCoroutine("resetTileHit");

            } else {
                if (items.Count < row * col) {
                    //�浹 ������ ������ ���  ������ġ�� ���ư���
                    gameObject.transform.localPosition = startPos;
                    StartCoroutine("resetTileHit");
                } else {

                    transform.localPosition = new Vector2(Mathf.Ceil(transform.localPosition.x / size) * size - size / 2, Mathf.Ceil(transform.localPosition.y / size) * size - size / 2);
                    for (int i = 0; i < items.Count; i++) {
                        if (items[i].tag == "invenTile") {
                            items[i].GetComponent<slotClass>().setTile(itemclass.Tag);
                        }
                    }
                }
            }
        }
        private bool setTileTargetCheck() {
            bool hit = false;
            for (int i = 0; i < items.Count; i++) {
                if (items[i].tag == "invenTile") {
                    hit = items[i].GetComponent<slotClass>().hitTile(itemclass.Tag);
                    if (hit) {
                        break;
                    }
                }

            }
            return hit;
        }
        private void OnTriggerEnter2D(Collider2D collision) {
            checkHit(collision);
        }
        private void OnTriggerStay2D(Collider2D collision) {
           // checkHit(collision);
        }
        void checkHit(Collider2D collision) {
            if (collision != null) {
                if (collision.tag == "invenTile") {

                    if (items.Contains(collision.gameObject)) {

                    } else {
                        collision.GetComponent<slotClass>().setTileColor(1);
                        collision.GetComponent<slotClass>().hitTile(itemclass.Tag);
                        items.Add(collision.gameObject);
                    }
                }
            }
        }
        private void OnTriggerExit2D(Collider2D collision) {
            if (collision != null) {
                if (collision.tag == "invenTile") {
                    if (items.Contains(collision.gameObject)) {
                        items.Remove(collision.gameObject);
                    }
                    collision.GetComponent<slotClass>().hitOutTile(itemclass.Tag);
                }
            }
        }*/
}
