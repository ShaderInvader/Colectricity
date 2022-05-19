using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public MenuController currentMenuController;

    void Start()
    {
        currentMenuController.enabled = true;
    }

    public void ChangeMenu(MenuController mc)
    {
        currentMenuController.enabled = false;
        currentMenuController = mc;
        currentMenuController.enabled = true;
    }
}
