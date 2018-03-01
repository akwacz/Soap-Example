using System;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.Xml;

namespace SoapExample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void sendButton_Click(object sender, EventArgs e)
        {
            if(apiAddress.Text.Length > 5 && requestText.Text.Length > 5)
                Execute(requestText.Text.ToString(), apiAddress.Text.ToString(), responseText);
        }


        public static void Execute(string req, string api, RichTextBox resultTextbox)
        {
            HttpWebRequest request = CreateWebRequest(api);
            XmlDocument soapEnvelopeXml = new XmlDocument();
            soapEnvelopeXml.LoadXml(req);

            using (Stream stream = request.GetRequestStream())
            {
                soapEnvelopeXml.Save(stream);
            }

            using (WebResponse response = request.GetResponse())
            {
                using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                {
                    string soapResult = rd.ReadToEnd();
                    resultTextbox.Text = soapResult;
                }
            }
        }

        public static HttpWebRequest CreateWebRequest(string apiAddress)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(apiAddress);
            webRequest.Headers.Add(@"SOAP:Action");
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            return webRequest;
        }

    }
}
