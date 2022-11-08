using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Book : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public GameObject leftDesk;
    public GameObject midDesk;
    public GameObject rightDesk;
    public bool isTop = false;

    public int size; // 1 small, 2 middle, 3 big

    private GameObject curDesk;
    private RectTransform r;

    // Start is called before the first frame update
    void Start()
    {
        curDesk = leftDesk;

        // Set size of books collider
        r = GetComponent<RectTransform>();
        BoxCollider2D c = GetComponent<BoxCollider2D>();
        c.size = new Vector2(r.rect.width, r.rect.height);

        // at the start, only the smallest book is at the top
        isTop = (size == 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isTop)
        {
            // Make book follow mouse
            transform.position += (Vector3)eventData.delta;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isTop)
        {
            Collider2D c = GetComponent<BoxCollider2D>();
            GameObject newDesk = curDesk;

            // Figure out which desk the book has been moved to
            if (c.IsTouching(leftDesk.GetComponent<BoxCollider2D>()))
            {
                newDesk = leftDesk;
            }
            if (c.IsTouching(midDesk.GetComponent<BoxCollider2D>()))
            {
                newDesk = midDesk;
            }
            if (c.IsTouching(rightDesk.GetComponent<BoxCollider2D>()))
            {
                newDesk = rightDesk;
            }

            if (newDesk != curDesk)
            {
                if (newDesk.GetComponent<Table>().CanPlaceOnTop(this))
                {
                    // Move this book off of the old desk stack
                    curDesk.GetComponent<Table>().Pop();

                    // Add this book to the new desk stack
                    newDesk.GetComponent<Table>().Push(this);
                    curDesk = newDesk;
                }
            }

            RectTransform deskR = curDesk.GetComponent<RectTransform>();
            // Calculates position of the book based on the position of the desk and # of books already on the table
            Vector2 newPos = deskR.localPosition + new Vector3(0, 85 + 20 * (curDesk.GetComponent<Table>().books.Count - 1));
            r.localPosition = newPos;
        }
    }
}
