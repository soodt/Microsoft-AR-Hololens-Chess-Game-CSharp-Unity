using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SquareSelector))]
public class Board : MonoBehaviour
{
    [SerializeField] private Transform bottomLeftSquareTransform;
    [SerializeField] public float squareSize;
    public ChessGameController controller;

    public Vector3 CalculatePositionFromCoords(Vector2Int coords)
    {
        
        return bottomLeftSquareTransform.position + new Vector3(coords.x * squareSize, 0f, coords.y * squareSize);
    }

    public void SetDependencies(ChessGameController chessController)
    {
        this.controller = chessController;
    }

    public Piece getPiece(Vector2Int coords)
    {

        for (int i = 0; i < 32; i++)
        {
            if (controller.activePieces[i] != null)
            {
                if (controller.activePieces[i].occupiedSquare == coords)
                {
                    return controller.activePieces[i];
                }
            }
        }
        return null;
    }


    public void takePiece(Piece itself, Vector2Int coords)
    {

        Piece pieceTaken = getPiece(coords);
        //Debug.Log("here");

        for (int i = 0; i < 32; i++)
        {
            /* if the piece is in activePieces, it is the same Piece as the one at given coords,
               it is not the piece we are looking to move, and the two pieces are not on the same team
            */
            if ((controller.activePieces[i] != null) && (controller.activePieces[i] == pieceTaken) && (pieceTaken != itself) && (!pieceTaken.IsFromSameTeam(itself)))
            {
                controller.activePieces[i] = null;
                Destroy(pieceTaken.gameObject);
                break;
            }
        }
    }

}