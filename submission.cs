using System;
using System.Xml.Schema;
using System.Xml;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http;


/**
 * This template file is created for ASU CSE445 Distributed SW Dev Assignment 4.
 * Please do not modify or delete any existing class/variable/method names. However, you can add more variables and functions.
 * Uploading this file directly will not pass the autograder's compilation check, resulting in a grade of 0.
 * **/


namespace ConsoleApp1
{
    public class Program
    {
        public static string xmlURL = "https://hainesroy389.github.io/CSE445Project4/Hotels.xml";                   //XML document
        public static string xmlErrorURL = "https://hainesroy389.github.io/CSE445Project4/HotelsErrors.xml";        //XML error document
        public static string xsdURL = "https://hainesroy389.github.io/CSE445Project4/Hotels.xsd";                   //XML schema document
        public static bool isXML = true;            //Used to check if an XML read error occured
        public static string errMsg = "";           //errMsg catches the error message
        
        public static void Main(string[] args)
        {
            string result = Verification(xmlURL, xsdURL);
            Console.WriteLine(result);


            result = Verification(xmlErrorURL, xsdURL);
            Console.WriteLine(result);


            result = Xml2Json(xmlURL);
            Console.WriteLine(result);
            
        }

        // Q2.1
        public static string Verification(string xmlUrl, string xsdUrl)
        {
            //Local Declarations
            string result = "";
            
            //Statements
            
            //Resets to true in case of further tests in same test run
            isXML = true;
            
            //Create XmlSchemaSet
            XmlSchemaSet sc = new XmlSchemaSet();
            
            try
            {
                //Get actual XML schema and add to collection
                sc.Add(null,xsdUrl);
                
                //Set validation settings
                XmlReaderSettings settings = new XmlReaderSettings();  //Reader class
                settings.ValidationType = ValidationType.Schema; //Knows we are dealing with a schema now
                settings.Schemas = sc; //Puts schema into settings Reader
                settings.ValidationEventHandler += new ValidationEventHandler(ValidationCallBack); //Enacts whenever parse error occures
                
                //Read XML document now
                XmlReader reader = XmlReader.Create(xmlUrl,settings);
                
                //Parse the file
                while(reader.Read());
                
                //return "No Error" if XML is valid. Otherwise, return the desired exception message.
               
                if(isXML==true) //No error occured
                {
                   result = "No Errors are found";
                }
                else
                {
                    result = errMsg;    //Error occured
                }
            }
            catch (XmlException ex) //Used for customized test with special error
            {
                result = ex.Message;
            }
            
            return result;
        }

        private static void ValidationCallBack(object sender, ValidationEventArgs e)
        {
            isXML = false; 
            errMsg = e.Message; //Error message
            
        }

        public static string Xml2Json(string xmlUrl)
        {
            //Local Declarations
            string jsonText = "";   
            
            //Statements
            HttpClient client = new HttpClient();   //Used to get xml content
            
            try
            {
                string xmlContent = client.GetStringAsync(xmlUrl).Result;   //xml string
                XmlDocument doc = new XmlDocument();    //Parser for xml doc
                doc.LoadXml(xmlContent);    //Put xml string inside it
                jsonText = JsonConvert.SerializeXmlNode(doc, Newtonsoft.Json.Formatting.Indented);  //convert to json
            }
            finally
            {
                client.Dispose(); //close http client
            }
            
            // The returned jsonText needs to be de-serializable by Newtonsoft.Json package. (JsonConvert.DeserializeXmlNode(jsonText))
            //return jsonText;
            return jsonText;
        }
    }
}
