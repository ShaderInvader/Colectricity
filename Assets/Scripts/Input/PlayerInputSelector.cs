using System.Collections;
using System.Collections.Generic;
using ModestTree;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputSelector : MonoBehaviour
{
    private enum Character
    {
        Taker, Giver    
    }

    [SerializeField] private Character _character;
    
    private PlayerInput _playerInput;
    private PlayerInputModule _playerInputModule;
    void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        _playerInputModule = PlayerInputModule.Instance;
        if (_playerInputModule.PlayerDevices.IsEmpty() || _playerInputModule.PlayersControlSchemes.IsEmpty())
        {
            _playerInput.SwitchCurrentControlScheme(_character == Character.Taker ? "KeyboardMouse" : "KeyboardP2",
                Keyboard.current);
        }
        else
        {
            _playerInput.SwitchCurrentControlScheme(_playerInputModule.PlayersControlSchemes[(int)_character], _playerInputModule.PlayerDevices[(int)_character]);
        }
    }
    
    void Update()
    {
        
    }
}
