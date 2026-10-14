# Unity 프로젝트 설정

수정일: 2026-10-15  
상태: 저장소 설정 기준 · 로컬 Editor 설치 및 빌드 성공 확인 아님

## Editor와 패키지

기준 Unity Editor는 `6000.0.75f1`이다. Unity Hub에서 저장소 루트를 열고 Package Manager가 `Packages/manifest.json` 및 `Packages/packages-lock.json`을 복원하게 둔다. 첫 씬을 열기 전에 스크립트 컴파일이 끝났는지 확인한다.

`manifest.json`의 주요 패키지:

| 패키지 | 버전 | 용도 |
| --- | --- | --- |
| `com.unity.netcode.gameobjects` | `2.13.1` | Unity Netcode for GameObjects |
| `com.unity.services.multiplayer` | `2.3.0` | Unity Multiplayer Services |
| `com.unity.services.vivox` | `16.11.0` | 음성 서비스 패키지 |
| `com.unity.behavior` | `1.0.9` | Enemy Behavior Graph |
| `com.unity.render-pipelines.universal` | `17.0.4` | URP 렌더링 |
| `com.unity.cinemachine` | `3.1.3` | 카메라와 차량 PIP |

패키지 설치 여부는 해당 기능이 현재 게임플레이에서 연결·검증됐다는 뜻이 아니다. 네트워크 범위는 [게임플레이 흐름](../architecture/gameplay-flow.md#네트워크-경계)을 참고한다.

## 빌드 씬

`ProjectSettings/EditorBuildSettings.asset`에서 아래 씬이 활성화되어 있다. `GameStartScene`이 빌드 씬 목록의 첫 번째 항목이다.

1. `Assets/Scene/GameStartScene.unity`
2. `Assets/Scene/MainGameScene.unity`
3. `Assets/Scene/GameWinScene.unity`
4. `Assets/Scene/GameLoseScene.unity`
5. `Assets/Scene/GameTieScene.unity`

## 입력과 확인 범위

현재 Player 컨트롤러는 `UnityEngine.Input` API를 사용한다. `ProjectSettings/ProjectSettings.asset`의 `activeInputHandler`는 `2`(Both)이고 `ProjectSettings/InputManager.asset`에 기본 축이 설정되어 있다. 입력 장치가 반응하지 않으면 이 두 설정과 스크립트 컴파일 상태를 확인한다. 패키지에 Input System이 있어도 현재 Player 코드가 새 Input System 액션을 사용한다는 뜻은 아니다.

Unity Editor를 열어 패키지 복원·컴파일 상태와 실제 씬 플레이를 확인한다. 이 문서는 전용 서버 빌드나 네트워크 매치 테스트 성공을 기록하지 않는다. 해당 검증 결과는 실행한 Editor·Build와 대상 씬을 함께 남긴다.
