using System;


[ProtoContract]
public class data
{
	public data()
	{
        [ProtoMember(1)]
        public string servicename { get; set; }

        [ProtoMember(2)]
        public List Conf { get; set; }
    }
}
