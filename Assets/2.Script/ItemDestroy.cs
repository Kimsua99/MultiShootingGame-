using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ItemDestroy : MonoBehaviourPunCallbacks, IPunObservable
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("startDestroy", 4f);
    }

    public void startDestroy()
    {
        photonView.RPC("destroyItem", RpcTarget.All);
    }

    [PunRPC]
    public void destroyItem()
    {
        Destroy(gameObject);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
    }
}
