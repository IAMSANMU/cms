using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebImow.Common.Framework.Strategy.UI
{
   public class MetaModel
   {
       public List<string> Keys;

       public Dictionary<string, MetaModel> InnerModel;

       public MetaModel()
       {
           Keys=new List<string>();
           InnerModel=new Dictionary<string, MetaModel>();
       }
   }
}
