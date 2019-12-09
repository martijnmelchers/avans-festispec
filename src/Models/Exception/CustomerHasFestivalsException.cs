﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Festispec.Models.Exception
{
    public class CustomerHasFestivalsException : System.Exception
    {
        public CustomerHasFestivalsException()
        {
        }

        public CustomerHasFestivalsException(string message) : base(message)
        {
        }

        public CustomerHasFestivalsException(string message, System.Exception inner) : base(message, inner)
        {
        }
    }
}
