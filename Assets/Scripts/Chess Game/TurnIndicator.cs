using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Input;

public class TurnIndicator : MonoBehaviour
{
    [SerializeField] private Material BlackTeam;
    [SerializeField] private Material WhiteTeam;
    [SerializeField] private Renderer BallRenderer;
    public ChessGameController controller;


    void Start ()
    {
        BallRenderer = GetComponent<Renderer>();
        BallRenderer.enabled = true;
        BallRenderer.material.color = Color.white;
    }
    public void SetDependencies(ChessGameController chessController)
    {
        this.controller = chessController;
    }
    public void ColourTeam() { // used to show whose turn it is... a simple implementation
        //Debug.Log("made it");
        if (BallRenderer.material.color == Color.white)
        {
            BallRenderer.material.color = Color.black;
        } else if (BallRenderer.material.color == Color.black)
        {
            BallRenderer.material.color = Color.white;
        }
    }

}
