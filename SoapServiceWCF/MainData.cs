using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SoapServiceWCF
{
    [DataContract]
    public class MainData
    {

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Text { get; set; }

    }
}