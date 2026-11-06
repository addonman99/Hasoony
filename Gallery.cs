using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Hasoony;

public sealed class Gallery
{
    private readonly List<string> images;
    private readonly Image viewer;
    private int index;

    public Gallery(List<string> files, Image target)
    {
        images = files;
        viewer = target;
        Show();
    }

    public void Next() => Change(1);
    public void Previous() => Change(-1);

    private void Change(int direction)
    {
        if (images.Count == 0) return;
        index = (index + direction + images.Count) % images.Count;
        Show();
    }

    private void Show()
    {
        for (int i = 0; i < images.Count; i++)
        {
            try
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.UriSource = new Uri(images[index]);
                bitmap.EndInit();
                bitmap.Freeze();
                viewer.Source = bitmap;
                return;
            }
            catch
            {
                index = (index + 1) % images.Count;
            }
        }
        viewer.Source = null;
    }
}
