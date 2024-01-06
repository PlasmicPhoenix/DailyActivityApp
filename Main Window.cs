using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Text;
using System.Runtime.Remoting.Channels;

namespace DailyActivityApp
{
    public partial class Main : Form
    {
        Random random = new Random();
        string[] levelOne;
        string[] levelTwo;
        string[] levelThree;

        string[] instructionsOne;
        string[] instructionsTwo;
        string[] instructionsThree;

        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            AddData();

            ToastNotificationManagerCompat.OnActivated += HandleToastActivation;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            ExerciseTick();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void snoozeBox_CheckedChanged(object sender, EventArgs e)
        {
            if (snoozeBox.Checked)
            {
                MessageBox.Show("You better not have this snoozed for too long!", "Are you sure?");
                return;
            }

            ExerciseTick();
        }

        private void ButtonOne_Click(object sender, EventArgs e)
        {
            ForwardExercise(1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ForwardExercise(2);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ForwardExercise(3);
        }

        private void HandleToastActivation(ToastNotificationActivatedEventArgsCompat args)
        {
            ToastArguments toastArgs = ToastArguments.Parse(args.Argument);
            string stringLevel;
            string stringExercise;
            toastArgs.TryGetValue("Level", out stringLevel);
            toastArgs.TryGetValue("Exercise", out stringExercise);

            int level = int.Parse(stringLevel);
            int exercise = int.Parse(stringExercise);

            switch (level)
            {
                case 1:
                    Process.Start(instructionsOne[exercise]);
                    break;
                case 2:
                    Process.Start(instructionsTwo[exercise]);
                    break;
                case 3:
                    Process.Start(instructionsThree[exercise]);
                    break;
            }
        }

        //Helpers
        private void AddData()
        {
            StreamReader reader = new StreamReader(@"..\..\Exercise Data\LevelOne.txt");
            levelOne = reader.ReadToEnd().Split('\n');

            reader = new StreamReader(@"..\..\Exercise Data\OneURL.txt");
            instructionsOne = reader.ReadToEnd().Split('\n');
            for (int i = 0; i < instructionsOne.Length; i++)
            {
                instructionsOne[i] = instructionsOne[i].Replace("\n", "");
            }

            reader = new StreamReader(@"..\..\Exercise Data\LevelTwo.txt");
            levelTwo = reader.ReadToEnd().Split('\n');

            reader = new StreamReader(@"..\..\Exercise Data\TwoURL.txt");
            instructionsTwo = reader.ReadToEnd().Split('\n');
            for (int i = 0; i < instructionsTwo.Length; i++)
            {
                instructionsTwo[i] = instructionsTwo[i].Replace("\n", "");
            }

            reader = new StreamReader(@"..\..\Exercise Data\LevelThree.txt");
            levelThree = reader.ReadToEnd().Split('\n');

            reader = new StreamReader(@"..\..\Exercise Data\ThreeURL.txt");
            instructionsThree = reader.ReadToEnd().Split('\n');
            for (int i = 0; i < instructionsThree.Length; i++)
            {
                instructionsThree[i] = instructionsThree[i].Replace("\n", "");
            }
        }

        private void ExerciseTick()
        {
            timer.Interval = random.Next(1800000, 3600000);

            if (snoozeBox.Checked)
            {
                return;
            }

            int randomResult;

            if (DateTime.Now.TimeOfDay.TotalHours > 18 || DateTime.Now.TimeOfDay.TotalHours < 8)
            {
                randomResult = random.Next(1, 9);
            }
            else
            {
                randomResult = random.Next(1, 11);
            }

            switch (randomResult)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                    ForwardExercise(1);
                    break;
                case 6:
                case 7:
                case 8:
                    ForwardExercise(2);
                    break;
                case 9:
                case 10:
                    ForwardExercise(3);
                    break;
            }
        }

        private void ForwardExercise(int level)
        {
            int randomResult;

            switch (level)
            {
                case 1:
                    randomResult = random.Next(0, levelOne.Length);
                    new ToastContentBuilder()
                        .AddText("Time for a level 1 exercise!")
                        .AddText("Do " + (random.Next(1, 11) * 5) + " " + levelOne[randomResult])
                        .AddButton(new ToastButton().SetContent("Show Activity").AddArgument("Level", 1).AddArgument("Exercise", randomResult))
                        .Show();
                    break;

                case 2:
                    randomResult = random.Next(0, levelTwo.Length);
                    new ToastContentBuilder()
                        .AddText("Time for a level 2 exercise!")
                        .AddText("Do " + (random.Next(1, 5) * 5) + " " + levelTwo[randomResult])
                        .AddButton(new ToastButton().SetContent("Show Activity").AddArgument("Level", 2).AddArgument("Exercise", randomResult))
                        .Show();
                    break;

                case 3:
                    randomResult = random.Next(0, levelThree.Length);
                    new ToastContentBuilder()
                        .AddText("Time for a level 3 exercise!")
                        .AddText(levelThree[randomResult])
                        .AddButton(new ToastButton().SetContent("Show Activity").AddArgument("Level", 3).AddArgument("Exercise", randomResult))
                        .Show();
                    break;
            }
        }
    }
}
