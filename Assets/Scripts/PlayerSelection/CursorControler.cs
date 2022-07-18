using UnityEngine;
using UnityEngine.InputSystem;

public class CursorControler : MonoBehaviour
{
    [SerializeField] private float cursorSpeed = 50f;

    private Vector2 _movement;
    private const float ScreenEdgeThreshold = .02f;

    public bool ObjectSelected { get; private set; }
    public PlayerInputSelector.Character PlayerSelection { get; private set; }

    public delegate void DoneSelecting();
    public static event DoneSelecting OnDoneSelecting;

    void Update()
    {
        var viewportPosition = Camera.main.WorldToViewportPoint(transform.position);
        if ((viewportPosition.x < ScreenEdgeThreshold && _movement.x < 0) ||
            (viewportPosition.x > 1 - ScreenEdgeThreshold && _movement.x > 0) ||
            (viewportPosition.y < ScreenEdgeThreshold && _movement.y < 0) ||
            (viewportPosition.y > 1 - ScreenEdgeThreshold && _movement.y > 0))
            return;
        
        transform.Translate(new Vector3(_movement.x, _movement.y, 0f) * (cursorSpeed * 0.001f));
    }

    public void OnCursorMove(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Canceled && !ObjectSelected)
        {
            _movement = context.ReadValue<Vector2>();
        }
        else
        {
            _movement = Vector2.zero;
        }
    }

    public void OnSelectButton(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.forward, out hit, 1000f, LayerMask.GetMask("PlayerObjects")))
            {
                if (!ObjectSelected)
                {
                    ObjectSelected = true;
                    PlayerSelection = hit.transform.gameObject.GetComponent<PlayerInputSelector>().selectedCharacter;
                    return;
                }
            }

            if (ObjectSelected)
            {
                ObjectSelected = false;
                PlayerSelection = PlayerInputSelector.Character.None;
            }
        }
    }

    public void OnStartButton(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnDoneSelecting?.Invoke();
        }
    }
}
