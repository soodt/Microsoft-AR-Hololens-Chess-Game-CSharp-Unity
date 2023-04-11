using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SquareSelector : MonoBehaviour
{
    [SerializeField] private Material freeSquareMaterial;
    [SerializeField] private Material enemySquareMaterial;
    [SerializeField] private GameObject selectorPrefab;
    private List<GameObject> instantiatedSelectors= new List<GameObject>();

    public void ShowSelection(Dictionary<Vector3, bool> squareInfo)  // used to select the squares and apply the materials
    {
        ClearSelection();
        //Debug.Log(squareInfo.Count);
        foreach (var data in squareInfo)
        {
            GameObject selector = Instantiate(selectorPrefab, data.Key, Quaternion.identity);
            instantiatedSelectors.Add(selector);
            foreach (var setter in selector.GetComponentsInChildren<MaterialSetter>())
            {
                setter.SetSingleMaterial(data.Value ? freeSquareMaterial : enemySquareMaterial);
            }
        }
    }


    public void ClearSelection() // clears the selections
    {
        for (int i = 0; i < instantiatedSelectors.Count; i++)
        {
            Destroy(instantiatedSelectors[i]);
        }
    }
}
