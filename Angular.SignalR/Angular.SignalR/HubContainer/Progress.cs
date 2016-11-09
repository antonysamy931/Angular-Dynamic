using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Angular.SignalR.HubContainer
{
    [HubName("ProgressHub")]
    public class ProgressHub : Hub
    {
        public override Task OnConnected()
        {
            var connectionId = Context.ConnectionId.ToString();
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {            
            return base.OnReconnected();
        }

        public void Send()
        {
            Clients.All.Send("hi");
        }

        public void Send(string message)
        {
            Clients.All.Send(message);
        }
    }
}