using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Archimedes.Framework.Context;

namespace Archimedes.Framework.Test
{
    public class TestApplicationContext : ApplicationContext
    {
        public TestApplicationContext()
        {
            
        }

        public void Start()
        {
            EnableAutoConfiguration();
        }
    }
}
