using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Hasoony;

public sealed class Credits
{
    private readonly Grid root;

    public Credits(Grid container) => root = container;

    public void Show(Action close)
    {
        root.Children.Clear();
        root.Background = new SolidColorBrush(
            Color.FromRgb(40, 40, 40));

        var viewport = new Grid { ClipToBounds = true };
        var stack = new StackPanel
        {
            Width = 550,
            HorizontalAlignment = HorizontalAlignment.Center
        };

        Add(stack, "Idea from Hasoony");
        Add(stack, "");
        Add(stack, "Lots of credits from Amoli");

        var transform = new TranslateTransform();
        stack.RenderTransform = transform;
        viewport.Children.Add(stack);
        root.Children.Add(viewport);

        transform.BeginAnimation(
            TranslateTransform.YProperty,
            new DoubleAnimation
            {
                From = 750,
                To = -400,
                Duration = TimeSpan.FromSeconds(14),
                RepeatBehavior = RepeatBehavior.Forever
            });

        var exit = new Button
        {
            Content = "×",
            Width = 42,
            Height = 42,
            HorizontalAlignment = HorizontalAlignment.Right,
            VerticalAlignment = VerticalAlignment.Top
        };
        exit.Click += (_, _) => close();
        root.Children.Add(exit);
    }

    private static void Add(StackPanel panel, string text)
    {
        panel.Children.Add(new TextBlock
        {
            Text = text,
            FontSize = 30,
            Foreground = Brushes.White,
            TextAlignment = TextAlignment.Center,
            Margin = new Thickness(10, 35, 10, 35)
        });
    }
}
