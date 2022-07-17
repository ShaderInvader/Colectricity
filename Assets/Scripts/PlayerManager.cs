using System;
using System.Collections;
using System.Collections.Generic;
using ModestTree;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }
    
    public static List<PlayerInput> PlayerList;
    public static List<string> PlayersControlSchemes;
    public static List<InputDevice> PlayerDevices;
    
    [SerializeField] private PlayerInput taker;
    [SerializeField] private PlayerInput giver;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            PlayerList = new List<PlayerInput>();
            PlayersControlSchemes = new List<string>();
            PlayerDevices = new List<InputDevice>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        if (PlayerDevices.IsEmpty() || PlayersControlSchemes.IsEmpty())
        {
            taker.SwitchCurrentControlScheme("KeyboardMouse", Keyboard.current);
            giver.SwitchCurrentControlScheme("KeyboardP2", Keyboard.current);
        }
        else
        {
            taker.SwitchCurrentControlScheme(PlayersControlSchemes[0], PlayerDevices[0]);
            giver.SwitchCurrentControlScheme(PlayersControlSchemes[1], PlayerDevices[1]);
        }
    }

    void OnPlayerJoined(PlayerInput playerInput)
    {
        PlayerList.Add(playerInput);
    }
    
    void OnPlayerLeft(PlayerInput playerInput)
    {
        
    }
}
