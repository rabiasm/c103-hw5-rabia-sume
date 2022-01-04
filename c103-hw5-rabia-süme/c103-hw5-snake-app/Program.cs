using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ce103_hw5_snake_dll;

namespace ce103_hw5_snake_app
{
	internal class Program
	{
		static void Main(string[] args)
		{
			Class1 snake = new Class1();
			snake.welcomeArt();

			do
			{
				switch (snake.mainMenu())
				{
					case 0:
						snake.loadGame();
						break;
					case 1:
						//displayHighScores();
						break;
					case 2:
						snake.controls();
						break;
					case 3:
						snake.exitYN();
						break;
				}
			} while (true);
		}
	}
}
