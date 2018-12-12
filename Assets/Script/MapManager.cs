using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MapManager : MonoBehaviour {
    public const int MapWidth = 11;
    public const int MapHeight = 17;

    private Block CurrentBlock;
    private BlockImage[] BlockImageArr;

    private bool isAttached;

    private void Awake()
    {
        BlockImageArr = GetComponentsInChildren<BlockImage>();
    }

    public void SetCurrentBlock(BlockID InputBlock)
    {
        CurrentBlock = new Block(InputBlock);
        DrawCurrentBlock();
    }

    public void MoveBlockLeft()
    {
        if (isAttached)
        {
            return;
            //이미 바닥에 붙였으니 추가적인 행동은 몬함!
        }
        Vector2Int[] leftPoints = CurrentBlock.LeftPoints;

        for (int i = 0; i < leftPoints.Length; i++)
        {
            if(leftPoints[i].x >= MapWidth || leftPoints[i].x < 0)
            {
                return;
            }
        }//맵 너비를 벗어나면 이동하지 않음

        ClearCurrentBlock();
        CurrentBlock.MovePointsLeft();
        DrawCurrentBlock();
    }

    public void MoveBlockRight()
    {
        if (isAttached)
        {
            return;
            //이미 바닥에 붙였으니 추가적인 행동은 몬함!
        }
        Vector2Int[] rightPionts = CurrentBlock.RightPoints;

        for (int i = 0; i < rightPionts.Length; i++)
        {
            if (rightPionts[i].x >= MapWidth || rightPionts[i].x < 0)
            {
                return;
            }
        }//맵 너비를 벗어나면 이동하지 않음

        ClearCurrentBlock();
        CurrentBlock.MovePointsRight();
        DrawCurrentBlock();
    }
    public void RotateBlock()
    {
        if (isAttached)
        {
            return;
            //이미 바닥에 붙였으니 추가적인 행동은 몬함!
        }
    }
    public void AttachCurrentBlock()
    {
        if (isAttached)
        {
            return;
            //이미 바닥에 붙였으니 추가적인 행동은 몬함!
        }
        ClearCurrentBlock();
        while(true)
        {
            int currentIndex = 0;
            bool breakFlag = false;
            for(int i =0; i < CurrentBlock.CurrentPoints.Length; i ++)
            {
                currentIndex = CurrentBlock.CurrentPoints[i].y * MapWidth + CurrentBlock.CurrentPoints[i].x;
                Debug.Log(CurrentBlock.CurrentPoints[i].y);
                if (CurrentBlock.CurrentPoints[i].y + 1 >= MapHeight || BlockImageArr[currentIndex+MapWidth].IsFilled)
                {
                    breakFlag = true;
                    break;
                }
            }
            if (breakFlag)
            {
                break;
            }
            else
            {
                CurrentBlock.MovePointsDown();
            }
        }

        FillBlock();
        isAttached = true;
    }

    /// <summary>
    /// 블록 내리기 성공시 true반환 실패시 false 반환(새로운 블록을 줘야함 버그가아님!)
    /// </summary>
    /// <returns></returns>
    public bool PushBlockDown()
    {
        if(isAttached)
        {
            isAttached = false;
            return false;
        }
        for(int i =0; i < CurrentBlock.CurrentPoints.Length; i++)
        {
            Vector2Int[] downPoints = CurrentBlock.DownPoints;

            int currentIndex = downPoints[i].y * MapWidth + downPoints[i].x;

            if (downPoints[i].y >= MapHeight)
            {
                FillBlock();
                return false;
            }
            else if (BlockImageArr[currentIndex].IsFilled)
            {
                FillBlock();
                return false;
            }
        }

        ClearCurrentBlock();
        CurrentBlock.MovePointsDown();
        DrawCurrentBlock();
        return true;
    }



    private void ClearCurrentBlock()
    {
        Vector2Int[] targetPoint = CurrentBlock.CurrentPoints;
        //이전 좌표의 색깔 지우기!
        for (int i = 0; i < CurrentBlock.CurrentPoints.Length; i++)
        {
            BlockImageArr[targetPoint[i].y* MapWidth + targetPoint[i].x].SetColor(Color.white);
        }
    }

    private void DrawCurrentBlock()
    {
        Vector2Int[] targetPoint = CurrentBlock.CurrentPoints;
        //현재 좌표 색깔 그리기!
        for (int i = 0; i < CurrentBlock.CurrentPoints.Length; i++)
        {
            BlockImageArr[targetPoint[i].y * MapWidth + targetPoint[i].x].SetColor(CurrentBlock.BlockColor);
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
                    }//점수획득 + 블록지우기
                    PullBlock(i);
                    
                }
            }
        }
        return returnBool;
    }

    private void PullBlock(int InputYPos)
    {
        for(int i = 0; i < MapWidth; i++)
        {
            int currentIndex = (InputYPos -1) * MapWidth + i;//입력받은 Y좌표에서 한칸 위부터 작업시작
            while (BlockImageArr[currentIndex].IsFilled)
            {
                BlockImageArr[currentIndex].UnfillBlock();//지우고
                BlockImageArr[currentIndex + MapWidth].FillBlock();//아래블록 채우고

                currentIndex += MapWidth;//CurrentIndex Update
            }
        }
    }
}
