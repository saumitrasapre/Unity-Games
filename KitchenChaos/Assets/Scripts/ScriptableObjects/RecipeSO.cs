using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu()]
public class RecipeSO : ScriptableObject
{
    //Scriptable object to store a recipe (which is just a list of KitchenObjectSOs, and its name)
    public List<KitchenObjectSO> kitchenObjectSOList;
    public string recipeName;
    public int recipeScore;
    public float recipeTimeBonus;

}
