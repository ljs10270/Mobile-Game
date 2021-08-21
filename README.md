# Mobile-Game Unity C#
대학교를 주제로 Unity C# Firebase로 개발한 안드로이드 모바일 게임

![image](https://user-images.githubusercontent.com/59761622/122669205-2ef70500-d1f7-11eb-8388-b84d82528563.png)

Unity에서 C# 스크립트에 Monobehaviour를 상속하여 Unity 게임 엔진 제어와 Firebase를 활용하여 학교의 각 학과와 건물들에 연관된 총 3개의 게임(퀴즈 게임, 바이러스 디펜스 게임, 리듬 게임)을 개발하였으며 튜토리얼을 진행할 수 있고 각 게임별 설명서가 존재한다. 각 게임을 단계적으로 플레이 하고 사용자의 획득 점수로 전체 랭킹 순위와 실시간 랭킹 순위를 볼 수 있으며 각 게임을 정상적으로 플레이 했을 시 학교에 관련된 정보를 획득할 수 있다. 

오브젝트 풀링 기법을 활용하여 메모리 힙 영역 최적화 및 FPS를 올려 성능을 개선하였으며 Singleton pattern으로 코드 재사용, 싱글 스레드 기반인 Unity 엔진에서 멀티 스레드를 위해 코루틴(IEnumerator, yield return)을 활용하여 비동기 프로그래밍을 하였다. 리듬 게임의 경우 학교의 교가를 곡으로 선택하였으며 박자와 떨어지는 노트를 맞추기 위해 곡의 BPM 분석, 생동감 있는 게임을 위해 오디오, 이펙트 및 애니메이션을 적용하였으며 GitHub를 통한 형상관리를 하여 Google Play Store에 모바일 앱으로 배포하였다.

1. 개발 환경 및 사용 도구와 패턴
- 개발 플랫폼(IDE): Unity 2019.3.3f1
- 코드 편집기: Visual Studio 2017
- 개발 언어: C#
- Unity API: Monobehaviour
- DBMS(NoSQL): Google Firebase Realtime Databse (Spark 요금제)
- 디자인 패턴: 싱글턴 패턴(Singleton pattern)
- BPM 분석 도구: BPM Analyzer
- 형상 관리: Git (GitHub, GitHub Desktop)
- 테스트 도구:　Android Studio Emulator / 갤럭시(Android) 스마트폰
- 배포: Google Play Store
- 통신 방식: Async, IDictionary-JSON, POSIX Time-TimeStamp, Android DataPath

2. Target 플랫폼
- Target OS: Linux
- Target Middleware: Android
- 해상도: 1920*1200(16:10)

3. Android 환경을 위한 Unity에 사용된 외부 개발 Kit 및 Plug-In 
- JDK 1.8.0_201
- Android SDK 29.02
- Android NDK r19
- Firebase Unity SDK
- Google-services.json

4. 사용된 오디오(Full CC license)
- 배경음(바이러스 디펜스 게임): www.dig.ccmixter.org/free
- 효과음(바이러스 디펜스 게임): www.zapsplat.com/free-sound-effects/ 
- 배경음(리듬게임): 한서대학교 교가
 https://www.hanseo.ac.kr/sub/info.do?page=010803&m=010803&s=hs

5. 사용된 Font(한글 글꼴)
- 나눔 고딕 무료 폰트: https://hangeul.naver.com/(네이버 한글한글 아름답게)
- BelloPro 무료 폰트: https://www.download-free-fonts.com/details/91171/bello-pro

![image](https://user-images.githubusercontent.com/59761622/130316830-b1980898-03db-4f57-8b71-facec325aeba.png)

![image](https://user-images.githubusercontent.com/59761622/130316843-317fec39-6e8b-4523-a39b-fae3cdc2e25b.png)

![image](https://user-images.githubusercontent.com/59761622/130316857-52195bdc-64b4-4f99-8933-7acc6c9ee565.png)

![image](https://user-images.githubusercontent.com/59761622/130316871-79d90b28-fa6a-4f77-aca8-4a57fc762a6c.png)
![image](https://user-images.githubusercontent.com/59761622/130316889-f9506666-60bb-4d19-83cc-77da5e34344c.png)
![image](https://user-images.githubusercontent.com/59761622/130316900-ce6babf1-5232-4a2a-a2e5-31ddbeb879aa.png)
![image](https://user-images.githubusercontent.com/59761622/130316903-b360b360-dabf-4dd3-ac56-acf1bef5342a.png)

