# Current-Venture

Unity 기반 3D RPG/액션 프로토타입입니다. 플레이어 상태 머신, 몬스터, 탄환, 카메라 처리, 상호작용 구조를 실험합니다.

## 프로젝트 개요

`Current-Venture`는 RPG 전투와 탐색 기능을 연습하기 위한 Unity 프로젝트입니다. 플레이어 상태를 세분화하고, 몬스터/탄환/스폰 로직을 포함해 액션 게임의 기본 루프를 구성합니다.

## 주요 구현 영역

- 플레이어 상태 머신
- Idle, Walk, Run, Jump, Fall, Attack 상태 분리
- 콤보 공격 처리
- 몬스터와 탄환 로직
- 몬스터 스포너
- 카메라 전환과 투명화 처리
- ScriptableObject 기반 플레이어 데이터 관리

## 기술 스택

- Unity `2022.3.2f1`
- C#
- Unity Input System
- Cinemachine
- Post Processing
- Unity UI
- DOTween 계열 패키지

## 폴더 구조

```text
.
├── Assets/
│   ├── Scripts/
│   │   ├── Characters/
│   │   │   ├── Player/
│   │   │   └── Monster/
│   │   ├── Camera/
│   │   └── StateMachine/
│   ├── ScripableObjects/
│   └── Scenes/
├── Packages/
├── ProjectSettings/
└── README.md
```

## 실행 방법

1. Unity Hub에서 `2022.3.2f1` 버전을 설치합니다.
2. 저장소 루트 폴더를 Unity 프로젝트로 엽니다.
3. 패키지 복원이 끝날 때까지 기다립니다.
4. `Assets/Scenes` 아래의 시작 씬을 열고 실행합니다.

## 학습 포인트

- 상태 머신 기반 플레이어 제어
- 몬스터 생성과 공격 흐름 구성
- 카메라 처리와 전투 조작 연결
- ScriptableObject로 플레이어 데이터를 분리하는 방식

## 개선 아이디어

- 시작 씬과 조작법 명시
- 전투 루프와 몬스터 패턴 설명 추가
- 플레이 영상 또는 스크린샷 추가
- 직접 구현한 스크립트 목록 정리
