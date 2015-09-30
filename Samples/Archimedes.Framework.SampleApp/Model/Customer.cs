using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archimedes.Framework.SampleApp.Model
{
    public class Customer
    {
        private readonly int _id;

        public Customer(int id)
        {
            _id = id;
        }

        public int Id
        {
            get { return _id; }
        }

        public string Name { get; set; }

        public int Age { get; set; }

        public override string ToString()
        {
            return string.Format("{0} - {1} (Age: {2})", Id, Name, Age);
        }
    }
}
