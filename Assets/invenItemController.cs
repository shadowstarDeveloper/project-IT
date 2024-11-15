using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class invenItemController : MonoBehaviour, IDragHandler
{
    private BoxCollider2D colider2D;
    private Image image;
    public Color color;
    public int row, col; //크기
    public float size;
    private List<GameObject> items = new List<GameObject>();
    bool mouseDown = false;
    public itemClass itemclass { get; set; }
    private Vector3 startPos;

    private void Start() {
        itemclass = GetComponent<itemClass>();
        setSize();
    }
    private void setSize() {
        if (row < 1) row = 1;
        if (col < 1) col = 1;
        image = GetComponent<Image>();
        image.rectTransform.sizeDelta = new Vector2((size * row) - 0f, (size * col) - 0f);
        colider2D = GetComponent<BoxCollider2D>();
        colider2D.size = new Vector2((size * row) - 6f, (size * col) - 6f);
        colider2D.offset = new Vector2((size * row) / 2, -(size * col) / 2);
    }
    //회전함수
    public void setRotation() {
        //Debug.Log($"{row}, {col}");
        int a = (int)row;
        int b = (int)col;
        col = (int)a; 
        row = (int)b;
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
    Vector3 mousePos;
    private void OnMouseDown() {
        StartCoroutine("rotation");
        startPos = gameObject.transform.localPosition;
        mousePos = Input.mousePosition - gameObject.transform.localPosition;
    }
    public void OnDrag(PointerEventData eventData) {
        transform.localPosition = new Vector2(Input.mousePosition.x-mousePos.x, Input.mousePosition.y-mousePos.y);
        
    }

    private void OnMouseUp() {
       StopCoroutine("rotation");
        setTileTarget();
    }

    IEnumerator resetTileHit() {
        yield return new WaitForFixedUpdate();
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
            transform.localPosition = new Vector2(Mathf.Ceil(transform.localPosition.x / size) * size - size / 2, Mathf.Ceil(transform.localPosition.y / size) * size - size / 2);
            for (int i = 0; i < items.Count; i++) {
                if (items[i].tag == "invenTile") {
                    items[i].GetComponent<slotClass>().setTile(itemclass.Tag);
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
    private void OnTriggerStay2D(Collider2D collision) {
        
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
    }
}
