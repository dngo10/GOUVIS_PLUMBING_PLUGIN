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
    class FixtureUnit : BlockModelBase
    {
        public double INDEX = ConstantName.invalidNum;
        public string TAG = ConstantName.invalidStr;
        public string NUMBER = ConstantName.invalidStr;
        public double CW_DIA = ConstantName.invalidNum;
        public double HW_DIA = ConstantName.invalidNum;
        public double WASTE_DIA = ConstantName.invalidNum;
        public double VENT_DIA = ConstantName.invalidNum;
        public double STORM_DIA = ConstantName.invalidNum;
        public double WSFU = ConstantName.invalidNum;
        public double CWSFU = ConstantName.invalidNum;
        public double HWSFU = ConstantName.invalidNum;
        public double DFU = ConstantName.invalidNum;

        public Point3dModel ventPos;
        public Point3dModel drainPos;
        public Point3dModel hotStub;
        public Point3dModel coldStub;
        public long drainType;
        public double studLength;
    }
}
