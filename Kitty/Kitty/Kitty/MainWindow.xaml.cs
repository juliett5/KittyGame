using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Kitty
{
    public partial class MainWindow : Window
    {
        DispatcherTimer GameTimer = new DispatcherTimer();
        double score;
        int Gravity = 5;
        bool GameOver;
        Rect CatHitBox;
        Rect StarsHitBox;

        public MainWindow()
        {
            InitializeComponent();
            GameTimer.Tick += MainEventTimer;
            GameTimer.Interval = TimeSpan.FromMilliseconds(10);
            StartGame();
        }

        private void MainEventTimer(object sender, EventArgs e)
        {
            Score.Content = "Score: " + score;
            Canvas.SetTop(Cat, Canvas.GetTop(Cat) + Gravity);

            CatHitBox = new Rect(Canvas.GetLeft(Cat), Canvas.GetTop(Cat), Cat.Width, Cat.Height);

            if (Canvas.GetTop(Cat) < -10 - Cat.Height || Canvas.GetTop(Cat) > 1080 - (Cat.Height * 2))
            {
                EndGame();
            }

            foreach (var x in Board.Children.OfType<Image>())
            {
                if ((string)x.Tag == "Stars1" || (string)x.Tag == "Stars2" || (string)x.Tag == "Stars3" || (string)x.Tag == "Stars4")
                {
                    StarsHitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);
                    if (CatHitBox.IntersectsWith(StarsHitBox))
                    {
                        EndGame();
                    }
                    Canvas.SetLeft(x, Canvas.GetLeft(x) - 5); //predkosc przesuwanie (5)
                    if (Canvas.GetLeft(x) < -100)
                    {
                        Canvas.SetLeft(x, 1500);
                        score += .5; //dwie przeszkody (góra oraz dół) dają jeden punkt
                    }
                }
                if ((string)x.Tag == "Cloud1")
                {
                    Canvas.SetLeft(x, Canvas.GetLeft(x) - 1);

                    if (Canvas.GetLeft(x) < -750)
                    {
                        Canvas.SetLeft(x, 1550);
                    }
                }
                if ((string)x.Tag == "Cloud2")
                {
                    Canvas.SetLeft(x, Canvas.GetLeft(x) - 1);

                    if (Canvas.GetLeft(x) < -750)
                    {
                        Canvas.SetLeft(x, 1550);
                    }
                }
            }
        }
        private void KeyIsDown(object sender, KeyEventArgs y)
        {
            if (y.Key == Key.Space)
            {
                Cat.RenderTransform = new RotateTransform(-20, Cat.Width / 2, Cat.Height / 2);
                Gravity = -5;
            }
            if (y.Key == Key.R && GameOver == true)
            {
                StartGame();
            }
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            Cat.RenderTransform = new RotateTransform(5, Cat.Width / 2, Cat.Height / 2);
            Gravity = 5;
        }

        public void StartGame()
        {
            Board.Focus();
            int temp = 300;
            score = 0;
            GameOver = false;
            Canvas.SetTop(Cat, 190);
            foreach (var x in Board.Children.OfType<Image>())
            {
                if ((string)x.Tag == "Stars1")
                {
                    Canvas.SetLeft(x, 500);
                }

                if ((string)x.Tag == "Stars2")
                {
                    Canvas.SetLeft(x, 900);
                }

                if ((string)x.Tag == "Stars3")
                {
                    Canvas.SetLeft(x, 1300);
                }
                if ((string)x.Tag == "Stars4")
                {
                    Canvas.SetLeft(x, 1700);
                }
                if ((string)x.Tag == "Cloud1")
                {
                    Canvas.SetLeft(x, 0);
                    temp = 800;
                }
                if ((string)x.Tag == "Cloud2")
                {
                    Canvas.SetLeft(x, 900);
                    temp = 800;
                }
            }
            GameTimer.Start();
        }
        private void EndGame()
        {
            GameTimer.Stop();
            GameOver = true;
            Score.Content += "  Przegrałeś! Wciśnij R aby rozpoacząć grę od nowa.";
        }
    }
}
