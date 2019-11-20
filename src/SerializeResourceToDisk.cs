using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace TaskForQuestionnaireExample
{
    public static class ResourceExtensionMethods
    {
        public static void SerializeResourceToDiskAsXml(this Resource resource, string path, bool pretty = false)
        {
            using (XmlWriter writer = XmlWriter.Create(new StreamWriter(path), new XmlWriterSettings { Indent = pretty }))
            {
                resource.WriteTo(writer);
            }
        }
        
        public static void SerializeResourceToDiskAsJson(this Resource resource, string path, bool pretty = false)
        {
            using (JsonWriter writer = new JsonTextWriter(new StreamWriter(path)))
            {
                writer.Formatting = pretty ? Newtonsoft.Json.Formatting.Indented : Newtonsoft.Json.Formatting.None;
                resource.WriteTo(writer);
            }
        }
    }
}
