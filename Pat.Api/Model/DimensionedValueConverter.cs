﻿using System;
using System.ComponentModel;
using System.Globalization;

namespace Pat.Api.Model
{
    public class DimensionedValueConverter : TypeConverter 
    {

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) 
        {
            if (sourceType == typeof(string))
                return true;
            
            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) 
        {
            if (value is string s)
                return (DimensionedValue) s;

            return base.ConvertFrom(context, culture, value);
        }
    }
}