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

    public int[] CurrentPoints
    {
        get
        {
            return points;
        }
    }

    //현재 좌표들을 기준으로 한줄 위의 좌표들
    public int[] UpPoints
    {
        get
        {
            int[] returnPoints = new int[points.Length];

            for (int i = 0; i < returnPoints.Length; i++)
            {
                returnPoints[i] = points[i] - MapManager.MapWidth;
            }

            return returnPoints;
        }
    }

    //현재 좌표들을 기준으로 한줄 아래의 자표들
    public int[] DownPoints
    {
        get
        {
            int[] returnPoints = new int[points.Length];

            for (int i = 0; i < returnPoints.Length; i++)
            {
                returnPoints[i] = points[i] -  MapManager.MapWidth;
            }

            return returnPoints;
        }
    }

    private BlockID ID;
    private int PivotIndex;
    private int[] points;
    private Color blockColor;


    public void PointsUp()
    {
        for(int i =0; i < points.Length; i++)
        {
            points[i] -= MapManager.MapWidth;
        }
    }

    public void PointsDown()
    {
        for(int i =0; i < points.Length; i ++)
        {
            points[i] += MapManager.MapWidth;
        }
    }

    public void RotateBlock()
    {
        if(ID == BlockID.O)
        {
            return;//O불록일땐 돌리는 의미가 없음!!
        }


        int pivotXPos = points[PivotIndex] % MapManager.MapWidth;
        int pivotYPos = points[PivotIndex] / MapManager.MapWidth;

        
        for (int i = 0; i < points.Length; i++)
        {
            if (i == PivotIndex)
            {
                continue;
            }
            else
            {
                int rotatedXPos = points[i] % MapManager.MapWidth - pivotXPos;
                int rotatedYPos = points[i] / MapManager.MapWidth - pivotYPos;


                rotatedXPos = -rotatedYPos;
                rotatedYPos = +rotatedXPos;
                // x * cos(a) - y * sin(a) = x'
                // x * sin(a) + y * cos(a) = y'  a -> 돌릴각도
                //회전방향은 반시계방향

                rotatedXPos = pivotXPos + rotatedXPos;
                rotatedYPos = pivotYPos + rotatedYPos;

                Debug.Log("X : " + rotatedXPos);
                Debug.Log("Y : " + rotatedYPos);
                points[i] = rotatedYPos * MapManager.MapWidth + rotatedXPos;
            }
        }
    }

    public Block(BlockID InputID)
    {
        ID = InputID;
        points = new int[4];
        switch (ID)
        {
            case BlockID.I:
                {
                    points[0] = 0;
                    points[1] = 1;
                    points[2] = 2;
                    points[3] = 3;
                    PivotIndex = 1;
                    blockColor = Color.red;
                    break;
                }
            case BlockID.J:
                {
                    points[0] = -MapManager.MapWidth;
                    points[1] = 0;
                    points[2] = 1;
                    points[3] = 2;
                    PivotIndex = 2;
                    blockColor = Color.black;
                    break;
                }
            case BlockID.L:
                {
                    points[0] = 0;
                    points[1] = 1;
                    points[2] = 2;
                    points[3] = 2 - MapManager.MapWidth;
                    PivotIndex = 1;
                    blockColor = Color.Lerp(Color.red, Color.grey, 0.5f);
                    break;
                }
            case BlockID.O:
                {
                    points[0] = 0;
                    points[1] = 1;
                    points[2] = -MapManager.MapWidth;
                    points[3] = 1 - MapManager.MapWidth;
                    PivotIndex = -1; //돌리는 의미가 없음
                    blockColor = Color.green;
                    break;
                }
            case BlockID.S:
                {
                    points[0] = 0;
                    points[1] = 1;
                    points[2] = 1 - MapManager.MapWidth;
                    points[3] = 2 - MapManager.MapWidth;
                    PivotIndex = 1;
                    blockColor = Color.yellow;
                    break;
                }
            case BlockID.T:
                {
                    points[0] = 0;
                    points[1] = 1;
                    points[2] = 2;
                    points[3] = 1 - MapManager.MapWidth;
                    PivotIndex = 1;
                    blockColor = Color.blue;
                    break;
                }
            case BlockID.Z:
                {
                    points[0] = -MapManager.MapWidth;
                    points[1] = 1 - MapManager.MapWidth;
                    points[2] = 1;
                    points[3] = 2;
                    PivotIndex = 2;
                    blockColor = Color.cyan;
                    break;
                }
        }
    }
}

