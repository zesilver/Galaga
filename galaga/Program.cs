using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace game
{
    class Program
    {
        static int row = 15; //행
        static int column = 20; //열
        static int score = 0; //점수 저장
        static int life = 5;
        static char[,] pixel = new char[row, column]; //적, 플레이어 표시할 2차원 배열
        public static int[] player = { 13, 9 }; //플레이어 위치
        public static int[] bullet = { player[0] - 1, player[1] }; //총알 기본 위치
        public static int[] enemy = { 3, 10 }; //적 위치
        public static int[] enemyBullet = { enemy[0] + 1, enemy[1] }; //적 총알 위치
        public static int[] lifeItem = { enemy[0] - 2, enemy[1] }; //아이템 생성 위치
        public static bool isAttack;

        static IntPtr ConsoleWindowHnd = GetForegroundWindow();
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();
        [DllImport("User32.Dll")]
        private static extern bool PostMessage(IntPtr hWnd, uint msg, int wParam, int lParam);
        const int VK_RETURN = 0x0D;
        const int WM_KEYDOWN = 0x100;

        static void Main(string[] args)
        {

            Console.Clear();
            System.Console.WriteLine("Galaga");
            System.Console.WriteLine("Game Start");
           

            for (int i = 5; i > 0; i--)
            {
                System.Console.WriteLine($"{i}초 전");
                Thread.Sleep(1000);
            }
            
            theThreadTimer();
            

            Console.Clear();
            int width = 30;
            int height = 20;
            const int time = 1000 / 10;
            int lastTick = 0;
            int nowTick;

            isAttack = false;
            InitializeWindow(width, height);

            
            while (true)
            {
                nowTick = System.Environment.TickCount;

                if (nowTick - lastTick < time)
                {
                    continue;
                }
                else
                {
                    lastTick = nowTick;

                    if (Console.KeyAvailable == true)
                    {
                        var key = Console.ReadKey();
                        if (key.Key == ConsoleKey.RightArrow)
                        {
                            player[1] += 1;
                        }
                        if (key.Key == ConsoleKey.LeftArrow)
                        {
                            player[1] -= 1;
                        }
                        if (key.Key == ConsoleKey.Spacebar)
                        {
                            if (isAttack == false)
                            {
                                isAttack = true;
                            }
                        }
                    }

                    MapFrame(isAttack);
                    CheckBullet();

                    if (life == 0) // 생명점수 0이면 강제종료
                    {
                        Console.WriteLine("You DIE! GAME OVER!!!");
                        Thread.Sleep(1000);
                        Environment.Exit(0);
                    }
                }
            }

        }



        static async Task theThreadTimer()
        {
            int theTime = 0;            // 현재 타이머 시간
            int timeLimit = 5;          // 타이머 종료 시간(해당 시간이 되면 꺼짐. 3 -> 3초)
            while (theTime < timeLimit)
            {
                await Task.Delay(1000); //1초의 딜레이 (1초가 지났다는 뜻)\
                theTime++;
            }

            //해당 메세지를 보내면 ReadLine 강제 종료
            //===================================================
            PostMessage(ConsoleWindowHnd, WM_KEYDOWN, VK_RETURN, 0);
            //===================================================
        }



        //콘솔창 화면 크기 설정
        static void InitializeWindow(int width, int height)
        {
            System.Console.Title = "Galaga";
            System.Console.SetWindowSize(width, height);
            Console.Clear();
            Console.CursorVisible = false;
        }

        public static void MapFrame(bool isAttack)
        {
            Console.Clear();

            MoveEnemy();
            MoveItem();

            //적 위치, 플레이어 위치 프레임 안벗어나게 하기
            if (enemy[1] <= 0)
            {
                enemy[1] = 0;
            }
            if (enemy[1] >= column - 1)
            {
                enemy[1] = column - 1;
            }
            if (player[1] <= 0)
            {
                player[1] = 0;
            }
            if (player[1] >= column - 1)
            {
                player[1] = column - 1;
            }

            lifeItem[0] += 1;

            enemyBullet[0] += 1;  // 적 총알 발사
            
            if (isAttack == true) //플레이어 총알 발사
            {
                bullet[0] -= 1;
            }
            else
            {
                bullet[0] = player[0] - 1;
                bullet[1] = player[1];
            }


            /*플레이어, 적, 총알, 아이템 이미지*/
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    if (i == 0 && j >= 0 && j <= 19)
                    {
                        pixel[i, j] = '■';
                    }
                    else if (i == 14 && j >= 0 && j <= 19)
                    {
                        pixel[i, j] = '■';
                    }
                    else if (i == bullet[0] && j == bullet[1] && isAttack == true) // 총알 위치라면
                    {
                        pixel[i, j] = '†'; // 총알 그림 대입
                    }
                    else if (i == enemyBullet[0] && j == enemyBullet[1])
                    {
                        pixel[i, j] = '∥'; // 적 총알
                    }
                    // 적의 포지션이라면
                    else if (i == enemy[0] && j == enemy[1])
                    {
                        pixel[i, j] = '▼'; // 적 캐릭터 저장
                    }
                    else if (i == player[0] && j == player[1]) // 플레이어 라면
                    {
                        pixel[i, j] = '▲'; // 플레이어 저장
                    }
                    else if (i == lifeItem[0] && j == lifeItem[1]) //아이템
                    {
                        pixel[i, j] = 'ⓜ';
                    }
                    else // 아무것도 아니면
                    {
                        pixel[i, j] = ' '; // 공백
                    }
                }
            }


            // 점수판 생성
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            if (score == 0)
            {
                Console.WriteLine("Score " + "000");
            }
            else
            {
                Console.WriteLine("Score " + score);
            }

            if (life == 3)
            {
                Console.WriteLine("Life Point : " + "3");
            }
            else
            {
                Console.WriteLine($"Life Point : {life}");
            }

            Console.ResetColor(); // 색상 초기화
            Console.BackgroundColor = ConsoleColor.White; // 배경색 흰색

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    if (i == bullet[0] && j == bullet[1] && isAttack == true) //총알
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write(pixel[i, j]);
                        Console.ResetColor();
                        Console.BackgroundColor = ConsoleColor.White;
                    }
                    else if (i == enemyBullet[0] && j == enemyBullet[1]) //적 총알
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.Write(pixel[i, j]);
                        Console.ResetColor();
                        Console.BackgroundColor = ConsoleColor.White;
                    }
                    else if (i == player[0] && j == player[1]) // 플레이어
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.Write(pixel[i, j]);
                        Console.ResetColor();
                        Console.BackgroundColor = ConsoleColor.White;
                    }
                    else if (i == enemy[0] && j == enemy[1]) // 적
                    {
                        Console.ForegroundColor = ConsoleColor.Red; // 빨간색
                        Console.Write(pixel[i, j]);
                        Console.ResetColor();
                        Console.BackgroundColor = ConsoleColor.White;
                    }
                    else if (i == lifeItem[0] && j == lifeItem[1])
                    {
                        Console.ForegroundColor = ConsoleColor.DarkMagenta;
                        Console.Write(pixel[i, j]);
                        Console.ResetColor();
                        Console.BackgroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.Write(pixel[i, j]); // 아무것도 아니면 공백 있던거 출력
                    }
                }
                Console.WriteLine(); // 한 행 끝날때마다 개행
            }

            Console.SetCursorPosition(0, 0);
        }

        static void MoveEnemy() // 적 이동
        {
            Random rand = new Random();
            int number = rand.Next(-1, 2); // -1, 0, 1 중 난수를 생성하고 적 위치에 합산한다.
            enemy[1] += number;  // 프레임 호출될 때마다 적은 랜덤하게 좌우로 한칸씩 이동

            if (enemy[1] == 19) // 너무 오른쪽이나 왼쪽으로 붙지 않게 조정
            {
                enemy[1] -= 1;
            }
            else if (enemy[0] == 0)
            {
                enemy[1] += 1;
            }
        }

        static void MoveItem()
        {
            Random rand = new Random();
            int num = rand.Next(-1, 2);
            lifeItem[1] += num;

            if (lifeItem[1] == 19) // 너무 오른쪽이나 왼쪽으로 붙지 않게 조정
            {
                lifeItem[1] -= 1;
            }
            else if (lifeItem[0] == 0)
            {
                lifeItem[1] += 1;
            }
        }

        static void CheckBullet()
        {
            if (bullet[0] == enemy[0] && bullet[1] == enemy[1])  // 만약 총이 적에게 맞는다면
            {
                pixel[bullet[0], bullet[1]] = ' '; // 총알 사라짐
                score += 100;                           // 점수는 100점 증가
                Console.Beep();
                bullet[0] = player[0] - 1;    // 앞으로 생성 될 총알 위치 초기화
                bullet[1] = player[1];
                //pixel[enemy[0], enemy[1]] = '□'; // 적 사라짐
                isAttack = false;
            }
            else if (bullet[0] <= 1) // 만약 시야를 벗어난다면
            {
                pixel[bullet[0], bullet[1]] = ' '; // 총알 사라짐
                bullet[0] = player[0] - 1; // 초기화
                bullet[1] = player[1];
                isAttack = false;
            }
            else if (enemyBullet[0] == bullet[0] && enemyBullet[1] == bullet[1]) // 적 총알 위치와 플레이어 총알 위치가 같을 때
            {
                pixel[enemyBullet[0], enemyBullet[1]] = ' ';
                enemyBullet[0] = enemy[0] + 1; // 초기화
                enemyBullet[1] = enemy[1];

                pixel[bullet[0], bullet[1]] = ' '; // 총알 사라짐
                bullet[0] = player[0] - 1; // 초기화
                bullet[1] = player[1];
                isAttack = false;
            }

            if (enemyBullet[0] == player[0] && enemyBullet[1] == player[1]) // 적 총알 위치와 플레이어 위치가 같을 때
            {
                pixel[enemyBullet[0], enemyBullet[1]] = ' ';
                enemyBullet[0] = enemy[0] + 1;
                enemyBullet[1] = enemy[1];
                if (score > 0)
                {
                    score -= 100;   // 점수 차감
                }
                if (life > 0) // 생명 점수 차감
                {
                    life -= 1;
                }
                
            }
            else if (enemyBullet[0] >= 14) // 적 총알 시야 이탈
            {
                pixel[enemyBullet[0], enemyBullet[1]] = ' ';
                enemyBullet[0] = enemy[0] + 1;
                enemyBullet[1] = enemy[1];
            }

            if (lifeItem[0] == player[0] && lifeItem[1] == player[1]) // 플레이어가 아이템 먹었을 때
            {
                pixel[lifeItem[0], lifeItem[1]] = ' ';
                lifeItem[0] = enemy[0] - 1;
                lifeItem[1] = enemy[1];
                life += 1;

                if (life == 5)
                {
                    life = 5;
                }
            }
            else if (lifeItem[0] >= 14) // 아이템 시야 이탈
            {
                pixel[lifeItem[0], lifeItem[1]] = ' ';
                lifeItem[0] = enemy[0] - 2;
                lifeItem[1] = enemy[1];
            }
        }
    }


}