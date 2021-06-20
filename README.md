# Mobile-Game Unity C#
대학교를 주제로 Unity C# Firebase로 개발한 안드로이드 모바일 게임

![image](https://user-images.githubusercontent.com/59761622/122669205-2ef70500-d1f7-11eb-8388-b84d82528563.png)

(Unity, C#, Firebase, JSON, Singleton pattern)

Unity에서 C# 스크립트에 Monobehaviour를 상속하여 Unity 게임 엔진 제어와 Firebase를 활용하여 학교의 각 학과와 건물들에 연관된 총 3개의 게임(퀴즈 게임, 바이러스 디펜스 게임, 리듬 게임)을 개발하였으며 각 게임을 단계적으로 플레이 하고 사용자의 획득 점수로 전체 랭킹 순위를 볼 수 있으며 각 게임을 정상적으로 플레이 했을 시 학교에 관련된 정보를 획득할 수 있다. 

오브젝트 풀링 기법을 활용하여 메모리 힙 영역 최적화 및 FPS를 올려 성능을 개선하였으며 싱글턴 기법으로 코드 재사용, 멀티 스레드를 위해 코루틴(IEnumerator, yield return)을 활용하여 비동기 프로그래밍을 하였다. 리듬 게임의 경우 학교의 교가를 곡으로 선택하였으며 박자와 떨어지는 노트를 맞추기 위해 곡의 BPM 분석, 생동감 있는 게임을 위해 이펙트 및 애니메이션을 적용하였으며 구글 플레이 스토어에 모바일 앱으로 배포하였다.

