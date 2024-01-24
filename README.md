# ArcanaDungeon
## 1. 프로젝트 설명
본 프로젝트는 2022년 상명대학교 게임학과 팀 소고기추가마라샹궈에서 개발한 게임이다.

본 프로젝트의 목적은 Shattered Pixel Dungeon 과 Slay the Spire를 합친 게임으로 고전적인 로그라이크 형태의 움직임에 현대적인 카드 기반의 전투방식을 사용하는 게임을 개발하는 것이다.

맵생성 알고리즘은 아래글을 기반으로 따로 연구 개발 하였고

<https://www.reddit.com/r/roguelikedev/comments/1sd730/my_dungeon_generation_algorithm/>

게임에 필요한 리소스는 오픈소스인 아래 레포를 통하여 얻었다.

<https://github.com/hazard999/SharpDungeon/blob/master/levels/RegularLevel.cs>

## 2. 프로젝트 인원

프로그래머 강민석, 최정훈, 장근희, 한태종 

기획 최정훈

## 3. 업무 분담
강민석 : 맵 생성 알고리즘

최정훈 : 카드 기획 및 개발

장근희 : GUI

한태종 : 플레이어 이동, 몬스터 기획 및 개발

## 4. 프로젝트 환경

Unity 2020.3.12f1

Visual Studio

Github

## 5. 프로젝트 구조 
```
├─Assets
│  ├─Resources
│  │  ├─prefabs
│  │  │  ├─Enemies
│  │  │  ├─Player
│  │  │  └─Tiles
│  │  │      ├─BiomeExample1
│  │  │      ├─BiomeExample2
│  │  │      └─Normal
│  │  └─sprites
│  │      ├─animation
│  │      └─Enemies
│  ├─Scenes
│  └─Scripts
│      ├─Object
│      │  ├─Cards
│      │  ├─Enemy
│      │  │  └─Boss
│      │  └─player
│      ├─System
│      │  ├─levels
│      │  ├─painters
│      │  ├─Rooms
│      │  └─UI
│      └─util
├─Library
│  ├─APIUpdater
│  ├─Artifacts
│  ├─Collab
│  ├─PackageCache
│  ├─PackageManager
│  ├─ScriptAssemblies
│  ├─ShaderCache
│  ├─StateCache
│  │  ├─LayerSettings
│  │  ├─MainStageHierarchy
│  │  ├─PrefabStageHierarchy
│  │  └─SceneView
│  ├─Temp
│  │  └─ScriptUpdater
│  └─TempArtifacts
│      ├─Extra
│      └─Primary
├─Logs
├─obj
│  └─Debug
├─Packages
├─ProjectSettings
├─Temp
│  └─RefreshSync
└─UserSettings
```
