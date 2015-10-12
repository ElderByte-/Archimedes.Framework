using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Archimedes.Framework.DI.Attribute;
using Archimedes.Framework.Stereotype;

namespace Archimedes.Framework.Test.ConfigurationTest.Values
{
    [Service]
    public class TestValueService
    {
        
        [Value("${test.simpleBool}")]
        private bool _simpleBool;

        [Value("${test.simpleBool2}")]
        private bool _simpleBool2;

        [Value("${test.simpleBoolNegative}")]
        private bool simpleBoolNegative;

        [Value("${test.simpleBoolNegative2}")]
        private bool simpleBoolNegative2;


        [Value("${test.simpleBoolOpt}")]
        private bool? simpleBoolOpt;

        [Value("${test.simpleBoolNegativeOpt}")]
        private bool? simpleBoolNegativeOpt;

        [Value("${unset}")]
        private bool _simpleBoolUnset;

        [Value("${unset2}")]
        private bool? _simpleBoolUnsetOpt;


        public bool SimpleBool
        {
            get { return _simpleBool; }
        }

        public bool SimpleBool2
        {
            get { return _simpleBool2; }
        }

        public bool SimpleBoolNegative
        {
            get { return simpleBoolNegative; }
        }

        public bool SimpleBoolNegative2
        {
            get { return simpleBoolNegative2; }
        }

        public bool? SimpleBoolOpt
        {
            get { return simpleBoolOpt; }
        }

        public bool? SimpleBoolNegativeOpt
        {
            get { return simpleBoolNegativeOpt; }
        }

        public bool BoolUnset
        {
            get { return _simpleBoolUnset; }
        }

        public bool? BoolUnsetOpt {
            get { return _simpleBoolUnsetOpt; }
        }
    }
}
