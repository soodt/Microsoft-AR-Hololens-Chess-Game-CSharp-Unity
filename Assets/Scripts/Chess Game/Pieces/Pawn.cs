using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece
{
    public override List<Vector2Int> SelectAvaliableSquares()
    {
        throw new NotImplementedException();
    }

    //returns true if can move to coords passed in as parameter , false otherwise
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
            if ((displacement.x == 1 || displacement.x == -1) && (displacement.y == 1) && board.getPiece(coords))
            {
                return true;
            }
            return false;
        }
        else
        {
            if ((displacement.x == 1 || displacement.x == -1) && (displacement.y == -1) && board.getPiece(coords))
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
        if (this.getTeam() == controller.getActivePlayer().getTeam())
        {
            // If it is this team's turn
            if (this.team == TeamColor.White && ((this.occupiedSquare.y != 1 && coords.x - this.occupiedSquare.x == 0 && coords.y - this.occupiedSquare.y == 1) ||
            (this.occupiedSquare.y == 1 && coords.x - this.occupiedSquare.x == 0 && coords.y - this.occupiedSquare.y <= 2 && coords.y - this.occupiedSquare.y >= 1)) && canMoveThere(coords))
            {
                this.occupiedSquare = coords;
                transform.position = this.board.CalculatePositionFromCoords(coords);
                controller.endTurn();
            }
            else if (this.team == TeamColor.Black && ((this.occupiedSquare.y != 6 && coords.x - this.occupiedSquare.x == 0 && this.occupiedSquare.y - coords.y == 1) ||
                (this.occupiedSquare.y == 6 && coords.x - this.occupiedSquare.x == 0 && this.occupiedSquare.y - coords.y <= 2 & this.occupiedSquare.y - coords.y >= 1)) && canMoveThere(coords))
            {
                this.occupiedSquare = coords;
                transform.position = this.board.CalculatePositionFromCoords(coords);
                controller.endTurn();
            }
            else if (canPawnTake(coords) && board.getPiece(coords))
            {
                board.takePiece(this, coords);
                this.occupiedSquare = coords;
                transform.position = this.board.CalculatePositionFromCoords(coords);
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
            Debug.Log("NoMoving!");

        }
    }

}