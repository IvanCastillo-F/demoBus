using System.ComponentModel.DataAnnotations;
using System;
using BusProyectApi.Data;

public class ForeignKeyExistsAttribute : ValidationAttribute
{
    private readonly Type _entityType;
    private readonly string _propertyName;

    public ForeignKeyExistsAttribute(Type entityType, string propertyName)
    {
        _entityType = entityType;
        _propertyName = propertyName;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return ValidationResult.Success;
        }

        var dbContext = (ApplicationDBContext)validationContext.GetService(typeof(ApplicationDBContext));
        var entity = dbContext.Find(_entityType, value);

        if (entity == null)
        {
            return new ValidationResult($"The {_propertyName} with value {value} does not exist.");
        }

        return ValidationResult.Success;
    }
}

