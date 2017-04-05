namespace AspDotNetSandbox.Utils.Phantom
{
    using System;
    using System.Configuration;
    using System.Diagnostics;
    using System.IO;
    using System.Text;

    public class PhantomjsRasterizer : IRasterizer
    {
        public PhantomjsRasterizer()
        {
            if (!int.TryParse(ConfigurationManager.AppSettings["PhantomjsTimeout"], out _timeout))
            {
                _timeout = 60000;
            }
        }

        public void Rasterize(string url, string fileName)
        {
            var phantomjsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin", "phantomjs.exe");
            var rasterizejsPath = $"\"{Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin", "Utils", "Phantom", "rasterize.js")}\"";

            var startInfo = new ProcessStartInfo
            {
                FileName = phantomjsPath,
                CreateNoWindow = true,
                Arguments = string.Join(" ", rasterizejsPath, url, fileName, "8.5in*11in", "1.25"),
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = true
            };

            var output = new StringBuilder();
            using (var process = new Process())
            {
                process.StartInfo = startInfo;
                try
                {
                    process.Start();

                    process.WaitForExit(_timeout);

                    var stdError = process.StandardError.ReadToEnd();
                    if (!string.IsNullOrWhiteSpace(stdError))
                    {
                        output.Append(" ").Append(stdError);
                    }

                    var stdOutput = process.StandardOutput.ReadToEnd();
                    if (!string.IsNullOrWhiteSpace(stdOutput))
                    {
                        output.Append(" ").Append(stdOutput);
                    }
                }
                catch
                {
                    process.Kill();
                    throw;
                }
            }

            if (!File.Exists(fileName.Replace("\"", "")))
            {
                throw new InvalidOperationException(string.Format("Phantomjs rasterize failed for URL '{0}'. Output file '{1}' not found. {2}", url, fileName, output));
            }
        }

        private readonly int _timeout;
    }
}