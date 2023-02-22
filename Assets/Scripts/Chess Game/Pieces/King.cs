using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Piece
{
    public override List<Vector2Int> SelectAvaliableSquares()
    {
        throw new NotImplementedException();
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

    public override void MovePiece(Vector2Int coords)
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
        }
        else
        {
            transform.position = this.board.CalculatePositionFromCoords(this.occupiedSquare);
        }
    }

}