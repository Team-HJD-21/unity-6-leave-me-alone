# The Boss (AI 농부) 안내

수정일: 2026-10-15  
상태: 씬·Behavior Graph·AI 스크립트 정적 확인 · AI 플레이 테스트 전

## The Boss가 가리키는 대상

`MainGameScene`의 “The Boss”는 별도의 보스전이나 체력·전투 시스템을 가진 적이 아니라, 플레이어와 점수를 겨루는 AI 농부(Enemy) 캐릭터를 가리킨다. AI는 `Assets/07.Animations/Behavior/BossBehavior.asset`의 Unity Behavior Graph 및 `Assets/02.1BehaviorScripts/`의 Action/Condition 코드로 작동한다. Player가 보스를 직접 조작하는 입력은 현재 코드에서 확인되지 않았다.

## 경기 중 확인 가능한 AI 역할

- AI 농장에서 씨앗을 심고 작물에 물을 줘 성장시킨다.
- 익은 AI 작물을 수확해 6점을 얻는다. Player 밭의 익은 작물을 수확하면 4점을 얻는다.
- AI 농장에서 익은 작물 수확을 두고 Player와 경쟁한다. Player도 가까이에서 익은 AI 작물을 수확해 4점을 얻을 수 있다.
- AI 작물 수확 시 무작위로 차량 또는 드론 보유 상태를 얻을 수 있다.
- 아이템 사용 시점은 Player의 키 입력처럼 고정되어 있지 않다. `EnemyCtrl`은 Player 아이템 사용 여부, Player 농장의 작물 상태, 무작위 결과 등을 조건으로 차량·드론을 생성한다.
- AI 차량/드론은 Behavior Graph를 사용하며, C# 액션과 그래프 에셋의 Blackboard 설정이 함께 동작한다.

## 상대하는 방법

경기 제한 시간 안에 내 작물을 키워 꾸준히 수확하고, 기회가 있으면 AI가 사용 중이 아닌 익은 AI 작물도 수확한다. 차량·Toxic Drone은 현재 구현상 작물 진행도를 되돌릴 수 있다. Boost Drone은 사용자의 작물 성장 시간을 줄인다. 대상 차이는 [아이템 사용 안내](items-guide.md)에서 확인한다.

이 설명은 Behavior Graph 에셋과 액션의 정적 확인에서 요약한 AI 역할이다. 난이도, 행동 우선순위, 각 상황에서의 실제 반응은 Unity에서 경기를 실행해 검증하지 않았다. “The Boss”라는 이름만으로 보스 체력전이나 특정 난이도를 가정하지 않는다.
