using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockImage : MonoBehaviour {
    private Image image;
    
    public bool IsFilled
    {
        get
        {
            return isFilled;
        }
    }

    private bool isFilled;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void SetColor(Color InputColor)
    {
        image.color = InputColor;
    }

    public void FillBlock()
    {
        if(IsFilled)
        {
            Debug.Log("이미 채워진 블록을 또 채울려고함!! 디버깅 수고~~");
            gameObject.SetActive(false); //For Debugging;
            return;
        }
        isFilled = true;
        image.color = Color.gray;
    }

    public void UnfillBlock()
    {
        if(!IsFilled)
        {
            Debug.Log("이미 지워진 블록을 또 지울려고함!! 디버깅 수고~~");
            gameObject.SetActive(false); //For Debugging;
            return;
        }
        isFilled = false;
        image.color = Color.white;
    }
}
