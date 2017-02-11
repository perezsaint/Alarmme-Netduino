using System;
using Microsoft.SPOT;
using System.Net.Sockets;
using System.Text;
using System.Net;
using System.IO;
using System.Threading;

namespace HomeTrid
{
    class HTTPHandler
    {
        public static void sendRequest()
        {
            try
            {

                var request = WebRequest.Create(new Uri("http://hometriad.azurewebsites.net/tables/hcsensor"));

                var json = "{}";
                request.ContentType = "application/json";
                request.Headers.Add("zumo-api-version", "2.0.0");
                request.Method = "POST";
                request.ContentLength = 2;

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                var httpResponse = (HttpWebResponse)request.GetResponse();
                using (var streamReader = new StreamReader(stream: httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    Debug.Print(result);
                }
            } catch(Exception e)
            {
                Debug.Print(e.ToString());
            }
            
        }
    }

}
