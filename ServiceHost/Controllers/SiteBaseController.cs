using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.Controllers
{
    public class SiteBaseController : Controller
    {
        protected string ErrorMessage = "Error Message";
        protected string SuccessMessage = "Success Message";
        protected string InfoMessage = "Info Message";
        protected string WarningMessage = "Warning Message";
    }
}
