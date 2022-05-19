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
        KeyCode tempKeyCode;

        if (!Event.current.isKey || Event.current.type != EventType.KeyDown)
        {
            return;
        }

        tempKeyCode = Event.current.keyCode;
        Debug.Log(tempKeyCode);

        if (!selectKeys.Contains(tempKeyCode))
        {
            return;
        }

        if(currentSelectable>=0)
        {
            if(tempKeyCode == KeyCode.Return)
            {
                selectables[currentSelectable].button.onClick.Invoke();
                
                if(selectables[currentSelectable].nextController == null)
                {
                    return;
                }

                GetComponent<MenuManager>().ChangeMenu(selectables[currentSelectable].nextController);
                return;
            }
            selectables[currentSelectable].button.OnPointerExit(null);
        }

        if (tempKeyCode == KeyCode.DownArrow)
        {
            currentSelectable = (currentSelectable + 1) % selectables.Count;
        }
        else if (tempKeyCode == KeyCode.UpArrow)
        {
            currentSelectable = currentSelectable < 0 ? 0 : currentSelectable;
            currentSelectable = mod(currentSelectable-1, selectables.Count);
        }
        selectables[currentSelectable].button.OnPointerEnter(null);
    }
}
