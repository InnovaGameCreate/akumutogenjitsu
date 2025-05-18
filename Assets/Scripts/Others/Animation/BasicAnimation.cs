using System.Collections.Generic;
using UnityEngine;

public class BasicAnimation : MonoBehaviour
{
    [Header("�X�v���C�g")]
    [SerializeField] private List<Sprite> _sprites;

    [Header("�P�����Ƃɑ҂���")]
    [SerializeField] private float _waitTime;

    private SpriteRenderer _spriteRenderer;

    private float _time = 0;
    private int _currentSpriteIndex = 0;

    [SerializeField] private bool _isEnabled = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        if (_spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer���R���|�[�l���g����Ă��܂���B");
        }

        _spriteRenderer.sprite = _sprites[0];

        _spriteRenderer.enabled = _isEnabled;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isEnabled)
        {
            return;
        }

        _time += Time.deltaTime;
        if (_time >= _waitTime)
        {
            _time = 0;
            _currentSpriteIndex = (_currentSpriteIndex + 1) % _sprites.Count;

            _spriteRenderer.sprite = _sprites[_currentSpriteIndex];
        }
    }

    public bool Enabled
    {
        get
        {
            return _isEnabled;
        }

        set
        {
            _isEnabled = value;
            _spriteRenderer.enabled = _isEnabled;
        }
    }
}
