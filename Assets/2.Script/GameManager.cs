using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviourPunCallbacks, IPunObservable
{
    public static GameManager Instance //�̱���
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
        playerHP = new[] { 3, 3 };//ü��
        playerSpeed = new[] { 1, 1 };//�ӵ�
        playerCoolTime = new[] { 5, 5 };//źȯ��Ÿ��
        playerBulletSpeed = new[] { 1, 1 };//źȯ�ӵ�

    }
    public void GameStart()
    {
        photonView.RPC("resetGame", RpcTarget.All);
    }
    public void damageHP(string tagName)//�Ѿ� �¾Ƽ� ü�� ���̴� ���
    {
        if (!PhotonNetwork.IsMasterClient)//ȣ��Ʈ�� �ƴ� �������� ���������� ��� ���� �Ұ�
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
    public void bulletSpeedControl(int shootPlayer)//�Ѿ� �ӵ� ����
    {
        if (!PhotonNetwork.IsMasterClient)//ȣ��Ʈ�� �ƴ� �������� ���������� ��� ���� �Ұ�
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

    public void SpeedControl(int shootPlayer)//�÷��̾� ü�� ����
    {
        if (!PhotonNetwork.IsMasterClient)//ȣ��Ʈ�� �ƴ� �������� ���������� ��� ���� �Ұ�
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

    public void coolTimeControl(int shootPlayer)//�Ѿ� ��Ÿ�� ����
    {
        if (!PhotonNetwork.IsMasterClient)//ȣ��Ʈ�� �ƴ� �������� ���������� ��� ���� �Ұ�
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

    public void HPControl(int shootPlayer)//�÷��̾� ü�� ����
    {
        if (!PhotonNetwork.IsMasterClient)//ȣ��Ʈ�� �ƴ� �������� ���������� ��� ���� �Ұ�
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
    private void RPCUpdateHPText(string player1HPText, string player2HPText)//ȭ�鿡 ü�� �ؽ�Ʈ ���
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
    private void RPCUpdateBSText(string player1BSText, string player2BSText)//ȭ�鿡 źȯ �ӵ� �ؽ�Ʈ ���
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
    private void RPCUpdateSpeedText(string player1SpeedText, string player2SpeedText)//ȭ�鿡 �ӵ� �ؽ�Ʈ ���
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
    private void RPCUpdateCTText(string player1CTText, string player2CTText)//ȭ�鿡 ��Ÿ�� �ؽ�Ʈ ���
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
    public void resetGame()//���� ���� �� �⺻ ����
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
    public void Win()//�¸� ȭ��
    {
        WinPanel.SetActive(true);
    }
    [PunRPC]
    public void Lose()//�й� ȭ��
    {
        LosePanel.SetActive(true);
    }
    [PunRPC]
    public void GoBackLobby()//�κ� �̵�
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
