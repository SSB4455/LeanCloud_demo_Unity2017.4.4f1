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
		LoginOrSignin();
		UpdateLeaderBoardText();
	}

	private void LoginOrSignin()
	{
		if (AVUser.CurrentUser == null)
		{
			AVUser user = new AVUser();
			user.Username = "player1";
			user.Password = "player1";
			user.Email = "player1@player1.com";
			user.SignUpAsync();
		}
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
				if (AVUser.CurrentUser != null)
				{
					leaderBoardScore.ACL = new AVACL();
					leaderBoardScore.ACL.PublicReadAccess = true;
					leaderBoardScore.ACL.SetWriteAccess(AVUser.CurrentUser, true);
				}
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
			if (AVUser.CurrentUser != null)
			{
				scoresStr = "CurrentUser is \'" + AVUser.CurrentUser.Username + "\'\n";
			}
			foreach (AVObject avobj in t.Result)
			{
				Debug.Log(avobj["playerName"] + " " + avobj["score"] + " " + avobj.UpdatedAt);
				scoresStr += avobj.UpdatedAt.Value.ToString("yyyy-MM-dd HH:mm:ss") + " |\t" + avobj["score"] + "\t|\t" + avobj["playerName"] + "\n";
			}
			outputText.text = scoresStr;
		});
	}
}
