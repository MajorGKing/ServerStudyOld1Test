using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Server;

namespace ServerCore
{
    class SessionManager
    {
        static SessionManager _session = new SessionManager();
        public static SessionManager Instance {get {return _session;}}

        int _sessionId = 0;
        Dictionary<int, ClientSession> _sessions = new Dictionary<int, ClientSession>();
        object _lock = new object();

        public ClientSession Generate()
        {
            lock(_lock)
            {
                int sessionId = ++_sessionId;

                ClientSession session = new ClientSession();
                session.SessionId = sessionId;
                _sessions.Add(sessionId, session);

                System.Console.WriteLine($"Connected : {sessionId}");

                return session;
            }
        }

        public ClientSession Find(int id)
        {
            lock (_lock)
            {
                ClientSession session = null;
                _sessions.TryGetValue(id, out session);
                return session;
            }
        }

        public void Remove(ClientSession sessioin)
        {
            lock (_lock)
            {
                _sessions.Remove(sessioin.SessionId);
            }
        }
    }
}