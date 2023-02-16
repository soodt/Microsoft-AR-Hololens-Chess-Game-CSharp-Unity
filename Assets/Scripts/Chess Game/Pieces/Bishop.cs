using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : Piece
{
    public override List<Vector2Int> SelectAvaliableSquares()
    {
        throw new System.NotImplementedException();
    }

    public override void MovePiece(Vector2Int coords)

    {
        Vector2Int displacement = coords - this.occupiedSquare;
        //bool moved = false;
        //int x = System.Math.Abs(displacement.x);
        //int y = System.Math.Abs(displacement.y); 

        if (System.Math.Abs(displacement.x) == System.Math.Abs(displacement.y))
        {
            this.occupiedSquare = coords;
            transform.position = this.board.CalculatePositionFromCoords(coords);
            //moved = true;
        }
        else
        {
            transform.position = this.board.CalculatePositionFromCoords(this.occupiedSquare);
        }  

    }


}