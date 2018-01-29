using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using FluentValidation;
using imow.Framework.Strategy.ValidatorsFactory;
using imow.Model.EntityModel.Admin;

namespace imow.AdminEtl.Areas.Admin.Validators
{
    public class SchoolValidator :BaseValidator<SchoolEntity>
    {

        public SchoolValidator()
        {
            RuleFor(model => model.Name).NotNull().NotEmpty().WithMessage("校区名称不能为空");
        }
    }
}
