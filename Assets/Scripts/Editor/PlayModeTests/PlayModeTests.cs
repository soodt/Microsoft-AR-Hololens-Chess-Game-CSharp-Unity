#if !WINDOWS_UWP

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
//using UnityEngine.InputSystem.XR;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;

//using Microsoft.MixedReality.Toolkit.UI;


namespace Microsoft.MixedReality.Toolkit.Tests
{
    public class PlayModeTests
{

    private const string sceneName = "ScaledDownScene";
    Scene mainScene;

    public void Setup()
    {
        PlayModeTestUtilities.InstallTextMeshProEssentials();
    }
    [UnitySetUp]
    public IEnumerator Init() 
    {
        TestUtilities.InitializeMixedRealityToolkit(true);
        
        //SceneManager.LoadScene(sceneName);
        AsyncOperation loadScene = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        loadScene.allowSceneActivation = true;
        while(loadScene.isDone != true)
        {
            yield return null;
        }
        yield return true;
    }
    
    // A Test behaves as an ordinary method
    [UnityTest]
    public static void TestLoadedScene()
    {
        // Use the Assert class to test conditions
        Scene scene = SceneManager.GetActiveScene();
        Assert.IsNotNull(scene);
        Debug.Log("Scene is loaded");
        //SceneManager.GetActiveScene();
    }

    [UnityTest]
    public IEnumerator TestSceneObjects()
    {
        Scene scene = SceneManager.GetActiveScene();
        GameObject[] list = scene.GetRootGameObjects();
        Assert.AreEqual(list.Length, 1);

        GameObject object1 = list[0];
        Debug.Log(object1.ToString());

        yield return null;

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

}
#endif