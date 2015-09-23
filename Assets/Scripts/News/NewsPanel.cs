using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using OmiyaGames;

public class NewsPanel : MonoBehaviour
{
	[Header("Timings")]
	[SerializeField]
	[Tooltip("In seconds")]
	float updateNewsEvery = 7.5f;

	[Header("Important Components")]
	[SerializeField]
	GamePanel parentPanel;
	[SerializeField]
	Text currentText;
	[SerializeField]
	Text newText;

	[Header("Animations")]
	[SerializeField]
	Animator animator;
	[SerializeField]
	string nextEntryTrigger = "next";

	[Header("News")]
	[SerializeField]
	List<NewsEntry> startingEntries;
	[SerializeField]
	int numberOfEntriesToCache = 3;

	RandomList<NewsEntry> randomEntries = null;
	readonly Queue<NewsEntry> cacheEntries = new Queue<NewsEntry>();
	float lastStarted = -1f;

	public List<NewsEntry> AllEntries
	{
		get
		{
			return startingEntries;
		}
	}

	public void Pause()
	{
		lastStarted = -1f;
	}
	
	public void Resume()
	{
		lastStarted = Time.time;
		if(lastStarted < 0.1f)
		{
			lastStarted = 0.1f;
		}
	}

	public void NextNewsEntry(NewsEntry entry = null)
	{
		// Check if any entry is provided
		if(entry == null)
		{
			// If not, choose a random element
			entry = randomEntries.RandomElement;

			// Get an entry that isn't in the cache list
			//Debug.Log("Random Entry Selected");
			while(cacheEntries.Contains(entry) == true)
			{
				// Keep grabbing a random element
				Debug.Log("Duplicate News found:\n" + entry.News);
				entry = randomEntries.RandomElement;
			}
		}

		// Display this news
		newText.text = entry.News;
		animator.SetTrigger(nextEntryTrigger);

		// Reset time
		if(lastStarted > 0)
		{
			Resume();
		}

		// Add entry to the list
		cacheEntries.Enqueue(entry);

		// If this exceeds the size, start removing entries
		while(cacheEntries.Count > numberOfEntriesToCache)
		{
			entry = cacheEntries.Dequeue();
			Debug.Log("Removing News from cache:\n" + entry.News);
		}
		Debug.Log("Cache size: " + cacheEntries.Count);
	}

	public void SwapLabels()
	{
		currentText.text = newText.text;
	}

	// Use this for initialization
	void Start ()
	{
		// Setup random list
		randomEntries = new RandomList<NewsEntry>(startingEntries);

		// Setup current text
		NewsEntry tempEntry = randomEntries.RandomElement;
		currentText.text = tempEntry.News;

		// Cache news
		cacheEntries.Clear();
		cacheEntries.Enqueue(tempEntry);
	}

	void Update()
	{
		// Check if the second passed
		if ((lastStarted > 0) && ((Time.time - lastStarted) > updateNewsEvery))
		{
			// Switch to the next news
			NextNewsEntry();
		}
	}
}
