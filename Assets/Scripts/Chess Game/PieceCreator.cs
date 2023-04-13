using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;
using Photon.Pun;
using Photon.Realtime;

public class PieceCreator : MonoBehaviour
{
    [SerializeField] private GameObject[] piecesPrefabs;
    [SerializeField] private Material blackMaterial;
    [SerializeField] private Material whiteMaterial;
    private Dictionary<string, GameObject> nameToPieceDict = new Dictionary<string, GameObject>();

    private void Awake()
    {
        foreach (var piece in piecesPrefabs)
        {
            nameToPieceDict.Add(piece.GetComponent<Piece>().GetType().ToString(), piece);
        }
    }


    public GameObject CreatePiece(Type type)
    {
        GameObject prefab = nameToPieceDict[type.ToString()];
        if (prefab)
        {
            GameObject newPiece = Instantiate(prefab);
            Vector3 scaleChange = new Vector3(-0.9f, -0.9f, -0.9f);
            newPiece.transform.localScale += scaleChange;
            return newPiece;
        }
        return null;
    }

    public GameObject[] GetPrefabs()
    {
        return piecesPrefabs;
    }

    public GameObject CreateNetworkPiece(Type type, int layoutIndex)
    {
        GameObject prefab = nameToPieceDict[type.ToString()];
        if (prefab)
        {
            object[] InstantiationData = new object[1];
            InstantiationData[0] = layoutIndex;
            GameObject newPiece = PhotonNetwork.Instantiate(prefab.name, new Vector3(0f, 0f, 0f), prefab.transform.rotation, 0, InstantiationData);
            //Vector3 scaleChange = new Vector3(-0.9f, -0.9f, -0.9f);
            //newPiece.transform.localScale += scaleChange;
            return newPiece;
        }
        return null;
    }

    public Material GetTeamMaterial(TeamColor team)
    {
        return team == TeamColor.White ? whiteMaterial : blackMaterial;
    }
}
