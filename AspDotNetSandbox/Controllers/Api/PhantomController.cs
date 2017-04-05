namespace AspDotNetSandbox.Controllers.Api
{
    using System;
    using System.Configuration;
    using System.Globalization;
    using System.IO;
    using System.Web;
    using System.Web.Hosting;
    using System.Web.Http;
    using AspDotNetSandbox.Utils.Phantom;

    public class PhantomController : ApiController
    {
        public PhantomController()
        {
            Rasterizer = new PhantomjsRasterizer();
        }

        public IRasterizer Rasterizer { get; private set; }

        [HttpPost]
        public string ExportReport()
        {
            var fileId = Guid.NewGuid();

            var localPdfLocation = HostingEnvironment.MapPath(ConfigurationManager.AppSettings["LocalPdfLocation"] ?? "~/Temp");
            var fileName = "\"" + Path.Combine(localPdfLocation, fileId + ".pdf") + "\"";

            using (var pdfManager = new PdfManager(Rasterizer, fileName))
            {
                HttpContext.Current.Cache.Insert("Report-" + fileId, 1);
                var path = $"http://{Request.RequestUri.Authority}/{Url.Route("Default", new { controller = "Phantom", action = "Report", fileId })}";
                pdfManager.Write(path);
            }

            return string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}", Url.Content("~/Report/"), fileId, ".pdf");
        }
    }
}
