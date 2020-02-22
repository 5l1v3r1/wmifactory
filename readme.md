# WmiFactory
This is a library I made to make it easier to populate models with data from Wmi on Windows systems.

Example:

I describe my model as such, this is information I later want to send to my server
```c#
public class PCInfo
{
	// describe the root namespace of the wmi data
	// followed by the name of the key, 
	// and finally whether you want to cache this field or not
	// 
	[WmiObject("Win32_Processor", "Name", true)]
	public string ProcessorName { get; set; }
}
```

Then to pull this data into the model using our factory
```c#
	WmiObjectFactory factory = new WmiObjectFactory("cache.json");
	PCInfo info = factory.Create<PCInfo>();
```
We can now browse the data using the info object!