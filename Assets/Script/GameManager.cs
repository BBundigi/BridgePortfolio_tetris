using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private float DelayTime; //대기시간

    [SerializeField]
    private MapManager mapManager;

    private void Awake()
    {
        DelayTime = 1.0f;
    }
    private void Start()
    {
        StartCoroutine(WaitAndGameStart());
    }

    private IEnumerator WaitAndGameStart()
    {
        yield return new WaitForSeconds(3);

        StartCoroutine(MainGameLogic());
    }
    
    private IEnumerator MainGameLogic()
    {
        var WaitTime = new WaitForSeconds(DelayTime);

        mapManager.SetCurrentBlock((BlockID)Random.Range(0, 7));//썩 좋아하는 방법은 아니지만 아무튼...
        while (true)
        {
            yield return (WaitTime);

            if (mapManager.PushBlockDown())
            {
                yield return (WaitTime);
                mapManager.SetCurrentBlock((BlockID)Random.Range(0, 7));
            }
        }
    }
}
