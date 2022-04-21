using System;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;

namespace Enemy
{
    class MainClass
    {
        // Very unfamiliar with what any of these do, (copy pasted from: https://www.meziantou.net/cancelling-console-read.htm )
        // BUT it's required to cancel the input once 20 seconds is up (player ran out of time)
        const int STD_INPUT_HANDLE = -10;

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool CancelIoEx(IntPtr handle, IntPtr lpOverlapped);

        public static void Main(string[] args)
        {
            // Variables
            int Player_HP = 5;
            int Player_Damage = 3;
            int Damage_Inflicted = 0;
            int Enemy_HP = 999; // just to make it not null
            int Enemy_Max_HP = 999; 
            string Enemy_Name = "";

            // Extra Coding Features, stopwatch comes from System.Diagnostics
            Random rand = new Random(); // initiate RNG
            Stopwatch stopwatch = new Stopwatch(); // initiate TIME

            // Process
            for (int i = 1; i < 7; i++) // Enemies 1 to 6
            {
                // Show current enemy
                Console.Clear();
                Console.WriteLine("Enemy " + i);
                Thread.Sleep(1000);

                // NEXT THING TO CODE ---------------------------------------------------
                // 3. Friendly NPC dialogue and Player HP heal before boss fight
                // 4. Create boss fight, the Divider

                // Select enemy
                int Enemy_select = rand.Next(1, 4);

                if (Enemy_select == 1) // Addition enemy
                {
                    Enemy_HP = 20;
                    Enemy_Max_HP = 20;
                    Enemy_Name = "Addition";
                }
                else if (Enemy_select == 2) // Subtraction enemy
                {
                    Enemy_HP = 15;
                    Enemy_Max_HP = 15;
                    Enemy_Name = "Subtraction";
                }
                else if (Enemy_select == 3) // Multiplication enemy
                {
                    Enemy_HP = 12;
                    Enemy_Max_HP = 12;
                    Enemy_Name = "Multiplication";
                }

                // Combat
                while (Player_HP > 0 && Enemy_HP > 0) // while both player and enemy are alive
                {
                    // Clear
                    Console.Clear();

                    // Game Over initiated 1
                    if (Player_HP == 0)
                    {
                        break; // escape while loop
                    }

                    // UI
                    Console.WriteLine("Your HP: " + Player_HP + "/5");
                    Console.WriteLine(Enemy_Name + " Enemy HP: " + Enemy_HP + "/" + Enemy_Max_HP);

                    // Create Problem (call the Problem Generator.cs)
                    Problem_Generator a = new Problem_Generator();
                    if (Enemy_select == 1) // Addition enemy
                    {
                        a.AddProblem();
                    }
                    else if (Enemy_select == 2) // Subtraction enemy
                    {
                        a.SubtractProblem();
                    }
                    else if (Enemy_select == 3) // Multiplication enemy
                    {
                        a.MultiplyProblem();
                    }

                    // INPUT PROCESS SECTION START --------------------------------------------------------------
                    // Start the timeout
                    var read = false;

                    // modify    V this value to change delay (in milliseconds)
                    Task.Delay(20000).ContinueWith(_ =>
                    {
                        if (!read)
                        {
                            // Timeout => cancel the console read
                            var handle = GetStdHandle(STD_INPUT_HANDLE);
                            CancelIoEx(handle, IntPtr.Zero);
                        }
                    });

                    try
                    {
                        // Start reading from the console
                        Console.Write("\nInput Answer: ");
                        stopwatch.Start(); // Time start

                        var answer = Console.ReadLine(); // input answer
                        read = true; // checks if an input was given on time

                        stopwatch.Stop(); // Time end
                        // NOTE: variable for time is "stopwatch.ElapsedMilliseconds"
                        int time = (int)stopwatch.ElapsedMilliseconds; // extra info: it's also a "long" data type, 
                                                                       // it can be turned into "int" using (int)
                        stopwatch.Reset(); // time reset to 0

                        // checks if input is a number (copy pasted from https://stackoverflow.com/questions/46246472/a-local-or-parameter-named-e-cannot-be-declared-in-this-scope)
                        int value;
                        if (int.TryParse(answer, out value))
                        {
                            // is number correct?
                            if (int.Parse(answer) == a.correct_answer())
                            {
                                if (time <= 5000) // <5 sec, critical hit!
                                {
                                    Enemy_HP -= 2;
                                    Enemy_HP -= Player_Damage;
                                    Damage_Inflicted = Player_Damage + 2;
                                    Console.WriteLine("Correct! Enemy lost " + Damage_Inflicted + " HP");
                                    Console.WriteLine("Answered in " + time / 1000 + " seconds");
                                    Console.WriteLine("Quick answer!");
                                }
                                else if (time <= 10000) // <10 sec, average hit.
                                {
                                    Enemy_HP -= 1;
                                    Enemy_HP -= Player_Damage;
                                    Damage_Inflicted = Player_Damage + 1;
                                    Console.WriteLine("Correct! Enemy lost " + Damage_Inflicted + " HP");
                                    Console.WriteLine("Answered in " + time / 1000 + " seconds");
                                    Console.WriteLine("Solved nicely.");
                                }
                                else if (time > 10000) // >10 sec, lowered hit.
                                {
                                    Enemy_HP -= Player_Damage;
                                    Damage_Inflicted = Player_Damage;
                                    Console.WriteLine("Correct! Enemy lost " + Damage_Inflicted + " HP");
                                    Console.WriteLine("Answered in " + time / 1000 + " seconds");
                                    Console.WriteLine("Took a little too long...");
                                }
                            }
                            else if (int.Parse(answer) != a.correct_answer()) // wrong answer
                            {                                                
                                // Player lose HP
                                Player_HP -= 1;
                                Console.WriteLine("Wrong answer! You lost 1 HP");                                
                            }
                        }
                        else
                        {
                            Console.WriteLine("Input is not a number! You lost 1 HP");
                            Player_HP -= 1;
                        }
                    }
                    // Handle the exception when the operation is canceled
                    // Player ran out of time, lose 1 HP and make new problem
                    catch (InvalidOperationException)
                    {
                        Player_HP -= 1;
                        Console.WriteLine("You've run out of time! You lost 1 HP");
                        stopwatch.Stop();
                        stopwatch.Reset();

                    }
                    catch (OperationCanceledException)
                    {
                        Player_HP -= 1;
                        Console.WriteLine("You've run out of time! You lost 1 HP");
                        stopwatch.Stop();
                        stopwatch.Reset();
                    }
                    // INPUT PROCESS SECTION END ------------------------------------------------------

                    Thread.Sleep(1500); // wait 1.5 seconds
                }
                // ^^^ turn into a function

                // Game Over initiated 2
                if (Player_HP == 0)
                {
                    break; // escape for loop
                }
            }

            // Game Over screen
            if (Player_HP == 0)
            {
                Console.WriteLine("GAME OVER");
                Console.WriteLine(Player_HP + "/5");
                Thread.Sleep(1500);
            }
            else
            {
                // Preparing for Boss Fight (WIP)
                Console.Clear();
                Console.WriteLine("6 enemies defeated, prepare for boss battle! (WIP)");
                Thread.Sleep(1500);
            }

        }
    }
}

// KNOWN BUGS
// 1. if you let the time run out, then put an invalid input (basically anything thats not a number), it will not process the invalid input
// --- so you can just do another input and it will process it correctly
// --- idk how to fix this, it probably has something to do with the unfamiliar codes in the INPUT PROCESS SECTION, but for now it doesnt seem like a big issue.