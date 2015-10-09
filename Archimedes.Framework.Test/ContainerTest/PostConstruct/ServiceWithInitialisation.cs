using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Archimedes.Framework.Stereotype;

namespace Archimedes.Framework.Test.ContainerTest.PostConstruct
{
    public class ServiceWithInitialisation
    {


        public string Value { get; private set; }

        [PostConstruct]
        public void Init()
        {
            Value = "PostConstruct";
        }

    }
}
