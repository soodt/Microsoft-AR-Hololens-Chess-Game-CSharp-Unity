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

    public override void MovePiece(Vector2Int coords)
    {
        if (this.team == TeamColor.White & ((this.occupiedSquare.y != 1 & coords.x - this.occupiedSquare.x == 0 & coords.y - this.occupiedSquare.y == 1) |
            (this.occupiedSquare.y == 1 & coords.x - this.occupiedSquare.x == 0 & coords.y - this.occupiedSquare.y <= 2 & coords.y - this.occupiedSquare.y >= 1)))
        {
            this.occupiedSquare = coords;
            transform.position = this.board.CalculatePositionFromCoords(coords);
        }
        else if (this.team == TeamColor.Black & ((this.occupiedSquare.y != 6 & coords.x - this.occupiedSquare.x == 0 & this.occupiedSquare.y - coords.y == 1) |
            (this.occupiedSquare.y == 6 & coords.x - this.occupiedSquare.x == 0 & this.occupiedSquare.y - coords.y <= 2 & this.occupiedSquare.y - coords.y >= 1)))
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