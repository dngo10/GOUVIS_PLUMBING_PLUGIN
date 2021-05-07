using ClassLibrary1.DATABASE.DBModels.BaseBlockModel;
using GouvisPlumbingNew.DATABASE.DBModels;
using GouvisPlumbingNew.HELPERS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.DATABASE.DBModels
{
    class TableModel : BlockModelBase
    {
        public string ALIAS = ConstantName.invalidStr;
        public string A_VALUE = ConstantName.invalidStr;
    }
}
