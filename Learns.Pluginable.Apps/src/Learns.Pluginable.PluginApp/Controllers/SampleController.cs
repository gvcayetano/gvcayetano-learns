using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Learns.Pluginable.PluginApp.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Learns.Pluginable.PluginApp.Controllers
{
    //[Route("Plugins")]
    public class SampleController : PluginController
    {
        private readonly ILogger<SampleController> _logger;
        private readonly IViewCompilerProvider _viewCompilerProvider;
        private readonly ApplicationPartManager _applicationPartManager;

        public SampleController(ILogger<SampleController> logger,
        IViewCompilerProvider viewCompilerProvider,
        ApplicationPartManager applicationPartManager)
        {
            _logger = logger;
            _viewCompilerProvider = viewCompilerProvider;
            _applicationPartManager = applicationPartManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            var model = new List<string>();
            //foreach(var text in GetEmbeddedResource("some.config")){
                model.Add(GetEmbeddedResource("some.config"));
            //}
            return View(model);
        }

        public IActionResult GetEmbeddedResourcesList(){
            var payload = new string[]{};
            var someType = Type.GetType("Learns.Pluginable.PluginApp.Controllers.SampleController");
            if (someType != null){
            var sampleAssembly = someType.Assembly;
            // var sampleAssembly = typeof(SampleController).Assembly;
            payload = sampleAssembly.GetManifestResourceNames();
           
            }
            return Json(new {
                EmbeddedResources = payload
            });
        }

        private string GetEmbeddedResource(string v)
        {
            var sampleAssembly = typeof(SampleController).Assembly;
            if (sampleAssembly != null){
                var targetResource = sampleAssembly.GetManifestResourceNames().FirstOrDefault(r => r.EndsWith(v));
                if (targetResource != null){
                    using (Stream stream = sampleAssembly.GetManifestResourceStream(targetResource))
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            return string.Empty;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    [Area("Plugins/Sample")]
    public class PluginController : Controller
    {
    }
}