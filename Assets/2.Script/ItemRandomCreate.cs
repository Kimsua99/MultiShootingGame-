using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ItemRandomCreate : MonoBehaviour
{
    public GameObject[] itemArray;
    public bool createStart = false;
    public float Timer = 7f;//������ �ð� 7��
    
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

    public void RandomItem()//���� ������, ���� ��ġ ����
    {
        nowPrefab = itemArray[Random.Range(0, 4)];
        RandomX = Random.Range(-8f, -1f);
        RandomY = Random.Range(-2f, 2f);
    }

    IEnumerator createItem()//������ ����
    {
        RandomItem();
 
        Vector3 nowPos = new Vector3(RandomX, RandomY, 0);
        PhotonNetwork.Instantiate(nowPrefab.name, nowPos, Quaternion.identity);

        yield return new WaitForSeconds(Timer);
        StartCoroutine(createItem());
    }
}
