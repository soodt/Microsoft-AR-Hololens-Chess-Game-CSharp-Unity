using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
//using UnityEditor.Build;
//using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
/*
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Tests;
using Microsoft.MixedReality.Toolkit.Utilities;
*/
public class PlayModeTests
{

    private const string sceneName = "ScaledDownScene";
    Scene mainScene;

    public void Setup() 
    {
        SceneManager.LoadScene(sceneName);
    }
    
    // A Test behaves as an ordinary method
    [Test]
    public static void PlayModeTestsSimplePasses()
    {
        // Use the Assert class to test conditions
        Scene scene = SceneManager.GetActiveScene();
        Assert.IsNotNull(scene);
        //SceneManager.GetActiveScene();
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator PlayModeTestsWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}
