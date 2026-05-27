using System;
using System.Collections.Generic;
using System.IO;

namespace flpicker
{
    public static class FlScanner
    {
        public static List<FlInstall> FindInstalls()
        {
            var results = new List<FlInstall>();

            var roots = new[]
            {
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Image-Line"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Image-Line")
            };

            foreach (var root in roots)
            {
                if (!Directory.Exists(root)) continue;

                foreach (var dir in Directory.GetDirectories(root))
                {
                    var fl64 = Path.Combine(dir, "FL64.exe");
                    var fl32 = Path.Combine(dir, "FL.exe");

                    if (File.Exists(fl64))
                    {
                        results.Add(new FlInstall
                        {
                            Name = new DirectoryInfo(dir).Name,
                            Path = fl64,
                            Is64Bit = true
                        });
                    }

                    if (File.Exists(fl32))
                    {
                        results.Add(new FlInstall
                        {
                            Name = new DirectoryInfo(dir).Name,
                            Path = fl32,
                            Is64Bit = false
                        });
                    }
                }
            }

            return results;
        }
    }
}