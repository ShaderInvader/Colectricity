using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // full spagetti here, you enter at your won risk

    public static int whichPlayer = 0; // 0 - Oddawacz, 1 - Pobieracz
    public Transform pobieracz;
    public Transform oddawacz;

    void Update()
    {
        if (Input.GetKeyDown("p") == true) whichPlayer = whichPlayer == 1 ? 0 : 1;
    }
}