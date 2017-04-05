namespace AspDotNetSandbox.Utils.Phantom
{
    using System;
    using System.IO;

    public sealed class PdfManager : IDisposable
    {
        public PdfManager(IRasterizer rasterizer, string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) throw new ArgumentNullException("fileName");

            FileName = fileName;
            Rasterizer = rasterizer ?? throw new ArgumentNullException("rasterizer");
        }

        public string FileName { get; private set; }
        public IRasterizer Rasterizer { get; private set; }

        public void Write(string url)
        {
            Rasterizer.Rasterize(url, FileName);
        }

        public Stream Read()
        {
            var fileInfo = new FileInfo(FileName);
            _pdfStream = fileInfo.OpenRead();
            return _pdfStream;
        }

        public void Dispose()
        {
            if (_pdfStream != null)
            {
                _pdfStream.Dispose();
            }

            var fileInfo = new FileInfo(FileName.Replace("\"", ""));
            if (fileInfo.Exists)
            {
                //fileInfo.Delete();
            }
        }

        private FileStream _pdfStream;
    }
}