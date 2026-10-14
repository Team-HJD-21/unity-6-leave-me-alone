# 아이템 사용 안내

수정일: 2026-10-15  
상태: Player·Enemy 아이템 스크립트 정적 확인 · 플레이 테스트 전

Player는 작물을 수확해 아이템 보유 상태를 얻을 수 있다. 보유 아이콘이 켜졌을 때 해당 키를 눌러 사용한다. 아이템 오브젝트는 자동으로 작동하며, 별도 조준이나 추가 입력은 없다.

## 아이템 한눈에 보기

| 아이템 | Player 입력 | 코드에서 확인한 효과 | 사용 조건 |
| --- | --- | --- | --- |
| Pickup Truck | `1` | 상대편 밭을 지나가며 작물 진행도를 초기화한다. 차량 이동은 자동이다. | 차량 아이템 보유 중이고 Enemy 차량이 사용 중이 아니어야 한다. |
| Boost Drone | `2` | 사용자 소유 농장의 작물 성장 간격을 2초에서 1초로, 익는 대기시간을 10초에서 5초로 줄인다. 60초 뒤 기본값 복구를 예약한다. | Drone 아이템 보유 중이어야 한다. |
| Toxic Drone | `3` | 아래 주의사항처럼 현재는 상대 밭이 아니라 사용자의 작물 배열을 초기화한다. | Drone 아이템 보유 중이어야 한다. |

Drone 아이템은 Boost와 Toxic 중 하나를 골라 쓰며, 사용하면 공통 Drone 보유 상태를 소비한다. Pickup Truck도 한 번 사용하면 보유 상태를 소비한다. Player 차량은 Enemy 차량 사용 중에 `1`을 눌러도 생성되지 않고 아이템 보유는 유지된다.

## Pickup Truck

`1`을 누르면 플레이어 위치에 차량이 생성된다. 차량은 별도 운전 조작을 받지 않고 경로를 따라 자동으로 이동한다. Enemy 밭의 작물 슬롯에 닿으면 작물을 빈 밭 상태로 되돌린다. 단, Enemy 작물이 물주기 성장 중(`UnripeEnemyTomato`)이면 완전히 빈 밭으로 만들지 않고 가장 초기 성장 단계로 되돌린다.

AI 차량은 Player 밭의 작물을 대상으로 같은 방식으로 충돌 효과를 낸다. 차량 연출에는 카메라 화면(PIP)과 소리·라이트가 포함된다. 사용자 입력으로 차량을 조종하는 코드는 확인되지 않았다.

## Boost Drone

`2`를 누르면 플레이어 위치에 드론이 생성된다. 드론은 잠시 후 살포 동작을 하고, Player의 `playerTomatoes`에 등록된 모든 작물에 성장 촉진을 적용한다. 성장 중인 작물은 더 빠르게 성장하고, 익는 대기시간도 짧아진다. 각 작물 컨트롤러는 60초 후 기본 간격을 복구하도록 예약한다.

Enemy가 사용하는 Boost Drone은 `enemyTomatoes`에 효과를 준다. 따라서 Boost 효과는 코드상 드론 소유자의 농장을 대상으로 한다.

## Toxic Drone

`3`을 누르면 플레이어 위치에 드론이 생성되어 작물 초기화 효과를 적용한다. 현재 코드에서 `PlayerToxicDrone`은 `GameManager.playerTomatoes`를, `EnemyToxicDrone`은 `GameManager.enemyTomatoes`를 순회한다. 즉 양쪽 모두 자기 소유자의 작물 목록을 대상으로 작물을 초기 상태(빈 밭)로 되돌린다.

이 동작은 독 드론이 상대 농장을 공격할 것이라는 게임 설명과 다를 수 있다. 실제 플레이에서 의도된 공격 방향은 확인되지 않았으므로, 현재 가이드는 코드에서 확인한 효과를 기준으로 설명한다.

## 아이템 획득 및 사용의 현재 제한

- Player 쪽 아이템 보상은 자기 작물 수확 코드에서만 확인했다. 내 작물을 수확할 때 무작위 결과 하나는 보상 없음, 하나는 차량, 하나는 드론이다. 이미 같은 종류 아이템이 있으면 보유 상태를 덮어쓰지 않는다.
- 상대 작물을 훔쳐 수확할 때 점수는 오르지만 해당 처리 경로에서 Player 아이템 보상은 지급하지 않는다.
- AI는 다른 조건으로 아이템을 소비한다. 차량은 Player가 아이템을 사용 중이 아닐 때와 Player 밭의 작물 수 등 조건을 검사하고, 드론은 AI 판단에 따라 자동 생성된다.
- 이 문서의 효과와 시간은 코드에서 확인한 값이다. 밸런스, AI 선택 확률의 체감, 화면 UI 안내의 완전성은 플레이 테스트 전이다.

## 코드 위치

| 기능 | 파일 |
| --- | --- |
| Player 입력과 아이템 생성 | `Assets/02.Scripts/Player/PlayerCtrl.cs` |
| 작물 수확 보상·성장·초기화 | `Assets/02.Scripts/Player/PlayerTomatoCtrl.cs`, `Assets/02.Scripts/Enemy/EnemyTomatoCtrl.cs` |
| AI 아이템 조건 | `Assets/02.Scripts/Enemy/EnemyCtrl.cs` |
| 드론 효과 | `Assets/02.Scripts/OtherObjects/PlayerBoostDrone.cs`, `PlayerToxicDrone.cs`, `EnemyBoostDrone.cs`, `EnemyToxicDrone.cs` |
| 차량 화면 효과 | `Assets/02.Scripts/OtherObjects/VehicleLightCtrl.cs` 및 Player/Enemy Vehicle Behavior Graph |
