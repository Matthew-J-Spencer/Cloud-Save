using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectManager : MonoBehaviour
{
    [SerializeField] private CharacterSlot _slotPrefab;
    [SerializeField] private ScriptablePersistentData _persistentData;
    [SerializeField] private Transform _slotsParent;
    [SerializeField] private Animator _interfaceAnimator;
    [SerializeField] private GameObject _characterActions;

    private readonly List<CharacterSlot> _spawnedSlots = new();
    public SlotData SelectedData { get; private set; }

    private void Awake()
    {
        CharacterSlot.SlotSelected += SlotSelected;
        _characterActions.SetActive(false);
        DeleteSlots();
    }

    private void OnDestroy()
    {
        CharacterSlot.SlotSelected -= SlotSelected;
    }

    private void Start()
    {
        LoadSlots();
    }

    private async void LoadSlots()
    {
        using (new LoaderSystem.Load())
        {
            DeleteSlots();

            var slotData = await SaveService.GetSlots();
            for (var i = 0; i < slotData.Count; i++)
            {
                _spawnedSlots.Add(Instantiate(_slotPrefab, _slotsParent));
                _spawnedSlots[i].InitSlot(i, slotData[i]);
            }
        }
    }

    private async Task LoadSlotAsync(int index)
    {
        _spawnedSlots[index].InitSlot(index, await SaveService.GetSlot(index));
    }

    private void DeleteSlots()
    {
        foreach (Transform child in _slotsParent)
        {
            Destroy(child.gameObject);
        }

        _spawnedSlots.Clear();
    }

    private void SlotSelected(SlotData data)
    {
        SelectedData = data;
        if (SelectedData.Data == null) _interfaceAnimator.SetTrigger("ToCreate");

        _characterActions.SetActive(SelectedData.Data != null);
    }

    public async Task RefreshSlot()
    {
        await LoadSlotAsync(SelectedData.Index);
    }

    public void PlayClicked()
    {
        _persistentData.CharacterData = SelectedData.Data;
        SceneManager.LoadScene("Game");
    }

    public async void DeleteClicked()
    {
        using (new LoaderSystem.Load())
        {
            await SaveService.DeleteSlotData(SelectedData.Index);
            await LoadSlotAsync(SelectedData.Index);
            _characterActions.SetActive(false);
        }
    }
}