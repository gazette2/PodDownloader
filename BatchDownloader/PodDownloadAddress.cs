﻿//------------------------------------------------------------------------------
// <auto-generated>
//     이 코드는 도구를 사용하여 생성되었습니다.
//     런타임 버전:4.0.30319.42000
//
//     파일 내용을 변경하면 잘못된 동작이 발생할 수 있으며, 코드를 다시 생성하면
//     이러한 변경 내용이 손실됩니다.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Xml.Serialization;

// 
// 이 소스 코드는 xsd, 버전=4.6.1055.0에서 자동 생성되었습니다.
// 


/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://gazette2.ddns.net/PodDownloadAddr.xsd")]
[System.Xml.Serialization.XmlRootAttribute(Namespace="http://gazette2.ddns.net/PodDownloadAddr.xsd", IsNullable=false)]
public partial class PodAddressList {
    
    private PodAddressListPodAddress[] podAddressField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("PodAddress")]
    public PodAddressListPodAddress[] PodAddress {
        get {
            return this.podAddressField;
        }
        set {
            this.podAddressField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://gazette2.ddns.net/PodDownloadAddr.xsd")]
public partial class PodAddressListPodAddress {
    
    private string addressTemplateField;
    
    private int[] sequenceNumberField;
    
    private string nameField;
    
    /// <remarks/>
    public string AddressTemplate {
        get {
            return this.addressTemplateField;
        }
        set {
            this.addressTemplateField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlArrayItemAttribute("id", IsNullable=false)]
    public int[] SequenceNumber {
        get {
            return this.sequenceNumberField;
        }
        set {
            this.sequenceNumberField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Name {
        get {
            return this.nameField;
        }
        set {
            this.nameField = value;
        }
    }
}
