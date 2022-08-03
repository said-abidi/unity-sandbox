using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public enum Type { Wood, None };

    [SerializeField] public Type type { get; set; }

    [SerializeField] private Sprite _woodSprite;

    private Vector3 _dragOffset;

    [SerializeField] private float _dragSpeed = 10;

    [SerializeField] private SpriteRenderer _spritRenderer;

    [SerializeField] private BoxCollider2D _boxCollider2D;


    public Case occupiedCase;

    public void Init(Case inputCase, Type type)
    {
        GenerateResource(inputCase, type);
    }

    private void GenerateResource(Case inputCase, Type initType)
    {
        type = initType;

        switch (type)
        {
            case Type.Wood:
                {
                    _spritRenderer.sprite = _woodSprite;
                    break;
                }
            default: break;
        }

        setPostionAtCase(inputCase);
        _boxCollider2D.size = new Vector2(_spritRenderer.size.x, _spritRenderer.size.y);
    }

    private void setPostionAtCase(Case inputCase)
    {
        occupiedCase = inputCase;
        transform.SetParent(occupiedCase.transform, true);
        transform.position = new Vector3 (occupiedCase.transform.position.x, occupiedCase.transform.position.y, occupiedCase.transform.position.z - 1);
    }

    public void RegenerateResource(Case inputCase)
    {
        setPostionAtCase(inputCase);
    }

    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _dragOffset = transform.position - GridManager.Instance.GetMousePos();
            GameManager.Instance.TriggerResourceClick(this);
        }
    }

    void OnMouseDrag()
    {
        if(GameManager.Instance.resourceSelected == this)
        {
            transform.position = Vector3.MoveTowards(transform.position, GridManager.Instance.GetMousePos() + _dragOffset, _dragSpeed * Time.deltaTime);
        }
    }

    void OnMouseUp()
    {
        var outCase = GridManager.Instance.GetCaseAtMousePos();
        GameManager.Instance.TriggerCaseClick(outCase);
    }

    public void SetOccupiedCase(Case inputCase)
    {
        occupiedCase = inputCase;
    }

    public void Select()
    {
        GameManager.Instance.resourceSelected = this;
        _spritRenderer.color = new Color(1, 1, 1, 0.5f);
    }

    public void Unselect()
    {
        GameManager.Instance.resourceSelected = null;
        _spritRenderer.color = new Color(1, 1, 1, 1);
    }

}
