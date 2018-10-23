using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuf;
namespace GaoJD.Club.BusinessEntity
{
    [ProtoContract]
    public class ProtoBufTest
    {
        [ProtoMember(1)]
        public int ID { get; set; }
        [ProtoMember(2)]
        public string Name { get; set; }
        [ProtoMember(3)]
        public string Address { get; set; }
    }
}
