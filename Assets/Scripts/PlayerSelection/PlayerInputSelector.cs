using ModestTree;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputSelector : MonoBehaviour
{
    public enum Character
    {
        Taker, Giver, None    
    }

    public Character selectedCharacter;
    
    private PlayerInput _playerInput;
    private PlayerInputModule _playerInputModule;
    void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        if (!_playerInput)
            return;
        _playerInputModule = PlayerInputModule.Instance;
        if (_playerInputModule.PlayerDevices.IsEmpty() || _playerInputModule.PlayersControlSchemes.IsEmpty())
        {
            _playerInput.SwitchCurrentControlScheme(selectedCharacter == Character.Taker ? "KeyboardMouse" : "KeyboardP2",
                Keyboard.current);
        }
        else
        {
            _playerInput.SwitchCurrentControlScheme(_playerInputModule.PlayersControlSchemes[selectedCharacter], _playerInputModule.PlayerDevices[selectedCharacter]);
        }
    }
}
