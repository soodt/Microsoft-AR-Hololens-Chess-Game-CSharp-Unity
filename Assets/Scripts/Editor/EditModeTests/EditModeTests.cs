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
