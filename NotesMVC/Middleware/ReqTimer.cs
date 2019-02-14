using Microsoft.AspNetCore.Http;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace NotesMVC.Middleware {
    public class ReqTimer {

        private readonly RequestDelegate _next;

        public ReqTimer(RequestDelegate request) {
            _next = request;
        }

        public async Task Invoke(HttpContext context) {

            var sw = new Stopwatch();
            sw.Start();

            context.Response.OnStarting((state) => {

                sw.Stop();

                (state as HttpContext).Response.Headers.Add("X-Req-Milliseconds", sw.ElapsedMilliseconds.ToString());
                return Task.FromResult(0);

            }, context);

            await _next.Invoke(context);

        }

    }
}
