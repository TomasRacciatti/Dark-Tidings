using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InteractIconUI : MonoBehaviour
{
    [SerializeField] private Image _iconImage;
    [SerializeField] private Image _keyImage;
    [SerializeField] private TextMeshProUGUI _keyText;

    private Transform _target;
    
    public void Initialize(Transform target)
    {
        _target = target;
        ShowIconOnly();
    }

    private void Update()
    {
        if (_target == null)
        {
            gameObject.SetActive(false);
            return;
        }
        
        transform.position = Camera.main.WorldToScreenPoint(_target.position);
    }

    public void ShowKey(string keyText)
    {
        _iconImage.enabled = false;
        _keyImage.enabled = true;
        _keyText.enabled = true;
        _keyText.text = keyText;
    }
    
    public void ShowIconOnly()
    {
        _iconImage.enabled = true;
        _keyImage.enabled = false;
        _keyText.text = string.Empty;
    }
    
    public void Clear()
    {
        _target = null;
        gameObject.SetActive(false);
    }
}
