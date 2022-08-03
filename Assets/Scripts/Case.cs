using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Case : MonoBehaviour
{
    [SerializeField] private Color _baseColor, _offsetColor;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private GameObject _highlight;
    [SerializeField] private Resource _resourcePrefab;

    public bool highlighted;

    public Vector2 coordinate;

    public Resource occupiedResource;

    public void Init(Vector2 coord, bool isOffset, Resource.Type? type = null)
    {
        coordinate = coord;
        _renderer.color = isOffset ? _offsetColor : _baseColor;
        highlighted = false;
        if (type.HasValue)
        {
            var spawnedResource = Instantiate(_resourcePrefab, transform.position, Quaternion.identity);
            spawnedResource.Init(this, type.Value);
            occupiedResource = spawnedResource;
        }
    }

    public void regenerateResource()
    {
        if(occupiedResource != null)
        {
            occupiedResource.RegenerateResource(this);
        }
    }

    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameManager.Instance.TriggerCaseClick(this);
        }
    }

    public void Highlight()
    {
        _highlight.SetActive(true);
        highlighted = true;
        GridManager.Instance.highlightedCase = this;
    }

    public void StopHighlight()
    {
        _highlight.SetActive(false);
        highlighted = false;
        GridManager.Instance.highlightedCase = null;
    }

    public void AddResource(Resource resource)
    {
        occupiedResource = resource;
        regenerateResource();
    }

    public void RemoveResource()
    {
        occupiedResource = null;
    }
}
