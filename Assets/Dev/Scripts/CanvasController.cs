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
    [SerializeField] private Image floatingGoldRenderer = null;

    private int totalGoldValue = 0;
    private Transform canvasTransform;
    private Transform textTransform;

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
        canvasTransform = transform;
        textTransform = totalGoldText.transform;
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
        totalGoldValue = value + PlayerPrefs.GetInt("TotalSoldGold");
        PlayerPrefs.SetInt("TotalSoldGold", totalGoldValue);
        
        totalGoldText.text = totalGoldValue.ToString();

        totalGoldText.rectTransform.DOScale(1.5f, 0.3f).OnComplete(() =>
        {
            totalGoldText.rectTransform.DOScale(1, 0.3f);
        });

        var characterPos = Character.Instance.transform.position;

        var floatingTextObject = Instantiate(floatingGoldText, characterPos, UnityEngine.Quaternion.identity);
        var textComponent = floatingTextObject.GetComponent<Text>();
        textComponent.text = "+" + value;
        floatingTextObject.transform.SetParent(canvasTransform, false);
        
        PlayFloatingTextAnimation(floatingTextObject);
    }
    
    private void PlayFloatingTextAnimation(GameObject floatingTextObject)
    {
        floatingTextObject.transform.DOMoveY(textTransform.position.y, 1f).SetEase(Ease.InSine).OnUpdate(() =>
        {
            floatingTextObject.transform.DOScale(0f, .5f).SetDelay(.5f);
        }).OnComplete(() => Destroy(floatingTextObject));
    }
}
