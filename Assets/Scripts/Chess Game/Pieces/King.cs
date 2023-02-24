using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Piece
{
    public bool isInCheck{ get; set; }
    public override List<Vector2Int> SelectAvaliableSquares()
    {
        throw new NotImplementedException();
    }

    public bool getInCheck() {
        return this.isInCheck;
    }

    public void setInCheck(bool val) {
        this.isInCheck = val;
    }

    public bool canMoveThere(Vector2Int coords) {
		Piece temp = board.getPiece(coords);
        if (temp && temp != this) {
            if (temp.IsFromSameTeam(this)) {
                return false;
            }
            return true;
        }
        return true;
	}

    public override bool isAttackingSquare(Vector2Int coords) {
        return canMoveThere(coords);
    }

    public override void MovePiece(Vector2Int coords)
    {
        if (this.getTeam() == controller.getActivePlayer().getTeam())
        {
            if ((coords.x - this.occupiedSquare.x <= 1 & coords.x - this.occupiedSquare.x >= -1 &
             coords.y - this.occupiedSquare.y <= 1 & coords.y - this.occupiedSquare.y >= -1) && canMoveThere(coords))
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

}