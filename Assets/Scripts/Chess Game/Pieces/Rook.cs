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
<<<<<<< Updated upstream
=======

    public override void MovePiece(Vector2Int coords)
	{

        if (coords.x - this.occupiedSquare.x == 0 | coords.y - this.occupiedSquare.y == 0) {
            this.occupiedSquare = coords;
		    transform.position = this.board.CalculatePositionFromCoords(coords);
        } 
        else {
            transform.position = this.board.CalculatePositionFromCoords(this.occupiedSquare);
        }
	}
>>>>>>> Stashed changes
}