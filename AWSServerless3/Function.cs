using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace AWSServerless3
{
    public class Functions
    {
        /// <summary>
        /// Default constructor that Lambda will invoke.
        /// </summary>
        public Functions()
        {
        }


        /// <summary>
        /// A Lambda function to respond to HTTP Get methods from API Gateway
        /// </summary>
        /// <param name="request"></param>
        /// <returns>The list of blogs</returns>
        public APIGatewayProxyResponse Get(APIGatewayProxyRequest request, ILambdaContext context)
        {
            var key = request.QueryStringParameters["Key"];
            var token = request.QueryStringParameters["Token"];
            string mode = request.QueryStringParameters["mode"];
            if (mode == null)
            {
                mode = "encrypt";
            }
;            var generator = new ecencryptstdlib.ECTokenGenerator();
            string result = string.Empty;
            if (mode == "encrypt")
                result = generator.EncryptV3(key, token, false);
            else
                result = generator.DecryptV3(key, token, false);

            var response = new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.OK,
                Body = result,
                Headers = new Dictionary<string, string> {
                    { "Content-Type", "text/plain" },
                    { "Access-Control-Allow-Headers", "Content-Type,X-Amz-Date,Authorization,X-Api-Key,x-requested-with" },
                    { "Access-Control-Allow-Origin", "*" },
                    { "Access-Control-Allow-Credentials", "true" },
                    { "Access-Control-Allow-Methods", "GET" }
                }
            };

            return response;
        }
    }
}
