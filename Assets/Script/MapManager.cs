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
            Vector2Int[] downPoints = CurrentBlock.DownPoints;

            int currentIndex = downPoints[i].y * MapWidth + downPoints[i].x;

            if (downPoints[i].y > MapHeight)
            {
                FillBlock();
                return true;
            }
            else if (BlockImageArr[currentIndex].IsFilled)
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
        Vector2Int[] upPoints = CurrentBlock.UpPoints;

        //이전 좌표의 색깔 지우기!
        for(int i =0; i < upPoints.Length; i++)
        {
            if(upPoints[i].y < 0)
            {
                continue;
                //예외 처리
            }
            BlockImageArr[upPoints[i].y * MapWidth + upPoints[i].x].SetColor(Color.white);
        }
        
        //현재 좌표 색깔 그리기!
        for(int i =0; i < CurrentBlock.CurrentPoints.Length; i++)
        {
            Vector2Int tempPoint = CurrentBlock.CurrentPoints[i];

            BlockImageArr[tempPoint.y * MapWidth + tempPoint.x].SetColor(CurrentBlock.BlockColor);
        }
    }

    private void FillBlock()
    {
        for (int i = 0; i < CurrentBlock.CurrentPoints.Length; i++)
        {
            Vector2Int tempPoint = CurrentBlock.CurrentPoints[i];

            if (tempPoint.y < 0)
            {
                //GameOver
            }

            BlockImageArr[tempPoint.y * MapWidth + tempPoint.x].FillBlock();
        }

        CheckBlock_Score();
    }

    private bool CheckBlock_Score()
    {
        bool returnBool = false;

        for(int i =0; i < MapHeight; i++)
        {
            int count = 0;
            for(int j =0; j < MapWidth; j++)
            {
                int currentIndex = i * MapWidth + j;

                if(BlockImageArr[currentIndex].IsFilled)
                {
                    count++;
                }

                if(count == MapWidth)
                {
                    for (;j >=0 ; j--)
                    {
                        BlockImageArr[i * MapWidth + j].UnfillBlock();
                        returnBool = true;
                    }
                }
            }
        }

        return returnBool;
    }
}
