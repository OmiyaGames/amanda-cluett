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

	RandomList<NewsEntry> randomEntries = null;
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

	public void NextNewsEntry()
	{
		newText.text = randomEntries.RandomElement.News;
		animator.SetTrigger(nextEntryTrigger);
	}

	public void SwapLabels()
	{
		currentText.text = newText.text;
	}

	// Use this for initialization
	void Start ()
	{
		randomEntries = new RandomList<NewsEntry>(startingEntries);
		currentText.text = randomEntries.RandomElement.News;
	}

	void Update()
	{
		// Check if the second passed
		if ((lastStarted > 0) && ((Time.time - lastStarted) > updateNewsEvery))
		{
			// Switch to the next news
			NextNewsEntry();

			// Reset time
			lastStarted = Time.time;
		}
	}
}
