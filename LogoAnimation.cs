using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Hasoony;

public sealed class LogoAnimation
{
    private readonly Canvas canvas;
    private readonly Image first, second;
    private readonly DispatcherTimer timer;

    private double x1 = 20, y1 = 30, x2 = 280, y2 = 160;
    private double vx1 = 3.1, vy1 = 2.4, vx2 = -2.8, vy2 = 3;

    public LogoAnimation(Canvas target)
    {
        canvas = target;
        first = MakeImage("terraria.png");
        second = MakeImage("stardew.png");
        canvas.Children.Add(first);
        canvas.Children.Add(second);

        timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(16)
        };
        timer.Tick += (_, _) => Update();
        timer.Start();
    }

    private static Image MakeImage(string name)
    {
        var bitmap = new BitmapImage();
        bitmap.BeginInit();
        bitmap.UriSource = new Uri(
            "pack://application:,,,/Assets/" + name);
        bitmap.EndInit();
        return new Image
        {
            Source = bitmap,
            Width = 190,
            Height = 100,
            Stretch = Stretch.Uniform
        };
    }

    private static void Move(ref double p, ref double v,
        double size, double limit)
    {
        double max = Math.Max(0, limit - size);
        p += v;
        if (p < 0) { p = 0; v = Math.Abs(v); }
        if (p > max) { p = max; v = -Math.Abs(v); }
    }

    private void Update()
    {
        Move(ref x1, ref vx1, first.Width, canvas.ActualWidth);
        Move(ref y1, ref vy1, first.Height, canvas.ActualHeight);
        Move(ref x2, ref vx2, second.Width, canvas.ActualWidth);
        Move(ref y2, ref vy2, second.Height, canvas.ActualHeight);
        Canvas.SetLeft(first, x1);
        Canvas.SetTop(first, y1);
        Canvas.SetLeft(second, x2);
        Canvas.SetTop(second, y2);
    }

    public void Stop() => timer.Stop();
}
