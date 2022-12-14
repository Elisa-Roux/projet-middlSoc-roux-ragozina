using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Apache.NMS;
using Apache.NMS.ActiveMQ;

namespace LetsGoBikingRoutingLibrary
{
    internal class ActiveMQProducer
    {
        static ISession session;
        static IMessageProducer producer;
        static IConnection connection;


        public static void activeMQInitialize()
        {
            // Create a Connection Factory.
            Uri connecturi = new Uri("activemq:tcp://localhost:61616");
            ConnectionFactory connectionFactory = new ConnectionFactory(connecturi);

            // Create a single Connection from the Connection Factory.
            connection = connectionFactory.CreateConnection();
            connection.Start();

            // Create a session from the Connection.
            session = connection.CreateSession();

            // Use the session to target a queue.
            IDestination destination = session.GetQueue("LetsGoBiking");

            // Create a Producer targetting the selected queue.
            producer = session.CreateProducer(destination);
        }

        public static void activeMQSendMessage(String directions)
        {
            //create a message and send it to a queue
            ITextMessage message = session.CreateTextMessage(directions);
            //send a message with a lifetime of 1 minute, non-persistent in the database so that we can purge the queue semi-automatically
            producer.Send(message:message,deliveryMode:MsgDeliveryMode.NonPersistent, priority: MsgPriority.Normal, timeToLive:TimeSpan.FromMinutes(1));
        }

        public static void activeMQClose()
        {
            session.Close();
            connection.Close();
        }

    }
}
