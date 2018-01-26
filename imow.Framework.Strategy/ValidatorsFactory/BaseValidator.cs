using FluentValidation;

namespace imow.Framework.Strategy.ValidatorsFactory
{
    public class BaseValidator<T> : AbstractValidator<T> where T : class
    {
        protected BaseValidator()
        {
            PostInitialize();
        }

    
        protected virtual void PostInitialize()
        {

        }

     /// <summary>
     /// 从数据库中获取长度限制
     /// </summary>
     /// <typeparam name="TObject"></typeparam>
     /// <param name="dbContext"></param>
     /// <param name="filterPropertyNames"></param>
        protected virtual void SetStringPropertiesMaxLength<TObject>( params string[] filterPropertyNames)
        {
        
        }
    }
}