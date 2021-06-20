using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System.Threading.Tasks;

public class GameResultManager : MonoBehaviour
{
    public Text QuizMaxAnswerUI;
    public Text DefenseMaxLifeUI;
    public Text RhythmScoreUI;
    public Text TotalScoreUI;

    public Text rank1UI;
    public Text rank2UI;
    public Text rank3UI;
    public Text rank4UI;
    public Text rank5UI;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerInformation.rhythmGameClear)
        {
            // ""+로 int를 문자열로 바꾸기 UI 텍스트는 스트링임
            QuizMaxAnswerUI.text = "이학관 :" + PlayerInformation.quizScore + "점";
            DefenseMaxLifeUI.text = "보건관 :" + PlayerInformation.defenseScore + "점";
            RhythmScoreUI.text = "예술관 :" + PlayerInformation.rhythmScore + "점, 최대 콤보 : " + PlayerInformation.rhythmMaxCombo;
            TotalScoreUI.text = "최종 점수 :" + PlayerInformation.gameScore + "점";
        }

        //Firebase에 접근할 때 비동기적으로 통신하기에 설명
        rank1UI.text = "데이터를 불러오는 중입니다.";
        rank2UI.text = "데이터를 불러오는 중입니다.";
        rank3UI.text = "데이터를 불러오는 중입니다.";
        rank4UI.text = "데이터를 불러오는 중입니다.";
        rank5UI.text = "데이터를 불러오는 중입니다.";

        // DB 접근 싱글톤 기법 적용
        DatabaseReference reference = PlayerInformation.GetDatabaseReference().Child("~~~~").Child("~~~~");

        // DB 데이터를 JSON 형태로 가져오기, OrderByChild - 오름차순
        reference.OrderByChild("~~~").GetValueAsync().ContinueWith(
            task => {

            if (task.IsCompleted) //성공적으로 데이터를 task로 가져왔다면
            {
                List<string> rankList = new List<string>();
                List<string> emailList = new List<string>();
                DataSnapshot snapshot = task.Result; //데이터 결과(루트, JSON 데이터) 저장

                // 파이어베이스 루트의 자식인 JSON 데이터의 각 원소에 접근
                foreach (DataSnapshot data in snapshot.Children)
                {
                    IDictionary rank = (IDictionary)data.Value; //딕셔너리 자료형으로 접근 json은 딕셔너리에 특화됨
                    emailList.Add(rank["~~~~"].ToString()); //딕셔너리 키가 이메일인 값만 뽑아서 담음
                    rankList.Add(rank["~~~~"].ToString());
                }

                // 오름차순으로 데이터들 정렬되어서 내림차순으로 바꿈
                emailList.Reverse();
                rankList.Reverse();

                // 상위 3개 랭크 사용자 보여주기, 플레이한 사용자가 없을 상황을 대비해 디폴트 값 적용
                rank1UI.text = "아직 플레이 한 사용자가 없습니다.";
                rank2UI.text = "아직 플레이 한 사용자가 없습니다.";
                rank3UI.text = "아직 플레이 한 사용자가 없습니다.";
                rank4UI.text = "아직 플레이 한 사용자가 없습니다.";
                rank5UI.text = "아직 플레이 한 사용자가 없습니다.";

                List<Text> textList = new List<Text>();
                textList.Add(rank1UI);
                textList.Add(rank2UI);
                textList.Add(rank3UI);
                textList.Add(rank4UI);
                textList.Add(rank5UI);
                     
                int count = 1; //순위
                for (int i = 0; i < rankList.Count && i < 5; i++) //상위 5개만 출력
                {
                    textList[i].text = count + "위: " + emailList[i] + " (" + rankList[i] + "점)";
                    count = count + 1;
                }
            }
        },TaskScheduler.FromCurrentSynchronizationContext());
    }

    public void Back() //스테이지 선택창으로 이동
    {
        PlayerInformation.quizGameClear = false;
        PlayerInformation.defenseGameClear = false;
        PlayerInformation.rhythmGameClear = false;
        PlayerInformation.gameScore = 0;

        SceneManager.LoadScene("StageSelectScene");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
