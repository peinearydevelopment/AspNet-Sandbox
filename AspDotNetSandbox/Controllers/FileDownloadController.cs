namespace AspDotNetSandbox.Controllers
{
    using System.Web.Mvc;

    public class FileDownloadController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Download()
        {
            return File(Server.MapPath("~/Docs/TestDoc.xls"), "application/vnd.ms-excel", "DataExport.xls");
        }
    }
}