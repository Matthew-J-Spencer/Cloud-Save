using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateManager : MonoBehaviour
{
    [SerializeField] private SelectManager _selectManager;
    [SerializeField] private TMP_InputField _nameInput;
    [SerializeField] private GameObject _createBtn;
    [SerializeField] private Image _heroImage;
    [SerializeField] private TMP_Text _classText;
    [SerializeField] private Animator _interfaceAnimator;

    private int _selectedHeroIndex;
    private ScriptableHero _selectedHero;

    private void Awake()
    {
        SetCreationHero();
        _createBtn.SetActive(false);
    }

    public void IterateCreationHeroIndex(int amount)
    {
        _selectedHeroIndex = _selectedHeroIndex + amount < 0
            ? ResourcesService.Heroes.Count - 1
            : _selectedHeroIndex + amount;
        SetCreationHero();
    }

    private void SetCreationHero()
    {
        _selectedHero = ResourcesService.Heroes[_selectedHeroIndex % ResourcesService.Heroes.Count];

        _heroImage.sprite = _selectedHero.Image;
        _classText.text = _selectedHero.Type.ToString();
        _classText.color = _selectedHero.Color;
    }

    public void OnHeroNameInputChanged(string text)
    {
        _createBtn.SetActive(text.Length > 0);
    }

    public async void CreatePressed()
    {
        var data = new CharacterData
        {
            Slot = _selectManager.SelectedData.Index,
            Level = 1,
            Name = _nameInput.text,
            Type = _selectedHero.Type
        };

        using (new LoaderSystem.Load())
        {
            await SaveService.SaveSlotData(data);
            await _selectManager.RefreshSlot();
        }

        BackToSelect();
    }

    public void BackToSelect()
    {
        _nameInput.text = "";
        _interfaceAnimator.SetTrigger("ToSelect");
    }
}