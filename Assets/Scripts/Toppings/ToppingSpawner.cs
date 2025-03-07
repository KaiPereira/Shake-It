using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToppingSpawner : MonoBehaviour
{
    private float speed = 1.5f;

    List<GameObject> prefabList = new List<GameObject>();
    public GameObject pearlPre;
    public GameObject rockPre;
    public GameObject treePre;

    public Collider2D cupCollider;
    private List<Collider2D> objectsInside = new List<Collider2D>();

    public bool isRunning = true;

    private GameManager gameManager;
    private LevelLoader levelLoader;
    private MinigameHelper minigameHelper;
    private OrderManager orderManager;

    private float endingDuration = 4f;
    public AudioSource audioSource;
    public AudioSource completionAudioSource;
    public Transform middlePosition;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        minigameHelper = FindObjectOfType<MinigameHelper>();
        levelLoader = FindObjectOfType<LevelLoader>();
        orderManager = FindObjectOfType<OrderManager>();

        // Double odds of pearl
        prefabList.Add(pearlPre);
        prefabList.Add(pearlPre);
        prefabList.Add(pearlPre);
        prefabList.Add(rockPre);
        prefabList.Add(rockPre);
        prefabList.Add(treePre);
        prefabList.Add(treePre);
    
        StartCoroutine(Score());    
        StartCoroutine(Ending());
        StartCoroutine(DecreaseSpeed());
        StartCoroutine(SpawnToppings());
    }

    IEnumerator SpawnToppings()
    {
        while (isRunning)
        {
            int prefabIndex = Random.Range(0, 7);
            Vector3 randomPos = new Vector3(Random.Range(-7, 7), 3, 0);

            Instantiate(prefabList[prefabIndex], randomPos, Quaternion.identity);

            // Gets faster and faster
            if (speed > 0.3f)
            {
                speed -= 0.1f;
            }

            yield return new WaitForSeconds(speed);
        }
    }

    IEnumerator Score()
    {
        while (isRunning)
        {
            objectsInside.Clear();
            ContactFilter2D filter = new ContactFilter2D();
            cupCollider.OverlapCollider(filter, objectsInside);

            objectsInside.RemoveAll(collider => !collider.CompareTag("Boba"));

            gameManager.UpdateTopping(objectsInside.Count);

            yield return new WaitForSeconds(1);
        }
    }

    IEnumerator DecreaseSpeed()
    {
        while (isRunning)
        {
            yield return new WaitForSeconds(1f);

            if (speed > 0.3f)
            {
                speed -= 0.1f;
            }
        }
    }
    
    IEnumerator Ending()
    {
        yield return new WaitForSeconds(25);

        isRunning = false;

        StartCoroutine(minigameHelper.FadeOutMusic(endingDuration, audioSource));

		completionAudioSource.volume = 0;
		completionAudioSource.Play();
		StartCoroutine(minigameHelper.FadeInCompletionMusic(endingDuration, completionAudioSource));

		StartCoroutine(minigameHelper.FadeStarsToMiddle(endingDuration, middlePosition));

		orderManager.CompleteOrder();

		yield return new WaitForSeconds(4);
		StartCoroutine(levelLoader.UnloadAdditiveScene());
    }
}
