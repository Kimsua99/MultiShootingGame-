using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviourPunCallbacks, IPunObservable
{
    public static GameManager Instance //싱글톤
    {
        get 
        {
            if (instance == null) instance = FindObjectOfType<GameManager>();

            return instance;
        }
    }

    private static GameManager instance;

    public TextMeshProUGUI p1hpNum;
    public TextMeshProUGUI p2hpNum;
    public TextMeshProUGUI p1SpeedNum;
    public TextMeshProUGUI p2SpeedNum;
    public TextMeshProUGUI p1BSNum;
    public TextMeshProUGUI p2BSNum;
    public TextMeshProUGUI p1BCTNum;
    public TextMeshProUGUI p2BCTNum;

    public int[] playerHP;
    public int[] playerSpeed;
    public int[] playerCoolTime;
    public int[] playerBulletSpeed;

    public PhotonView PV;

    public GameObject LosePanel;
    public GameObject WinPanel;
    public GameObject LobbyPanel;
    public GameObject ItemGenerater;

    public bool gameStart = false;

    private void Start()
    {
        playerHP = new[] { 3, 3 };//체력
        playerSpeed = new[] { 1, 1 };//속도
        playerCoolTime = new[] { 5, 5 };//탄환쿨타임
        playerBulletSpeed = new[] { 1, 1 };//탄환속도

    }
    public void GameStart()
    {
        photonView.RPC("resetGame", RpcTarget.All);
    }
    public void damageHP(string tagName)//총알 맞아서 체력 깎이는 경우
    {
        if (!PhotonNetwork.IsMasterClient)//호스트가 아닌 측에서는 내부적으로 계산 실행 불가
        {
            return;
        }
        int playerNumber = 0;

        if (tagName == "Player")
            playerNumber = 0;
        if (tagName == "Player2")
            playerNumber = 1;

        playerHP[playerNumber] -= 1;

        photonView.RPC("RPCUpdateHPText", RpcTarget.All,
            playerHP[0].ToString(), playerHP[1].ToString());

        if (playerHP[0] == 0)
        {
            photonView.RPC("Lose", RpcTarget.MasterClient);
            photonView.RPC("Win", RpcTarget.Others);
            gameStart = false;
            ItemGenerater.SetActive(false);
            photonView.RPC("resetGame", RpcTarget.All);
        }
        else if(playerHP[1] == 0)
        {
            photonView.RPC("Lose", RpcTarget.Others);
            photonView.RPC("Win", RpcTarget.MasterClient);
            gameStart = false;
            ItemGenerater.SetActive(false);
            photonView.RPC("resetGame", RpcTarget.All);
        }
    }
    public void bulletSpeedControl(int shootPlayer)//총알 속도 증가
    {
        if (!PhotonNetwork.IsMasterClient)//호스트가 아닌 측에서는 내부적으로 계산 실행 불가
        {
            return;
        }

        if (playerBulletSpeed[shootPlayer] <= 5)
            playerBulletSpeed[shootPlayer] += 1;
        else
            return;
        photonView.RPC("RPCUpdateBSText", RpcTarget.All,
            playerBulletSpeed[0].ToString(), playerBulletSpeed[1].ToString());
    }

    public void SpeedControl(int shootPlayer)//플레이어 체력 증가
    {
        if (!PhotonNetwork.IsMasterClient)//호스트가 아닌 측에서는 내부적으로 계산 실행 불가
        {
            return;
        }

        if (playerSpeed[shootPlayer] <= 5)
            playerSpeed[shootPlayer] += 1;
        else
            return;
        photonView.RPC("RPCUpdateSpeedText", RpcTarget.All,
            playerSpeed[0].ToString(), playerSpeed[1].ToString());
    }

    public void coolTimeControl(int shootPlayer)//총알 쿨타임 감소
    {
        if (!PhotonNetwork.IsMasterClient)//호스트가 아닌 측에서는 내부적으로 계산 실행 불가
        {
            return;
        }

        if (playerCoolTime[shootPlayer] >= 1)
            playerCoolTime[shootPlayer] -= 1;
        else
            return;
        photonView.RPC("RPCUpdateCTText", RpcTarget.All,
            playerCoolTime[0].ToString(), playerCoolTime[1].ToString());
    }

    public void HPControl(int shootPlayer)//플레이어 체력 증가
    {
        if (!PhotonNetwork.IsMasterClient)//호스트가 아닌 측에서는 내부적으로 계산 실행 불가
        {
            return;
        }

        if (playerBulletSpeed[shootPlayer] <= 3)
            playerBulletSpeed[shootPlayer] += 1;
        else
            return;
        photonView.RPC("RPCUpdateHPText", RpcTarget.All,
            playerHP[0].ToString(), playerHP[1].ToString());
    }

    [PunRPC]
    private void RPCUpdateHPText(string player1HPText, string player2HPText)//화면에 체력 텍스트 띄움
    {
        if (PV.IsMine)
        {
            p1hpNum.text = player1HPText;
            p2hpNum.text = player2HPText;
        }
        else
        {
            p1hpNum.text = player2HPText;
            p2hpNum.text = player1HPText;
        }
    }

    [PunRPC]
    private void RPCUpdateBSText(string player1BSText, string player2BSText)//화면에 탄환 속도 텍스트 띄움
    {
        if (PV.IsMine)
        {
            p1BSNum.text = player1BSText;
            p2BSNum.text = player2BSText;
        }
        else
        {
            p1BSNum.text = player2BSText;
            p2BSNum.text = player1BSText;
        }
    }

    [PunRPC]
    private void RPCUpdateSpeedText(string player1SpeedText, string player2SpeedText)//화면에 속도 텍스트 띄움
    {
        if (PV.IsMine)
        {
            p1SpeedNum.text = player1SpeedText;
            p2SpeedNum.text = player2SpeedText;
        }
        else
        {
            p1SpeedNum.text = player2SpeedText;
            p2SpeedNum.text = player1SpeedText;
        }
    }

    [PunRPC]
    private void RPCUpdateCTText(string player1CTText, string player2CTText)//화면에 쿨타임 텍스트 띄움
    {
        if (PV.IsMine)
        {
            p1BCTNum.text = player1CTText;
            p2BCTNum.text = player2CTText;
        }
        else
        {
            p1BCTNum.text = player2CTText;
            p2BCTNum.text = player1CTText;
        }
    }

    [PunRPC]
    public void resetGame()//게임 시작 시 기본 세팅
    {
        playerHP[0] = 3; playerHP[1] = 3;
        playerBulletSpeed[0] = 1; playerBulletSpeed[1] = 1;
        playerSpeed[0] = 1; playerSpeed[1] = 1;
        playerCoolTime[0] = 5; playerCoolTime[1] = 5;

        p1hpNum.text = "3"; p2hpNum.text = "3";
        p1SpeedNum.text = "1"; p2SpeedNum.text = "1";
        p1BSNum.text = "1"; p2BSNum.text = "1";
        p1BCTNum.text = "5"; p2BCTNum.text = "5";
    }

    [PunRPC]
    public void Win()//승리 화면
    {
        WinPanel.SetActive(true);
    }
    [PunRPC]
    public void Lose()//패배 화면
    {
        LosePanel.SetActive(true);
    }
    [PunRPC]
    public void GoBackLobby()//로비 이동
    {
        ItemGenerater.SetActive(false);
        WinPanel.SetActive(false);
        LosePanel.SetActive(false);
        PhotonNetwork.Disconnect();
        LobbyPanel.SetActive(true);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
      
    }
}
