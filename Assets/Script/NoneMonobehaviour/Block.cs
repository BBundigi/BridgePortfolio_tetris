using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block
{
    public Color BlockColor
    {
        get
        {
            return blockColor;
        }
    }

    public Vector2Int[] CurrentPoints
    {
        get
        {
            return points;
        }
    }

    //현재 좌표들을 기준으로 한줄 위의 좌표들
    public Vector2Int[] UpPoints
    {
        get
        {
            Vector2Int[] returnPoints = new Vector2Int[points.Length];

            for (int i = 0; i < returnPoints.Length; i++)
            {
                returnPoints[i] = points[i] - new Vector2Int(0, 1); // 맨위블록의 Y좌표 -> 0, 맨아래블록의 Y좌표 -> MapHeight 
                                                                    // 따라서 더하는게 아니라 빼는게맞음!!
            }

            return returnPoints;
        }
    }

    //현재 좌표들을 기준으로 한줄 아래의 자표들
    public Vector2Int[] DownPoints
    {
        get
        {
            Vector2Int[] returnPoints = new Vector2Int[points.Length];

            for (int i = 0; i < returnPoints.Length; i++)
            {
                returnPoints[i] = points[i] + new Vector2Int(0, 1);
            }

            return returnPoints;
        }
    }


    public Vector2Int[] RightPoints
    {
        get
        {
            Vector2Int[] returnPoints = new Vector2Int[points.Length];

            for (int i = 0; i < returnPoints.Length; i++)
            {
                returnPoints[i] = points[i] + new Vector2Int(1, 0);
            }

            return returnPoints;
        }
    }


    public Vector2Int[] LeftPoints
    {
        get
        {
            Vector2Int[] returnPoints = new Vector2Int[points.Length];

            for (int i = 0; i < returnPoints.Length; i++)
            {
                returnPoints[i] = points[i] - new Vector2Int(1, 0);
            }

            return returnPoints;
        }
    }


    private BlockID ID;
    private int PivotIndex;
    private Vector2Int[] points;
    private Color blockColor;


    public void PointsUp()
    {
        for(int i =0; i < points.Length; i++)
        {
            points[i] = points[i] - new Vector2Int(0, 1);
        }
    }

    public void MovePointsDown()
    {
        for(int i =0; i < points.Length; i ++)
        {
            points[i] = points[i] + new Vector2Int(0, 1);
        }
    }

    public void MovePointsRight()
    {
        for (int i = 0; i < points.Length; i++)
        {
            points[i] = points[i] + new Vector2Int(1, 0);
        }
    }

    public void MovePointsLeft()
    {
        for (int i = 0; i < points.Length; i++)
        {
            points[i] = points[i] - new Vector2Int(1, 0);
        }
    }

    public void RotateBlock()
    {
        if(ID == BlockID.O)
        {
            return;//O불록일땐 돌리는 의미가 없음!!
        }

        
        for (int i = 0; i < points.Length; i++)
        {
            if (i == PivotIndex)
            {
                continue;
            }
            else
            {
                int rotatedXPos = points[i].x - points[PivotIndex].x;
                int rotatedYPos = points[i].y - points[PivotIndex].y;


                rotatedXPos = -rotatedYPos;
                rotatedYPos = +rotatedXPos;
                // x * cos(a) - y * sin(a) = x'
                // x * sin(a) + y * cos(a) = y'  a -> 돌릴각도
                //회전방향은 반시계방향

                rotatedXPos = points[PivotIndex].x + rotatedXPos;
                rotatedYPos = points[PivotIndex].y + rotatedYPos;

                Debug.Log("X : " + rotatedXPos);
                Debug.Log("Y : " + rotatedYPos);
                points[i] = new Vector2Int(rotatedXPos,rotatedYPos);
            }
        }
    }

    public Block(BlockID InputID)
    {
        ID = InputID;
        points = new Vector2Int[4];
        switch (ID)
        {
            case BlockID.I:
                {
                    points[0] = new Vector2Int(3,0);
                    points[1] = new Vector2Int(4, 0);
                    points[2] = new Vector2Int(5, 0);
                    points[3] = new Vector2Int(6, 0);
                    PivotIndex = 1;
                    blockColor = Color.red;
                    break;
                }
            case BlockID.J:
                {
                    points[0] = new Vector2Int(4, 0);
                    points[1] = new Vector2Int(4, 1);
                    points[2] = new Vector2Int(5, 1);
                    points[3] = new Vector2Int(6, 1);
                    PivotIndex = 2;
                    blockColor = Color.black;
                    break;
                }
            case BlockID.L:
                {
                    points[0] = new Vector2Int(4, 1);
                    points[1] = new Vector2Int(5, 1);
                    points[2] = new Vector2Int(6, 1);
                    points[3] = new Vector2Int(6, 0);
                    PivotIndex = 1;
                    blockColor = Color.Lerp(Color.red, Color.grey, 0.5f);
                    break;
                }
            case BlockID.O:
                {
                    points[0] = new Vector2Int(4, 1);
                    points[1] = new Vector2Int(5, 1);
                    points[2] = new Vector2Int(4, 0);
                    points[3] = new Vector2Int(5, 0);
                    PivotIndex = -1; //돌리는 의미가 없음
                    blockColor = Color.green;
                    break;
                }
            case BlockID.S:
                {
                    points[0] = new Vector2Int(4, 1);
                    points[1] = new Vector2Int(5, 1);
                    points[2] = new Vector2Int(5, 0);
                    points[3] = new Vector2Int(6, 0);
                    PivotIndex = 1;
                    blockColor = Color.yellow;
                    break;
                }
            case BlockID.T:
                {
                    points[0] = new Vector2Int(4, 1);
                    points[1] = new Vector2Int(5, 1);
                    points[2] = new Vector2Int(6, 1);
                    points[3] = new Vector2Int(5, 0);
                    PivotIndex = 1;
                    blockColor = Color.blue;
                    break;
                }
            case BlockID.Z:
                {
                    points[0] = new Vector2Int(4, 0);
                    points[1] = new Vector2Int(5, 0);
                    points[2] = new Vector2Int(5, 1);
                    points[3] = new Vector2Int(6, 1);
                    PivotIndex = 2;
                    blockColor = Color.cyan;
                    break;
                }
        }
    }
}

