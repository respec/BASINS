using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace atcFtableBuilder
{
    public interface IFTableOperations
    {
        ArrayList GenerateFTable();
    }

    public abstract class FTableCalculator : IFTableOperations
    {
        public enum ChannelType
        {
            NONE, CIRCULAR, RECTANGULAR, TRIANGULAR, TRAPEZOIDAL, PARABOLIC, NATURAL
        };
        public string[] geoInputNames;
        protected ArrayList vectorRowData;
        protected ArrayList vectorColNames;
        public ChannelType CurrentType = ChannelType.NONE;

        public FTableCalculator()
        {
            vectorColNames = new ArrayList();
        }
        public ArrayList GetColumnNames()
        {
            return vectorColNames;
        }
        public virtual ArrayList GenerateFTable()
        {
            return null;
        }
    }
}
