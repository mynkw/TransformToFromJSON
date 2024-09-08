using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Xml;
using System.Xml.Linq;

namespace TransformToFromJSON.Controllers
{
    public class XmlJSONController : ApiController
    {
        [HttpPost]
        [Route("api/XmlJSON/XML2JSON")]
        public HttpResponseMessage XML2JSON(HttpRequestMessage request)
        {
            string json = string.Empty;
            try 
            { 
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(request.Content.ReadAsStreamAsync().Result);
                json = JsonConvert.SerializeXmlNode(xmlDoc,Newtonsoft.Json.Formatting.None,false);
                var jObj = JObject.Parse(json);

                return Request.CreateResponse(HttpStatusCode.OK, jObj);
            }
            catch (Exception ex)  
            { 
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("api/XmlJSON/JSON2XML")]
        public HttpResponseMessage JSON2XML(dynamic request)
        {
            string json = string.Empty;
            try
            {
                string ijson = JsonConvert.SerializeObject(request); 
                XNode node = JsonConvert.DeserializeXNode(ijson,"Root");
                return new HttpResponseMessage()
                {
                    Content = new StringContent(node.ToString(), Encoding.UTF8, "application/xml")
                };

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
