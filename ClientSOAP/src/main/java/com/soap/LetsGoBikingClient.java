package com.soap;

import com.soap.ws.client.generated.ILetsGoBikingService;
import com.soap.ws.client.generated.LetsGoBikingService;
import org.apache.activemq.ActiveMQConnection;
import org.apache.activemq.ActiveMQConnectionFactory;

import javax.jms.*;
import java.util.Scanner;

public class LetsGoBikingClient implements MessageListener {

    private String name;
    private String password;
    public static final String RECEIVING_QUEUE = "LetsGoBiking";
    public static final String DEFAULT_BROKER_URL = "tcp://localhost:61616";

    public LetsGoBikingClient() {
        name ="client";
        password="client";
    }

    public LetsGoBikingClient(String uname, String upassword){
        name = uname;
        password = upassword;
    }

    public String getPassword() {
        return password;
    }

    public String getName() {
        return name;
    }

    public static void main(String[] args) {

        //initialize the client data
        LetsGoBikingClient client = new LetsGoBikingClient();

        //get the client's itinerary starting and end points
        String[] inputData = getUserInput();

        Connection connection = null;

        ConnectionFactory factory = new ActiveMQConnectionFactory(client.getName(), client.getPassword(), DEFAULT_BROKER_URL);

        try {

            connection = (ActiveMQConnection) factory.createConnection();
            Session session = connection.createSession(false, Session.AUTO_ACKNOWLEDGE);

            Queue rQueue = session.createQueue("LetsGoBiking");

            MessageConsumer consumer = session.createConsumer(rQueue);

            connection.start();

            consumer.setMessageListener(client);

            //fetch the user's input coordinates and pass them to the SOAP server
            getUserItinerary(inputData[0], inputData[1]);

            consumer.close();
            session.close();

        } catch (JMSException e) {
            //display the itinerary if we cannot access the receiving queue
            System.out.println("WARNING :Could not connect to the LetsGoBiking queue");
            System.out.println(getUserItinerary(inputData[0], inputData[1]));
        } finally {
            if (connection != null) {
                try {
                    connection.close();
                } catch (JMSException e) {
                    System.out.println("Could not close an open connection...");
                }
            }
        }
    }

    @Override
    public void onMessage(Message message) {
        try {
            if (message instanceof TextMessage) {
                String text = ((TextMessage) message).getText();
                if ("STOP".equals(text)) {
                    System.out.println("You have arrived at your destination !");
                    message.acknowledge();
                } else {
                    System.out.println(text);
                    message.acknowledge();
                    //consume messages at our own pace
                    Thread.sleep(1000);
                }
            }
        } catch (JMSException e) {
            System.out.println("JMS Exception");
            System.exit(-1);
        } catch (InterruptedException e) {
            e.printStackTrace();
        }
    }

    public static String[] getUserInput(){
        String[] input = new String[2];

        System.out.println("Hi! Welcome to Lets Go Biking");
        System.out.println("Where are you heading today?");
        Scanner scanner = new Scanner(System.in);
        input[1] = scanner.nextLine();
        System.out.println("What is your starting point address?");
        input[0] = scanner.nextLine();
        return input;
    }

    public static String getUserItinerary(String origin, String destination){
        if(origin.isEmpty() || destination.isEmpty() || origin.matches("[0-9]+") || destination.matches("[0-9]+")){
            System.err.println("Couldn't fetch your origin and/or destination, please try again");
            System.exit(-1);
        }
        System.out.println("Processing the coordonates...");

        //get the LetsGoBiking service
        LetsGoBikingService l = new LetsGoBikingService();
        ILetsGoBikingService i = l.getBasicHttpBindingILetsGoBikingService();

        //pass the coordinates to the server
        return i.getItinerary(origin,destination);
    }
}
