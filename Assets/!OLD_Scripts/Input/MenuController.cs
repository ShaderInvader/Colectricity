using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class MenuController : MonoBehaviour
{
    public string menuName;
    public List<SelectableType> selectables;
    int currentSelectable;

    float ignoreBelow = 0.2f;
    bool inputed = false;

    KeyCode[] selectKeys = new KeyCode[] { KeyCode.DownArrow, KeyCode.UpArrow, KeyCode.Return };

    private void OnEnable()
    {
        currentSelectable = -1;
    }

    int mod(int x, int m)
    {
        int r = x % m;
        return r < 0 ? r + m : r;
    }

    void OnGUI()
    {   
        float leftInput = Input.GetAxisRaw("LeftVertical");
        float rightInput = Input.GetAxisRaw("RightVertical");

        int inputFromPad = 0;

        if (Mathf.Abs(leftInput) > ignoreBelow && !inputed)
        {
            if (leftInput > 0)
            {
                inputFromPad = 1;
            }
            else
            {
                inputFromPad = -1;
            }
        }

        if (Mathf.Abs(rightInput) > ignoreBelow && !inputed)
        {
            if (rightInput > 0)
            {
                inputFromPad = 1;
            }
            else
            {
                inputFromPad = -1;
            }
        }

        if(inputed && Mathf.Abs(leftInput) < ignoreBelow && Mathf.Abs(rightInput) < ignoreBelow)
        {
            inputFromPad = 0;
            inputed = false;
        }

        if (inputFromPad != 0)
        {
            inputed = true;

            if (currentSelectable >= 0)
            {
                selectables[currentSelectable].button.OnPointerExit(null);
            }

            if (inputFromPad == 1)
            {
                currentSelectable = currentSelectable < 0 ? 0 : currentSelectable;
                currentSelectable = mod(currentSelectable - 1, selectables.Count);
            }
            else
            {
                currentSelectable = (currentSelectable + 1) % selectables.Count;
            }
            selectables[currentSelectable].button.OnPointerEnter(null);
            return;
        }

        if (currentSelectable >= 0)
        {
            if (Input.GetButtonDown("Submit") || Input.GetButton("LeftEnv") || Input.GetButton("RightEnv"))
            {
                selectables[currentSelectable].button.onClick.Invoke();

                if (selectables[currentSelectable].nextController == null)
                {
                    return;
                }

                GetComponent<MenuManager>().ChangeMenu(selectables[currentSelectable].nextController);
                return;
            }
        }

        //if (!Event.current.isKey || Event.current.type != EventType.KeyDown)
        //{
        //    return;
        //}

        //KeyCode tempKeyCode = Event.current.keyCode;

        //if (!selectKeys.Contains(tempKeyCode))
        //{
        //    return;
        //}

        //if(currentSelectable>=0)
        //{
        //    if(tempKeyCode == KeyCode.Return)
        //    {
        //        selectables[currentSelectable].button.onClick.Invoke();

        //        if(selectables[currentSelectable].nextController == null)
        //        {
        //            return;
        //        }

        //        GetComponent<MenuManager>().ChangeMenu(selectables[currentSelectable].nextController);
        //        return;
        //    }
        //    selectables[currentSelectable].button.OnPointerExit(null);
        //}

        //if (tempKeyCode == KeyCode.DownArrow)
        //{
        //    currentSelectable = (currentSelectable + 1) % selectables.Count;
        //}
        //else if (tempKeyCode == KeyCode.UpArrow)
        //{
        //    currentSelectable = currentSelectable < 0 ? 0 : currentSelectable;
        //    currentSelectable = mod(currentSelectable-1, selectables.Count);
        //}
        //selectables[currentSelectable].button.OnPointerEnter(null);
    }
}
