using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : Piece
{
    public override List<Vector2Int> SelectAvaliableSquares()
    {
        throw new System.NotImplementedException();
    }
    // Checks to see if there is a piece in the way
    public bool canMoveThere(Vector2Int coords) {
        int xPos = this.occupiedSquare.x;
        int yPos = this.occupiedSquare.y;
        if (xPos < coords.x) {
            for (xPos = this.occupiedSquare.x; xPos < coords.x; xPos++) {
                if (board.getPiece(new Vector2Int(xPos, yPos))) {
                    return false;
                }
                yPos++;
            }
        } else if (xPos > coords.x){
            for (xPos = this.occupiedSquare.x; xPos > coords.x; xPos--) {
                if (board.getPiece(new Vector2Int(xPos, yPos))) {
                    return false;
                }
                yPos--;
            }
        }
        return true;
    }

    public override void MovePiece(Vector2Int coords)

    {
        Vector2Int displacement = coords - this.occupiedSquare;
        //bool moved = false;
        //int x = System.Math.Abs(displacement.x);
        //int y = System.Math.Abs(displacement.y); 

        if (System.Math.Abs(displacement.x) == System.Math.Abs(displacement.y) && canMoveThere(coords))
        {
            Piece pieceCheck = board.getPiece(coords);
            if (pieceCheck)
            {
                board.takePiece(this, coords);
            }
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