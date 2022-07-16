using UnityEngine;
public class SelectKeys : MonoBehaviour
{
    public enum Keys { left, right };
    public Keys selection;

    public float ignoreBelow = 0.2f;

    private float lastAxisValue = 0.0f;

    public float Horizontal => Input.GetAxisRaw(selection == Keys.left ? "LeftHorizontal" : "RightHorizontal");
    public float Vertical => Input.GetAxisRaw(selection == Keys.left ? "LeftVertical" : "RightVertical");
    public bool Env => Input.GetButtonDown(selection == Keys.left ? "LeftEnv" : "RightEnv");
    public bool Play => Input.GetButtonDown(selection == Keys.left ? "LeftPlay" : "RightPlay");
    public bool Dash => Input.GetButtonDown(selection == Keys.left ? "LeftDash" : "RightDash") || Input.GetAxisRaw(selection == Keys.left ? "LeftDash" : "RightDash") > 0.5f;
}
