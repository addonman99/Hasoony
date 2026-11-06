using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Hasoony;

public partial class MainWindow : Window
{
    private readonly List<string> images = new();
    private Gallery? gallery;
    private Grid root = null!;
    private LogoAnimation? animation;
    private bool creditsOpen;

    public MainWindow()
    {
        InitializeComponent();
        root = new Grid();
        Content = root;
        PreviewKeyDown += OnKey;
        Loaded += async (_, _) => await StartScan();
    }

    private async Task StartScan()
    {
        root.Children.Clear();
        root.Background = Brushes.Gray;

        var canvas = new Canvas
        {
            ClipToBounds = true
        };
        root.Children.Add(canvas);

        animation = new LogoAnimation(canvas);

        var scanner = new Scanner();
        scanner.ImageFound += file =>
            Dispatcher.Invoke(() => images.Add(file));

        await Task.Run(scanner.ScanAllDrives);

        animation.Stop();
        images.Sort(StringComparer.OrdinalIgnoreCase);
        ShowViewer();
    }

    private Button MakeButton(string text)
    {
        return new Button
        {
            Content = text,
            Width = 65,
            Height = 55,
            FontSize = 25,
            Background = Brushes.LightGray,
            Foreground = Brushes.Black,
            Focusable = false
        };
    }

    private void ShowViewer()
    {
        creditsOpen = false;
        root.Children.Clear();

        var layout = new Grid();
        layout.RowDefinitions.Add(new RowDefinition());
        layout.RowDefinitions.Add(
            new RowDefinition { Height = new GridLength(55) });

        var area = new Grid();

        var picture = new Image
        {
            Stretch = Stretch.Uniform,
            Margin = new Thickness(85, 15, 85, 15)
        };

        area.Children.Add(picture);

        var left = MakeButton("←");
        left.HorizontalAlignment = HorizontalAlignment.Left;
        left.VerticalAlignment = VerticalAlignment.Center;
        left.Click += (_, _) => gallery?.Previous();

        var right = MakeButton("→");
        right.HorizontalAlignment = HorizontalAlignment.Right;
        right.VerticalAlignment = VerticalAlignment.Center;
        right.Click += (_, _) => gallery?.Next();

        area.Children.Add(left);
        area.Children.Add(right);
        layout.Children.Add(area);

        var footer = new Grid
        {
            Background = new SolidColorBrush(
                Color.FromRgb(145, 145, 145))
        };

        var credits = MakeButton("Credits");
        credits.Width = 105;
        credits.FontSize = 16;
        credits.HorizontalAlignment = HorizontalAlignment.Right;
        credits.Click += (_, _) => ShowCredits();

        footer.Children.Add(credits);
        Grid.SetRow(footer, 1);
        layout.Children.Add(footer);

        root.Children.Add(layout);
        gallery = new Gallery(images, picture);
    }

    private void OnKey(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape && creditsOpen)
        {
            ShowViewer();
            return;
        }

        if (creditsOpen) return;

        if (e.Key == Key.Left) gallery?.Previous();
        if (e.Key == Key.Right) gallery?.Next();
    }

    private void ShowCredits()
    {
        creditsOpen = true;
        new Credits(root).Show(ShowViewer);
    }
}
