using System.ComponentModel.DataAnnotations;
using android.Enumerables;

namespace android.Tools;

[AttributeUsage(AttributeTargets.Property)]
public class MaxAttribute : ValidationAttribute
{
    private readonly int value;
    public MaxAttribute(EMaxValue maxValue)
    {
        value = (int)maxValue;
    }

    protected override ValidationResult? IsValid(object? parameter, ValidationContext validationContext)
    {
        if (parameter is string strValue && strValue.Length > value)
        {
            return new ValidationResult(@$"Trường {validationContext.DisplayName}
                phải là 1 chuỗi có độ dài tối đa : {value}.");
        }
        if (value is int intValue && intValue < 0 || intValue > value)
        {
            return new ValidationResult(@$"Trường {validationContext.DisplayName}
                phải là 1 số nhỏ hơn {value}.");
        }
        return ValidationResult.Success;
    }
}