using Microsoft.MixedReality.Toolkit.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Pawn : Piece, IMixedRealityPointerHandler
{
    Piece promotionQueen;
    string pQueenName;
    Type pQueenType;
    private void start()
    {
        promotionQueen = board.getPiece(new Vector2Int(3, 0));
        pQueenName = promotionQueen.typeName;
        pQueenType = Type.GetType(pQueenName);
    }
    public override List<Vector2Int> SelectAvaliableSquares()
    {
        throw new NotImplementedException();
    }
    public bool canMoveThere(Vector2Int coords)
    {
        Piece temp = board.getPiece(coords);
        if (temp && temp != this)
        {
            return false;
        }
        return true;
    }
    public bool canPawnTake(Vector2Int coords)
    {
        int xPos = this.occupiedSquare.x;
        int yPos = this.occupiedSquare.y;
        Vector2Int displacement = new Vector2Int(xPos, yPos);
        displacement = coords - displacement;
        if (this.team == TeamColor.White)
        {
            if ((displacement.x == 1 || displacement.x == -1) && (displacement.y == 1) && board.getPiece(coords) && (board.getPiece(coords).getTeam() != this.getTeam()))
            {
                return true;
            }
            return false;
        }
        else
        {
            if ((displacement.x == 1 || displacement.x == -1) && (displacement.y == -1) && board.getPiece(coords) && (board.getPiece(coords).getTeam() != this.getTeam()))
            {
                return true;
            }
            return false;
        }
    }
    public override bool isAttackingSquare(Vector2Int coords)
    {
        return canPawnTake(coords);
    }
    public override void MovePiece(Vector2Int coords)
    {
        if (!taken)
        {
            if (this.getTeam() == controller.getActivePlayer().getTeam() && this.avaliableMoves.Contains(coords))
            {
                // If it is this team's turn
                if (squareIsMoveable(coords))
                {
                    this.occupiedSquare = coords;
                    transform.position = this.board.CalculatePositionFromCoords(coords);
                    if (this.occupiedSquare.y == 7 || this.occupiedSquare.y == 0)
                    {
                        Debug.Log("Queening");
                        //Debug.Log("" + pQueenName);
                        Type type = Type.GetType("Queen");
                        controller.CreatePieceAndInitialize(this.occupiedSquare, this.team, type);
                        Destroy(this.gameObject);
                    }
                    controller.endTurn();
                }
                else if (canPawnTake(coords))
                {
                    board.takePiece(this, coords);
                    this.occupiedSquare = coords;
                    transform.position = this.board.CalculatePositionFromCoords(coords);
                    if (this.occupiedSquare.y == 7 || this.occupiedSquare.y == 0)
                    {
                        Debug.Log("Queening");
                        Debug.Log("" + pQueenName);
                        Type type = Type.GetType("Queen");
                        controller.CreatePieceAndInitialize(this.occupiedSquare, this.team, type);
                        Destroy(this.gameObject);
                    }
                    controller.endTurn();
                }
                else
                {
                    transform.position = this.board.CalculatePositionFromCoords(this.occupiedSquare);
                }

            }
            else
            {
                // If not this team's turn, snap back to occupied square
                transform.position = this.board.CalculatePositionFromCoords(this.occupiedSquare);
            }
        }
        else
        {
            transform.position = finalCoords;
        }
    }
    public override void PossibleMoves()
    {
        avaliableMoves.Clear();
        if (!taken)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Vector2Int square = new Vector2Int(i, j); // this is to go through all the squares checking which are safe to move to
                    if (squareIsMoveable(square) || canPawnTake(square)) // this should be implemented when the obj is picked up to highlight the possible squares. 
                    {
                        avaliableMoves.Add(square);
                    }
                }
            }
        }
    }
    private bool squareIsMoveable(Vector2Int square)
    {
        if (this.team == TeamColor.White & ((this.occupiedSquare.y != 1 & square.x - this.occupiedSquare.x == 0 & square.y - this.occupiedSquare.y == 1) |
        (this.occupiedSquare.y == 1 & square.x - this.occupiedSquare.x == 0 & square.y - this.occupiedSquare.y <= 2 & square.y - this.occupiedSquare.y >= 1)))
        {
            //Debug.Log("Turn Green");
            if ((square.y - this.occupiedSquare.y == 2) && (!canMoveThere(new Vector2Int(this.occupiedSquare.x, this.occupiedSquare.y + 1))))
            {
                return false;
            }
            if (canMoveThere(square))
            {
                return true;
            }
            return false;
        }
        else if (this.team == TeamColor.Black & ((this.occupiedSquare.y != 6 & square.x - this.occupiedSquare.x == 0 & this.occupiedSquare.y - square.y == 1) |
            (this.occupiedSquare.y == 6 & square.x - this.occupiedSquare.x == 0 & this.occupiedSquare.y - square.y <= 2 & this.occupiedSquare.y - square.y >= 1)))
        {
            // Debug.Log("Turn Green");
            if ((this.occupiedSquare.y - square.y == 2) && (!canMoveThere(new Vector2Int(this.occupiedSquare.x, this.occupiedSquare.y - 1))))
            {
                return false;
            }
            if (canMoveThere(square))
            {
                return true;
            }
            return false;
        }
        else
        {
            return false;
        }
    }
    /*
    public void OnPointerDown(MixedRealityPointerEventData eventData)
    {
        PossibleMoves();
        board.HightlightTiles(avaliableMoves);
        Debug.Log("Down"); ;
    }
    public void OnPointerDragged(MixedRealityPointerEventData eventData)
    {
        Debug.Log("Drag");
    }
    public void OnPointerUp(MixedRealityPointerEventData eventData)
    {
        avaliableMoves.Clear();
        board.HightlightTiles(avaliableMoves);
        Debug.Log("up");
    }
    public void OnPointerClicked(MixedRealityPointerEventData eventData)
    {
        Debug.Log("click");
    }
    */
}