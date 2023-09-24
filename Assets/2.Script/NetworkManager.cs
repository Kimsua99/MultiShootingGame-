using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public TMP_InputField NickName;
    public GameObject LobbyPanel;
    public Transform[] spawnPos;
    public GameObject playerPrefab;
    public GameObject ItemGenerater;
    public PhotonView PV;
    public GameObject HowToPlay;

    private void Awake()
    {
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30;
        //���̸� ����ȭ�� ���� ��
    }
    public void Connect() => PhotonNetwork.ConnectUsingSettings();
    //���� ������ �����ؼ� �Ʒ��� �Լ��� �ݹ�

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.LocalPlayer.NickName = NickName.text;
        PhotonNetwork.JoinOrCreateRoom("newRoom", new RoomOptions { MaxPlayers = 2 }, null);//�ִ� 2�� �� ����, ������ �Ǹ� �Ʒ� �Լ� �ݹ�
    }

    public override void OnJoinedRoom()//������ ����
    {
        LobbyPanel.SetActive(false);

        SpawnPlayer();
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)//�÷��̾� �ߵ� ���� �� ������ �÷��̾ ���� ����
    {
        StartCoroutine(LeaveEnd(3f));
    }

    public IEnumerator LeaveEnd(float ftime)//�κ� �̵�
    {
        yield return new WaitForSeconds(ftime);
        GameManager.Instance.GoBackLobby();
    }

    public void BtnClick()//���� ����
    {
        if(HowToPlay.activeSelf)
            HowToPlay.SetActive(false);
        else
            HowToPlay.SetActive(true);
    }
    public void SpawnPlayer()//�÷��̾� ��ġ
    {
        var localPlayerIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1;
        var spawnPosition = spawnPos[localPlayerIndex];//�÷��̾� 1�̸� �ε��� 0

        if (localPlayerIndex == 1)
            PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition.position, Quaternion.Euler(180.0f, 0, 0));
        else
            PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition.position, spawnPosition.rotation);
       
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            PV.RPC("startGame", RpcTarget.All);
        }
    }

    [PunRPC]
    public void startGame()//���� ����
    {
        GameManager.Instance.gameStart = true;
        ItemGenerater.SetActive(true);
        GameManager.Instance.GameStart();
    }

}
