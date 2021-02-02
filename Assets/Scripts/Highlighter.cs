using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class Highlighter : MonoBehaviour
{
    
    [SerializeField]
    private Sprite move;
    public Sprite Move => move;

    [SerializeField]
    private Sprite attack;
    public Sprite Attack => attack;

    private HighlightType _highlightType;
    public HighlightType HighlightType
    {
        get => _highlightType;
        set
        {
            _highlightType = value;
            switch (value)
            {
                case HighlightType.Move:
                    highlightSpriteRenderer.sprite = move;
                    break;
                case HighlightType.Attack:
                    highlightSpriteRenderer.sprite = attack;
                    break;
                case HighlightType.None:
                    highlightSpriteRenderer.sprite = null;
                    break;
            }
        }
    }

    private SpriteRenderer highlightSpriteRenderer;

    private void Start()
    {
        // Create a new empty GameObject to display the highlighting sprite
        var highlightObject = new GameObject();
        // Add the new GameObject as a child of the scripts main GameObject
        highlightObject.transform.parent = this.gameObject.transform;
        // Setting the new GameObjects parent has resized it. Reset the size to 1,1,1
        highlightObject.transform.localScale = new Vector3(1, 1, 1);
        // Set the coordinates of this new GameObject so it is always on top
        highlightObject.transform.localPosition = new Vector3(0, 0, -1);
        // Add a SpriteRenderer to the new GameObject in order for it to be able to display highlightings
        highlightSpriteRenderer = highlightObject.AddComponent(typeof(SpriteRenderer)) as SpriteRenderer;
        // Add opacity to the SpriteRenderer
        highlightSpriteRenderer.color = new Color(255, 255, 255, 0.65f);
    }
}

public enum HighlightType
{
    None,
    Move,
    Attack
}
