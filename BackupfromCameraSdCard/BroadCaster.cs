using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupfromCameraSdCard
{
    public class Broadcaster
    {
        readonly List<IHandleEvent> _eventList;
        static Broadcaster _eventBroadcast;
        static Broadcaster()
        {
            _eventBroadcast = new Broadcaster();
        }

        protected Broadcaster()
        {
            _eventList = new List<IHandleEvent>();
        }

        public static Broadcaster GetObject()
        {
            return _eventBroadcast;
        }

        public void Register(IHandleEvent handler)
        {
            lock(_eventList)
            _eventList.Add(handler);
        }

        public void NotifyRegistered(CopyEvent copyInfo)
        {
            lock(_eventList)
            {
                _eventList.AsParallel()
                    .ForAll(x => x.Notify(copyInfo));
            }
        }
    }
}