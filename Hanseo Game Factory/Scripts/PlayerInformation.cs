using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System.IO; //파일에서 게임 제목 등 정보 읽어오기

public class PlayerInformation //싱글톤 기법 적용, 전체 게임 관리
{
    //파이어베이스 인증 객체(Auth)는 한번 로그인이 완료되면 자동으로 로그인 한 사용자의 정보를 가지고 있는다.
    // 단 씬이 전환되면 인증 객체는 정보를 초기화한다. 그러므로 여기서 인증객체를 관리한다.
    public static Firebase.Auth.FirebaseAuth auth;
    //PlayerInformation.auth = auth; 이렇게 다른 cs 파일에서 초기화하여 관리할 수 있다. 

    public static string selectedGame { get; set; } //스테이지에서 사용자가 선택한 게임

    public static bool tutorialselected { get; set; } //사용자 튜토리얼 선택

    public static int gameScore { get; set; }

    public static bool quizGameClear { get; set; }

    public static bool defenseGameClear { get; set; }

    public static bool rhythmGameClear { get; set; }

    public static int quizScore { get; set; }
    public static int defenseScore { get; set; }
    public static int rhythmScore { get; set; }
    public static int rhythmMaxCombo { get; set; }

    public static bool fullTipUser { get; set; }

    // 파이어베이스 접근 싱글톤 기법 적용, Reference 변수 여기서 초기화
    public static DatabaseReference GetDatabaseReference()
    {
        //Firebase 접속 설정
        DatabaseReference reference;
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("Firebase URI ......");
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        // 테이블의 루트(데이터베이스명)를 가르키게 함
        return reference;
    }
    /*
    // 각 OS 별 파일 경로
    public static string pathForDocumentsFile(string filename)
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            string path = Application.dataPath.Substring(0, Application.dataPath.Length - 5);
            path = path.Substring(0, path.LastIndexOf('/'));
            return Path.Combine(Path.Combine(path, "Documents"), filename);
        }

        else if (Application.platform == RuntimePlatform.Android)
        {
            string path = Application.persistentDataPath + "/";
            path = path.Substring(0, path.LastIndexOf('/'));
            return Path.Combine(path, filename); // 자동적으로 / 추가해서 결합해줌
        }
        else
        {
            string path = Application.dataPath;
            path = path.Substring(0, path.LastIndexOf('/'));
            return Path.Combine(path, filename);
        }
    }
    */
    /* 
     * Application.persistentDataPath : /mnt/sdcard/Android/data/번들이름/files

     * Application.persistentDataPath : /data/data/번들이름/files/

     * Substring(int1, int2) : int1에서부터 int2까지의 문자열

     * LastIndexOf(string) : 마지막 string문자의 인덱스를 리턴

    */

    public static void createUserTipFile() //로그인 매니저 스크립트에서 사용자 팁 파일 생성함.
    {
        string path = Application.persistentDataPath + "/" + auth.CurrentUser.Email + ".txt"; //pathForDocumentsFile(auth.CurrentUser.Email + ".txt");

        
        if (!File.Exists(path))
        {
            FileStream userFile = new FileStream(path, FileMode.Create);
            
            userFile.Close();
        }
        else
            return;
    }

    public static void UpdateTip()
    {
        string path = Application.persistentDataPath + "/" + auth.CurrentUser.Email + ".txt"; //pathForDocumentsFile(auth.CurrentUser.Email + ".txt");

        FileStream userFile = new FileStream(path, FileMode.Open, FileAccess.Read);
        StreamReader stringReader = new StreamReader(userFile);

        int tipNumber = 1;
        while (stringReader.ReadLine() != null)
        {
            tipNumber++;
        }

        stringReader.Close();
        userFile.Close();

        if(tipNumber >= 13) //팁은 총 12개, 이미 획득 팁이 12개 이상이여서 13 이라면 파일에 쓰지 않고 함수 종료
        {
            fullTipUser = true;
            return;
        }
        else
        {
            fullTipUser = false;

            FileStream userWriteFile = new FileStream(path, FileMode.Append, FileAccess.Write); //Append = 이어쓰기, Open으로 열면 기존 내용 다 지우고 새로 덮어씀
            StreamWriter sw = new StreamWriter(userWriteFile);
            sw.WriteLine(tipNumber.ToString());

            sw.Close();
            userWriteFile.Close();
        }
    }

    public static string getTip()
    {
        if(fullTipUser)
        {
            return "0";
        }

        string path = Application.persistentDataPath + "/" + auth.CurrentUser.Email + ".txt"; //pathForDocumentsFile(auth.CurrentUser.Email + ".txt");

        FileStream userFile = new FileStream(path, FileMode.Open, FileAccess.Read);
        StreamReader stringReader = new StreamReader(userFile);

        string tipNumber = null;
        string temp;

        while ((temp = stringReader.ReadLine()) != null)
        {

            tipNumber = temp;

            if(tipNumber == "12")
            {
                fullTipUser = true;
                break;
            }
            
        }

        stringReader.Close();
        userFile.Close();

        if(tipNumber == null)
        {
            return "0";
        }
        else
        {
            return tipNumber;
        }  
    }

    public static List<string> getTipList()
    {
        List<string> tipList = new List<string>();
        string path = Application.persistentDataPath + "/" + auth.CurrentUser.Email + ".txt"; //pathForDocumentsFile(auth.CurrentUser.Email + ".txt");

        FileStream userFile = new FileStream(path, FileMode.Open, FileAccess.Read);
        StreamReader stringReader = new StreamReader(userFile);
        
        string tipNumber;
        while ((tipNumber = stringReader.ReadLine()) != null)
        {
            if (tipNumber == "12")
            {
                fullTipUser = true;
            }
            tipList.Add(tipNumber);
        }

        stringReader.Close();
        userFile.Close();

        return tipList;
    }
}
