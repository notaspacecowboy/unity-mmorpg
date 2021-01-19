using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Network;
using SkillBridge.Message;

namespace GameServer.Managers
{
    class StatusManager: Singleton<StatusManager>
    {
        public StatusManager()
        {
        }

        public void AddStatusNotify(NetConnection<NetSession> conn, NStatus status)
        {
            if (conn.Session.Response.Notifies == null)
            {
                conn.Session.Response.Notifies = new StatusNotify();
            }

            conn.Session.Response.Notifies.Status.Add(status);
        }


    }
}
