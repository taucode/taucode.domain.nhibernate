using NHibernate;
using System;
using TauCode.Domain.NHibernate.Tests.Domain.Users;

namespace TauCode.Domain.NHibernate.Tests.Base
{
    public static class TestHelper
    {
        public static readonly UserId AkId = new UserId("ca2c8f0a-00eb-49a3-b495-cd14fe830b0f");
        public static readonly UserId OliaId = new UserId("54bdcb8b-f3d9-4944-97d6-e5716dc1fe66");
        public static readonly UserId IraId = new UserId("413d4230-e179-4c0d-a6e6-fc641c1d9c24");

        public static readonly Guid NonExistingId = new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee");

        public static void DoInTransaction(this ISession session, Action action)
        {
            using (var tran = session.BeginTransaction())
            {
                action();

                tran.Commit();
            }
        }
    }
}
