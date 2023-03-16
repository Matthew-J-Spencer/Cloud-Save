using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSlot : MonoBehaviour
{
    [SerializeField] private GameObject _filledObject, _emptyObject;
    [SerializeField] private TMP_Text _nameText, _classText, _levelText;
    [SerializeField] private Image _classImage;
    [SerializeField] private Image _slotImage;
    [SerializeField] private Transform _spinnerImage;
    [SerializeField] private Sprite _slotDefaultSprite, _slotSelectedSprite;

    private bool _selected;
    private SlotData _data;
    public static event Action<SlotData> SlotSelected;

    private void Awake()
    {
        SlotSelected += ToggleSlot;

        _filledObject.SetActive(false);
        _emptyObject.SetActive(false);
    }

    private void OnDestroy() => SlotSelected -= ToggleSlot;

    public void InitSlot(int index, CharacterData data)
    {
        _data = new SlotData
        {
            Index = index,
            Data = data
        };

        _filledObject.SetActive(data != null);
        _emptyObject.SetActive(data == null);

        if (data != null)
        {
            var scriptable = ResourcesService.GetHeroByType(data.Type);

            _nameText.text = data.Name;
            _classText.text = data.Type.ToString();
            _classText.color = scriptable.Color;
            _levelText.text = $"Level: {data.Level}";
            _classImage.sprite = scriptable.Image;
        }

        DeSelectSlot();
    }

    public void SelectSlot()
    {
        SlotSelected?.Invoke(_data);
    }

    private void ToggleSlot(SlotData data)
    {
        _selected = data.Index == _data.Index;
        _slotImage.sprite = _selected ? _slotSelectedSprite : _slotDefaultSprite;
    }

    private void DeSelectSlot()
    {
        _selected = false;
        _slotImage.sprite = _slotDefaultSprite;
    }
}

public class SlotData
{
    public int Index;
    public CharacterData Data;
}