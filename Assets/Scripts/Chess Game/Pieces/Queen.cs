using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : Piece
{
    private Vector2Int[] directions = new Vector2Int[]
    {
        // all directions Queen can move 
        new Vector2Int(1,1),
        new Vector2Int(1,-1),
        new Vector2Int(-1,1),
        new Vector2Int(-1,-1),
        new Vector2Int(1,0),
        new Vector2Int(0,1),
        new Vector2Int(-1,0),
        new Vector2Int(0,-1),
    };
    public override List<Vector2Int> SelectAvaliableSquares()
    {
        throw new System.NotImplementedException();
    }

   public bool canMoveThere(Vector2Int coords) {
		 int xPos = this.occupiedSquare.x;
        int yPos = this.occupiedSquare.y;
        int yModifier = 0;
        if (yPos > coords.y) {
            yModifier = -1;
        } else if (yPos < coords.y) {
            yModifier = 1;
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
                Debug.Log("xPos: " + xPos);
                Debug.Log("yPos: " + yPos);
                Piece temp = board.getPiece(new Vector2Int(xPos, yPos));
                if (temp && temp != this) {
                    if (!temp.IsFromSameTeam(this) && (xPos == coords.x && yPos == coords.y)) {
                        return true;
                    }

                    return false;
                }
                yPos += yModifier;
            }
        } else if (yPos < coords.y) {
            for (yPos = this.occupiedSquare.y; yPos <= coords.y; yPos++) {
                Piece temp = board.getPiece(new Vector2Int(xPos, yPos));
                if (temp && temp != this) {
                    if (!temp.IsFromSameTeam(this) && (xPos == coords.x && yPos == coords.y)) {
                        return true;
                    }
                    return false;
                }
            }
            return true;
        } else if (yPos > coords.y) {
            for (yPos = this.occupiedSquare.y; yPos >= coords.y; yPos--) {
                Piece temp = board.getPiece(new Vector2Int(xPos, yPos));
                if (temp && temp != this) {
                    if (!temp.IsFromSameTeam(this) && (xPos == coords.x && yPos == coords.y)) {
                        return true;
                    }
                    return false;
                }
            }
        }
        return true;
	}

public override void MovePiece(Vector2Int coords)
{
    Vector2Int displacement = coords - this.occupiedSquare;
    bool available = false;
    foreach (var direction in directions)
    {
        for(int i = 1; i < 20; i++)
            //for(int i = 1;i<board.size;i++)
        {
            if ((coords == this.occupiedSquare + direction * i) && canMoveThere(coords))
            {
                    Piece pieceCheck = board.getPiece(coords);
                    if (pieceCheck)
                    {
                        board.takePiece(this, coords);
                    }
                    this.occupiedSquare = coords;
                transform.position = this.board.CalculatePositionFromCoords(coords);
                available = true;
                break;
            }
        }
        if (available) break;
    }
    if (!available)
    {
        transform.position = this.board.CalculatePositionFromCoords(this.occupiedSquare);
    }
  
    
}

}