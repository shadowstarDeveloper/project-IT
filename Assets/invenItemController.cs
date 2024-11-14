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
    public int row, col; //Å©±â
    public float size;

    private void Start() {
        if(row < 1) row = 1;
        if(col <1) col = 1;
        image = GetComponent<Image>();
        image.rectTransform.sizeDelta = new Vector2((size * row) - 4f, (size * col) - 4f);
        colider2D = GetComponent<BoxCollider2D>();
        colider2D.size = new Vector2((size * row)-6f, (size * col)-6f);
        colider2D.offset = new Vector2((size * row)/2, -(size * col)/2);
    }


    Vector3 mousePos;
    private void OnMouseDown() {
        mousePos = Input.mousePosition - gameObject.transform.localPosition;
    }
    public void OnDrag(PointerEventData eventData) {
        transform.localPosition = new Vector2(Input.mousePosition.x-mousePos.x, Input.mousePosition.y-mousePos.y);

    }

    private void OnMouseUp() {
        transform.localPosition = new Vector2( Mathf.Ceil(transform.localPosition.x/size)*size -size/2, Mathf.Ceil(transform.localPosition.y/size)*size-size/2);
      
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        
    }
    private void OnTriggerStay2D(Collider2D collision) {
        
    }
    private void OnTriggerExit2D(Collider2D collision) {
        
    }
}
