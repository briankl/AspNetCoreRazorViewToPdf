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
        [HttpGet, Route("Download")]
        public async Task<IActionResult> Download([FromServices] INodeServices nodeServices)
        {
            var templatePath = $@"{Directory.GetCurrentDirectory()}\PDFTemplates";
            IRazorLightEngine razorEngine = new RazorLightEngineBuilder().UseFilesystemProject(templatePath).UseMemoryCachingProvider().Build();

            var model = new ResultsPdf
            {
                Title = "Hello World",
                Description = "Aliquam erat volutpat. Vestibulum ipsum leo, molestie nec ligula auctor, auctor facilisis justo. Aenean at bibendum lorem. Quisque ac nisl dolor. Vestibulum eu tortor vitae nisl pretium feugiat sed non neque."
            };

            var pdfHtml = await razorEngine.CompileRenderAsync("Results.cshtml", model);

            var result = await nodeServices.InvokeAsync<byte[]>("./pdf", pdfHtml);

            HttpContext.Response.ContentType = "application/pdf";

            string filename = @"results.pdf";
            HttpContext.Response.Headers.Add("x-filename", filename);
            HttpContext.Response.Headers.Add("Access-Control-Expose-Headers", "x-filename");
            HttpContext.Response.Body.Write(result, 0, result.Length);
            return new ContentResult();
        }

        [HttpGet, Route("Share")]
        public async Task<IActionResult> Share([FromServices] INodeServices nodeServices)
        {
            var templatePath = $@"{Directory.GetCurrentDirectory()}\PDFTemplates";
            IRazorLightEngine razorEngine = new RazorLightEngineBuilder().UseFilesystemProject(templatePath).UseMemoryCachingProvider().Build();

            var model = new ResultsPdf
            {
                Title = "Hello World",
                Description = "Aliquam erat volutpat. Vestibulum ipsum leo, molestie nec ligula auctor, auctor facilisis justo. Aenean at bibendum lorem. Quisque ac nisl dolor. Vestibulum eu tortor vitae nisl pretium feugiat sed non neque."
            };

            var pdfHtml = await razorEngine.CompileRenderAsync("Results.cshtml", model);

            var result = await nodeServices.InvokeAsync<byte[]>("./pdf", pdfHtml);

            // send email.

            return Ok();
        }
    }
}