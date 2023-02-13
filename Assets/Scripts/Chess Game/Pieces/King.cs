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

    public override void MovePiece(Vector2Int coords)
    {
        if (coords.x - this.occupiedSquare.x <= 1 & coords.x - this.occupiedSquare.x >= -1 &
             coords.y - this.occupiedSquare.y <= 1 & coords.y - this.occupiedSquare.y >= -1)
        {
            this.occupiedSquare = coords;
            transform.position = this.board.CalculatePositionFromCoords(coords);
        }
        else
        {
            transform.position = this.board.CalculatePositionFromCoords(this.occupiedSquare);
        }
    }
}