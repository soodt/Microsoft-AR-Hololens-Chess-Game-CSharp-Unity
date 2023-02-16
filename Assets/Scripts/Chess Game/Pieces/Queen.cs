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

public override void MovePiece(Vector2Int coords)
{
    Vector2Int displacement = coords - this.occupiedSquare;
    bool available = false;
    foreach (var direction in directions)
    {
        for(int i = 1; i < 20; i++)
            //for(int i = 1;i<board.size;i++)
        {
            if (coords == this.occupiedSquare + direction * i)
            {
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