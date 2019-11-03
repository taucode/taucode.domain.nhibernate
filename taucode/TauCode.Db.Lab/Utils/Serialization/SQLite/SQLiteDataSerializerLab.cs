using System;
using System.Data;
using TauCode.Db.Model;
using TauCode.Db.Utils.Building;
using TauCode.Db.Utils.Building.SQLite;
using TauCode.Db.Utils.Crud;
using TauCode.Db.Utils.Crud.SQLite;
using TauCode.Db.Utils.Inspection;
using TauCode.Db.Utils.Inspection.SQLite;
using TauCode.Db.Utils.Serialization;

namespace TauCode.Db.Lab.Utils.Serialization.SQLite
{
    public class SQLiteDataSerializerLab : DataSerializerBase
    {
        private const int MONEY_TYPE_PRECISION = 19;
        private const int MONEY_TYPE_SCALE = 4;

        public SQLiteDataSerializerLab()
        {
        }

        protected override ICruder CreateCruder()
        {
            return new SQLiteCruder();
        }

        protected override IScriptBuilder CreateScriptBuilder()
        {
            return new SQLiteScriptBuilder
            {
                CurrentOpeningIdentifierDelimiter = '[',
            };
        }

        protected override IDbInspector GetDbInspector(IDbConnection connection)
        {
            return new SQLiteInspector(connection);
        }

        protected override ParameterInfo GetParameterInfo(TableMold tableMold, string columnName)
        {
            throw new NotImplementedException();
        }
    }
}
