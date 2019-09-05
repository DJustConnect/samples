﻿using System;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Ilvo.DataHub.Samples.Provider.Filters
{
    [AttributeUsage(AttributeTargets.Class)]
    public class RequireClientCertificateAttribute : Attribute, IAuthorizationFilter
    {
        private const string SelfSignedCertThumbprint = "5297bc7b3c0bcb2905cf4158e50e2b65162edc1b";

        public void OnAuthorization(AuthorizationFilterContext context)
        {
#if DEBUG
            // Get certificate from the HTTP pipeline
            X509Certificate2 certificate = context.HttpContext.Connection.ClientCertificate;
            if (certificate == null)
            {
                // Missing certificate (Note that this can have already been validated by the host, on transport level!)
                context.Result = new UnauthorizedResult();
            }
            else
            {
                Trace.WriteLine(certificate.SubjectName.Name, "RequireClientCertificate");
                
                // Authorization (Note that without explicit authorization on application level, ANY valid client certificate is granted access)
                if (!string.Equals(certificate.Thumbprint, SelfSignedCertThumbprint,
                    StringComparison.OrdinalIgnoreCase))
                    context.Result = new ForbidResult();

            }
#else
            // Azure App Service will pass base64 encoded certificate in a header
            string header = context.HttpContext.Request.Headers["X-ARR-ClientCert"];
            if (String.IsNullOrEmpty(header))
            {
                // Missing certificate (Note that this should have already been validated by the host, on transport level!)
                context.Result = new UnauthorizedResult();
            }
            else
            {
                byte[] data = Convert.FromBase64String(header);
                using (var certificate = new X509Certificate2(data)) {
                    Trace.WriteLine(certificate.SubjectName.Name, "RequireClientCertificate");

                // Authorization (Note that without explicit authorization on application level, ANY valid client certificate is granted access)
                if (!string.Equals(certificate.Thumbprint, SelfSignedCertThumbprint,
                    StringComparison.OrdinalIgnoreCase))
                    context.Result = new ForbidResult();
                }
            }
#endif
        }
    }
}