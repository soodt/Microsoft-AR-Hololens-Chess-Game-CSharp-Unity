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
        this.occupiedSquare = coords;
        transform.position = this.board.CalculatePositionFromCoords(coords);
    }

}