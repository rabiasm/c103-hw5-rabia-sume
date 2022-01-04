using System;
using System.Threading;

namespace ce103_hw5_snake_dll
{
    public class Class1
    {
        public const int SNAKE_ARRAY_SIZE = 310;
        public const ConsoleKey UP_ARROW = ConsoleKey.UpArrow;
        public const ConsoleKey LEFT_ARROW = ConsoleKey.LeftArrow;
        public const ConsoleKey RIGHT_ARROW = ConsoleKey.RightArrow;
        public const ConsoleKey DOWN_ARROW = ConsoleKey.DownArrow;
        public const ConsoleKey ENTER_KEY = ConsoleKey.Enter;
        public const ConsoleKey EXIT_BUTTON = ConsoleKey.Escape; // ESC
        public const ConsoleKey PAUSE_BUTTON = ConsoleKey.P; //p
        const char SNAKE_HEAD = (char)177;
        const char SNAKE_BODY = (char)178;
        const char WALL = (char)219;
        const char FOOD = (char)254;
        const char BLANK = ' ';

        public ConsoleKey waitForAnyKey()
        {
            ConsoleKey pres;

            while (!Console.KeyAvailable) ;

            pres = Console.ReadKey(false).Key;
            //pressed = tolower(pressed);
            return pres;
        }

        public int getGameSpeed()
        {
            int speeds = 0;
            Console.Clear();
            Console.SetCursorPosition(10, 5);
            Console.Write("Select The game speed between 1 and 9 and press enter\n");
            Console.SetCursorPosition(10, 6);
            int selection = Convert.ToInt32(Console.ReadLine());
            switch (selection)
            {
                case 1:
                    speeds = 90;
                    break;
                case 2:
                    speeds = 80;
                    break;
                case 3:
                    speeds = 70;
                    break;
                case 4:
                    speeds = 60;
                    break;
                case 5:
                    speeds = 50;
                    break;
                case 6:
                    speeds = 40;
                    break;
                case 7:
                    speeds = 30;
                    break;
                case 8:
                    speeds = 20;
                    break;
                case 9:
                    speeds = 10;
                    break;

            }
            return speeds;
        }

        public void pauseMenu()
        {
            int i;
            Console.SetCursorPosition(28, 23);
            Console.Write("Paused");

            waitForAnyKey();
            Console.SetCursorPosition(28, 23);
            Console.Write("            ");
            return;
        }

        //This function checks if a key has pressed, then checks if its any of the arrow keys/ p/esc key. It changes direction acording to the key pressed.
        public ConsoleKey checkKeysPressed(ConsoleKey directions)
        {
            ConsoleKey pres;

            if (Console.KeyAvailable == true) //If a key has been pressed
            {
                pres = Console.ReadKey(false).Key;
                if (directions != pres)
                {
                    if (pres == DOWN_ARROW && directions != UP_ARROW)
                    {
                        directions = pres;
                    }
                    else if (pres == UP_ARROW && directions != DOWN_ARROW)
                    {
                        directions = pres;
                    }
                    else if (pres == LEFT_ARROW && directions != RIGHT_ARROW)
                    {
                        directions = pres;
                    }
                    else if (pres == RIGHT_ARROW && directions != LEFT_ARROW)
                    {
                        directions = pres;
                    }
                    else if (pres == EXIT_BUTTON || pres == PAUSE_BUTTON)
                    {
                        pauseMenu();
                    }
                }
            }
            return (directions);
        }
        //Cycles around checking if the r s coordinates ='s the snake coordinates as one of this parts
        //One thing to note, a snake of length 4 cannot collide with itself, therefore there is no need to call this function when the snakes length is <= 4
        public bool collisionSnake(int r, int s, int[,] snakeXY, int snakeLength, int detect)
        {
            int i;
            for (i = detect; i < snakeLength; i++) //Checks if the snake collided with itself
            {
                if (r == snakeXY[0, i] && s == snakeXY[1, i])
                    return true;
            }
            return false;
        }
        //Generates food & Makes sure the food doesn't appear on top of the snake <- This sometimes causes a lag issue!!! Not too much of a problem tho
        public void generateFood(int[] foodXY, int width, int height, int[,] snakeXY, int snakeLength)
        {
            Random RandomNumbers = new Random();
            do
            {
                //RandomNumbers.Seed(time(null));
                foodXY[0] = RandomNumbers.Next() % (width - 2) + 2;
                //RandomNumbers.Seed(time(null));
                foodXY[1] = RandomNumbers.Next() % (height - 6) + 2;
            } while (collisionSnake(foodXY[0], foodXY[1], snakeXY, snakeLength, 0)); //This should prevent the "Food" from being created on top of the snake. - However the food has a chance to be created ontop of the snake, in which case the snake should eat it...

            Console.SetCursorPosition(foodXY[0], foodXY[1]);
            Console.Write(FOOD);
        }

        /*
        Moves the snake array forward, i.e. 
        This:
         x 1 2 3 4 5 6
         y 1 1 1 1 1 1
        Becomes This:
         x 1 1 2 3 4 5
         y 1 1 1 1 1 1

         Then depending on the direction (in this case west - left) it becomes:

         x 0 1 2 3 4 5
         y 1 1 1 1 1 1

         snakeXY[0][0]--; <- if direction left, take 1 away from the x coordinate
        */
        public void moveSnakeArray(int[,] snakeXY, int snakeLength, ConsoleKey directions)
        {
            int r;
            for (r = snakeLength - 1; r >= 1; r--)
            {
                snakeXY[0, r] = snakeXY[0, r - 1];
                snakeXY[1, r] = snakeXY[1, r - 1];
            }

            /*
            because we dont actually know the new snakes head x y, 
            we have to check the direction and add or take from it depending on the direction.
            */
            switch (directions)
            {
                case DOWN_ARROW:
                    snakeXY[1, 0]++;
                    break;
                case RIGHT_ARROW:
                    snakeXY[0, 0]++;
                    break;
                case UP_ARROW:
                    snakeXY[1, 0]--;
                    break;
                case LEFT_ARROW:
                    snakeXY[0, 0]--;
                    break;
            }

            return;
        }

        /**
        *
        *	  @name   Move Snake Body (move)
        *
        *	  @brief Move snake body
        *
        *	  Moving snake body
        *
        *	  @param  [in] snakeXY [\b int[,]]  snake coordinates
        *	  
        *	  @param  [in] snakeLength [\b int]  index of fibonacci number in the serie
        *	  
        *	  @param  [in] direction [\b ConsoleKey]  index of fibonacci number in the serie
        **/
        public void move(int[,] snakeXY, int snakeLength, ConsoleKey directions)
        {
            int x;
            int y;

            //Remove the tail ( HAS TO BE DONE BEFORE THE ARRAY IS MOVED!!!!! )
            x = snakeXY[0, snakeLength - 1];
            y = snakeXY[1, snakeLength - 1];

            Console.SetCursorPosition(x, y);
            Console.Write(BLANK);

            //Changes the head of the snake to a body part
            Console.SetCursorPosition(snakeXY[0, 0], snakeXY[1, 0]);
            Console.Write(SNAKE_BODY);

            moveSnakeArray(snakeXY, snakeLength, directions);

            Console.SetCursorPosition(snakeXY[0, 0], snakeXY[1, 0]);
            Console.Write(SNAKE_HEAD);

            Console.SetCursorPosition(1, 1); //Gets rid of the darn flashing underscore.

            return;
        }

        //This function checks if the snakes head his on top of the food, if it is then it'll generate some more food...
        public bool eatFood(int[,] snakeXY, int[] foodXY)
        {
            if (snakeXY[0, 0] == foodXY[0] && snakeXY[1, 0] == foodXY[1])
            {
                foodXY[0] = 0;
                foodXY[1] = 0; //This should prevent a nasty bug (loops) need to check if the bug still exists...

                return true;
            }

            return false;
        }

        public bool collisionDetection(int[,] snakeXY, int consoleWidth, int consoleHeight, int snakeLength) //Need to Clean this up a bit
        {
            bool colision = false;
            if ((snakeXY[0, 0] == 1) || (snakeXY[1, 0] == 1) || (snakeXY[0, 0] == consoleWidth) || (snakeXY[1, 0] == consoleHeight - 4)) //Checks if the snake collided wit the wall or it's self
                colision = true;
            else
                if (collisionSnake(snakeXY[0, 0], snakeXY[1, 0], snakeXY, snakeLength, 1)) //If the snake collided with the wall, theres no point in checking if it collided with itself.
                colision = true;

            return (colision);
        }

        public void refreshInfoBar(int score, int speeds)
        {
            Console.SetCursorPosition(5, 23);
            Console.Write("Score: " + score);

            Console.SetCursorPosition(5, 24);
            switch (speeds)
            {
                case 90:
                    Console.Write("Speed: 1");
                    break;
                case 80:
                    Console.Write("Speed: 2");
                    break;
                case 70:
                    Console.Write("Speed: 3");
                    break;
                case 60:
                    Console.Write("Speed: 4");
                    break;
                case 50:
                    Console.Write("Speed: 5");
                    break;
                case 40:
                    Console.Write("Speed: 6");
                    break;
                case 30:
                    Console.Write("Speed: 7");
                    break;
                case 20:
                    Console.Write("Speed: 8");
                    break;
                case 10:
                    Console.Write("Speed: 9");
                    break;
            }

            Console.SetCursorPosition(52, 23);
            Console.Write("Coder: Rabia SUME");

            Console.SetCursorPosition(52, 24);
            Console.Write("Version: 0.5");

            return;
        }

        ////**HIGHSCORE STUFF**//

        ////-> The highscores system seriously needs to be clean. There are some bugs, entering a name etc

        //public void createHighScores()
        //{
        //    FILE* file;
        //    int i;

        //    file = fopen("highscores.txt", "w+");

        //    if (file == null)
        //    {
        //        Console.Write("FAILED TO CREATE HIGHSCORES!!! EXITING!");
        //        Environment.Exit(0);
        //    }

        //    for (i = 0; i < 5; i++)
        //    {
        //        Console.Write(file, "%d", i + 1);
        //        Console.Write(file, "%s", "\t0\t\t\tEMPTY\n");
        //    }

        //    fclose(file);
        //    return;
        //}

        //public int getLowestScore()
        //{
        //    FILE* fp;
        //    char[] str = new char[128];
        //    int lowestScore = 0;
        //    int i;
        //    int intLength;

        //    if ((fp = fopen("highscores.txt", "r")) == null)
        //    {
        //        //Create the file, then try open it again.. if it fails this time exit.
        //        createHighScores(); //This should create a highscores file (If there isn't one)
        //        if ((fp = fopen("highscores.txt", "r")) == null)
        //            Environment.Exit(1);
        //    }

        //    while (!feof(fp))
        //    {
        //        gets(str, 126, fp);
        //    }
        //    object p = close(fp);

        //    i = 0;

        //    //Gets the Int length
        //    while (str[2 + i] != '\t')
        //    {
        //        i++;
        //    }

        //    intLength = i;

        //    //Gets converts the string to int
        //    for (i = 0; i < intLength; i++)
        //    {
        //        lowestScore = lowestScore + ((int)str[2 + i] - 48) * pow(10, intLength - i - 1);
        //    }

        //    return (lowestScore);
        //}

        //public void inputScore(int score) //This seriously needs to be cleaned up
        //{
        //    FILE* fp;
        //    FILE* file;
        //    char[] str = new char[20];
        //    int fScore;
        //    int i, s, y;
        //    int intLength;
        //    int[] scores = new int[5];
        //    int x;
        //    char[] highScoreName = new char[20];
        //    char[,] highScoreNames = new char[5,20];

        //    char[] name = new char[20];

        //    int entered = 0;

        //    Console.Clear(); //clear the console

        //    if ((fp = fopen("highscores.txt", "r")) == null)
        //    {
        //        //Create the file, then try open it again.. if it fails this time exit.
        //        createHighScores(); //This should create a highscores file (If there isn't one)
        //        if ((fp = fopen("highscores.txt", "r")) == null)
        //            Environment.Exit(1);
        //    }
        //    Console.SetCursorPosition(10, 5);
        //    Console.Write("Your Score made it into the top 5!!!");
        //    Console.SetCursorPosition(10, 6);
        //    Console.Write("Please enter your name: ");
        //    gets(name);

        //    x = 0;
        //    while (!feof(fp))
        //    {
        //        fgets(str, 126, fp);  //Gets a line of text

        //        i = 0;

        //        //Gets the Int length
        //        while (str[2 + i] != '\t')
        //        {
        //            i++;
        //        }

        //        s = i;
        //        intLength = i;
        //        i = 0;
        //        while (str[5 + s] != '\n')
        //        {
        //            //Console.Write("%c",str[5+s]);
        //            highScoreName[i] = str[5 + s];
        //            s++;
        //            i++;
        //        }
        //        //Console.Write("\n");

        //        fScore = 0;
        //        //Gets converts the string to int
        //        for (i = 0; i < intLength; i++)
        //        {
        //            //Console.Write("%c", str[2+i]);
        //            fScore = fScore + ((int)str[2 + i] - 48) * pow(10, intLength - i - 1);
        //        }

        //        if (score >= fScore && entered != 1)
        //        {
        //            scores[x] = score;
        //            strcpy(highScoreNames[x], name);

        //            //Console.Write("%d",x+1);
        //            //Console.Write("\t%d\t\t\t%s\n",score, name);		
        //            x++;
        //            entered = 1;
        //        }

        //        //Console.Write("%d",x+1);
        //        //Console.Write("\t%d\t\t\t%s\n",fScore, highScoreName);
        //        //strcpy(text, text+"%d\t%d\t\t\t%s\n");
        //        strcpy(highScoreNames[x], highScoreName);
        //        scores[x] = fScore;

        //        //highScoreName = "";
        //        for (y = 0; y < 20; y++)
        //        {
        //            highScoreName[y] = 0x00; //NULL
        //        }

        //        x++;
        //        if (x >= 5)
        //            break;
        //    }

        //    fclose(fp);

        //    file = fopen("highscores.txt", "w+");

        //    for (i = 0; i < 5; i++)
        //    {
        //        //Console.Write("%d\t%d\t\t\t%s\n", i+1, scores[i], highScoreNames[i]);
        //        Console.Write(file, "%d\t%d\t\t\t%s\n", i + 1, scores[i], highScoreNames[i]);
        //    }

        //    fclose(file);

        //    return;
        //}

        //public void displayHighScores() //NEED TO CHECK THIS CODE!!!
        //{
        //    FILE* fp;
        //    char[] str = new char[128];
        //    int y = 5;

        //    Console.Clear(); //clear the console

        //    if ((fp = fopen("highscores.txt", "r")) == NULL)
        //    {
        //        //Create the file, then try open it again.. if it fails this time exit.
        //        createHighScores(); //This should create a highscores file (If there isn't one)
        //        if ((fp = fopen("highscores.txt", "r")) == NULL)
        //            Environment.Exit(1);
        //    }

        //    Console.SetCursorPosition(10, y++);
        //    Console.Write("High Scores");
        //    Console.SetCursorPosition(10, y++);
        //    Console.Write("Rank\tScore\t\t\tName");
        //    while (!feof(fp))
        //    {
        //        Console.SetCursorPosition(10, y++);
        //        if (fgets(str, 126, fp))
        //            Console.Write("%s", str);
        //    }

        //    fclose(fp); //Close the file
        //    Console.SetCursorPosition(10, y++);

        //    Console.Write("Press any key to continue...");
        //    waitForAnyKey();
        //    return;
        //}

        //**END HIGHSCORE STUFF**//

        public void youWinScreen()
        {
            Console.Clear();
            int x = 6, y = 7;
            Console.SetCursorPosition(x, y++);
            Console.Write("'##:::'##::'#######::'##::::'##::::'##:::::'##:'####:'##::: ##:'####:");
            Console.SetCursorPosition(x, y++);
            Console.Write(". ##:'##::'##.... ##: ##:::: ##:::: ##:'##: ##:. ##:: ###:: ##: ####:");
            Console.SetCursorPosition(x, y++);
            Console.Write(":. ####::: ##:::: ##: ##:::: ##:::: ##: ##: ##:: ##:: ####: ##: ####:");
            Console.SetCursorPosition(x, y++);
            Console.Write("::. ##:::: ##:::: ##: ##:::: ##:::: ##: ##: ##:: ##:: ## ## ##:: ##::");
            Console.SetCursorPosition(x, y++);
            Console.Write("::: ##:::: ##:::: ##: ##:::: ##:::: ##: ##: ##:: ##:: ##. ####::..:::");
            Console.SetCursorPosition(x, y++);
            Console.Write("::: ##:::: ##:::: ##: ##:::: ##:::: ##: ##: ##:: ##:: ##:. ###:'####:");
            Console.SetCursorPosition(x, y++);
            Console.Write("::: ##::::. #######::. #######:::::. ###. ###::'####: ##::. ##: ####:");
            Console.SetCursorPosition(x, y++);
            Console.Write(":::..::::::.......::::.......:::::::...::...:::....::..::::..::....::");
            Console.SetCursorPosition(x, y++);

            waitForAnyKey();
            Console.Clear(); //clear the console
            return;
        }

        public void gameOverScreen()
        {
            int x = 17, y = 3;
            Console.Beep(2500, 275); //Beep
            Console.Clear();
            //http://www.network-science.de/ascii/ <- Ascii Art Gen

            Console.SetCursorPosition(x, y++);
            Console.Write(":'######::::::'###::::'##::::'##:'########:\n");
            Console.SetCursorPosition(x, y++);
            Console.Write("'##... ##::::'## ##::: ###::'###: ##.....::\n");
            Console.SetCursorPosition(x, y++);
            Console.Write(" ##:::..::::'##:. ##:: ####'####: ##:::::::\n");
            Console.SetCursorPosition(x, y++);
            Console.Write(" ##::'####:'##:::. ##: ## ### ##: ######:::\n");
            Console.SetCursorPosition(x, y++);
            Console.Write(" ##::: ##:: #########: ##. #: ##: ##...::::\n");
            Console.SetCursorPosition(x, y++);
            Console.Write(" ##::: ##:: ##.... ##: ##:.:: ##: ##:::::::\n");
            Console.SetCursorPosition(x, y++);
            Console.Write(". ######::: ##:::: ##: ##:::: ##: ########:\n");
            Console.SetCursorPosition(x, y++);
            Console.Write(":......::::..:::::..::..:::::..::........::\n");
            Console.SetCursorPosition(x, y++);
            Console.Write(":'#######::'##::::'##:'########:'########::'####:\n");
            Console.SetCursorPosition(x, y++);
            Console.Write("'##.... ##: ##:::: ##: ##.....:: ##.... ##: ####:\n");
            Console.SetCursorPosition(x, y++);
            Console.Write(" ##:::: ##: ##:::: ##: ##::::::: ##:::: ##: ####:\n");
            Console.SetCursorPosition(x, y++);
            Console.Write(" ##:::: ##: ##:::: ##: ######::: ########::: ##::\n");
            Console.SetCursorPosition(x, y++);
            Console.Write(" ##:::: ##:. ##:: ##:: ##...:::: ##.. ##::::..:::\n");
            Console.SetCursorPosition(x, y++);
            Console.Write(" ##:::: ##::. ## ##::: ##::::::: ##::. ##::'####:\n");
            Console.SetCursorPosition(x, y++);
            Console.Write(". #######::::. ###:::: ########: ##:::. ##: ####:\n");
            Console.SetCursorPosition(x, y++);
            Console.Write(":.......::::::...:::::........::..:::::..::....::\n");

            waitForAnyKey();
            Console.Clear(); //clear the console
            return;
        }

        //Messy, need to clean this function up
        public void startGame(int[,] snakeXY, int[] foodXY, int consoleWidth, int consoleHeight, int snakeLength, ConsoleKey directions, int scores, int speeds)
        {
            bool gameOver = false;
            ConsoleKey oldDirection = ConsoleKey.NoName;
            bool canChangeDirection = true;
            int gameOver2 = 1;
            do
            {
                if (canChangeDirection)
                {
                    oldDirection = directions;
                    directions = checkKeysPressed(directions);
                }

                if (oldDirection != directions)//Temp fix to prevent the snake from colliding with itself
                    canChangeDirection = false;

                if (true) //haha, it moves according to how fast the computer running it is...
                {
                    //Console.SetCursorPosition(1,1);
                    //Console.Write("%d - %d",clock() , endWait);
                    move(snakeXY, snakeLength, directions);
                    canChangeDirection = true;


                    if (eatFood(snakeXY, foodXY))
                    {
                        generateFood(foodXY, consoleWidth, consoleHeight, snakeXY, snakeLength); //Generate More Food
                        snakeLength++;
                        switch (speeds)
                        {
                            case 90:
                                scores += 5;
                                break;
                            case 80:
                                scores += 7;
                                break;
                            case 70:
                                scores += 9;
                                break;
                            case 60:
                                scores += 12;
                                break;
                            case 50:
                                scores += 15;
                                break;
                            case 40:
                                scores += 20;
                                break;
                            case 30:
                                scores += 23;
                                break;
                            case 20:
                                scores += 25;
                                break;
                            case 10:
                                scores += 30;
                                break;
                        }

                        refreshInfoBar(scores, speeds);
                    }
                    Thread.Sleep(speeds);
                }

                gameOver = collisionDetection(snakeXY, consoleWidth, consoleHeight, snakeLength);

                if (snakeLength >= SNAKE_ARRAY_SIZE - 5) //Just to make sure it doesn't get longer then the array size & crash
                {
                    gameOver2 = 2;//You Win! <- doesn't seem to work - NEED TO FIX/TEST THIS
                    scores += 1500; //When you win you get an extra 1500 points!!!
                }

            } while (!gameOver);

            switch (gameOver2)
            {
                case 1:
                    gameOverScreen();

                    break;
                case 2:
                    youWinScreen();
                    break;
            }

            //if (score >= getLowestScore() && score != 0)
            //{
            //    inputScore(score);
            //    displayHighScores();
            //}

            return;
        }

        public void loadEnviroment(int consoleWidth, int consoleHeight)//This can be done in a better way... FIX ME!!!! Also i think it doesn't work properly in ubuntu <- Fixed
        {
            int i;
            int r = 1, y = 1;
            int rectangleHeight = consoleHeight - 4;
            Console.Clear(); //clear the console

            Console.SetCursorPosition(r, y); //Top left corner

            for (; y < rectangleHeight; y++)
            {
                Console.SetCursorPosition(r, y); //Left Wall 
                Console.Write("|", WALL);

                Console.SetCursorPosition(consoleWidth, y); //Right Wall
                Console.Write("|", WALL);
            }

            y = 1;
            for (; r < consoleWidth + 1; r++)
            {
                Console.SetCursorPosition(r, y); //Left Wall 
                Console.Write("-", WALL);

                Console.SetCursorPosition(r, rectangleHeight); //Right Wall
                Console.Write("-", WALL);
            }

            /*
                for (i = 0; i < 80; i++)
                {
                    Console.Write("%c",WALL);
                }

                for (i = 0; i < 17; i++)
                {
                    Console.Write("%c\n",WALL);
                }

                for (i = 0; i < 21; i++)
                {
                    Console.Write("%c\n",WALL);
                    Console.SetCursorPosition(80,i);
                }

                for (i = 0; i < 81; i++)
                {
                    Console.Write("%c",WALL);
                }	
            */
            return;
        }

        public void loadSnake(int[,] snakeXY, int snakeLength)
        {
            int i;
            /*
            First off, The snake doesn't actually have enough XY coordinates (only 1 - the starting location), thus we use
            these XY coordinates to "create" the other coordinates. For this we can actually use the function used to move the snake.
            This helps create a "whole" snake instead of one "dot", when someone starts a game.
            */
            //moveSnakeArray(snakeXY, snakeLength); //One thing to note ATM, the snake starts of one coordinate to whatever direction it's pointing...

            //This should print out a snake :P
            for (i = 0; i < snakeLength; i++)
            {
                Console.SetCursorPosition(snakeXY[0, i], snakeXY[1, i]);
                Console.Write(SNAKE_BODY); //Meh, at some point I should make it so the snake starts off with a head...
            }

            return;
        }

        /* NOTE, This function will only work if the snakes starting direction is left!!!! 
        Well it will work, but the results wont be the ones expected.. I need to fix this at some point.. */
        public void prepairSnakeArray(int[,] snakeXY, int snakeLength)
        {
            int i, x;
            int snakeX = snakeXY[0, 0];
            int snakeY = snakeXY[1, 0];

            // this is used in the function move.. should maybe create a function for it...
            /*switch(direction)
            {
                case DOWN_ARROW:
                    snakeXY[1][0]++;
                    break;
                case RIGHT_ARROW:
                    snakeXY[0][0]++;
                    break;
                case UP_ARROW:
                    snakeXY[1][0]--;
                    break;
                case LEFT_ARROW:
                    snakeXY[0][0]--;
                    break;			
            }
            */


            for (i = 1; i <= snakeLength; i++)
            {
                snakeXY[0, i] = snakeX + i;
                snakeXY[1, i] = snakeY;
            }

            return;
        }

        //This function loads the enviroment, snake, etc
        public void loadGame()
        {
            int[,] snakeXY = new int[2, SNAKE_ARRAY_SIZE]; //Two Dimentional Array, the first array is for the X coordinates and the second array for the Y coordinates

            int snakeLength = 4; //Starting Length

            ConsoleKey directions = ConsoleKey.LeftArrow; //DO NOT CHANGE THIS TO RIGHT ARROW, THE GAME WILL INSTANTLY BE OVER IF YOU DO!!! <- Unless the prepairSnakeArray function is changed to take into account the direction....

            int[] foodXY = { 5, 5 };// Stores the location of the food

            int scores = 0;
            //int level = 1;

            //Window Width * Height - at some point find a way to get the actual dimensions of the console... <- Also somethings that display dont take this dimentions into account.. need to fix this...
            int consoleWidth = 80;
            int consoleHeight = 25;

            int speeds = getGameSpeed();

            //The starting location of the snake
            snakeXY[0, 0] = 40;
            snakeXY[1, 0] = 10;

            loadEnviroment(consoleWidth, consoleHeight); //borders
            prepairSnakeArray(snakeXY, snakeLength);
            loadSnake(snakeXY, snakeLength);
            generateFood(foodXY, consoleWidth, consoleHeight, snakeXY, snakeLength);
            refreshInfoBar(scores, speeds); //Bottom info bar. Score, Level etc
            startGame(snakeXY, foodXY, consoleWidth, consoleHeight, snakeLength, directions, scores, speeds);

            return;
        }

        //**MENU STUFF**//

        public int menuSelector(int x, int y, int yStart)
        {
            char key;
            int i = 0;
            x = x - 2;
            Console.SetCursorPosition(x, yStart);

            Console.Write(">");

            Console.SetCursorPosition(1, 1);


            do
            {
                key = (char)waitForAnyKey();
                //Console.Write("%c %d", key, (int)key);
                if (key == (char)UP_ARROW)
                {
                    Console.SetCursorPosition(x, yStart + i);
                    Console.Write(" ");

                    if (yStart >= yStart + i)
                        i = y - yStart - 2;
                    else
                        i--;
                    Console.SetCursorPosition(x, yStart + i);
                    Console.Write(">");
                }
                else
                    if (key == (char)DOWN_ARROW)
                {
                    Console.SetCursorPosition(x, yStart + i);
                    Console.Write(" ");

                    if (i + 2 >= y - yStart)
                        i = 0;
                    else
                        i++;
                    Console.SetCursorPosition(x, yStart + i);
                    Console.Write(">");
                }
                //Console.SetCursorPosition(1,1);
                //Console.Write("%d", key);
            } while (key != (char)ENTER_KEY); //While doesn't equal enter... (13 ASCII code for enter) - note ubuntu is 10
            return (i);
        }

        public void welcomeArt()
        {
            Console.Clear(); //clear the console
                             //Ascii art reference: http://www.chris.com/ascii/index.php?art=animals/reptiles/snakes
            Console.Write("\n");
            Console.Write("\t\t    _         _ 			\n");
            Console.Write("\t\t   /         \\       /         \\ 			\n");
            Console.Write("\t\t  /  /~\\  \\     /  /~\\  \\ 			\n");
            Console.Write("\t\t  |  |     |  |     |  |     |  | 			\n");
            Console.Write("\t\t  |  |     |  |     |  |     |  | 			\n");
            Console.Write("\t\t  |  |     |  |     |  |     |  |         /	\n");
            Console.Write("\t\t  |  |     |  |     |  |     |  |       //	\n");
            Console.Write("\t\t (o  o)    \\  \\_/  /     \\  \\_/ / 	\n");
            Console.Write("\t\t  \\__/      \\         /       \\        / 	\n");
            Console.Write("\t\t    |        ~         ~~ 		\n");
            Console.Write("\t\t    ^											\n");
            Console.Write("\t		Welcome To The Snake Game!			\n");
            Console.Write("\t			    Press Any Key To Continue...	\n");
            Console.Write("\n");

            waitForAnyKey();
            return;
        }

        public void controls()
        {
            int x = 10, y = 5;
            Console.Clear(); //clear the console
            Console.SetCursorPosition(x, y++);
            Console.Write("\t\t-- Controls --");
            Console.SetCursorPosition(x++, y++);
            Console.Write(" Use the following arrow keys to direct the snake to the food: ");
            Console.SetCursorPosition(x, y++);
            Console.Write("- Right Arrow");
            Console.SetCursorPosition(x, y++);
            Console.Write("- Left Arrow");
            Console.SetCursorPosition(x, y++);
            Console.Write("- Top Arrow");
            Console.SetCursorPosition(x, y++);
            Console.Write("- Bottom Arrow");
            Console.SetCursorPosition(x, y++);
            Console.SetCursorPosition(x, y++);
            Console.Write("- P & Esc pauses the game.");
            Console.SetCursorPosition(x, y++);
            Console.SetCursorPosition(x, y++);
            Console.Write("Press any key to continue...");
            Console.ReadKey();
            return;
        }

        public void exitYN()
        {
            char pres;
            Console.SetCursorPosition(9, 8);
            Console.Write("Are you sure you want to exit(Y/N)\n");

            do
            {
                pres = (char)waitForAnyKey();
                pres = char.ToLower(pres);
            } while (!(pres == 'y' || pres == 'n'));

            if (pres == 'y')
            {
                Console.Clear(); //clear the console
                Environment.Exit(0);
            }
            return;
        }

        public int mainMenu()
        {
            int x = 10, y = 5;
            int yStart = y;

            int select;

            Console.Clear(); //clear the console
                             //Might be better with arrays of strings???
            Console.SetCursorPosition(x, y++);
            Console.Write("New Game\n");
            Console.SetCursorPosition(x, y++);
            Console.Write("High Scores\n");
            Console.SetCursorPosition(x, y++);
            Console.Write("Controls\n");
            Console.SetCursorPosition(x, y++);
            Console.Write("Exit\n");
            Console.SetCursorPosition(x, y++);

            select = menuSelector(x, y, yStart);

            return (select);
        }

        //**END MENU STUFF**//
        public int main() //Need to fix this up
        {

            welcomeArt();

            do
            {
                switch (mainMenu())
                {
                    case 0:
                        loadGame();
                        break;
                    case 1:
                        //displayHighScores();
                        break;
                    case 2:
                        controls();
                        break;
                    case 3:
                        exitYN();
                        break;
                }
            } while (true);    //
        }

    }
}
