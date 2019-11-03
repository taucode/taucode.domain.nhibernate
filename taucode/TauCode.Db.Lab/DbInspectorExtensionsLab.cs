using TauCode.Db.Utils.Building;
using TauCode.Db.Utils.Inspection;

namespace TauCode.Db.Lab
{
    public static class DbInspectorExtensionsLab
    {
        public static void ExecuteScript(this IDbInspector dbInspector, string script)
        {
            var sqls = ScriptBuilderBase.SplitSqlByComments(script);

            foreach (var sql in sqls)
            {
                using (var command = dbInspector.Connection.CreateCommand())
                {
                    command.CommandText = sql;
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
