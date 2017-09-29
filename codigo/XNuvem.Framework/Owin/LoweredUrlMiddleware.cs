﻿using Microsoft.Owin;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace XNuvem.Owin
{
    public class LoweredUrlMiddleware : OwinMiddleware
    {
        private static readonly Regex _upperCase = new Regex("[A-Z]");
        public LoweredUrlMiddleware(OwinMiddleware next) : base(next) {
        }

        public async override Task Invoke(IOwinContext context) {
            var request = context.Request;
            if (request.Method.Equals("GET", StringComparison.InvariantCultureIgnoreCase)) {
                string url = request.Uri.Scheme + "://" + request.Uri.Authority + request.Uri.AbsolutePath;
                if (_upperCase.IsMatch(url)) {
                    var response = context.Response;
                    response.Headers.Add("Location", new[] { url.ToLower() + request.Uri.Query });
                    response.StatusCode = 301;
                    response.ReasonPhrase = "Moved Permanently";
                    return; // End response and not continue to next pipe
                }
            }

            await Next.Invoke(context);
        }

        
    }
}
