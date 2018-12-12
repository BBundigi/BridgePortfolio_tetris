using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour {
    [SerializeField]
    private MapManager mapManager;

    private void Update()
    {
        bool isLeftClick = Input.GetKeyDown(KeyCode.LeftArrow);
        bool isRightClick = Input.GetKeyDown(KeyCode.RightArrow);

        if (isLeftClick ^ isRightClick)
        {
            if (isLeftClick)
            {
                mapManager.MoveBlockLeft();
            }
            else
            {
                mapManager.MoveBlockRight();
            }
        }

        if(Input.GetKeyDown(KeyCode.Z))
        {
            mapManager.RotateBlock();
        }

        if(Input.GetKeyDown(KeyCode.X))
        {
            mapManager.AttachCurrentBlock();
        }

        if(Input.GetKeyDown(KeyCode.C))
        {
            mapManager.RotateBlock();
        }
    }
}
