using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PDIWT_MS_CZ.ValidationRules
{
    public class Grate0Rule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null)
                return new ValidationResult(false, "该字段不能为空值！");
            if (string.IsNullOrEmpty(value.ToString()))
                return new ValidationResult(false, "该字段不能为字符串！");
            double _reuslt;
            if (!double.TryParse(value.ToString(), out _reuslt))
                return new ValidationResult(false, "该字段不能转换为浮点数！");
            if (_reuslt <= 0)
                return new ValidationResult(false, "该字段输入值应大于0！");
            return new ValidationResult(true, null);
        }
    }
}
