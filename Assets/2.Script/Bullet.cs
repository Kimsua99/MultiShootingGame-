using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Bullet : MonoBehaviourPunCallbacks, IPunObservable
{
    public PhotonView PV;

    private void Start()
    {
        if (!PV.IsMine)
        {
            tag = "Bullet2";
        }
    }
    private void Update()
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            if(PV.IsMine)
                transform.Translate(Vector3.up * Time.deltaTime * GameManager.Instance.playerBulletSpeed[0]);
            else
                transform.Translate(Vector3.down * Time.deltaTime * GameManager.Instance.playerBulletSpeed[1]);

        }
        else
        {
            if(PV.IsMine)
                transform.Translate(Vector3.down * Time.deltaTime * GameManager.Instance.playerBulletSpeed[1]);
            else
                transform.Translate(Vector3.up * Time.deltaTime * GameManager.Instance.playerBulletSpeed[0]);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)//ÃÑ¾Ë ¸íÁß
    {
        switch (collision.tag)
        {
            case "wall":
                Debug.Log("1");
                PV.RPC("DestroyRPC", RpcTarget.All);
                break;

            case "Player":
                collision.GetComponent<PlayerScript>().Hit("Player");
                PV.RPC("DestroyRPC", RpcTarget.All);
                break;

            case "Player2":
                collision.GetComponent<PlayerScript>().Hit("Player2");
                PV.RPC("DestroyRPC", RpcTarget.All);
                break;

            case "bulletSpeedItem":
                Destroy(collision.gameObject);
                PV.RPC("DestroyRPC", RpcTarget.All);
                int pb;
                if (tag == "Bullet") pb = 0;
                else pb= 1;
                GameManager.Instance.bulletSpeedControl(pb);
                break;

            case "SpeedItem":
                Destroy(collision.gameObject);
                PV.RPC("DestroyRPC", RpcTarget.All);
                int ps;
                if (tag == "Bullet") ps = 0;
                else ps = 1;
                GameManager.Instance.SpeedControl(ps);
                break;

            case "HPHeelItem":
                Destroy(collision.gameObject);
                PV.RPC("DestroyRPC", RpcTarget.All);
                int ph;
                if (tag == "Bullet") ph = 0;
                else ph = 1;
                GameManager.Instance.HPControl(ph);
                break;

            case "CoolTimeItem":
                Destroy(collision.gameObject);
                PV.RPC("DestroyRPC", RpcTarget.All);
                int pc;
                if (tag == "Bullet") pc = 0;
                else pc = 1;
                GameManager.Instance.coolTimeControl(pc);
                break;
        }
    }

    [PunRPC]
    void DestroyRPC()
    {
        Destroy(gameObject);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
        }
        else if (stream.IsReading)
        {
            transform.position = (Vector3)stream.ReceiveNext();
        }
    }
}
