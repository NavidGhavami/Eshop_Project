using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ServiceHost.Areas.Administration.Controllers
{
    [Authorize("AdminArea")]
    [Area("Administration")]
    [Route("administration")]
    public class AdminBaseController : Controller
    {
            protected string SuccessMessage = "SuccessMessage";
            protected string WarningMessage = "WarningMessage";
            protected string InfoMessage = "InfoMessage";
            protected string ErrorMessage = "ErrorMessage";
        }
    }

