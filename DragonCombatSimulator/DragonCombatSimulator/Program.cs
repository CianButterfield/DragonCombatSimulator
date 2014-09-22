using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonCombatSimulator
{
    class Program
    {
        //declare random number generator for use durring attacks
        private static Random randNum = new Random();

        //declare variables for current and max player and dragon health
        private static int userHPMax = 100;
        private static int dragonHPMax = 200;
        private static int userHP = 100;
        private static int dragonHP = 200;

        //declare a variable for whether the game is still playing
        private static bool playing = true;

        static void Main(string[] args)
        {
            //change the console window title to "Dragon Slayer"
            Console.Title = "Dragon Slayer";

            //greet the user
            Greet();

            //start the game
            Play();

            //keep the console window open
            Console.ReadKey();
        }

        static void Greet()
        {
            //greet the user, give instructions
            Console.WriteLine(@"Welcome to Dragon Slayer.

As the greatest hero in the land, you have been sent to slay the mighty dragon.

You have three options per turn: Sword, Magic, and Heal.
    Your sword deals the most damage but sometimes misses.
    Your magic always hits but does less damage.
    You may use heal to recover some health.

(Press any key to continue)");

            //wait for user to hit a key
            Console.ReadKey();

            //clear the screen for the begining of the game
            Console.Clear();

        }

        static void Play()
        {
            /*
             * variables
             * max health
             * current health
             * max magic
             * current magic
             * armour
             * level 
             * experiance
             * crit chance
             * base magic damage
             * base physical damage
             * base healing amount
             * stat buffs
             * action points max
             * action points current
             * dragon on fire
             * you on fire
             * is this action a critical
             * xp needed
             * xp total
             * xp to next level
             * mp recharge
             * 
            */

            //play until the game is over
            while (playing)
            {
                //check if user is still alive
                if (userHP > 0)
                {
                    //user turn
                    UserInput();
                    ScreenRefresh();
                }
                
                //check if dragon is still alive
                if (dragonHP > 0)
                {
                    //dragon turn
                    DragonAI();
                    ScreenRefresh();
                }
            }

            


        }

        static void LevelUp()
        {
            //checks if the user has leveled up in any skill and adjusts the relavant skills
        }

        static void DragonAI()
        {
            //plays the game for the dragon, attacking with the best attack and healing when possible

            //tell the user that it is the dragons turn
            Console.WriteLine("\nThe dragon goes for an attack.");

            //check if the dragon hits
            int hitChance = randNum.Next(1, 101);
            if (hitChance > 20)
            {
                //the dragon hits
                //calculate dragons damage
                int damage = randNum.Next(5, 16);
                userHP -= damage;
                Console.WriteLine("The dragon hits you for " + damage + " damage.");
                EndGame();
            }
            else
            {
                //the dragon misses
                Console.WriteLine("The dragon misses you, this time.");
            }

        }

        static void UserInput()
        {
            //takes the user's input and processes it


            //ask the user what they would like to do
            Console.WriteLine("\n\nWhat would you like to use? (Please type 1 2 or 3)\n1: Sword   2: Magic   3: Heal");

            //take the users input and store it
            string input = "";
            input = Console.ReadLine().ToLower();

            //clear the console screen after user input
            Console.Clear();

            //determine what to do
            if (input == "1" || input == "sword" || input == "s" || input == "one")
            {
                //use sword
                //check if the user hits
                int hitChance = randNum.Next(1, 101);
                if (hitChance > 30)
                {
                    //user hits
                    int damage = randNum.Next(20, 36);
                    dragonHP -= damage;
                    Console.WriteLine("You hit the dragon with your sword for " + damage + " damage.\n");
                    EndGame();
                }
                else
                {
                    //user misses
                    Console.WriteLine("You miss the dragon.\n");
                }
            }
            else if (input == "2" || input == "magic" || input == "m" || input == "two")
            {
                //use magic
                int damage = randNum.Next(10, 16);
                dragonHP -= damage;
                Console.WriteLine("You fling your death magic at the dragon and deal " + damage + " damage to it.\n");
                EndGame();
            }
            else if (input == "3" || input == "heal" || input == "h" || input == "three")
            {
                //use heal
                int healNum = randNum.Next(10, 21);
                userHP += healNum;
                Console.WriteLine("You use your healing magic to restore yourself, you now have " + userHP + " health.\n");
            }
            else
            {
                //invalid input
                Console.WriteLine("You do not know how to do that, please select something else.\n");
                UserInput();
            }
        }

        static void Win()
        {
            //end game where the user won, asks if they would like to slay another dragon
            Console.WriteLine("You have slain the dragon, good job.");

             //give the user a moment to read
            System.Threading.Thread.Sleep(3000);
            //add high score
            AddHighScore(userHP);
            //display highscores
            DisplayHighScores();
        }

        static void ItemPick()
        {
            //lets the user pick an item, they get one to begin with and one for each dragon they have killed
            //do not let the user take the same one twice
        }

        static void Lost()
        {
            //end game where the user died
            Console.WriteLine("\nFatality!!!\nYou have been killed by the dragon.");
        }

        static void EndGame()
        {
            //checks if the game is over
            if (dragonHP <= 0)
            {
                //the dragon is dead
                playing = false;
                Win();
            }
            else if (userHP <= 0)
            {
                //the user is dead
                playing = false;
                Lost();
            }
        }

        static void ScreenRefresh()
        {
            //updates the screen each turn
            Console.Title = "Dragon Slayer   Dragon Health: " + dragonHP + "/" + dragonHPMax + "   Player Health: " + userHP + "/" + userHPMax;
        }

        static void AddHighScore(int playerScore)
        {
            Console.Clear();

            //get player name for high score
            Console.Write("Your name: "); string playerName = Console.ReadLine();

            //create a gateway to the database
            CianEntities db = new CianEntities();

            //create a new high score object
            // fill it with our user's data
            HighScore newHighScore = new HighScore();
            newHighScore.DateCreated = DateTime.Now;
            newHighScore.Game = "Dragon Slayer";
            newHighScore.Name = playerName;
            newHighScore.Score = playerScore;

            //add it to the database
            db.HighScores.Add(newHighScore);

            //save our changes
            db.SaveChanges();
        }

        static void DisplayHighScores()
        {
            //clear the console
            Console.Clear();
            Console.Title = "ΦDragon Slayer ScoresΦ";
            Console.WriteLine("Dragon Slayer Scores");
            Console.WriteLine("≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡≡");

            //create a new connection to the database
            CianEntities db = new CianEntities();
            //get the high score list
            List<HighScore> highScoreList = db.HighScores.Where(x => x.Game == "Dragon Slayer").OrderByDescending(x => x.Score).Take(10).ToList();

            foreach (HighScore highScore in highScoreList)
            {
                Console.WriteLine("{0}. {1} - {2}", highScoreList.IndexOf(highScore) + 1, highScore.Name, highScore.Score);
            }
            Console.ReadKey();
        }
    }
}
