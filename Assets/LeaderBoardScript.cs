using LeanCloud;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class LeaderBoardScript : MonoBehaviour
{
	public Text outputText;
	public Button submitButton;
	public InputField intputField;



	// Use this for initialization
	void Start()
	{
		UpdateLeaderBoardText();
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void SubmitScore()
	{
		AVObject leaderBoardScore = null;
		new AVQuery<AVObject>("GameScore").WhereEqualTo("playerName", "player1").OrderByDescending("score").FindAsync().ContinueWith(t =>
		{
			foreach (AVObject avobj in t.Result)
			{
				Debug.Log(avobj["playerName"] + " " + avobj["score"] + " " + avobj.CreatedAt);
				leaderBoardScore = avobj;
				break;
			}

			if (leaderBoardScore == null)
			{
				leaderBoardScore = new AVObject("GameScore");
				leaderBoardScore["playerName"] = "player1";
				leaderBoardScore["score"] = int.Parse(intputField.text);
				intputField.text = int.Parse(intputField.text) + 1 + "";
			}
			if ((int)(leaderBoardScore["score"]) < int.Parse(intputField.text))
			{
				leaderBoardScore["score"] = int.Parse(intputField.text);
				leaderBoardScore.SaveAsync().ContinueWith(t2 =>
				{
					UpdateLeaderBoardText();
				});
			}
		});
	}

	void UpdateLeaderBoardText()
	{
		new AVQuery<AVObject>("GameScore").OrderByDescending("score").FindAsync().ContinueWith(t =>
		{
			string scoresStr = "";
			foreach (AVObject avobj in t.Result)
			{
				Debug.Log(avobj["playerName"] + " " + avobj["score"] + " " + avobj.UpdatedAt);
				scoresStr += avobj.UpdatedAt.Value.ToString("yyyy-MM-dd HH:mm:ss") + " |\t" + avobj["score"] + "\t|\t" + avobj["playerName"] + "\n";
			}
			outputText.text = scoresStr;
		});
	}
}
