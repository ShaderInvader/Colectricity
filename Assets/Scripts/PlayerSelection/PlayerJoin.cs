using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJoin : MonoBehaviour
{
    [SerializeField] private GameObject player1CursorPrefab;
    [SerializeField] private GameObject player2CursorPrefab;

    private bool _firstPlayer = true;
    void Start()
    {
        var action = new InputAction(binding: "/*/<button>");
        action.performed += context =>
        {
            AddPlayer(context.control.device);
        };
        action.Enable();
    }

    private void AddPlayer(InputDevice device)
    {
        foreach (var player in PlayerInput.all)
        {
            foreach (var playerDevice in player.devices)
            {
                if (device == playerDevice)
                {
                    return;
                }
            }            
        }

        string controlScheme = "";
        if (device.displayName.Contains("Controller") || device.displayName.Contains("Gamepad"))
            controlScheme = "Gamepad";
        if (device.displayName.Contains("Keyboard"))
            controlScheme = "KeyboardMouse";

        if (_firstPlayer)
        {
            PlayerInput.Instantiate(player1CursorPrefab, -1, controlScheme, -1, device);
            _firstPlayer = false;
        }
        else
        {
            PlayerInput.Instantiate(player2CursorPrefab, -1, controlScheme, -1, device);
        }
    }

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        Debug.Log(playerInput);
    }
}
