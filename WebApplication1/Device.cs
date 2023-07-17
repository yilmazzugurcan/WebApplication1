using System.Collections.Generic;

public class Device
{
    public string Id { get; set; }
    public List<string> Keys { get; set; }
    public Address Address { get; set; }
}

public class Address
{
    public string Id { get; set; }
    public List<string> Keys { get; set; }
}
