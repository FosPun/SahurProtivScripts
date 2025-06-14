using UnityEngine;

public class AndroidInputs : MonoBehaviour
{
    public  bool moveUp;
    public  bool moveDown;
    public  bool moveLeft;
    public  bool moveRight;

    public void PressUp() => moveUp = true;
    public void ReleaseUp() => moveUp = false;

    public void PressDown() => moveDown = true;
    public void ReleaseDown() => moveDown = false;

    public void PressLeft() => moveLeft = true;
    public void ReleaseLeft() => moveLeft = false;

    public void PressRight() => moveRight = true;
    public void ReleaseRight() => moveRight = false;
}
