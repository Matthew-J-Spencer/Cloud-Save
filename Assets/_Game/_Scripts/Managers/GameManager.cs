using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private ScriptablePersistentData _data;
    [SerializeField] private SpriteRenderer _heroRenderer;
    [SerializeField] private Transform _saveIndicator;
    [SerializeField] private GameObject _saveGroup;
    [SerializeField] private TMP_Text _levelText;
    [SerializeField] private ParticleSystem _levelUpEffect;
    private AsyncOperation _scene;

    private void Start()
    {
        _saveGroup.SetActive(false);
        _heroRenderer.sprite = ResourcesService.GetHeroByType(_data.CharacterData.Type).Image;
        SetLevelText();

        _scene = SceneManager.LoadSceneAsync("Select");
        _scene.allowSceneActivation = false;
    }

    public async void LevelUpClicked()
    {
        _data.CharacterData.Level++;
        SetLevelText();
        _levelUpEffect.Play();

        _saveGroup.SetActive(true);

        await SaveService.SaveSlotData(_data.CharacterData);

        _saveGroup.SetActive(false);
    }

    private void Update()
    {
        _saveIndicator.Rotate(0, 0, 20 * Time.deltaTime);
    }

    private void SetLevelText() => _levelText.text = $"Level: {_data.CharacterData.Level}";

    public void BackToSelect()
    {
        _scene.allowSceneActivation = true;
    }
}