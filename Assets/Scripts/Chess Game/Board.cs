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


    public void takePiece(Vector2Int coords)
    {

        Piece pieceTaken = getPiece(coords);
        //Debug.Log("here");

        for (int i = 0; i < 32; i++)
        {
            if (controller.activePieces[i] != null)
            {
                if (controller.activePieces[i] == pieceTaken)
                {
                    controller.activePieces[i] = null;
                    break;
                }
            }
        }
               Destroy(pieceTaken.gameObject);
    }

}