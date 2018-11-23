using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;

namespace GaoJD.Club.Core
{
    public class ClassicMovieAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return true;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            //实体类
            var obj = validationContext.ObjectInstance;

            PropertyInfo propertyInfo = validationContext.ObjectType.GetProperty(validationContext.MemberName);
            if (propertyInfo != null && propertyInfo.PropertyType == typeof(string))
            {
                propertyInfo.SetValue(obj, ((string)value).Trim());

            }

            return base.IsValid(value, validationContext);
        }
    }
}
