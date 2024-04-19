using System.Xml;

namespace UniLaunch.MacOS.Plist;

public class PlistDictWriter : IDisposable
{
    private readonly XmlWriter _writer;

    public PlistDictWriter(string filePath)
    {
        _writer = XmlWriter.Create(new FileStream(filePath, FileMode.Create), new XmlWriterSettings { Indent = true });
        _writer.WriteDocType(
            "plist",
            "-//Apple//DTD PLIST 1.0//EN",
            "http://www.apple.com/DTDs/PropertyList-1.0.dtd",
            null
        );
        _writer.WriteStartElement("plist");
        _writer.WriteAttributeString("version", "1.0");
        _writer.WriteStartElement("dict");
    }

    private void WriteTag(string key, string content)
    {
        _writer.WriteStartElement(key);
        _writer.WriteString(content);
        _writer.WriteEndElement();
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
        _writer.WriteStartElement(value ? "true" : "false");
        _writer.WriteEndElement();
    }

    public void WriteArray(string key, string[] values)
    {
        WriteKey(key);
        _writer.WriteStartElement("array");

        foreach (var value in values)
        {
            WriteTag("string", value);
        }

        _writer.WriteEndElement();
    }

    public void Dispose()
    {
        _writer.WriteEndElement();
        _writer.WriteEndElement();
        _writer.Close();
        _writer.Dispose();
        GC.SuppressFinalize(this);
    }
}