using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ClientApp
{
    [DataContract]
    public class MainData
    {

        [DataMember(Name = "id", EmitDefaultValue = false)]
        public int? Id { get; set; }

        [DataMember(Name = "text")]
        public string Text { get; set; }

    }
}