using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;

[RequireComponent(typeof(PieceCreator))]
public class ChessGameController : MonoBehaviour
{
    [SerializeField] private BoardLayout startingBoardLayout;
    [SerializeField] private Board board;
    private PieceCreator pieceCreator;

     private void Awake()
    {
        SetDependencies();
    }

    private void SetDependencies()
    {
        pieceCreator = GetComponent<PieceCreator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartNewGame();
    }

     private void StartNewGame()
    {
        CreatePiecesFromLayout(startingBoardLayout);
    }


    private void CreatePiecesFromLayout(BoardLayout layout)
    {
        for (int i = 0; i < layout.GetPiecesCount(); i++)
        {
            Vector2Int squareCoords = layout.GetSquareCoordsAtIndex(i);
            TeamColor team = layout.GetSquareTeamColorAtIndex(i);
            string typeName = layout.GetSquarePieceNameAtIndex(i);

            Type type = Type.GetType(typeName);
            CreatePieceAndInitialize(squareCoords, team, type);
        }
    }

    private void CreatePieceAndInitialize(Vector2Int squareCoords, TeamColor team, Type type)
    {
        Piece newPiece = pieceCreator.CreatePiece(type).GetComponent<Piece>();
        //make each piece interactable with AR
        newPiece.gameObject.AddComponent<BoxCollider>();
        newPiece.gameObject.AddComponent<NearInteractionGrabbable>();
        newPiece.gameObject.AddComponent<ObjectManipulator>();

        newPiece.SetData(squareCoords, team, board);


        Material teamMaterial = pieceCreator.GetTeamMaterial(team);
        newPiece.SetMaterial(teamMaterial);
        if (team == TeamColor.Black) {
            newPiece.transform.Rotate(0.0f, 180.0f, 0.0f, Space.Self);
        }
    }
}
