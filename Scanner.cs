using System;
using System.Collections.Generic;
using System.IO;

namespace Hasoony;

public sealed class Scanner
{
    private static readonly HashSet<string> Extensions =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ".jpg", ".jpeg", ".png", ".gif",
            ".bmp", ".tif", ".tiff", ".webp"
        };

    public event Action<string>? ImageFound;

    public void ScanAllDrives()
    {
        foreach (DriveInfo drive in DriveInfo.GetDrives())
        {
            try
            {
                if (drive.IsReady)
                    ScanFolder(drive.RootDirectory.FullName);
            }
            catch { }
        }
    }

    private void ScanFolder(string start)
    {
        var folders = new Stack<string>();
        folders.Push(start);

        while (folders.Count > 0)
        {
            string folder = folders.Pop();

            try
            {
                foreach (string file in Directory.EnumerateFiles(folder))
                {
                    try
                    {
                        if (Extensions.Contains(Path.GetExtension(file)))
                            ImageFound?.Invoke(file);
                    }
                    catch { }
                }
            }
            catch { }

            try
            {
                foreach (string directory in Directory.EnumerateDirectories(folder))
                {
                    try
                    {
                        var info = new DirectoryInfo(directory);

                        if ((info.Attributes & FileAttributes.ReparsePoint) == 0)
                            folders.Push(directory);
                    }
                    catch { }
                }
            }
            catch { }
        }
    }
}
