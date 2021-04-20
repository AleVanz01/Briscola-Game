using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
//using System.Windows.Forms;

namespace Briscola
{
    /// <summary>
    /// Logica di interazione per example.xaml
    /// </summary>
    public partial class Test : Window
    {
        public Test()
        {
            InitializeComponent();

            //img1.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + "\\Resources\\CarteTrevisane\\AssoBastoni.png"));
            /*img1.Fill = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + "\\Resources\\CarteTrevisane\\AssoBastoni.png")));
            img1.RenderTransform = new RotateTransform();*/

            Button button = new Button
            {
                Height = 28,
                Width = 58,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Content = "Button"
            };
            canvas1.Children.Add(button);

            button.Name = "button";


            Rectangle rectangle = new Rectangle
            {
                Width = 100,
                Height = 100,
                Fill = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + "\\Resources\\CarteTrevisane\\AssoBastoni.png"))),
                Stroke = new SolidColorBrush(Colors.Black)
            };
            Canvas.SetLeft(rectangle, 100);
            Canvas.SetTop(rectangle, 100);
            canvas1.Children.Add(rectangle);

            rectangle.Name = "rectangle";
            this.RegisterName(rectangle.Name, rectangle);

            TranslateTransform tt = new TranslateTransform();
            RotateTransform st = new RotateTransform();


            TransformGroup tg = new TransformGroup();
            tg.Children.Add(tt);
            tg.Children.Add(st);

            rectangle.RenderTransform = tg;


            Duration duration = new Duration(TimeSpan.FromMilliseconds(10));
            DoubleAnimationUsingKeyFrames myDoubleAnim = new DoubleAnimationUsingKeyFrames();
            DoubleAnimationUsingKeyFrames myDoubleAnim2 = new DoubleAnimationUsingKeyFrames();
            DoubleAnimationUsingKeyFrames myDoubleAnim3 = new DoubleAnimationUsingKeyFrames();
            LinearDoubleKeyFrame myDoubleKey = new LinearDoubleKeyFrame();
            LinearDoubleKeyFrame myDoubleKey2 = new LinearDoubleKeyFrame();



            Storyboard s = new Storyboard();

            Storyboard.SetTargetName(myDoubleAnim, rectangle.Name);
            Storyboard.SetTargetProperty(myDoubleAnim, new PropertyPath("RenderTransform.Children[0].X"));
            Storyboard.SetTargetName(myDoubleAnim2, rectangle.Name);
            Storyboard.SetTargetProperty(myDoubleAnim2, new PropertyPath("RenderTransform.Children[0].Y"));
            Storyboard.SetTargetName(myDoubleAnim3, rectangle.Name);
            Storyboard.SetTargetProperty(myDoubleAnim3, new PropertyPath("RenderTransform.Children[1].Angle"));

            myDoubleKey.KeyTime = KeyTime.FromPercent(1);
            myDoubleKey.Value = 200;
            myDoubleKey2.KeyTime = KeyTime.FromPercent(1);
            myDoubleKey2.Value = 0;

            myDoubleAnim.KeyFrames.Add(myDoubleKey);
            s.Children.Add(myDoubleAnim);

            myDoubleAnim2.KeyFrames.Add(myDoubleKey);
            s.Children.Add(myDoubleAnim2);

            myDoubleAnim3.KeyFrames.Add(myDoubleKey2);
            s.Children.Add(myDoubleAnim3);

            button.Click += Button_Click;

            void Button_Click(object sender, RoutedEventArgs e) => s.Begin(rectangle);
            /*img2.Stretch = Stretch.Uniform;
            img2.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + "\\Resources\\CarteTrevisane\\AssoBastoni.png"));
                img2.RenderTransform = new TranslateTransform(50,25);*/

            /*Storyboard storyboard = new Storyboard();
            storyboard.Duration = new Duration(TimeSpan.FromSeconds(15.0));
            DoubleAnimation rotateAnimation = new DoubleAnimation()
            {
                
                From = 0,
                To = 360,
                Duration = storyboard.Duration
            };
            DoubleAnimation translateAnimation = new DoubleAnimation()
            {
                To = 20,
                Duration = new Duration(TimeSpan.FromSeconds(5.0))
            };

            //Storyboard.SetTarget(rotateAnimation, img1);
            //Storyboard.SetTargetProperty(rotateAnimation, new PropertyPath("(UIElement.RenderTransform).(RotateTransform.Angle)"));
            Storyboard.SetTarget(translateAnimation, img1);
            TransformGroup group = new TransformGroup();
            group.Children.Add(new RotateTransform(90));
            group.Children.Add(new TranslateTransform(50,50));
            Storyboard.SetTarget(group, translateAnimation);

            Storyboard.SetTargetProperty(translateAnimation, new PropertyPath(((RotateTransform)group.Children[0])));
            //Storyboard.SetTarget(translateAnimation, img2);
            //Storyboard.SetTargetProperty(translateAnimation, new PropertyPath(TranslateTransform.YProperty));

            //storyboard.Children.Add(rotateAnimation);
            storyboard.Children.Add(translateAnimation);
            
            Resources.Add("Storyboard", storyboard);
            */
            /* button.Content = "Begin";
             button.Click += button_Click;*/
        }


        /* public example()
{
    InitializeComponent();
    img1.Fill = new ImageBrush(new BitmapImage(new Uri(Environment.CurrentDirectory + "Briscola\\bin\\Debug\\Resources\\CarteTrevisane\\AssoBastoni.png")));
}*/

        private void button_Click(object sender, RoutedEventArgs e)
        {
            RotateTransform rt = new RotateTransform();
            DoubleAnimation rotateAnimation = new DoubleAnimation(0, 90, new Duration(TimeSpan.FromSeconds(5)));

            TranslateTransform tt = new TranslateTransform();
            DoubleAnimation moveXAnimation = new DoubleAnimation(0, 300, TimeSpan.FromSeconds(5));
            DoubleAnimation moveYAnimation = new DoubleAnimation(0, 300, TimeSpan.FromSeconds(5));

            TransformGroup transform = new TransformGroup();
            transform.Children.Add(rt);
            transform.Children.Add(tt);
            /* img1.RenderTransformOrigin = new Point(0.5, 0.5);
             img1.RenderTransform = transform;
            */
            rt.BeginAnimation(RotateTransform.AngleProperty, rotateAnimation);
            tt.BeginAnimation(TranslateTransform.XProperty, moveXAnimation);
            tt.BeginAnimation(TranslateTransform.YProperty, moveYAnimation);

            //((Storyboard)Resources["Storyboard"]).Begin();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            /*Title = "DoubleAnimationUsingKeyFrames Example";
            Background = Brushes.White;
            Margin = new Thickness(20);

            // Create a NameScope for this page so that
            // Storyboards can be used.
            NameScope.SetNameScope(this, new NameScope());

            // Create a rectangle.
            Rectangle aRectangle = new Rectangle();
            aRectangle.Width = 100;
            aRectangle.Height = 100;
            aRectangle.Stroke = Brushes.Black;
            aRectangle.StrokeThickness = 5;

            // Create a Canvas to contain and
            // position the rectangle.
            Canvas containerCanvas = new Canvas();
            containerCanvas.Width = 610;
            containerCanvas.Height = 300;
            containerCanvas.Children.Add(aRectangle);
            Canvas.SetTop(aRectangle, 100);
            Canvas.SetLeft(aRectangle, 10);

            // Create a TranslateTransform to
            // move the rectangle.
            TranslateTransform animatedTranslateTransform =
                new TranslateTransform();
            aRectangle.RenderTransform = animatedTranslateTransform;

            // Assign the TranslateTransform a name so that
            // it can be targeted by a Storyboard.
            this.RegisterName(
                "AnimatedTranslateTransform", animatedTranslateTransform);

            // Create a DoubleAnimationUsingKeyFrames to
            // animate the TranslateTransform.
            DoubleAnimationUsingKeyFrames translationAnimation
                = new DoubleAnimationUsingKeyFrames();
            translationAnimation.Duration = TimeSpan.FromSeconds(6);

            // Animate from the starting position to 500
            // over the first second using linear
            // interpolation.
            translationAnimation.KeyFrames.Add(
                new LinearDoubleKeyFrame(
                    500, // Target value (KeyValue)
                    KeyTime.FromTimeSpan(TimeSpan.FromSeconds(3))) // KeyTime
                );

            // Animate from 500 (the value of the previous key frame)
            // to 400 at 4 seconds using discrete interpolation.
            // Because the interpolation is discrete, the rectangle will appear
            // to "jump" from 500 to 400.
            translationAnimation.KeyFrames.Add(
                new DiscreteDoubleKeyFrame(
                    400, // Target value (KeyValue)
                    KeyTime.FromTimeSpan(TimeSpan.FromSeconds(4))) // KeyTime
                );

            // Animate from 400 (the value of the previous key frame) to 0
            // over two seconds, starting at 4 seconds (the key time of the
            // last key frame) and ending at 6 seconds.
            translationAnimation.KeyFrames.Add(
                new SplineDoubleKeyFrame(
                    0, // Target value (KeyValue)
                    KeyTime.FromTimeSpan(TimeSpan.FromSeconds(6)), // KeyTime
                    new KeySpline(0.6, 0.0, 0.9, 0.0) // KeySpline
                    )
                );

            // Set the animation to repeat forever.
            translationAnimation.RepeatBehavior = RepeatBehavior.Forever;

            // Set the animation to target the X property
            // of the object named "AnimatedTranslateTransform."
            Storyboard.SetTargetName(translationAnimation, "AnimatedTranslateTransform");
            Storyboard.SetTargetProperty(
                translationAnimation, new PropertyPath(TranslateTransform.XProperty));

            // Create a storyboard to apply the animation.
            Storyboard translationStoryboard = new Storyboard();
            translationStoryboard.Children.Add(translationAnimation);

            // Start the storyboard after the rectangle loads.
            aRectangle.Loaded += delegate (object senders, RoutedEventArgs ex)
            {
                translationStoryboard.Begin(this);
            };

            Content = containerCanvas;*/

        }

    }
}

