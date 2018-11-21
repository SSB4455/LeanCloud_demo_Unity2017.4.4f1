using LeanCloud;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
	public Text outputText;
	public Button submitButton;
	public InputField intputField;



	// Use this for initialization
	void Start()
	{
		new AVQuery<AVObject>("GameScore").FindAsync().ContinueWith(t2 =>
		{
			string scoresStr = "";
			foreach (AVObject avobj in t2.Result)
			{
				Debug.Log(avobj["playerName"] + " " + avobj["score"] + " " + avobj.CreatedAt);
				scoresStr += avobj.CreatedAt.Value.ToString("yyyy-MM-dd HH:mm:ss") + " |\t" + avobj["score"] + "\t|\t" + avobj["playerName"] + "\n";
			}
			outputText.text = scoresStr;
		});
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void SubmitScore()
	{
		AVObject gameScore = new AVObject("GameScore");
		gameScore["playerName"] = "player1";
		gameScore["score"] = int.Parse(intputField.text);
		intputField.text = int.Parse(intputField.text) + 1 + "";
		Task saveTask = gameScore.SaveAsync().ContinueWith(t =>
		{
			new AVQuery<AVObject>("GameScore").FindAsync().ContinueWith(t2 =>
			{
				string scoresStr = "";
				foreach(AVObject avobj in t2.Result )
				{
					Debug.Log(avobj["playerName"] + " " + avobj["score"] + " " + avobj.CreatedAt);
					scoresStr += avobj.CreatedAt.Value.ToString("yyyy-MM-dd HH:mm:ss") + " |\t" + avobj["score"] + "\t|\t" + avobj["playerName"] + "\n";
				}
				outputText.text = scoresStr;
			});
		});
	}
}
