using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ItemRandomCreate : MonoBehaviour
{
    public GameObject[] itemArray;
    public bool createStart = false;
    public float Timer = 7f;//리스폰 시간 7초
    
    public GameObject nowPrefab;
    float RandomX;
    float RandomY;

    public PhotonView PV;
    public void OnEnable()
    {
        startScript();
    }

    public void startScript()
    {
        if(PhotonNetwork.IsMasterClient)
            StartCoroutine("createItem");
    }

    public void RandomItem()//랜덤 프리팹, 랜덤 위치 결정
    {
        nowPrefab = itemArray[Random.Range(0, 4)];
        RandomX = Random.Range(-8f, -1f);
        RandomY = Random.Range(-2f, 2f);
    }

    IEnumerator createItem()//아이템 생성
    {
        RandomItem();
 
        Vector3 nowPos = new Vector3(RandomX, RandomY, 0);
        PhotonNetwork.Instantiate(nowPrefab.name, nowPos, Quaternion.identity);

        yield return new WaitForSeconds(Timer);
        StartCoroutine(createItem());
    }
}
