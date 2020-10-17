using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ItemController : MonoBehaviour
{
    [SerializeField] private AudioClip getSE;
    /// <summary>取得時の効果音</summary>
    PhotonView m_photonView;
    AudioSource audiosource;
    // Start is called before the first frame update
    void Start()
    {
        audiosource = GetComponent<AudioSource>(); //自身のAudioSource取得
        m_photonView = GetComponent<PhotonView>();
    }
    
    void OnTriggerEnter2D(Collider2D collision2D) //Collisionにぶつかったら
    {
        Debug.Log("exぶつかった");
        if (m_photonView.IsMine)
        {
            if (collision2D.gameObject.tag == "Player") //そのCollisionがPlayerだったら
            {
                audiosource.PlayOneShot(getSE);
                Debug.Log("Item取得");
                Destroy(this);
            }
        }
    }
}
