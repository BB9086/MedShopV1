using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedShop.Controllers
{
    //public class ErrorController : Controller
    //{
    //    [Route("Error/{statusCode}")]
    //    public IActionResult HttpStatusCodeHandler(int statusCode)
    //    {
    //        switch (statusCode)
    //        {
    //            case 404:
    //                ViewBag.ErrorMessage = "Sorry. The resource you requested could not be found";
    //                break;


    //        }
    //        return View("NotFound");
    //    }

    //    [Route("Error")]
    //    [AllowAnonymous]
    //    public IActionResult Error()
    //    {
    //        var exceptionDetails = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
    //        ViewBag.ExceptionPath = exceptionDetails.Path;
    //        ViewBag.ExceptionMessage = exceptionDetails.Error.Message;
    //        ViewBag.Stacktrace = exceptionDetails.Error.StackTrace;

    //        return View("Error");
    //    }
    //}

    public class ErrorController : Controller
    {

        private readonly ILogger<ErrorController> logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            this.logger = logger;
        }


        [Route("Error/{statusCode}")]
        protected IActionResult HttpStatusCodeHandler(int statusCode)
        {
            var statusCodeResult =
                    HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

            switch (statusCode)
            {
                case 404:
                    ViewBag.ErrorMessage =
                            "Sorry, the resource you requested could not be found";
                    //ViewBag.Path = statusCodeResult.OriginalPath;
                    //ViewBag.QS = statusCodeResult.OriginalQueryString;
                    logger.LogWarning($"404 error occured. Path = " +
                            $"{statusCodeResult.OriginalPath} and QueryString = " +
                            $"{statusCodeResult.OriginalQueryString}");

                    break;
            }

            return View("NotFound");
        }

        [AllowAnonymous]
        [Route("Error")]
        protected IActionResult Error()
        {
            // Retrieve the exception Details
            var exceptionHandlerPathFeature =
                    HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            ViewBag.ExceptionPath = exceptionHandlerPathFeature.Path;
            ViewBag.ExceptionMessage = exceptionHandlerPathFeature.Error.Message;
            ViewBag.StackTrace = exceptionHandlerPathFeature.Error.StackTrace;

            // LogError() method logs the exception under Error category in the log
            logger.LogError($"The path {exceptionHandlerPathFeature.Path} " +
                $"threw an exception {exceptionHandlerPathFeature.Error}");

            return View("Error");
        }
    }

}
