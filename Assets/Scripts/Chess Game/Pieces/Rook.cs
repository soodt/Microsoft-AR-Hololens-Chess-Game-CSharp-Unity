using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : Piece
{
    public override List<Vector2Int> SelectAvaliableSquares()
    {
        throw new System.NotImplementedException();
    }
    //returns true if can move to coords passed in as parameter , false otherwise
    public bool canMoveThere(Vector2Int coords)
    {
        //assigns current position to integer variables 
        int xPos = this.occupiedSquare.x;
        int yPos = this.occupiedSquare.y;

        //checking for pieces blocking the way from occupying square to the passed in coordinates
        if (xPos > coords.x)
        {
            for (xPos = this.occupiedSquare.x; xPos >= coords.x; xPos--)
            {
                Piece temp = board.getPiece(new Vector2Int(xPos, yPos));
                if (temp && temp != this)
                {
                    if (!temp.IsFromSameTeam(this) && (xPos == coords.x && yPos == coords.y))
                    {
                        return true;
                    }
                    return false;
                }
            }
            return true;
        }
        else if (xPos < coords.x)
        {
            for (xPos = this.occupiedSquare.x; xPos <= coords.x; xPos++)
            {
                Piece temp = board.getPiece(new Vector2Int(xPos, yPos));
                if (temp && temp != this)
                {
                    if (!temp.IsFromSameTeam(this) && (xPos == coords.x && yPos == coords.y))
                    {
                        return true;
                    }
                    return false;
                }
            }
            return true;
        }
        else if (yPos > coords.y)
        {
            for (yPos = this.occupiedSquare.y; yPos >= coords.y; yPos--)
            {
                Piece temp = board.getPiece(new Vector2Int(xPos, yPos));
                if (temp && temp != this)
                {
                    if (!temp.IsFromSameTeam(this) && (xPos == coords.x && yPos == coords.y))
                    {
                        return true;
                    }
                    return false;
                }
            }
            return true;
        }
        else if (yPos < coords.y)
        {
            for (yPos = this.occupiedSquare.y; yPos <= coords.y; yPos++)
            {
                Piece temp = board.getPiece(new Vector2Int(xPos, yPos));
                if (temp && temp != this)
                {
                    if (!temp.IsFromSameTeam(this) && (xPos == coords.x && yPos == coords.y))
                    {
                        return true;
                    }
                    return false;
                }
            }
            return true;
        }
        return false; // should never reach here, just clears an error
    }


    //calls canMoveThere() and returns boolean variable 
    public override bool isAttackingSquare(Vector2Int coords)
    {
        return canMoveThere(coords);
    }

    public override void MovePiece(Vector2Int coords)
    {
        //if current player's turn , move the piece
        if (this.getTeam() == controller.getActivePlayer().getTeam())
        {
            //if the coords is where rook can move to and if there is no pieces in the way
            if ((coords.x - this.occupiedSquare.x == 0 || coords.y - this.occupiedSquare.y == 0) && canMoveThere(coords))
            {
                Piece pieceCheck = board.getPiece(coords);
                if (pieceCheck)
                {
                    board.takePiece(this, coords);
                }
                this.occupiedSquare = coords;
                transform.position = this.board.CalculatePositionFromCoords(coords);
                controller.endTurn();
            }
            //if can't move, stay in the occupied square
            else
            {
                transform.position = this.board.CalculatePositionFromCoords(this.occupiedSquare);
            }
        }
        else
        {
            // If not this team's turn, snap back to occupied square
            transform.position = this.board.CalculatePositionFromCoords(this.occupiedSquare);
            Debug.Log("NoMoving!");
        }
    }
}