using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Threading;

namespace RekenApplicatie
{
    public partial class MainWindow : Window
    {
        private readonly List<string> operators = new List<string> { "+", "-", "*", "/" };
        private readonly Random random = new Random();
        private string playerName;
        private int currentLevel;
        private int score;
        private int questionsAnswered;

        public MainWindow()
        {
            InitializeComponent();
            InitializeLevels();
            ResetGame();
        }

        private void InitializeLevels()
        {
            cbxLevels.Items.Add("Niveau 1");
            cbxLevels.Items.Add("Niveau 2");
            cbxLevels.Items.Add("Niveau 3F");
            cbxLevels.SelectedIndex = 0;
        }

        private void ResetGame()
        {
            playerName = txtPlayerName.Text;
            score = 0;
            questionsAnswered = 0;
            currentLevel = cbxLevels.SelectedIndex + 1;
            lblScore.Content = "Score: 0";
            GenerateQuestion();
        }

        private void GenerateQuestion()
        {
            int operand1, operand2;
            string op = operators[random.Next(0, currentLevel)];

            if (op == "/")
            {
                operand2 = random.Next(1, 11);
                operand1 = operand2 * random.Next(1, 11);
            }
            else
            {
                operand1 = random.Next(1, currentLevel * 10 + 1);
                operand2 = random.Next(1, currentLevel * 10 + 1);
            }

            lblQuestion.Content = $"{operand1} {op} {operand2} = ?";
        }

        private void CheckAnswer()
        {
            if (int.TryParse(txtAnswer.Text, out int userAnswer))
            {
                string[] questionParts = lblQuestion.Content.ToString().Split(' ');
                int correctAnswer = CalculateAnswer(int.Parse(questionParts[0]), int.Parse(questionParts[2]), questionParts[1]);

                if (userAnswer == correctAnswer)
                {
                    lblFeedback.Content = "Goed!";
                    score++;
                    btnCheck.IsEnabled = false; // Schakel de knop uit tijdens het wachten
                    DispatcherTimer timer = new DispatcherTimer();
                    timer.Interval = TimeSpan.FromSeconds(1.5);
                    timer.Tick += (sender, e) =>
                    {
                        timer.Stop();
                        lblFeedback.Content = ""; // Wissen van de feedback na 1,5 seconde
                        questionsAnswered++;
                        lblScore.Content = $"Score: {score}";
                        btnCheck.IsEnabled = true; // Schakel de knop weer in na het wachten

                        if (questionsAnswered < 10)
                        {
                            GenerateQuestion();
                        }
                        else
                        {
                            MessageBox.Show($"Einde van het spel. Je totale score is {score}.");
                            ResetGame();
                            startPanel.Visibility = Visibility.Visible;
                            questionPanel.Visibility = Visibility.Collapsed;
                        }
                    };
                    timer.Start();
                }
                else
                {
                    lblFeedback.Content = $"Fout! Het antwoord was {correctAnswer}";
                    btnCheck.IsEnabled = false; // Schakel de knop uit tijdens het wachten
                    DispatcherTimer timer = new DispatcherTimer();
                    timer.Interval = TimeSpan.FromSeconds(1.5);
                    timer.Tick += (sender, e) =>
                    {
                        timer.Stop();
                        lblFeedback.Content = ""; // Wissen van de feedback na 1,5 seconde
                        questionsAnswered++;
                        lblScore.Content = $"Score: {score}";
                        btnCheck.IsEnabled = true; // Schakel de knop weer in na het wachten

                        if (questionsAnswered < 10)
                        {
                            GenerateQuestion();
                        }
                        else
                        {
                            MessageBox.Show($"Einde van het spel. Je totale score is {score}.");
                            ResetGame();
                            startPanel.Visibility = Visibility.Visible;
                            questionPanel.Visibility = Visibility.Collapsed;
                        }
                    };
                    timer.Start();
                }
            }
            else
            {
                lblFeedback.Content = "Voer een geldig antwoord in.";
            }
        }

        private int CalculateAnswer(int operand1, int operand2, string op)
        {
            switch (op)
            {
                case "+":
                    return operand1 + operand2;
                case "-":
                    return operand1 - operand2;
                case "*":
                    return operand1 * operand2;
                case "/":
                    return operand1 / operand2;
                default:
                    throw new ArgumentException("Ongeldige operator");
            }
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            startPanel.Visibility = Visibility.Collapsed;
            questionPanel.Visibility = Visibility.Visible;
            ResetGame();
        }

        private void btnCheck_Click(object sender, RoutedEventArgs e)
        {
            CheckAnswer();
        }

        private void cbxLevels_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            currentLevel = cbxLevels.SelectedIndex + 1;
            ResetGame();
        }
    }
}
