﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimQCore.Library.Events
{
    class SingleEvent
    {
        private BaseSensor _baseSensor;
        private double _p;
        public SingleEvent(double p)
        {
            _baseSensor = new BaseSensor();
        }

        public bool Generate()
        {
            return _p >= _baseSensor.Next();
        }
    }
}