using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class XordleRow : MonoBehaviour
{
    public XordleGrid grid;
    XordleInput[] inputs;
    EventSystem system;
    bool done = false;
    bool isActive = false;

    // Start is called before the first frame update
    void Start()
    {
        inputs = new XordleInput[transform.childCount];
        int i = 0;
        do
        {
            inputs[i] = transform.GetChild(i).GetComponent<XordleInput>();
        } while (i < transform.childCount);
        system = EventSystem.current;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckRow()
    {
        if (done || !isActive) return;
        done = true;

        string expected = grid.GetExpectedText();
        int greens = 0;
        bool[] greenSquare = new bool[expected.Length];

        for (int i = 0; i < transform.childCount; i++)
        {
            string t = (inputs[i].input.text == "") ? " " : inputs[i].input.text;
            if (expected[i] == t[0]) // if the input is the same as expected
            {
                // make box green 
                inputs[i].Green();
                greenSquare[i] = true;
                greens++;

                // Replace letter with "#" to make yellow boxes accurate
                // That way when we check for yellows, it won't find ones that are in the right spot already
                string newExpected = "";
                newExpected += expected.Substring(0, i) + "#";
                if (i != expected.Length - 1)
                {
                    newExpected += expected.Substring(i + 1);
                }
                expected = newExpected;
            } 
        }

        // if player guessed correctly
        if (greens == expected.Length)
        {
            GameManager.instance.XordleTaskDone();
            return;
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            string t = (inputs[i].input.text == "") ? " " : inputs[i].input.text;
            // if they didn't correctly guess this letter, but it is still in the equation
            if (!greenSquare[i] && expected.Contains(t)) 
            {
                inputs[i].Yellow();

                // replace the character with a "#" to prevent duplicate yellows
                int idx = expected.IndexOf(t[0]);
                string newExpected = "";
                newExpected += expected.Substring(0, idx) + "#";
                if (idx != expected.Length - 1)
                {
                    newExpected += expected.Substring(idx + 1);
                }
                expected = newExpected;
            }
        }
        Unusable();
        grid.EnableNext();
    }

    public void Usable()
    {
        isActive = true;
        for (int i = 0; i < inputs.Length; i++)
        {
            inputs[i].Enable();
        }
        system.SetSelectedGameObject(inputs[0].gameObject, new BaseEventData(system));
    }

    public void Unusable()
    {
        isActive = false;
        for (int i = 0; i < inputs.Length; i++)
        {
            inputs[i].Disable();
        }
    }

    public void Clear()
    {
        // Reset row

        done = false;
        for (int i = 0; i < inputs.Length; i++)
        {
            inputs[i].input.text = "";
            ColorBlock cb = inputs[i].input.colors;
            cb.normalColor = Color.white;
            cb.disabledColor = Color.white;
            inputs[i].input.colors = cb;
        }
    }

    public void WaitFrame()
    {
        for (int i = 0; i < inputs.Length; i++)
        {
            inputs[i].WaitFrame();
        }
    }
}
