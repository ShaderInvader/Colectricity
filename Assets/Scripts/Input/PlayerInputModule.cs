using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputModule : MonoBehaviour
{
    public static PlayerInputModule Instance { get; private set; }
    public List<PlayerInput> PlayerList { get; private set; }
    public List<string> PlayersControlSchemes { get; private set; }
    public List<InputDevice> PlayerDevices { get; private set; }
    
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
        
    }

    void OnPlayerJoined(PlayerInput playerInput)
    {
        PlayerList.Add(playerInput);
    }
    
    void OnPlayerLeft(PlayerInput playerInput)
    {
        
    }
}
