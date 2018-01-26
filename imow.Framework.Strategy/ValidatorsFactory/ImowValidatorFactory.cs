using System;
using FluentValidation;
using FluentValidation.Attributes;
using FluentValidation.Internal;
using Imow.Framework.Engine;

namespace imow.Framework.Strategy.ValidatorsFactory
{
    public class ImowValidatorFactory : AttributedValidatorFactory
    {
        private readonly InstanceCache _cache = new InstanceCache();
        public override IValidator GetValidator(Type type)
        {
            if (type != null)
            {
                var attribute = (ValidatorAttribute)Attribute.GetCustomAttribute(type, typeof(ValidatorAttribute));
                if ((attribute != null) && (attribute.ValidatorType != null))
                {
                    object instance = _cache.GetOrCreateInstance(attribute.ValidatorType,
                                               x => ImowEngineContext.Current.ContainerManager.ResolveUnregistered(x));
                    return instance as IValidator;
                }
            }
            return null;

        }
    }
}