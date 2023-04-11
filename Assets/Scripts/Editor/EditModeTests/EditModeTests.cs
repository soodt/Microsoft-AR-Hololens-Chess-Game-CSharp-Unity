using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class EditModeTests
{

    
// A Test behaves as an ordinary method
[Test]
    public void checkBoardInitialisation()
    {
        GameObject ChessBoard = GameObject.Find("Board");
        Assert.IsNotNull(ChessBoard);
        
    }

    [Test]
    public void noPieceGenerationbeforeLoadSceneQueen()
    {
        GameObject Queen = GameObject.Find("Queen");
        Assert.IsNull(Queen);

    }

    [Test]
    public void noPieceGenerationbeforeLoadSceneKing()
    {
        GameObject King = GameObject.Find("King");
        Assert.IsNull(King);

    }

    [Test]
    public void noPieceGenerationbeforeLoadSceneRook()
    {
        GameObject Rook = GameObject.Find("Rook");
        Assert.IsNull(Rook);

    }

    [Test]
    public void noPieceGenerationbeforeLoadSceneBishop()
    {
        GameObject Bishop = GameObject.Find("Bishop");
        Assert.IsNull(Bishop);

    }

    [Test]
    public void noPieceGenerationbeforeLoadSceneKnight()
    {
        GameObject Knight = GameObject.Find("Knight");
        Assert.IsNull(Knight);

    }

    [Test]
    public void noPieceGenerationbeforeLoadScenePawn()
    {
        GameObject Pawn = GameObject.Find("Pawn");
        Assert.IsNull(Pawn);

    }




    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator EditModeTestsWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}
