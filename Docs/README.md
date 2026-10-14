# LEAVE ME ALONE — Project Docs

수정일: 2026-10-15  
상태: `main` 코드 확인 기준 · Unity 런타임 검증 기록 아님

이 문서 폴더는 게임의 현재 코드 흐름, 프로젝트 실행에 필요한 환경, 저장소가 지향하는 범위를 설명한다. 제품 방향과 현재 구현을 구분한다. 문서에 적힌 코드 동작은 정적 소스 확인 결과이며, Editor·Client·Server에서 직접 실행해 확인했다는 뜻은 아니다.

## 현재 기준

기준으로 확인한 `main`은 `57e941c` (`Merge pull request #8 from hjd5031/develop`)이며 `develop`을 포함한다. Unity 버전은 `ProjectSettings/ProjectVersion.txt`의 `6000.0.75f1`이다.

저장소 설명은 Dedicated Server 멀티플레이어 버전을 지향한다. `Packages/manifest.json`에는 Netcode for GameObjects와 Multiplayer Services가 포함되어 있지만, 현재 `Assets`의 게임 C# 코드에서는 NGO의 `NetworkBehaviour`, `NetworkObject`, 네트워크 변수, RPC 사용을 확인하지 못했다. 따라서 패키지·빌드 설정만으로 온라인 매치, 서버 권한 상태 처리, 동기화를 구현 완료로 간주하지 않는다. 현재 확인 가능한 플레이 흐름은 로컬 Player와 Behavior Graph 기반 Enemy를 연결하는 단일 실행 구조다.

## 문서 안내

| 경로 | 내용 |
| --- | --- |
| [게임 개요](product/game-overview.md) | 프로젝트 목적과 현재 코드에서 확인한 경기·작물·아이템 규칙 |
| [게임플레이 흐름](architecture/gameplay-flow.md) | 씬 전환, 게임 시간, Player·Enemy, 작물과 아이템 연결 |
| [Unity 설정](tooling/unity-setup.md) | Editor 버전, 주요 패키지, Build Settings 씬, 입력 방식 |
| [플레이어 가이드](guides/player-guide.md) | 플레이어 조작, 작물 심기·물주기·수확, 점수와 아이템 획득 |
| [The Boss 안내](guides/boss-guide.md) | AI 농부의 역할과 확인된 동작, 정적 확인의 범위 |
| [아이템 사용 안내](guides/items-guide.md) | Pickup Truck·Boost Drone·Toxic Drone의 조작과 코드상 효과 |

저장소 소개와 빠른 실행 방법은 [최상위 README](../README.md)를 따른다.

## Source of Truth

| 확인할 내용 | 기준 |
| --- | --- |
| 현재 구현 동작 | `Assets/`의 실행 코드와 `Assets/Scene/`의 씬 |
| Unity·패키지 버전 | `ProjectSettings/ProjectVersion.txt`, `Packages/manifest.json`, `Packages/packages-lock.json` |
| 빌드 대상 씬 | `ProjectSettings/EditorBuildSettings.asset` |
| 프로젝트 방향과 진입 안내 | 최상위 [README](../README.md)와 [게임 개요](product/game-overview.md) |

수치나 설명이 코드와 다르면 실행 코드가 현재 동작의 기준이다. 계획·목표를 추가할 때는 현재 구현과 혼합하지 말고 상태를 표시한다. 인증 정보와 개인별 설정, 코드 전체 복사, 회의 원문은 이 문서에 넣지 않는다.
