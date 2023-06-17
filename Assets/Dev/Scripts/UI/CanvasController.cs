using Dev.Scripts;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Quaternion = System.Numerics.Quaternion;

public class CanvasController : MonoBehaviour
{
    public static CanvasController Instance { get; private set; }
    
    [SerializeField] private TextMeshProUGUI totalGoldText = null;
    [SerializeField] private GameObject totalGemCanvas=null;
    [SerializeField] private GameObject floatingGoldText = null;

    private int _totalGoldValue = 0;
    private Transform _canvasTransform;
    private Transform _textTransform;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        SignUpEvents();
        _canvasTransform = transform;
        _textTransform = totalGoldText.transform;
    }

    private void SignUpEvents()
    {
        GameEvents.GemSoldEvent += SetText;
    }

    #region Button Methods

    public void OpenTotalGemCanvas()
    {
        totalGemCanvas.SetActive(true);
    }

    public void CloseTotalGemCanvas()
    {
        totalGemCanvas.SetActive(false);
    }

    #endregion

    private void SetText(int value, Gem gem)
    {
        _totalGoldValue += value;
        PlayerPrefs.SetInt("TotalSoldGold",PlayerPrefs.GetInt("TotalSoldGold") + value);

        totalGoldText.text = _totalGoldValue.ToString();

        totalGoldText.rectTransform.DOScale(1.5f, 0.3f).OnComplete(() =>
        {
            totalGoldText.rectTransform.DOScale(1, 0.3f);
        });

        var characterPos = Character.Instance.transform.position;

        var floatingTextObject = Instantiate(floatingGoldText, characterPos, UnityEngine.Quaternion.identity);
        var textComponent = floatingTextObject.GetComponent<Text>();
        textComponent.text = "+" + value;
        floatingTextObject.transform.SetParent(_canvasTransform, false);
        
        PlayFloatingTextAnimation(floatingTextObject);
    }
    
    private void PlayFloatingTextAnimation(GameObject floatingTextObject)
    {
        var textRenderer = floatingTextObject.GetComponent<Text>(); 
        var color = textRenderer.color; 
        
        floatingTextObject.transform.DOMoveY(_textTransform.position.y, 5f).SetEase(Ease.OutExpo).OnUpdate(() =>
        {
            textRenderer.DOFade(0, 3f).SetEase(Ease.OutExpo).OnUpdate(() =>
            {
                color.a = textRenderer.color.a;
                textRenderer.color = color;
            });
        }).OnComplete(() => floatingTextObject.gameObject.SetActive(false));
    }
}
