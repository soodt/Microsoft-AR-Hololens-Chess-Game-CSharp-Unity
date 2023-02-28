using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Piece
{
    public bool isInCheck { get; set; }
    public override List<Vector2Int> SelectAvaliableSquares()
    {
        throw new NotImplementedException();
    }

    //check if the king is in check
    public bool getInCheck()
    {
        return this.isInCheck;
    }

    //if king piece is threatened with capture, set it to in check
    public void setInCheck(bool val)
    {
        this.isInCheck = val;
    }

    //returns true if can be moved to coords passed in as parameter , false otherwise
    public bool canMoveThere(Vector2Int coords)
    {
        Piece temp = board.getPiece(coords);
        if (temp && temp != this)
        {
            //if meets piece from the same team, return false
            if (temp.IsFromSameTeam(this))
            {
                return false;
            }
            return true;
        }
        return true;
    }
    //calls canMoveThere() and return boolean value
    public override bool isAttackingSquare(Vector2Int coords)
    {
        return canMoveThere(coords);
    }

    public override void MovePiece(Vector2Int coords)
    {
        //if current player's turn
        if (this.getTeam() == controller.getActivePlayer().getTeam())
        {

            //moves one square at a time in any direction
            if (((coords.x - this.occupiedSquare.x <= 1 )&& (coords.x - this.occupiedSquare.x >= -1) &&
             (coords.y - this.occupiedSquare.y <= 1) && (coords.y - this.occupiedSquare.y >= -1)) && canMoveThere(coords))
            {
                Piece pieceCheck = board.getPiece(coords);
                if (pieceCheck)
                {
                    board.takePiece(this, coords);
                }
                this.occupiedSquare = coords;
                //let oldcoord equals to current position, if next position is the current location, endturn do not execute (aka error fixed)
                Vector2Int oldcoord = this.occupiedSquare;
                transform.position = this.board.CalculatePositionFromCoords(coords);
                if (this.occupiedSquare != oldcoord)
                {
                    controller.endTurn();
                }
            
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
            Debug.Log("NoMoving!");
        }
    }

}
