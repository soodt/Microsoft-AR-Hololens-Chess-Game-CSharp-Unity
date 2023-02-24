using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : Piece
{
    public override List<Vector2Int> SelectAvaliableSquares()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; i< 8; j++)
            {

            }
        }

        throw new System.NotImplementedException();
    }
    // Checks to see if there is a piece in the way
    public bool canMoveThere(Vector2Int coords) {
        int xPos = this.occupiedSquare.x;
        int yPos = this.occupiedSquare.y;
        int yModifier = 1;
        if (yPos > coords.y) {
                yModifier = -1;
            }
        if (xPos < coords.x) {
            for (xPos = this.occupiedSquare.x; xPos <= coords.x; xPos++) {
                Piece temp = board.getPiece(new Vector2Int(xPos, yPos));
                /* If there is a piece at (xPos, yPos), it is not the bishop itself
                   They are not from the same team, and the (xPos, yPos) == coords (the exact square the piece wants to move to)
                   Then let the bishop move there
                   Else, not allowed move there
                */
                if (temp && temp != this) {
                    if (!temp.IsFromSameTeam(this) && (xPos == coords.x && yPos == coords.y)) {
                        return true;
                    }

                    return false;
                }
                yPos+= yModifier;
            }
        } else if (xPos > coords.x){
            for (xPos = this.occupiedSquare.x; xPos >= coords.x; xPos--) {
                Piece temp = board.getPiece(new Vector2Int(xPos, yPos));
                if (temp && temp != this) {
                    if (!temp.IsFromSameTeam(this) && (xPos == coords.x && yPos == coords.y)) {
                        return true;
                    }

                    return false;
                }
                yPos += yModifier;
            }
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
            Vector2Int displacement = coords - this.occupiedSquare;
            if (System.Math.Abs(displacement.x) == System.Math.Abs(displacement.y) && canMoveThere(coords))
            {
                Piece pieceCheck = board.getPiece(coords);
                if (pieceCheck)
                {
                    board.takePiece(this, coords);
                }
                this.occupiedSquare = coords;
                transform.position = this.board.CalculatePositionFromCoords(coords);
                controller.endTurn();
                //moved = true;
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