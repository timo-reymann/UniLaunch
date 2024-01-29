using System.Xml;

namespace UniLaunch.MacOS.Plist;

public class PlistDictWriter : IDisposable
{
    private XmlWriter writer;

    public PlistDictWriter(string filePath)
    {
        writer = XmlWriter.Create(new FileStream(filePath, FileMode.Create), new XmlWriterSettings { Indent = true });
        writer.WriteDocType(
            "plist",
            "-//Apple//DTD PLIST 1.0//EN",
            "http://www.apple.com/DTDs/PropertyList-1.0.dtd",
            null
        );
        writer.WriteStartElement("plist");
        writer.WriteAttributeString("version", "1.0");
        writer.WriteStartElement("dict");
    }

    private void WriteTag(string key, string content)
    {
        writer.WriteStartElement(key);
        writer.WriteString(content);
        writer.WriteEndElement();
    }

    private void WriteKey(string name)
    {
        WriteTag("key", name);
    }

    public void WriteString(string key, string value)
    {
        WriteKey(key);
        WriteTag("string", value);
    }

    public void WriteBool(string key, bool value)
    {
        WriteKey(key);
        writer.WriteStartElement(value ? "true" : "false");
        writer.WriteEndElement();
    }

    public void WriteArray(string key, string[] values)
    {
        WriteKey(key);
        writer.WriteStartElement("array");

        foreach (var value in values)
        {
            WriteTag("string", value);
        }

        writer.WriteEndElement();
    }

    public void Dispose()
    {
        writer.WriteEndElement();
        writer.WriteEndElement();
        writer.Close();
    }
}