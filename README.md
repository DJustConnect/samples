# DJustConnect Samples
In this repository you will find sample code for a consumer and provider for the DJustConnect platform.

## Consumer Sample

### How it works
The application will load in the certificate from the file and add it to the http call. Then the subscription key will be added to the http headers. 
Next the call will be made to the given URL using the given http method and that response is printed to the command line.

### How to use the sample
The sample consumer is a console app that will do a call to the platform to get the data from the API you requested and have access to.
You can run it by opening CMD in the directory of the exe. Then run it with the consume command and the parameters which are described below:

* -u the URL of the platform API that you want to call
* -v the http method of the call
* -c the path to the certificate file or the certificate filename used to call the platform
* -p the certificate’s password
* -s the subscription key that you received during registration

#### Command
Ilvo.DataHub.Samples.Consumer.exe consume
-u https://api-url
-v "Get"
-c "C:\Certificates\cert.pfx"
-p "certificate password"
-s "c177c5740d434a9f8d7caef793dbd463"

## Provider Sample
The sample is based on the .net core 2.2 Web API template and initially only contained a ValueController and some basic boilerplate code.

### Mutual SSL
A Provider API is a (REST) API that supports Mutual SSL Authentication, entailing:

* It is available on an HTTPS endpoint only (preferably with [HSTS](https://en.wikipedia.org/wiki/HTTP_Strict_Transport_Security) enabled), using a public CA-signed SSL certificate adhering to contemporary standards (encryption protocol, key length, etc.)
* It accepts client certificates as a way of authentication, with authorization based on the certificate contents in place. The DataHub’s client certificate will need to be authorized to access the necessary resources in the API.

Since the Mutual SSL Authentication is primarily enforced on the transport level, the configuration will strongly depend on the hosting mechanism used for the Provider API.
We opted to create a basic sample written in .net core 2.2, hosted on an Azure App Service.

Azure App Services provides hosting on a public CA-signed endpoint out of the box. The API is hosted [here](https://wa-sample-api.azurewebsites.net), using the azurewebsites.net wildcard SSL certificate.
In an actual production scenario you would create a CSR (Certificate Signing Request)
and choose a public CA (Certificate Authority, e.g. GlobalSign, Thawte, Verisign, …) to get a public CA-signed certificate.

The code repository contains a self-signed client certificate (“self-signed-for-local-test-trust.pfx") that can be used to authenticate to the API.
Another self-signed client certificate is added (“self-signed-for-local-test-do-not-trust.pfx”) you can use if you do not have other client certificates available on your machine.
Certificates added to the CurrentUser/My (Personal) store should be available for selection when accessing the API using your browser.

The Azure App Service hosted Web API is configured to require client certificates. Azure App Service only offers limited options, but it is possible to exclude the OpenAPI spec’s URL, for example. Note that this is not required for registering your API in the Datahub.

![alt text](https://github.com/DJustConnect/samples/blob/master/Appservice.png "Azure App service setting")

The ValuesController inside the Sample Provider API was decorated with a custom AuthorizationFilter, in which the client certificates are verified and authorized.
Azure App Service passes on the client certificate (in binary form) inside the X-ARR-ClientCert header.

The incoming certificate should be verified (it should be trusted, so either signed by a public CA, or explicitly trusted by the connecting client). Since Azure App Service in the Basic tier does not allow self-signed certificates to be added to the Trusted Root CA Certificates, this code is commented out.
Note that the DataHub Client Certificate will eventually be a public CA-signed one.

Subsequently, the thumbprint of the certificate is used to perform the authorization. You could also check the subject, compare it with a public certificate you load yourself, … there are many options to perform authorization.

### OpenAPI Spec
A Provider API should also expose its public contract in the form of an [OpenAPI spec](https://swagger.io/specification) version 2/3, with certain [limitation](https://docs.microsoft.com/en-us/azure/api-management/api-management-api-import-restrictions#a-nameopen-api-aopenapiswagger) imposed by Azure API Gateway.
Usually the OpenAPI spec lives in a generated JSON static file on your web host.

The sample uses the SwashBuckle library to generate the OpenAPI spec during the build, automatically creating a static route for exposing it to the host (“/swagger/v1/swagger.json ").
Swagger UI was also enabled to provide a visual representation on top of the generated OpenAPI spec.

Note that the OpenAPI spec can hold much more than just the structure of your API. A lot of metadata regarding the API, the company and resources can be put into it, and can be displayed to Datahub visitors.

### FarmID call
Apart from the provider Data API itself, DjustConnect also requires you to register an endpoint for a metacall the platform needs to make. This metacall can be part of your API itself, but that is not required. As long as DjustConnect is also authorized to invoke the metacall, any URL can be used. 
However, if you do include the metacall in your data API, we strongly recommend to **NOT** include it in your OpenAPI spec.

The goal of the metacall is to let DjustConnect query your resources for the presence of certain farms. In most cases, a consumer will have made data access requests for a specific list of farms (e.g. his customers), and DjustConnect will only create data access requests for those farms that are actually present in the resources. Therefore, DjustConnect will invoke the metacall endpoint, with the resource requested and a list of Farm IDs.

Your backend should then return only those Farm IDs that are present in the requested resource.
In some cases, consumers can request access to All Farms in your resource (e.g. for research purposes). In that case the metacall request will not contain a list of Farm IDs, and the response should return all Farms that have data in the requested resource.

In the existing Provider Sample API we created the farmID controller for this call. There we have a list of all the resources with all the farmIDs where a resource has data for.
To make the metacall, DjustConnect would need to call the following URL: https://wa-sample-api.azurewebsites.net/farmid

The request for this call will contain the ResourceUrl, a list of farmIDs where we want to know if the resource has data for, and an all flag. It will look like this:
```json
{
  "resourceUrl": "https://wa-sample-api.azurewebsites.net/api/number",
  "farmIds": [
    "100",
    "102"
  ],
  "all": false
}
```

#### Handling the request
When you receive this request from our system, we expect the following logic to happen. When the all flag is set to true, we need to get all the farmIDs for that resource.

Otherwise we expect you to compare the farmIDs in the request with the farmIDs for which you have data for in the resource and return those that are present in both lists.

If the resource URL from the request doesn’t match a resource in your system, then return an empty array. When the all flag is set to false and the farmID list is empty you should just return an empty list.

### Other Sources
The sample code contains a list of sources, specific to .net core 2.2 and Azure App Service, on how to configure the features described above.