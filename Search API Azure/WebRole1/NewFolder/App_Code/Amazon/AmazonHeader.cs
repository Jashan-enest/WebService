﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ServiceModel.Channels;
using System.Xml;
/// <summary>
/// Summary description for AmazonHeader
/// </summary>
public class AmazonHeader : MessageHeader
{
    private string name;
    private string value;

    public AmazonHeader(string name, string value)
    {
        this.name = name;
        this.value = value;
    }

    public override string Name { get { return name; } }
    public override string Namespace { get { return "http://security.amazonaws.com/doc/2007-01-01/"; } }
   // public override string Namespace { get { return "http://security.amazonaws.com/doc/2010-09-01/"; } }
    protected override void OnWriteHeaderContents(XmlDictionaryWriter xmlDictionaryWriter, MessageVersion messageVersion)
    {
        xmlDictionaryWriter.WriteString(value);
    }
}