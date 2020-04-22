using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cw3.Middlewares
{
    public class LoggingMiddleware
    {
        string path = "requestsLog.txt";

        private readonly RequestDelegate _next;
        public LoggingMiddleware(RequestDelegate next) { _next = next; }
        public async Task InvokeAsync(HttpContext httpContext)
        {


            httpContext.Request.EnableBuffering();

            var m = "Method: " + httpContext.Request.Method;

            var p = "Path: " + httpContext.Request.Path;

            var bodyStream = "Body: \n";
            using (var reader = new StreamReader(httpContext.Request.Body, Encoding.UTF8, true, 1024, true))
            {
                bodyStream += await reader.ReadToEndAsync();
                
            }


            var qs = "QueryString: " + httpContext.Request.QueryString;

            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine(m);
                sw.WriteLine(p);
                sw.WriteLine(bodyStream);
                sw.WriteLine(qs);
                sw.WriteLine();
                
            }









            httpContext.Request.Body.Seek(0, SeekOrigin.Begin);
            
            await _next(httpContext);
        }
    }
}
