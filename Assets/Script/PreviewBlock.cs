using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewBlock : MonoBehaviour {

    private Dictionary<BlockID, GameObject> PreviewGameObjectDic;
    private GameObject currentPreviewBlock;

    private void Awake()
    {
        PreviewGameObjectDic = new Dictionary<BlockID, GameObject>();
        for (int i = 0; i < transform.childCount; i++)
        {
            PreviewGameObjectDic.Add((BlockID)i, transform.GetChild(i).gameObject);
        }
    }

    public void SetPreviewBlock(BlockID InputID)
    { 
        if(currentPreviewBlock !=null)
        {
            currentPreviewBlock.SetActive(false);
        }

        currentPreviewBlock = PreviewGameObjectDic[InputID];

        currentPreviewBlock.SetActive(true);
    }
}
