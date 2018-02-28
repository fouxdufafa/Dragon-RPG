using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent( typeof(CameraRaycaster) )]
public class CursorAffordance : MonoBehaviour {

    [SerializeField] Vector2 cursorHotspot = new Vector2(0, 0);
    [SerializeField] Texture2D walkCursor = null;
    [SerializeField] Texture2D enemyCursor = null;
    [SerializeField] Texture2D unknownCursor = null;

    // TODO: pull layer numbers from source of truth
    [SerializeField] const int walkableLayerNumber = 8;
    [SerializeField] const int enemyLayerNumber = 9;

    CameraRaycaster cameraRaycaster;

	// Use this for initialization
	void Start () {
        cameraRaycaster = GetComponent<CameraRaycaster>();
        cameraRaycaster.notifyLayerChangeObservers += OnCursorLayerChange;
	}

    void OnCursorLayerChange(int newLayer)
    {
        Texture2D cursorTexture = null;

        switch (newLayer)
        {
            case walkableLayerNumber:
                cursorTexture = walkCursor;
                break;

            case enemyLayerNumber:
                cursorTexture = enemyCursor;
                break;

            default:
                cursorTexture = unknownCursor;
                break;
        }

        Cursor.SetCursor(cursorTexture, cursorHotspot, CursorMode.Auto);
    }
}
