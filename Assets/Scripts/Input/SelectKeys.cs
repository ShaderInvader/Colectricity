using UnityEngine;
public class SelectKeys : MonoBehaviour
{
    public enum Keys { left, right };
    public Keys selection;

    public float Horizontal => Input.GetAxisRaw(selection == Keys.left ? "LeftHorizontal" : "RightHorizontal");
    public float Vertical => Input.GetAxisRaw(selection == Keys.left ? "LeftVertical" : "RightVertical");
    public bool Env => Input.GetButtonDown(selection == Keys.left ? "LeftEnv" : "RightEnv");
    public bool Play => Input.GetButtonDown(selection == Keys.left ? "LeftPlay" : "RightPlay");
    public bool Dash => Input.GetButtonDown(selection == Keys.left ? "LeftDash" : "RightDash");
}
