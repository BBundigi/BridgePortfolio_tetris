using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MapManager : MonoBehaviour {
    public const int MapWidth = 11;
    public const int MapHeight = 17;

    private int Score;
    private Block CurrentBlock;


    private BlockImage[] BlockImageArr;

    private bool isAttached;

    private void Awake()
    {
        BlockImageArr = GetComponentsInChildren<BlockImage>();
    }

    private void Start()
    {
        Score = 0;
        UIManager.Instance.SetScore(Score);
    }

    public void SetCurrentBlock(BlockID InputBlock)
    {
        CurrentBlock = new Block(InputBlock);
        CheckGameOver();
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
            if (leftPoints[i].x >= MapWidth || leftPoints[i].x < 0
                                || BlockImageArr[ConvertPos2DTo1D(leftPoints[i].x, leftPoints[i].y)].IsFilled)
            {
                return;
            }
        }//맵 너비를 벗어나거나 옆에 채워진 블록이 있으면 이동하지 않음

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
            if (rightPionts[i].x >= MapWidth || rightPionts[i].x < 0
                                || BlockImageArr[ConvertPos2DTo1D(rightPionts[i].x, rightPionts[i].y)].IsFilled)
            {
                return;
            }
        }//맵 너비를 벗어나거나 옆에 채워진 블록이 있으면 이동하지 않음

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

        ClearCurrentBlock();
        CurrentBlock.RotateBlock();

        for(int i =0; i < CurrentBlock.CurrentPoints.Length; i++)
        {
            int currentPos_X = CurrentBlock.CurrentPoints[i].x;
            int currentPos_Y = CurrentBlock.CurrentPoints[i].y;

            if(currentPos_X < 0)
            {
                CurrentBlock.MovePointsRight();
                i = 0;//재검사
                continue;
            }
            else if(currentPos_X >= MapWidth)
            {
                CurrentBlock.MovePointsLeft();
                i = 0;
                continue;
            }
            else if(currentPos_Y >= MapHeight)
            {
                CurrentBlock.MovePointsUp();
                i = 0;
                continue;
            }
            else if(currentPos_Y < 0)
            {
                CurrentBlock.MovePointsDown();
                i = 0;
                continue;
            }

            int currentIndex = CurrentBlock.CurrentPoints[i].y * MapWidth + CurrentBlock.CurrentPoints[i].x;

            if(BlockImageArr[currentIndex].IsFilled)
            {
                CurrentBlock.MovePointsUp();
            }        
        }
        DrawCurrentBlock();
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
                currentIndex = ConvertPos2DTo1D(CurrentBlock.CurrentPoints[i].x,CurrentBlock.CurrentPoints[i].y);
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

            BlockImageArr[ConvertPos2DTo1D(tempPoint.x, tempPoint.y)].FillBlock();
        }

        CheckBlock_Score();
    }
    private void CheckGameOver()
    {
        for(int i =0; i< CurrentBlock.CurrentPoints.Length;i++)
        {
            int currentIndex = ConvertPos2DTo1D(CurrentBlock.CurrentPoints[i].x, CurrentBlock.CurrentPoints[i].y);
            if(BlockImageArr[currentIndex].IsFilled)
            {
                UIManager.Instance.GameOver();
                break;
            }
        }
    }
    private void CheckBlock_Score()
    {

        for(int i = MapHeight- 1; i >= 0; i--)//디버깅좀 편하게 할려구 Y축은 줄여가며 체크합니다
        {
            int count = 0;
            for(int j =0; j < MapWidth; j++)
            {
                int currentIndex = ConvertPos2DTo1D(j,i);

                if(BlockImageArr[currentIndex].IsFilled)
                {
                    count++;
                }

                if(count == MapWidth)
                {
                    for (;j >=0 ; j--)
                    {
                        BlockImageArr[i * MapWidth + j].UnfillBlock();
                    }//점수획득 + 블록지우기
                    PullBlock(i);
                    Score += 1000;
                    UIManager.Instance.SetScore(Score);
                    i++;//해당줄부터 재검사
                    break; 
                }
            }
            if(count == 0)
            {
                break; // 더 검사할 필요도 없으니 맵전체를 검사하지 맙시다.
            }
        }
    }

    private void PullBlock(int InputYPos)
    {
        for(int i = 0; i < MapWidth; i++)
        {
            int currentIndex = 0;//입력받은 Y좌표에서 한칸 위부터 작업시작
            for(int j = InputYPos - 1; j >=0; j-- )
            {
                currentIndex = ConvertPos2DTo1D(i, j);
                if (BlockImageArr[currentIndex].IsFilled)
                {
                    BlockImageArr[currentIndex].UnfillBlock();//지우고
                    BlockImageArr[currentIndex + MapWidth].FillBlock();//아래블록 채우고
                }             
            }
        }
    }

    private int ConvertPos2DTo1D(int InputXPos,int InputYPos)
    {
        return InputYPos * MapWidth + InputXPos;
    }
}
