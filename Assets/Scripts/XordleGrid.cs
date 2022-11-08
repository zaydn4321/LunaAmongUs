using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XordleGrid : MonoBehaviour
{
    XordleRow[] rows;
    int idx = 0;
    bool started = false;
    string realEquation = "";

    // Start is called before the first frame update
    void Start()
    {
        rows = new XordleRow[transform.childCount];
        int i = 0;
        do
        {
            rows[i] = transform.GetChild(i).GetComponent<XordleRow>();
            i++;
        } while (i < transform.childCount);
        GenExpectedText();
    }

    // Update is called once per frame
    void Update()
    {
        if (!started)
        {
            started = true;
            for (int i = 0; i < rows.Length; i++)
            {
                rows[i].Unusable();
            }
            rows[0].Usable();
        }
    }

    public string GetExpectedText()
    {
        return realEquation;
    }

    // Generate a random bitwise equation
    public void GenExpectedText()
    {
        int a = Random.Range(0, 10);
        int b = Random.Range(0, 10);
        string[] ops = { "|", "&", "^" };
        string op = ops[Random.Range(0, 3)];
        int c = 0;
        switch (op)
        {
            case "|":
                c = a | b;
                break;
            case "&":
                c = a & b;
                break;
            case "^":
                c = a ^ b;
                break;
        }
        realEquation = a.ToString() + op + b.ToString() + "=" + c.ToString();
    }

    // Activates the next Xordle row
    public void EnableNext()
    {
        idx++;
        if (idx < rows.Length)
        {
            rows[idx].Usable();
        }
        else
        {
            // if we are out of rows, reset the game
            for (int i = 0; i < rows.Length; i++)
            {
                rows[i].Clear();
                rows[i].Unusable();
            }
            rows[0].Usable();
            GenExpectedText();
            idx = 0;
        }
        rows[idx].WaitFrame();
    }
}
