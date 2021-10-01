using System;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Ilvo.DataHub.Samples.Provider.Filters
{
    [AttributeUsage(AttributeTargets.Class)]
    public class RequireClientCertificateAttribute : Attribute, IAuthorizationFilter
    {
        private const string SelfSignedCertForLocalTestThumbprint = "78077955e858238c16c151db383c5f18684dca53"; // A local certificate, included under the "Resources" directory
        private const string DjustConnectCertThumbprint = "CEB6DA224A27FC0376464633261452050723F24C"; // The default certificate DJustConnect will use to call your API

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

                // Note: IIS Express checks validity of the certificate for us. We can expect a trusted, valid certificate here.

                // Authorization (Note that without explicit authorization on application level, ANY valid client certificate is granted access)
                if (!IsValid(certificate))
                    context.Result = new StatusCodeResult(403);

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

                    // Check certificate validity (Azure Web Apps only forwards the certificate, without any checks)
                    // Note: Azure app services do not allow adding custom certificates to the LocalMachine/CA store when not using an 
                    //  App Service Environment (higher pricing, isolated environment). As a result, our self-signed certificate cannot be 
                    //  validated using the default policy (using a simple Verify, as below).
                    
                    //if (!certificate.Verify())
                    //    context.Result = new StatusCodeResult(403);
                    //else
                    
                    // Authorization (Note that without explicit authorization on application level, ANY valid client certificate is granted access)
                    if (!IsValid(certificate))
                        context.Result = new StatusCodeResult(403);
                }
            }
#endif
        }

        private static bool IsValid(X509Certificate2 certificate)
        {
            return string.Equals(certificate.Thumbprint, SelfSignedCertForLocalTestThumbprint, StringComparison.OrdinalIgnoreCase)
                || string.Equals(certificate.Thumbprint, DjustConnectCertThumbprint, StringComparison.OrdinalIgnoreCase);

        }
    }
}