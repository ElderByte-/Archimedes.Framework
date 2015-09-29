[![Downloads](https://img.shields.io/badge/download-nuget-blue.svg)](https://www.nuget.org/packages/archimedes.framework)

# Archimedes.Framework
A lightweight application framework, providing Dependency Injection Container with package / assembly scanning and configuration services.

Sample which shows how the framework is used on a typical service:

```csharp

[Service] // Attribute to mark services - used by Auto-Configuration
public class MyCoolService 
{
    [Inject]
    private ICustomerService _customerService;  // Field injection

    [Value("${sample.name}")]  // Easy access to configuration properties
    private string _name;
    
}
```

Much of the API design is inspired if not 1:1 taken from the great Spring Framework.

# How to use / Documentation
Documentation is currently lacking. However, you can have a look at [the sample WPF App](https://github.com/ElderByte-/Archimedes.Framework/tree/master/Samples/Archimedes.Framework.SampleApp) which shows the usage of the framework.
