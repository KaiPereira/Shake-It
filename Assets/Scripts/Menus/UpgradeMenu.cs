using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class MainMenu : MonoBehaviour
{
    private UIDocument _document;
    private Button _exit_button;
    public VisualTreeAsset upgradeTemplate;
    private VisualElement upgradeContainer;

    private GameManager gameManager;
    private ScoreManager scoreManager;

    public AudioSource buySound;
    public AudioSource errorSound;

    /*private Button _exit_button, _upgrade_customer_button;
    public VisualElement[] outlines;
    private int outline_index;
    public Color customerUpgradeColor;
    public float[] customerUpgradePrices = {5f, 15f, 35f, 50f};

    private float buttonErrorLength = 0.25f;*/

    [System.Serializable]
    public class Upgrade
    {
        public VisualElement[] outlines;
        public float[] upgradePrices;
        public int currentLevel;
        public System.Action<int> onUpgradeAction;
    }

    public List<Upgrade> upgrades = new List<Upgrade>();

    private void Awake()
    {
        _document = GetComponent<UIDocument>();
        gameManager = FindObjectOfType<GameManager>();
        scoreManager = FindObjectOfType<ScoreManager>();
    }

    private void OnEnable()
    {
        var root = _document.rootVisualElement;
        _exit_button = root.Q<Button>("ExitMenuButton");
        upgradeContainer = root.Q<GroupBox>("UpgradesContainer");

        _exit_button.RegisterCallback<ClickEvent>(ExitMenuClick);

        foreach (Upgrade upgrade in upgrades)
        {
            var upgradeElement = upgradeTemplate.CloneTree();

            upgradeContainer.Add(upgradeElement);
        }
    }

    private void OnDisable()
    {
        _exit_button.UnregisterCallback<ClickEvent>(ExitMenuClick);
    }

    private void ExitMenuClick(ClickEvent evt)
    {
        gameObject.SetActive(false);
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    /*private void OnEnable()
    {
        var root = _document.rootVisualElement;
        _upgrade_customer_button = root.Q<Button>("UpgradeCustomers");

        _exit_button.RegisterCallback<ClickEvent>(ExitMenuClick);
        _upgrade_customer_button.RegisterCallback<ClickEvent>(UpgradeCustomer);

        outlines = new VisualElement[4]
        {
            root.Q<VisualElement>("outline1"),
            root.Q<VisualElement>("outline2"),
            root.Q<VisualElement>("outline3"),
            root.Q<VisualElement>("outline4")
        };

        FillOutlines();
        CustomerUpgradePrice();
    }









    private void UpgradeCustomer(ClickEvent evt)
    {
        bool canUpgrade = scoreManager.BuyUpgrade(customerUpgradePrices[outline_index]);

        if (canUpgrade)
        {
            buySound.Play();

            if (outline_index < outlines.Length)
            {
                outline_index++;
            }

            FillOutlines();
            CustomerUpgradePrice();

            gameManager.customerLevelUpgrade = outline_index;
        } else {
            StartCoroutine(ButtonError(_upgrade_customer_button));
        }
    }

    private void UpgradeBuilding()
    {

    }

    private void FillOutlines()
    {
        for (int i = 0; i < outline_index; i++)
        {
            outlines[i].style.backgroundColor = customerUpgradeColor;
        }
    }

    private void CustomerUpgradePrice()
    {
        var upgradeButtonText = _upgrade_customer_button.Q<Label>();
        if (outline_index == outlines.Length) {
            upgradeButtonText.text = "max"; 
        } else {
            upgradeButtonText.text = customerUpgradePrices[outline_index].ToString();
        }
    }

    private IEnumerator ButtonError(Button button)
    {
        errorSound.Play();

        button.style.backgroundColor = Color.red;

        yield return new WaitForSeconds(buttonErrorLength);

        button.style.backgroundColor = customerUpgradeColor;
    }*/
}
