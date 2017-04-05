namespace AspDotNetSandbox.Controllers
{
    using System;
    using System.Web.Mvc;

    public class PhantomController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Report(Guid fileId)
        {
            var key = "Report-" + fileId;
            var reportParams = HttpContext.Cache[key] as int?;
            if (reportParams == null)
            {
                throw new InvalidOperationException("Report parameters not found.");
            }

            HttpContext.Cache.Remove(key);

            return View();
        }
    }
}