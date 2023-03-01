using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Board/Layout")]

public class BoardLayout : ScriptableObject
{
  [Serializable]
  private class BoardSquareSetup {
    public Vector2Int position;
    public PieceType pieceType;    //piecetype occpuying the square
    public TeamColor teamColor;    //teamcolor the piece belongs to 
  }

  [SerializeField] private BoardSquareSetup[] boardSquares;

 
    public int GetPiecesCount()
    {
        return boardSquares.Length;
    }



     //returns coordinates of the boardSquare object
    public Vector2Int GetSquareCoordsAtIndex(int index)
    {
        if (boardSquares.Length <= index) {
            Debug.LogError("Index is out of range");
            return new Vector2Int(-1, -1);
        }
        return new Vector2Int(boardSquares[index].position.x - 1, boardSquares[index].position.y - 1);
    }

    //return piece name at board square
    public string GetSquarePieceNameAtIndex(int index)
    {
        if (boardSquares.Length <= index) {
            Debug.LogError("Index is out of range");
            return "";
        }
        return boardSquares[index].pieceType.ToString();


    }

    //return team color of a board square
    public TeamColor GetSquareTeamColorAtIndex(int index)
    {
        if (boardSquares.Length <= index) {
            Debug.LogError("Index is out of range");
            return TeamColor.Black;
        }
        return boardSquares[index].teamColor;
    }
}
