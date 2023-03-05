# Galaga Consol Gamea

### 서영은 / 기간: 23/03/01 ~ 23/03/05

### 목차

#### 게임 소개
콘솔 슈팅 게임 \
![Gama_img](https://user-images.githubusercontent.com/123847823/222946273-4bb1943f-c4f6-4835-a694-7c8972b24371.png)

#### 플레이 방법 
- 자동실행
- 화살표 키보드를 움직이면서 적의 총알을 피하세요.
- 스페이스바를 눌러 총을 발사해 적을 맞춰 점수를 얻으세요.
- 적의 총알에 플레이어가 맞으면 Life Point가 1씩 줄어들다 Life Point가 0이 되면 게임이 종료됩니다.

#### 기술 스택
- static char[,] pixel \
=> 2차원 배열 사용: 맵 프레임 구성을 위해 사용. 2차원 배열로 플레이어, 적, 총알, 아이템 그림 삽입.
- 배경색, 콘솔 제목, 글씨색 변경 \
=> Console.Title = "Galaga"; : 콘솔 타이틀 변경 \
=> Console.SetWindowSize(width, height); : 콘솔 사이즈 변경 \
=> Console.BackgroundColor = ConsoleColor.White; : 콘솔 배경 변경 \
=> Console.ForegroundColor = ConsoleColor.Blue; : 콘솔 글씨 변경 \
=> Console.ResetColor(); : 컬러 리셋 
- 키 입력 받아 위, 아래, 오른쪽, 왼쪽으로 이동하기 : 키 입력 받아 ↑, ↓, →, ←, spacebar 사용 가능 \
=> var key = Console.ReadKey();
- 아이템을 획득하면 위치 랜덤으로 생성 : 랜덤 함수 사용 \
=> Random rand = new Random(); \
=> int number = rand.Next(-1, 2);
- Console.Clear(); 사용으로 전체 콘솔 내용 지우기 가능. 
- 위 Console.Clear(); 문제점으로 반복문 사용 시 콘솔 깜빡임 현상이 심해짐. \
=> Console.SetCursorPosition(0, 0); 으로 변경해주니 깜빡임 사라짐. 
- public static void ClearBuffer() \
=> 키입력 받을 때 입력 신호가 쌓여 버벅이거나 키가 한참 뒤에 입력되는 현상이 생겨 메서드 생성하여 키가 한 번 입력 될 때마다 쌓인 키입력 지워줌
- for문 \
=> 오프닝 숫자 카운트
- while문 \
=>게임 실행 됨. 키 입력 받고, 만든 프레임 띄우기
- Thread.Sleep(); \

#### 보완할 점
- 게임 강제 시작 및 다시 시작하기 기능 없음(선택지가 부족)
- 클래스 없이 if문 다량 사용, 다양한 클래스 사용으로 메인에서 필요해 보임.
- 적과 아이템이 다양했으면 좋았을 듯..
- 게임 타임 제한 필요해 보임
- 콘솔 깜빡임은 해결 되었으나 배경색 일부가 사라지는 현상 발생
