using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;

public class PieceCreator : MonoBehaviour
{
     [SerializeField] private GameObject[] piecesPrefabs;
    [SerializeField] private Material blackMaterial;
    [SerializeField] private Material whiteMaterial;
    private Dictionary<string, GameObject> nameToPieceDict = new Dictionary<string, GameObject>();

    private void Awake()
    {
        //fill dictionary with piece type names as keys and piece prefabs as values
        foreach (var piece in piecesPrefabs)
        {
            nameToPieceDict.Add(piece.GetComponent<Piece>().GetType().ToString(), piece);
        }
    }


    public GameObject CreatePiece(Type type)
    {
        //choose prefab based on type passed as parameter
        GameObject prefab = nameToPieceDict[type.ToString()];
        if (prefab)
        {
            GameObject newPiece = Instantiate(prefab);
            Vector3 scaleChange = new Vector3(-0.8f, -0.8f, -0.8f);
            newPiece.transform.localScale += scaleChange;
            return newPiece;
        }
        return null;
    }

    //return team material based on team passed as parameter
    public Material GetTeamMaterial(TeamColor team)
    {
        return team == TeamColor.White ? whiteMaterial : blackMaterial;
    }
}
