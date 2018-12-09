using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MapManager : MonoBehaviour {
    public const int MapWidth = 11;
    public const int MapHeight = 16;

    private Block CurrentBlock;
    private BlockImage[] BlockImageArr;

    private void Awake()
    {
        BlockImageArr = GetComponentsInChildren<BlockImage>();
    }

    public void SetCurrentBlock(BlockID InputBlock)
    {
        CurrentBlock = new Block(InputBlock);
    }

    public bool PushBlockDown()
    {

        for(int i =0; i < CurrentBlock.CurrentPoints.Length; i++)
        {
            int[] downPoints = CurrentBlock.DownPoints;
            if (BlockImageArr[downPoints[i]].IsFilled)
            {
                FillBlock();
                return true;
            }
        }

        CurrentBlock.PointsDown();
        DrawCurrentBlock();
        return false;
    }

    private void DrawCurrentBlock()
    {
        int[] upPoints = CurrentBlock.UpPoints;

        //이전 좌표의 색깔 지우기!
        for(int i =0; i < upPoints.Length; i++)
        {
            if (upPoints[i] < 0)
            {
                continue; //음수인 좌표는 그리지 않음 (초기좌표에 음수인 부분이 있음 버그가 아님!!);
            }

            BlockImageArr[upPoints[i]].SetColor(Color.white);
        }
        
        //현재 좌표 색깔 그리기!
        for(int i =0; i < CurrentBlock.CurrentPoints.Length; i++)
        {
            int tempPoint = CurrentBlock.CurrentPoints[i];

            if(tempPoint < 0)
            {
                continue; //음수인 좌표는 그리지 않음 (초기좌표에 음수인 부분이 있음 버그가 아님!!);
            }

            BlockImageArr[tempPoint].SetColor(CurrentBlock.BlockColor);
        }
    }

    private void FillBlock()
    {
        for (int i = 0; i < CurrentBlock.CurrentPoints.Length; i++)
        {
            int tempPoint = CurrentBlock.CurrentPoints[i];

            if (tempPoint < 0)
            {
                //GameOver
            }

            BlockImageArr[tempPoint].FillBlock();
        }

        CheckBlock_Score();
    }

    private bool CheckBlock_Score()
    {
        int count = 0;
        bool returnBool = false;
        for(int i = 0; i < BlockImageArr.Length; i++)
        {
            if(i / MapWidth == 0)
            {
                count = 0;
            }

            if (BlockImageArr[i].IsFilled)
            {
                count++;
            }

            if(count == MapWidth)
            {
                i = i - MapWidth + 1;

                for(;i < i + MapWidth; i++)
                {
                    BlockImageArr[i].UnfillBlock();
                    returnBool = true;
                }
            }
        }

        return returnBool;
    }
}
