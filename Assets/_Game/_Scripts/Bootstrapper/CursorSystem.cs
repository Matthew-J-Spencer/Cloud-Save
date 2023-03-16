using UnityEngine;

public class CursorSystem : MonoBehaviour
{
    [SerializeField] private Texture2D _cursorTexture, _clickedCursorTexture;
    private const CursorMode CURSOR_MODE = CursorMode.Auto;
    private readonly Vector2 _hotSpot = Vector2.zero;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) SetCursor(_clickedCursorTexture);
        else if (Input.GetMouseButtonUp(0)) SetCursor(_cursorTexture);
    }

    private void SetCursor(Texture2D tex) => Cursor.SetCursor(tex, _hotSpot, CURSOR_MODE);
}