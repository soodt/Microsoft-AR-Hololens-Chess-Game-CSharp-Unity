using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using NUnit.Framework;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using System.Collections.Generic;



public class SceneTest
{
    private const string sceneName = "ScaledDownScene";
    Scene mainScene;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        AsyncOperation loadScene = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        loadScene.allowSceneActivation = true;
        while(loadScene.isDone != true)
        {
            yield return null;
        }
        yield return true;
    }

    [UnitySetUp]
    public IEnumerator TearDown()
    {
        Scene scene = SceneManager.GetSceneByName(sceneName);
        if(scene.isLoaded)
        {
            SceneManager.UnloadSceneAsync(scene.buildIndex);
        }
        yield return null;
    }
    
    // A Test behaves as an ordinary method
    [Test]
    public static void TestLoadedScene()
    {
        // Use the Assert class to test conditions
        Scene scene = SceneManager.GetActiveScene();
        Assert.IsNotNull(scene);
        //SceneManager.GetActiveScene(); 
        
    }


    [UnityTest]
    public IEnumerator testPieces()
    {
        GameObject[] objectList = SceneManager.GetActiveScene().GetRootGameObjects();
        Assert.IsNotNull(objectList);
        List<GameObject> pieces = new List<GameObject>();

        for(int i = 0; i < objectList.Length; i++) 
        {
            if(objectList[i].name.Contains("Clone")) 
                {
                    pieces.Add(objectList[i]);
                }
        }
        Assert.IsTrue(pieces.Count == 32);
        yield return null;
    }


}
