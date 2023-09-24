using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviourPunCallbacks, IPunObservable
{
    public SpriteRenderer SR;
    public Rigidbody2D RB;
    public PhotonView PV;
    public TextMeshProUGUI NickName;
    public float leftTime;
    Image CoolTimeImg;
    public bool isSkillCharged = true;
    Transform P2;
    public void Start()
    {
        P2 = GameObject.Find("PlayerPosition").transform.GetChild(1);

        CoolTimeImg = GameObject.Find("BulletCoolTime").GetComponent<Image>();
        isSkillCharged = true;
         
        if (PV.IsMine)//플레이어 기준 닉네임, 색 적용
        {
            NickName.text = PhotonNetwork.NickName;
            NickName.color = Color.green;
            SR.color = Color.green;
            if (transform.position == P2.position)
            {
                transform.GetChild(0).GetChild(0).Rotate(new Vector3(180f, 0f, 0f));
            }
        }
        else//상대 기준 닉네임, 색 적용
        {
            NickName.text = PV.Owner.NickName;
            NickName.color = Color.red;
            SR.color = Color.red;
            tag = "Player2";
            if (transform.position == P2.position)
            {
                transform.GetChild(0).GetChild(0).Rotate(new Vector3(180f, 0f, 0f));
            }
        }
    }
    void Update()
    {
        if (!PV.IsMine)
        {
            return;
        }

        if (GameManager.Instance.gameStart)
        {
            var dist = Input.GetAxis("Horizontal") * Time.deltaTime;

            if (tag == "Player")
            {
                if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))//왼쪽 이동
                {
                    dist = GameManager.Instance.playerSpeed[0] * Time.deltaTime;
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))//오른쪽 이동
                {
                    dist = -1 * GameManager.Instance.playerSpeed[0] * Time.deltaTime;
                }

                transform.Translate(new Vector3(dist, 0, 0));
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))//왼쪽 이동
                {
                    dist = GameManager.Instance.playerSpeed[1] * Time.deltaTime;
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))//오른쪽 이동
                {
                    dist = -1 * GameManager.Instance.playerSpeed[1] * Time.deltaTime;
                }

                transform.Translate(new Vector3(dist, 0, 0));
            }

            if (Input.GetKeyDown(KeyCode.Space))//총알 발사
            {
                if (isSkillCharged)
                {
                    UseSkill();
                }
            }
        }
    }
    public void Hit(string tagName)
    {
        GameManager.Instance.damageHP(tagName);
    }
    public void UseSkill()//총알 발사
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            PhotonNetwork.Instantiate("bullet", transform.position + new Vector3(0f, 0.8f, 0f), Quaternion.identity);
        }
        else
        {
            PhotonNetwork.Instantiate("bullet", transform.position + new Vector3(0f, -0.8f, 0f), Quaternion.identity);
        }
        isSkillCharged = false;

        CoolTimeImg.fillAmount = 1;
        StartCoroutine(SkillCharging());
    }

    IEnumerator SkillCharging()//사용중
    {
        while (CoolTimeImg.fillAmount > 0)
        {
            CoolTimeImg.fillAmount -= Time.smoothDeltaTime / leftTime;
            yield return null;
        }
        StartCoroutine(ResetSkill());
    }

    IEnumerator ResetSkill()//쿨타임
    {
        while (CoolTimeImg.fillAmount < 1)
        {
            if (photonView.IsMine)
            {
                CoolTimeImg.fillAmount += Time.smoothDeltaTime / GameManager.Instance.playerCoolTime[0];
            }
            else
            {
                CoolTimeImg.fillAmount += Time.smoothDeltaTime / GameManager.Instance.playerCoolTime[1];
            }
            yield return null;
        }
        isSkillCharged = true;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
  
    }
}
