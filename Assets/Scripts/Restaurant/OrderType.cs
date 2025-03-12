using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OrderType {
	BROWN_SUGAR,
	TARO,
	MATCHA,
	THAI,
	JASMINE,
	MANGO,
	STRAWBERRY,
	COFFEE,
	RED_BEAN,
	LYCHEE,
	LEMONADE,
	ALOE_VERA,
	ALMOND_MILK,
	COCONUT,
	HONEYDEW,
	BLACK_SESAME,
	PINEAPPLE,
	AVOCADO,
	TIGER,
	OOLONG,
	EARL_GRAY,
	ACAI,
	CARAMEL,
	YUZU,
	BLUEBERRY	
}

public enum Toppings {
	TAPIOCA,
	JELLY,
	CHEESE,
	GRASS,
	ALOE,
	EGG,
	BEANS
}

public class Order
{
	public OrderType drinkName;
	public List<Toppings> toppings;
	public float timeLimit;
	public int step;
	public bool completed;
	public float accuracy;
	public float multiplier;
	public string id;

	public Order(OrderType drinkName, List<Toppings> toppings, float timeLimit, float multiplier, string id)
	{
		this.drinkName = drinkName;
		this.toppings = toppings;
		this.timeLimit = timeLimit;
		this.step = 0;
		this.accuracy = 0f;
		this.completed = false;
		this.multiplier = multiplier;
		this.id = id;
	}
}
