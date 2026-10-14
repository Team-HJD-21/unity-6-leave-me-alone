# LEAVE ME ALONE!!! 🍅

TeamHJD's Unity 6 tomato-farming game project. The current gameplay build is a five-minute match where a player competes with an AI farmer: plant and water crops, harvest ripe tomatoes, and use earned items that affect crop growth or farm plots.

The repository is intended to continue toward a dedicated-server multiplayer version. The current gameplay scripts still use a local player and AI flow; installed networking packages alone do not mean that online play or server-authoritative gameplay is implemented. See [Docs](Docs/README.md) for the code-based overview and limits.

## Project at a glance

- Unity Editor: `6000.0.75f1`
- First build scene: `Assets/Scene/GameStartScene.unity`
- Main match scene: `Assets/Scene/MainGameScene.unity`
- Match timer: five minutes; the result scene is selected by comparing player and enemy scores.
- Crops grow while watered, then pass through a ripening period before they can be harvested.
- Harvests may grant a pickup truck or drone item. The player uses keys `1`, `2`, and `3` for the truck, boost drone, and toxic drone respectively.

## Open the project

Open this repository's root folder in Unity Hub with Editor `6000.0.75f1`. Let Package Manager finish resolving the packages in `Packages/manifest.json` and wait for script compilation before opening a scene.

## Documentation

- [Docs index](Docs/README.md) — source notes, reading order, and documentation boundaries.
- [Game overview](Docs/product/game-overview.md) — project intent and observable crop/item rules.
- [Gameplay flow](Docs/architecture/gameplay-flow.md) — scene, manager, player, AI, and crop flow.
- [Unity setup](Docs/tooling/unity-setup.md) — editor version, key packages, build scenes, and input notes.
- [Player guide](Docs/guides/player-guide.md) — controls, crop interactions, scoring, and item rewards.
- [The Boss guide](Docs/guides/boss-guide.md) — the AI farmer's role and source-verified behavior.
- [Item guide](Docs/guides/items-guide.md) — how to use the truck and drones, with current code effects.

---

## 한국어

TeamHJD의 Unity 6 토마토 농사 게임 프로젝트입니다. 현재 게임 코드는 5분 동안 AI 농부와 경쟁하며 작물을 심고, 물을 주고, 익은 토마토를 수확하는 흐름을 구현합니다. 수확으로 얻은 차량·드론 아이템은 작물 성장이나 밭 상태에 영향을 줍니다.

저장소의 방향은 Dedicated Server 기반 멀티플레이어입니다. 다만 현재 게임 스크립트는 로컬 플레이어와 AI 흐름으로 동작합니다. 네트워크 패키지가 설치되어 있다는 사실만으로 온라인 플레이나 서버 권한 게임 로직이 구현됐다고 보지는 않습니다. 코드 기준과 제한은 [Docs](Docs/README.md)를 참고하세요.

## 프로젝트 정보

- Unity Editor: `6000.0.75f1`
- 첫 실행 씬: `Assets/Scene/GameStartScene.unity`
- 경기 씬: `Assets/Scene/MainGameScene.unity`
- 경기 시간은 5분이며 플레이어와 적 점수를 비교해 결과 씬을 선택합니다.
- 작물은 물을 주는 동안 성장하고, 익는 시간을 거쳐 수확할 수 있습니다.
- 수확으로 차량 또는 드론 아이템을 얻을 수 있습니다. 플레이어는 `1`로 차량, `2`로 성장 촉진 드론, `3`으로 독 드론을 사용합니다.

## 실행

Unity Hub에서 저장소 루트 폴더를 Editor `6000.0.75f1`로 여세요. `Packages/manifest.json`의 패키지 복원과 스크립트 컴파일이 끝난 뒤 씬을 여세요.

## 문서

- [Docs 목차](Docs/README.md) — 기준, 읽는 순서, 문서 범위
- [게임 개요](Docs/product/game-overview.md) — 프로젝트 목적과 코드에서 확인한 작물·아이템 규칙
- [게임플레이 흐름](Docs/architecture/gameplay-flow.md) — 씬, 매니저, 플레이어, AI, 작물 흐름
- [Unity 설정](Docs/tooling/unity-setup.md) — Editor, 주요 패키지, 빌드 씬, 입력 관련 참고
- [플레이어 가이드](Docs/guides/player-guide.md) — 조작, 작물 상호작용, 점수, 아이템 획득
- [The Boss 안내](Docs/guides/boss-guide.md) — AI 농부의 역할과 코드에서 확인한 동작
- [아이템 사용 안내](Docs/guides/items-guide.md) — 차량·드론 조작과 현재 코드상 효과
