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
        //높이면 동기화가 빨리 됨
    }
    public void Connect() => PhotonNetwork.ConnectUsingSettings();
    //포톤 서버에 접속해서 아래의 함수를 콜백

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.LocalPlayer.NickName = NickName.text;
        PhotonNetwork.JoinOrCreateRoom("newRoom", new RoomOptions { MaxPlayers = 2 }, null);//최대 2명 방 생성, 접속이 되면 아래 함수 콜백
    }

    public override void OnJoinedRoom()//본게임 시작
    {
        LobbyPanel.SetActive(false);

        SpawnPlayer();
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)//플레이어 중도 퇴장 시 나머지 플레이어도 게임 퇴장
    {
        StartCoroutine(LeaveEnd(3f));
    }

    public IEnumerator LeaveEnd(float ftime)//로비 이동
    {
        yield return new WaitForSeconds(ftime);
        GameManager.Instance.GoBackLobby();
    }

    public void BtnClick()//게임 설명서
    {
        if(HowToPlay.activeSelf)
            HowToPlay.SetActive(false);
        else
            HowToPlay.SetActive(true);
    }
    public void SpawnPlayer()//플레이어 배치
    {
        var localPlayerIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1;
        var spawnPosition = spawnPos[localPlayerIndex];//플레이어 1이면 인덱스 0

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
    public void startGame()//게임 시작
    {
        GameManager.Instance.gameStart = true;
        ItemGenerater.SetActive(true);
        GameManager.Instance.GameStart();
    }

}
