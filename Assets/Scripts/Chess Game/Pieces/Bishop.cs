using Microsoft.MixedReality.Toolkit.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bishop : Piece
{
    /*
    public UnityEvent onGrabStarted;

    private NearInteractionGrabbable grabbable;

    private void Start()
    {
        grabbable = GetComponent<NearInteractionGrabbable>();
        grabbable.OnGrabStarted.AddListener(HandleGrabStarted); // Starting to try to implement highlightSquare when the piece is picked up!
    }

    private void HandleGrabStarted(GrabEventData eventData)
    {
        onGrabStarted.Invoke();
    }
    */
    public override List<Vector2Int> SelectAvailableSquares()
    {
        throw new NotImplementedException();
    }

    // Checks to see if there is a piece in the way
    public bool canMoveThere(Vector2Int coords) {
        int xPos = this.occupiedSquare.x;
        int yPos = this.occupiedSquare.y;  
        int yModifier = 1;
        if (yPos > coords.y) {
                yModifier = -1;
            }
        if (xPos < coords.x) {
            for (xPos = this.occupiedSquare.x; xPos <= coords.x; xPos++) {
                Piece temp = board.getPiece(new Vector2Int(xPos, yPos));
                /* If there is a piece at (xPos, yPos), it is not the bishop itself
                   They are not from the same team, and the (xPos, yPos) == coords (the exact square the piece wants to move to)
                   Then let the bishop move there
                   Else, not allowed move there
                */
                if (temp && temp != this) {
                    if (!temp.IsFromSameTeam(this) && (xPos == coords.x && yPos == coords.y)) {
                        return true;
                    }

                    return false;
                }
                yPos+= yModifier;
            }
        } else if (xPos > coords.x){
            for (xPos = this.occupiedSquare.x; xPos >= coords.x; xPos--) {
                Piece temp = board.getPiece(new Vector2Int(xPos, yPos));
                if (temp && temp != this) {
                    if (!temp.IsFromSameTeam(this) && (xPos == coords.x && yPos == coords.y)) {
                        return true;
                    }

                    return false;
                }
                yPos += yModifier;
            }
        }
        return true;
    }

    public override bool isAttackingSquare(Vector2Int coords) {
        return canMoveThere(coords);
    }

    public override void MovePiece(Vector2Int coords)
    {
        if (!taken)
        {
            if (this.getTeam() == controller.getActivePlayer().getTeam() && this.availableMoves.Contains(coords))
            {
                Vector2Int displacement = coords - this.occupiedSquare;
                bool capture = false;

                if (coords != this.occupiedSquare && System.Math.Abs(displacement.x) == System.Math.Abs(displacement.y) && canMoveThere(coords))
                {
                    Piece pieceCheck = board.getPiece(coords);
                    if (pieceCheck)
                    {
                        board.takePiece(this, coords);
                        capture = true;
                    }
                    this.occupiedSquare = coords;
                    transform.position = this.board.CalculatePositionFromCoords(coords);
                    print(AlgebraicNotation(coords, coords, capture, false, false, false));
                    controller.endTurn();
                    //moved = true;
                }
                else
                {
                    transform.position = this.board.CalculatePositionFromCoords(this.occupiedSquare);
                }
            } else
            {
                // If not this team's turn, snap back to occupied square
                transform.position = this.board.CalculatePositionFromCoords(this.occupiedSquare);
                Debug.Log("NoMoving!");
            }
        }
         else
        {
            transform.position = finalCoords;
        }
    }

    public override void PossibleMoves()
    {
        availableMoves.Clear();
        if (!taken)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Vector2Int square = new Vector2Int(i, j);
                    Vector2Int displacementToSquare = square - this.occupiedSquare; // this is to go through all the squares checking which are safe to move to
                    if (square != this.occupiedSquare && squareIsMoveable(displacementToSquare) && canMoveThere(square)) // this should be implemented when the obj is picked up to highlight the possible squares. 
                    {
                        availableMoves.Add(square);
                    }
                }
            }
        }
    }

    private bool squareIsMoveable(Vector2Int displacementToSquare)
    {
        if (System.Math.Abs(displacementToSquare.x) == System.Math.Abs(displacementToSquare.y))// checks if the piece can move
        {
           // Debug.Log("Turn Green");
            return true;
        }
        
        
        return false;
    }
    public override bool hasMovedTwoSquares()
    {
        return false;
    }
    public override String AlgebraicNotation(Vector2Int coords, Vector2Int prevCoords, bool capture, bool pawnPromote, bool enPassant, bool castle)
    {
        String s = "B";

        foreach (Piece p in controller.getActivePlayer().activePieces)
        {
            if (!p.taken)
            {
                if (p.typeName == "Bishop" && p != this)
                {
                    if (p.CanMoveTo(coords))
                    {
                        if (prevCoords.x != p.occupiedSquare.x)
                        {
                            if (prevCoords.x == 0) s += "a";
                            if (prevCoords.x == 1) s += "b";
                            if (prevCoords.x == 2) s += "c";
                            if (prevCoords.x == 3) s += "d";
                            if (prevCoords.x == 4) s += "e";
                            if (prevCoords.x == 5) s += "f";
                            if (prevCoords.x == 6) s += "g";
                            if (prevCoords.x == 7) s += "h";
                        }
                        else
                        {
                            s += prevCoords.y + 1;
                        }
                    }
                }
            }
        }

        if (capture) s += "x";
        if (coords.x == 0) s += "a";
        if (coords.x == 1) s += "b";
        if (coords.x == 2) s += "c";
        if (coords.x == 3) s += "d";
        if (coords.x == 4) s += "e";
        if (coords.x == 5) s += "f";
        if (coords.x == 6) s += "g";
        if (coords.x == 7) s += "h";
        s += coords.y + 1;
        if (controller.checkmate()) s += "#";
        else if (controller.checkCond()) s += "+";
        return s;
    }

}