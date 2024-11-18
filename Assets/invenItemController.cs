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
    //회전함수
    public void setRotation() {
        //Debug.Log($"{row}, {col}");
        int a = (int)itemclass.row;
        int b = (int)itemclass.col;
        itemclass.col = (int)a;
        itemclass.row = (int)b;
        rotate = !rotate;
        setSize();
       // Debug.Log($"{row}, {col}");
    }

    bool rotate = false;
    bool drag = false;
    void Update() {
        if (drag) {
            if (Input.GetKeyDown(KeyCode.R)) {
                setRotation();
            }
        }
    }
    public void OnBeginDrag(PointerEventData eventData) {
        drag = true;
        resetTileTarget();
        // 드래그 시작 시
        originalParent = transform.parent; // 원래 부모 저장
                                           // transform.SetParent(canvas.transform); // Canvas로 부모 변경
        transform.SetParent(invenPanel.pos.gameObject.transform);
        startPosition = transform.localPosition;
        //canvasGroup.blocksRaycasts = false; // 드롭 감지를 위해 Raycast 비활성화
    }
    public void OnDrag(PointerEventData eventData) {
       // transform.localPosition = new Vector2(Input.mousePosition.x-mousePos.x, Input.mousePosition.y-mousePos.y);
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor; // 마우스 따라 이동

        
        //충돌검사
        

    }
    public void OnEndDrag(PointerEventData eventData) {
        // 드래그 종료 시
        //canvasGroup.blocksRaycasts = true; // Raycast 다시 활성화

        drag = false;
 
        setTileTarget();
        rotate = false;
        
    }

    public void setTileTarget( ) {
        int row = getRow();
        int col = getCol();
       


        if(invenPanel.AddItem(itemclass, row, col)) {
           
            transform.localPosition = invenPanel.slotPosition(row,col); //new Vector2(row * size - size / 2, -col * size + size / 2);
        } else {
            
            if(originalParent == canvas) {
                //오리지널 부모가 캔버스일 경우, 바깥일 경우
                Debug.Log($"out of range");
                transform.SetParent(canvas.transform);
                transform.localPosition = resetPosition;
            } else {
                //
                
                transform.localPosition = startPosition;
                if (rotate) {
                    setRotation();
                    //transform.localPosition = invenPanel.slotPosition(row, col);
                }
                if(invenPanel.canPlaceItem(itemclass, row, col)) {
                    Debug.Log($"{row},{col}");

                    transform.localPosition = invenPanel.slotPosition(row, col);
                    invenPanel.AddItem(itemclass, row, col);
                } else {

                    transform.localPosition = startPosition;
                    row = getRow();
                    col = getCol();
                    invenPanel.AddItem(itemclass, row, col);
                }

               
            }
            
        }
        
        invenPanel.printInventory();
    }
    public void resetTileTarget() {
        int row = getRow();
        int col = getCol();
        Debug.Log($"{row},{col}");
        if (invenPanel.RemoveItem(itemclass, row, col)) {

        } else {
            //지우기 실패
        }
    }

    public int getRow() {
        int row = (int)Mathf.Ceil(( transform.localPosition.x ) / size);
        // int row = (int)Mathf.Ceil(( transform.localPosition.x - invenPanel.pos.parent.localPosition.x) / size);
        return row;
    }
    public int getCol() {
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
                //무언가 충돌했을 때 원래위치로 돌아가기
                gameObject.transform.localPosition = startPos;
                StartCoroutine("resetTileHit");

            } else {
                if (items.Count < row * col) {
                    //충돌 갯수가 부족할 경우  원래위치로 돌아가기
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
