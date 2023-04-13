using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;

public class AddComponents : MonoBehaviour, IPunInstantiateMagicCallback
{
    private PhotonView photonView;
    private GameObject gameMaster;

    public void Start()
    {
        photonView = this.gameObject.GetComponent<PhotonView>();
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        gameMaster = GameObject.Find("GameMaster");
        object[] data = info.photonView.InstantiationData;
        int layoutIndex = (int)data[0];
        //Debug.Log(layoutIndex.ToString());
        //Debug.Log(gameMaster.ToString());
        gameMaster.GetComponent<ChessGameController>().NetworkInitialisePieces(layoutIndex, this.gameObject);
               
    }
   
    
}
