using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerInputModule : MonoBehaviour
{
    public static PlayerInputModule Instance { get; private set; }
    public Dictionary<PlayerInputSelector.Character, PlayerInput> PlayerList { get; private set; }
    public Dictionary<PlayerInputSelector.Character, string> PlayersControlSchemes { get; private set; }
    public Dictionary<PlayerInputSelector.Character, InputDevice> PlayerDevices { get; private set; }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            PlayerList = new Dictionary<PlayerInputSelector.Character, PlayerInput>();
            PlayersControlSchemes = new Dictionary<PlayerInputSelector.Character, string>();
            PlayerDevices = new Dictionary<PlayerInputSelector.Character, InputDevice>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        CursorControler.OnDoneSelecting += PlayersDoneSelecting;
    }

    private void OnDisable()
    {
        CursorControler.OnDoneSelecting -= PlayersDoneSelecting;
    }

    private void PlayersDoneSelecting()
    {
        var playerCursors = GameObject.FindGameObjectsWithTag("PlayerCursor");

        var first = playerCursors[0].GetComponent<CursorControler>().PlayerSelection;
        for (var i = 0; i < playerCursors.Length; i++)
        {
            if (!playerCursors[i].GetComponent<CursorControler>().ObjectSelected)
            {
                return;
            }

            if (i > 0 && playerCursors[i].GetComponent<CursorControler>().PlayerSelection == first)
            {
                return;
            }
        }

        foreach (var t in playerCursors)
        {
            var playerInputComponent = t.GetComponent<PlayerInput>();
            var playerIndex = t.GetComponent<CursorControler>().PlayerSelection;
            
            PlayerList.Add(playerIndex, playerInputComponent);
            PlayerDevices.Add(playerIndex, playerInputComponent.devices[0]);
            PlayersControlSchemes.Add(playerIndex, playerInputComponent.currentControlScheme);
        }

        SceneManager.LoadScene(1);
    }
}
