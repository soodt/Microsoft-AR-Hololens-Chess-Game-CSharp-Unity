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
    private SquareSelector squareSelector;

    private void Awake()
    {
        squareSelector= GetComponent<SquareSelector>();
    }

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


    public bool takePiece(Piece itself, Vector2Int coords)
    {
        bool isTaken = false;
        Piece pieceTaken = getPiece(coords);
        for (int i = 0; i < 32; i++)
        {
            /* if the piece is in activePieces, it is the same Piece as the one at given coords,
               it is not the piece we are looking to move, and the two pieces are not on the same team
            */
            if ((controller.activePieces[i] != null) && (controller.activePieces[i] == pieceTaken) && (pieceTaken != itself) && (!pieceTaken.IsFromSameTeam(itself)))
            {
                controller.hasCaptured = true;
                controller.recordPieceRemoval(pieceTaken);               // adds taken piece to takenPieces list of player who took the piece and removes the taken piece from the activePieces list of the other player
                controller.activePieces[i] = null;
                double boardX = this.transform.position.x;
                double boardY = this.transform.position.y;
                double boardZ = this.transform.position.z;
                isTaken = true;

                // Making it so after being taken pieces appear beside the table
                if (pieceTaken.getTeam() == TeamColor.White)
                {
                    //pieceTaken.setOut(pieceTaken);
                    double zOffset = boardZ-1 + (0.20 * (15 - controller.whitePlayer.activePieces.Count));
                    double xOffset = boardX-1;
                    if (controller.whitePlayer.activePieces.Count<=8)
                    {
                        zOffset = boardZ -1 + (0.20 * (8 - controller.whitePlayer.activePieces.Count));
                        xOffset = boardX -1.30;
                    }
                    Vector3 finalCoord = new Vector3((float)xOffset, (float)boardY, (float)zOffset);
                    pieceTaken.transform.position = finalCoord;
                    pieceTaken.finalCoords = finalCoord;
                    //  pieceTaken.GetComponent<NearInteractionGrabbable>().enabled = false;
                    pieceTaken.taken = true;
                }
                else
                {
                    double zOffset = boardZ + 0.1890001 - (0.20 * (15 - controller.blackPlayer.activePieces.Count));
                    double xOffset = boardX + 0.8;
                    if (controller.blackPlayer.activePieces.Count <= 8)
                    {
                        zOffset = boardZ + 0.1890001 - (0.20 * (8 - controller.blackPlayer.activePieces.Count));
                        xOffset = boardX + 1.1;
                    }
                    Vector3 finalCoord = new Vector3((float)xOffset, (float)boardY, (float)zOffset);
                    pieceTaken.transform.position = finalCoord;
                    pieceTaken.finalCoords = finalCoord;
                    // pieceTaken.GetComponent<NearInteractionGrabbable>().enabled = false;
                    pieceTaken.taken = true;
                }
                // pieceTaken.GetComponent<MeshRenderer>().enabled = false; // turns piece invisible instead of destroying it
                //Destroy(pieceTaken.gameObject);
                break;
            }
        }
        return isTaken;
    }

    //to highlight the tiles that can be taken
    public void HightlightTiles(List<Vector2Int> selection)
    {
        Dictionary<Vector3, bool> squaresInfo = new Dictionary<Vector3, bool>();
        for (int i = 0; i < selection.Count; i++)
        {
            Vector3 position = CalculatePositionFromCoords(selection[i]);// counts through all available place - (at the moment everything is set to true)
            if (getPiece(selection[i]) != null)
            {
                squaresInfo.Add(position, false);
            }
            else
            {
                squaresInfo.Add(position, true); // eventually true can be free places and false can be places with opponents on
            }
        }
        squareSelector.ShowSelection(squaresInfo);
    }


}