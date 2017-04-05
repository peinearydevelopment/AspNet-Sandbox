namespace AspDotNetSandbox.Controllers
{
    using System.Web.Mvc;
    using AspDotNetSandbox.Models.FileDownload;
    using Newtonsoft.Json;

    public class JsonCustomSerializationController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Serialize(string accountIds, string accountContactIds, string fields)
        {
            var accountIdsArray = JsonConvert.DeserializeObject<int[]>(accountIds);
            var accountContactsIdsArray = JsonConvert.DeserializeObject<int[]>(accountContactIds);
            var fieldsArray = JsonConvert.DeserializeObject<FieldResult[]>(fields);

            return null;
        }
    }
}