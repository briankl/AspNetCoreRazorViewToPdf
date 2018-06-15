using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.NodeServices;
using PDFGenService.Models;
using RazorLight;

namespace PDFGenService.Controllers
{
    [Route("api/[controller]")]
    public class PdfController : Controller
    {
        private RazorLightEngine _razorEngine;

        public PdfController()
        {
            var templatePath = $@"{Directory.GetCurrentDirectory()}\PDFTemplates";
            _razorEngine = new RazorLightEngineBuilder().UseFilesystemProject(templatePath).UseMemoryCachingProvider().Build();
        }

        [HttpGet, Route("Download")]
        public async Task<IActionResult> Download([FromServices] INodeServices nodeServices)
        {
            var model = new ResultsPdf
            {
                Title = "Hello World",
                Description = "This PDF is generated from a Razor view.",
                Results = new List<string>
                {
                    "List Item 1",
                    "List Item 2",
                    "List Item 3"
                }
            };

            var pdfHtml = await _razorEngine.CompileRenderAsync("Results.cshtml", model);

            var result = await nodeServices.InvokeAsync<byte[]>("./pdf", pdfHtml);

            HttpContext.Response.ContentType = "application/pdf";

            string filename = @"results.pdf";
            HttpContext.Response.Headers.Add("x-filename", filename);
            HttpContext.Response.Headers.Add("Access-Control-Expose-Headers", "x-filename");
            HttpContext.Response.Body.Write(result, 0, result.Length);
            return new ContentResult();
        }

    }
}