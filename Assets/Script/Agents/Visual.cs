using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Visual : MonoBehaviour
{
    [SerializeField] Image _barLife;
    Renderer _renderer;
    Color _originalColor;
    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        _originalColor = _renderer.material.color;
    }

    public void ModififyBarLife(float Life, float MaxLife)
    {
        _barLife.fillAmount = Mathf.Lerp(0, 1, Life / MaxLife);
    }
    public void ChangeColor(Color color)
    {
        _renderer.material.color = color;
    }
    public void BackToOriginal()
    {
        Debug.Log("VOLVE");
        _renderer.material.color = _originalColor;
    }
}
