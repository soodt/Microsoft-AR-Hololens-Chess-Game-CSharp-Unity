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

    public bool canMoveThere(Vector2Int coords) {
		int xPos = this.occupiedSquare.x;
        int yPos = this.occupiedSquare.y;
        return true;
	}

    public override void MovePiece(Vector2Int coords)
	{
        if (coords.x - this.occupiedSquare.x == 0 | coords.y - this.occupiedSquare.y == 0) {
            Piece pieceCheck = board.getPiece(coords);
            if (pieceCheck)
            {
                board.takePiece(this, coords);
            }
            this.occupiedSquare = coords;
		    transform.position = this.board.CalculatePositionFromCoords(coords);
        } 
        else {
            transform.position = this.board.CalculatePositionFromCoords(this.occupiedSquare);
        }
	}

}