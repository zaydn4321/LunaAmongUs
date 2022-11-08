using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    public Stack<Book> books;
    public int loc; // 0 = left, 1 = middle, 2 = right

    public GameObject sBook, mBook, bBook;

    // Start is called before the first frame update
    void Start()
    {
        books = new Stack<Book>();
        if (loc == 0) // if table is on the left
        {
            books.Push(bBook.GetComponent<Book>());
            books.Push(mBook.GetComponent<Book>());
            books.Push(sBook.GetComponent<Book>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool CanPlaceOnTop(Book other)
    {
        return books.Count == 0 ? true : other.size < books.Peek().size;
        /*
         * if (table is empty)
         * {
         *   return true;
         * }
         * if (new book size < current top book size)
         * {
         *   return true;
         * }
         * else
         * {
         *   return false;
         * }
         */
    }

    public void Pop()
    {
        books.Pop();
        if (books.Count > 0) // if table isn't empty
        {
            books.Peek().isTop = true; // tell the top book that it is now on top
        }
    }

    public void Push(Book b)
    {
        if (books.Count > 0) // if the table isn't empty
        {
            books.Peek().isTop = false; // tell the top book that it isn't on top anymore
        }
        books.Push(b);

        if (loc == 2 && books.Count == 3) // if puzzle is solved
        {
            GameManager.instance.TowerTaskDone();
        }
    }
}
