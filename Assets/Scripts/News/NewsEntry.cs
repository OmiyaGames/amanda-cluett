using UnityEngine;

public class NewsEntry : MonoBehaviour
{
	[SerializeField]
	[Multiline]
	string news;

	public string News
	{
		get
		{
			return news;
		}
	}
}
