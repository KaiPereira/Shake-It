using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
	public GameObject timerPrefab;

	public int index;
	public float moveSpeed = 2f;
	private Vector3 targetPosition;
	private bool isMoving = false;
	public SpriteRenderer spriteRenderer;
	private SpriteRenderer orderRenderer;
	private SpriteRenderer timerInstanceRenderer;
	public bool isReadyToSit = false;

	private SeatManager seatManager;
	private OrderManager orderManager;

	public OrderType orderType;
	public List<Toppings> toppings = new List<Toppings>();
	public Order order;
	public CustomerType customerType;

	private Sprite customerSprite;
	private Sprite orderSprite;
	public Sprite angryFaceSprite;
	private RuntimeAnimatorController customerAnimationController;
	private Animator animator;

	public ParticleSystem effect;
	public ParticleSystemRenderer particleRenderer;
	public Material bronzeCoin;
	public Material silverCoin;
	public Material goldCoin;
	public Material trophyCoin;
	public Vector3 entrancePosition;

	private float valueMultiplier = 1f;

	public void Initialize(int index)
	{
		this.index = index;
		gameObject.name = "Customer " + index;

		seatManager = FindObjectOfType<SeatManager>();
		orderManager = FindObjectOfType<OrderManager>();

		// Coin effect
		effect = Instantiate(effect, gameObject.transform);
		particleRenderer = effect.GetComponent<ParticleSystemRenderer>();
		effect.transform.localPosition = Vector2.zero;

		GetOrder();
		GetCustomerType();

		SetOrderSprite();
		SetCustomerSprite();

		spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
		spriteRenderer.sprite = customerSprite;

		spriteRenderer.sortingLayerName = "Customer";
		spriteRenderer.sortingOrder = index;

		animator = gameObject.AddComponent<Animator>();
		animator.runtimeAnimatorController = customerAnimationController;

		GameObject orderObject = new GameObject("Order");
		orderObject.transform.parent = transform;
		orderObject.transform.localPosition = new Vector3(1f, 0.5f, 0);

		orderRenderer = orderObject.AddComponent<SpriteRenderer>();
		orderRenderer.sprite = orderSprite;
		orderRenderer.sortingLayerName = "UI";
		orderRenderer.enabled = false;

		if (timerPrefab != null)
		{
			GameObject timerInstance = Instantiate(timerPrefab, orderObject.transform);
			timerInstance.GetComponent<Timer>().customer = this;

			timerInstance.transform.localPosition = Vector2.zero;

			timerInstanceRenderer = timerInstance.GetComponent<SpriteRenderer>();
			timerInstanceRenderer.sortingLayerName = "UI";
			timerInstanceRenderer.sortingOrder = 5;
			timerInstanceRenderer.enabled = false;
		}

		BoxCollider2D orderCollider = orderObject.AddComponent<BoxCollider2D>();
		orderCollider.isTrigger = true;
		orderCollider.size = new Vector2(1f, 1f);

		orderObject.AddComponent<OrderBubble>();
	}

	public void MoveTo(Vector3 target, System.Action onMoveComplete = null)
	{
		targetPosition = target;
		StartCoroutine(MoveCoroutine(onMoveComplete));
	}

	private IEnumerator MoveCoroutine(System.Action onMoveComplete)
	{
		isMoving = true;

		while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
		{
			transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
			yield return null;
		}
		
		transform.position = targetPosition;

		isMoving = false;
		onMoveComplete?.Invoke();
	}

	public static Customer CreateCustomer(Vector3 position, int index)
	{
		GameObject customerObject = new GameObject("Customer");
		customerObject.transform.position = position;

		Customer customer = customerObject.AddComponent<Customer>();
		customer.Initialize(index);

		return customer;
	}

	public void MoveToSeat() {
		Vector3 nextAvailableSeat = seatManager.GetNextAvailableSeat();

		if (nextAvailableSeat != Vector3.zero) {
		HideOrder();

		WaitingQueue waitingQueue = FindObjectOfType<WaitingQueue>();
		waitingQueue.RemoveCustomerAndShift(this);

		MoveTo(nextAvailableSeat, () => {
			orderRenderer.enabled = true;
			timerInstanceRenderer.enabled = true;
		});
	    } else {
			Debug.Log("No seats available");
	    }
	}

	private void OnMouseDown()
	{
		if (isReadyToSit)
		{
			MoveToSeat();
		}
	}

	public void LeaveRestaurant()
	{
		orderRenderer.sprite = angryFaceSprite;
		timerInstanceRenderer.enabled = false;
		orderManager.FailOrder();

		MoveTo(entrancePosition, () => {
			Destroy(gameObject);
		});
	}

	public void ShowOrder()
	{
		orderRenderer.enabled = true;
		timerInstanceRenderer.enabled = true;
		isReadyToSit = true;
	}

	public void HideOrder()
	{
		orderRenderer.enabled = false;
		timerInstanceRenderer.enabled = false;
		isReadyToSit = false;
	}

	private void GetOrder() {
		orderType = (OrderType)Random.Range(0, System.Enum.GetValues(typeof(OrderType)).Length);
		int toppings_amount = Random.Range(0, 4);
		
		for (int i = 0; i < toppings_amount; i++)
		{
			toppings.Add((Toppings)Random.Range(0, System.Enum.GetValues(typeof(Toppings)).Length));
		}

		order = new Order(orderType, toppings, 30, valueMultiplier);
	}

	private void GetCustomerType() {
		float range = Random.Range(0, 100);

		if (range >= 0 && range <= 70) {
			float range2 = Random.Range(1, 5);
			particleRenderer.enabled = false;

			switch (range2) {
				case 1: customerType = CustomerType.ADULT_FISHERMAN; break;
				case 2: customerType = CustomerType.ADULT_FARMER; break;
				case 3: customerType = CustomerType.ADULT_BLACKSMITH; break;
				case 4: customerType = CustomerType.ADULT_BARTENDER; break;
				case 5: customerType = CustomerType.ADULT_BARMAID; break;
			}
		} else if (range >= 71 && range <= 85) {
			valueMultiplier = 1.5f;
			particleRenderer.material = bronzeCoin;

			float range2 = Random.Range(1, 2);

			switch (range2) {
				case 1: customerType = CustomerType.KID_1; break;
				case 2: customerType = CustomerType.KID_2; break;
			}
		} else if (range >= 86 && range <= 95) {
			valueMultiplier = 2.5f;
			particleRenderer.material = silverCoin;

			float range2 = Random.Range(1, 4);

			switch (range2) {
				case 1: customerType = CustomerType.WIZARD_BLUE; break;
				case 2: customerType = CustomerType.WIZARD_GREEN; break;
				case 3: customerType = CustomerType.WIZARD_YELLOW; break;
				case 4: customerType = CustomerType.WIZARD_PURPLE; break;
			}
		} else if (range >= 96 && range <= 99) {
			valueMultiplier = 5;
			particleRenderer.material = goldCoin;

			float range2 = Random.Range(1, 4);
			
			switch (range2) {
				case 1: customerType = CustomerType.MERCHANT_BLUE; break;
				case 2: customerType = CustomerType.MERCHANT_GREEN; break;
				case 3: customerType = CustomerType.MERCHANT_GOLD; break;
				case 4: customerType = CustomerType.MERCHANT_PURPLE; break;
			}
		} else if (range == 100) {
			valueMultiplier = 10;
			particleRenderer.material = trophyCoin;
			customerType = CustomerType.KING;
		}
	}

	private void SetCustomerSprite() {
		switch (customerType) {
			case CustomerType.KID_1:
				customerSprite = Resources.Load<Sprite>("Customers/Kid1/Kid01_walk");
				customerAnimationController = Resources.Load<RuntimeAnimatorController>("Customers/Kid1/kid1_anim");
				break;
			case CustomerType.KID_2:
				customerSprite = Resources.Load<Sprite>("Customers/Kid2/Kid02_walk");
				customerAnimationController = Resources.Load<RuntimeAnimatorController>("Customers/Kid2/kid2_anim");
				break;
			case CustomerType.ADULT_BARMAID:
				customerSprite = Resources.Load<Sprite>("Customers/Barmaid/Barmaid_walk");
				customerAnimationController = Resources.Load<RuntimeAnimatorController>("Customers/Barmaid/barmaid_anim");
				break;
			case CustomerType.ADULT_BARTENDER:
				customerSprite = Resources.Load<Sprite>("Customers/Bartender/Bartender_walk");
				customerAnimationController = Resources.Load<RuntimeAnimatorController>("Customers/Bartender/bartender_anim");
				break;
			case CustomerType.ADULT_BLACKSMITH:
				customerSprite = Resources.Load<Sprite>("Customers/Blacksmith/Blacksmith_walk");
				customerAnimationController = Resources.Load<RuntimeAnimatorController>("Customers/Blacksmith/blacksmith_anim");
				break;
			case CustomerType.ADULT_FARMER:
				customerSprite = Resources.Load<Sprite>("Customers/Farmer/Farmer_walk");
				customerAnimationController = Resources.Load<RuntimeAnimatorController>("Customers/Farmer/farmer_anim");
				break;
			case CustomerType.ADULT_FISHERMAN:
				customerSprite = Resources.Load<Sprite>("Customers/Fisherman/Fisherman_walk");
				customerAnimationController = Resources.Load<RuntimeAnimatorController>("Customers/Fisherman/fisherman_anim");
				break;
			case CustomerType.WIZARD_BLUE:
				customerSprite = Resources.Load<Sprite>("Customers/Alchemist/Blue/Alchemist_blue_walk");
				customerAnimationController = Resources.Load<RuntimeAnimatorController>("Customers/Alchemist/Blue/alchemist_blue_anim");
				break;
			case CustomerType.WIZARD_GREEN:
				customerSprite = Resources.Load<Sprite>("Customers/Alchemist/Green/Alchemist_green_walk");
				customerAnimationController = Resources.Load<RuntimeAnimatorController>("Customers/Alchemist/Green/alchemist_green_anim");
				break;
			case CustomerType.WIZARD_PURPLE:
				customerSprite = Resources.Load<Sprite>("Customers/Alchemist/Purple/Alchemist_purple_walk");
				customerAnimationController = Resources.Load<RuntimeAnimatorController>("Customers/Alchemist/Purple/alchemist_purple_anim");
				break;
			case CustomerType.WIZARD_YELLOW:
				customerSprite = Resources.Load<Sprite>("Customers/Alchemist/Yellow/Alchemist_yellow_walk");
				customerAnimationController = Resources.Load<RuntimeAnimatorController>("Customers/Alchemist/Yellow/alchemist_yellow_anim");
				break;
			case CustomerType.MERCHANT_BLUE:
				customerSprite = Resources.Load<Sprite>("Customers/Merchant/Blue/Merchant_blue_walk");
				customerAnimationController = Resources.Load<RuntimeAnimatorController>("Customers/Merchant/Blue/merchant_blue_anim");
				break;
			case CustomerType.MERCHANT_GREEN:
				customerSprite = Resources.Load<Sprite>("Customers/Merchant/Green/Merchant_green_walk");
				customerAnimationController = Resources.Load<RuntimeAnimatorController>("Customers/Merchant/Green/merchant_green_anim");
				break;
			case CustomerType.MERCHANT_PURPLE:
				customerSprite = Resources.Load<Sprite>("Customers/Merchant/Purple/Merchant_purple_walk");
				customerAnimationController = Resources.Load<RuntimeAnimatorController>("Customers/Merchant/Purple/merchant_purple_anim");
				break;
			case CustomerType.MERCHANT_GOLD:
				customerSprite = Resources.Load<Sprite>("Customers/Merchant/Gold/Merchant_gold_walk");
				customerAnimationController = Resources.Load<RuntimeAnimatorController>("Customers/Merchant/Gold/merchant_gold_anim");
				break;
			case CustomerType.KING:
				customerSprite = Resources.Load<Sprite>("Customers/King/King_walk");
				customerAnimationController = Resources.Load<RuntimeAnimatorController>("Customers/King/king_anim");
				break;
		}
	}

	private void SetOrderSprite() {
		switch (orderType)
		{
			case OrderType.BROWN_SUGAR: orderSprite = Resources.Load<Sprite>("Boba/brown_sugar_boba"); break;
			case OrderType.TARO: orderSprite = Resources.Load<Sprite>("Boba/taro_boba"); break;
			case OrderType.MATCHA: orderSprite = Resources.Load<Sprite>("Boba/matcha_boba"); break;
			case OrderType.THAI: orderSprite = Resources.Load<Sprite>("Boba/thai_boba"); break;
			case OrderType.JASMINE: orderSprite = Resources.Load<Sprite>("Boba/jasmine_boba"); break;
			case OrderType.MANGO: orderSprite = Resources.Load<Sprite>("Boba/mango_boba"); break;
			case OrderType.STRAWBERRY: orderSprite = Resources.Load<Sprite>("Boba/strawberry_boba"); break;
			case OrderType.COFFEE: orderSprite = Resources.Load<Sprite>("Boba/coffee_boba"); break;
			case OrderType.RED_BEAN: orderSprite = Resources.Load<Sprite>("Boba/red_bean_boba"); break;
			case OrderType.LYCHEE: orderSprite = Resources.Load<Sprite>("Boba/lychee_boba"); break;
			case OrderType.LEMONADE: orderSprite = Resources.Load<Sprite>("Boba/lemonade_boba"); break;
			case OrderType.ALOE_VERA: orderSprite = Resources.Load<Sprite>("Boba/aloe_vera_boba"); break;
			case OrderType.ALMOND_MILK: orderSprite = Resources.Load<Sprite>("Boba/almond_milk_boba"); break;
			case OrderType.COCONUT: orderSprite = Resources.Load<Sprite>("Boba/coconut_boba"); break;
			case OrderType.HONEYDEW: orderSprite = Resources.Load<Sprite>("Boba/honeydew_boba"); break;
			case OrderType.BLACK_SESAME: orderSprite = Resources.Load<Sprite>("Boba/black_sesame_boba"); break;
			case OrderType.PINEAPPLE: orderSprite = Resources.Load<Sprite>("Boba/pineapple_boba"); break;
			case OrderType.AVOCADO: orderSprite = Resources.Load<Sprite>("Boba/avocado_boba"); break;
			case OrderType.TIGER: orderSprite = Resources.Load<Sprite>("Boba/tiger_boba"); break;
			case OrderType.OOLONG: orderSprite = Resources.Load<Sprite>("Boba/oolong_boba"); break;
			case OrderType.EARL_GRAY: orderSprite = Resources.Load<Sprite>("Boba/earl_gray_boba"); break;
			case OrderType.ACAI: orderSprite = Resources.Load<Sprite>("Boba/acai_boba"); break;
			case OrderType.CARAMEL: orderSprite = Resources.Load<Sprite>("Boba/caramel_boba"); break;
			case OrderType.YUZU: orderSprite = Resources.Load<Sprite>("Boba/yuzu_boba"); break;
			case OrderType.BLUEBERRY: orderSprite = Resources.Load<Sprite>("Boba/blueberry_boba"); break;
		}
	}
}
