using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archimedes.Framework.SampleApp.Model
{
    /// <summary>
    /// A dummy external service - it has no component attribute such as [Service],
    /// thus manual registering this component is required <see cref="ApplicationConfiguration"/>.
    /// </summary>
    public sealed class ExternalService : IExternalService
    {

    }
}
