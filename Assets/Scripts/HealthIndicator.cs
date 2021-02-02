using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Unit))]
public class HealthIndicator : MonoBehaviour
{
    [SerializeField] private Sprite NumberZero;

    [SerializeField] private Sprite NumberOne;

    [SerializeField] private Sprite NumberTwo;

    [SerializeField] private Sprite NumberThree;

    [SerializeField] private Sprite NumberFour;

    [SerializeField] private Sprite NumberFive;

    [SerializeField] private Sprite NumberSix;

    [SerializeField] private Sprite NumberSeven;

    [SerializeField] private Sprite NumberEight;

    [SerializeField] private Sprite NumberNine;
    
    [SerializeField] private Sprite NumberTen;

    private SpriteRenderer healthNumberSpriteRenderer;
    
    void Awake()
    {
        // Create a new empty GameObject to display the health sprite
        var healthNumberObject = new GameObject();
        // Add the new GameObject as a child of the scripts main GameObject
        healthNumberObject.transform.parent = this.gameObject.transform;
        // Setting the new GameObjects parent has resized it. Reset the size to 1,1,1
        healthNumberObject.transform.localScale = new Vector3(0.2f, 0.2f, 1);
        // Set the coordinates of this new GameObject so it is always on top
        healthNumberObject.transform.localPosition = new Vector3(0.054f, -0.085f, -1);
        // Add a SpriteRenderer to the new GameObject in order for it to be able to display the health sprites
        healthNumberSpriteRenderer = healthNumberObject.AddComponent(typeof(SpriteRenderer)) as SpriteRenderer;
        
        // Register listener for the the units current remaining health
        gameObject.GetComponent<Unit>().RemainingHealthChanged += OnRemainingHealthChanged;
    }

    /// <summary>
    /// Update the health indicator according to the new remaining health
    /// </summary>
    private void OnRemainingHealthChanged(object sender, (float oldValue, float newValue) e)
    {
        var roundedHealth = Math.Ceiling(e.newValue);
        switch (roundedHealth)
        {
            case 1:
                healthNumberSpriteRenderer.sprite = NumberOne;
                break;
            
            case 2:
                healthNumberSpriteRenderer.sprite = NumberTwo;
                break;
            
            case 3:
                healthNumberSpriteRenderer.sprite = NumberThree;
                break;
            
            case 4:
                healthNumberSpriteRenderer.sprite = NumberFour;
                break;
            
            case 5:
                healthNumberSpriteRenderer.sprite = NumberFive;
                break;
            
            case 6:
                healthNumberSpriteRenderer.sprite = NumberSix;
                break;
            
            case 7:
                healthNumberSpriteRenderer.sprite = NumberSeven;
                break;
            
            case 8:
                healthNumberSpriteRenderer.sprite = NumberEight;
                break;
            
            case 9:
                healthNumberSpriteRenderer.sprite = NumberNine;
                break;
        }
    }
}
