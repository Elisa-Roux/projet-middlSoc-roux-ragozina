package com.soap;

import com.soap.ws.client.generated.ILetsGoBikingService;
import com.soap.ws.client.generated.LetsGoBikingService;
import org.apache.activemq.ActiveMQConnection;
import org.apache.activemq.ActiveMQConnectionFactory;
import org.apache.activemq.command.ActiveMQQueue;

import javax.jms.Queue;

import javax.jms.*;
import java.util.Scanner;
import java.util.Set;

public class LetsGoBikingClient {

    private String name;
    private String password;
    public static final String RECEIVING_QUEUE = "LetsGoBiking";
    public static final String DEFAULT_BROKER_URL = "tcp://localhost:61616";

    //Client data
    public LetsGoBikingClient(){
        name = "client";
        password = "client";
    }

    public LetsGoBikingClient(String uname, String upassword){
        name = uname;
        password = upassword;
    }

    public String getName() {
        return name;
    }

    public String getPassword() {
        return password;
    }

    public static void main(String[] argv){

        //initialize the client data
        LetsGoBikingClient client = new LetsGoBikingClient();

        //get the client's itinerary starting and end points
        String[] inputData = getUserInput();

        //create a connection
        ConnectionFactory factory = new ActiveMQConnectionFactory(client.getName(), client.getPassword(), DEFAULT_BROKER_URL);
        ActiveMQConnection connection = null;
        try{
            connection = (ActiveMQConnection) factory.createConnection();
            Session session = connection.createSession(false, Session.AUTO_ACKNOWLEDGE);

            Queue rQueue = session.createQueue(RECEIVING_QUEUE);
            //declare a consumer for the receiving queue
            MessageConsumer consumer = session.createConsumer(rQueue);

            //fetch the user's input coordinates and pass them to the SOAP server
            getUserItinerary(inputData[0], inputData[1]);

            connection.start();

            //listen to incoming messages from the SOAP server via the receiving queue
            while(true){
                Message message = consumer.receive();
                if(message instanceof TextMessage){
                    TextMessage textMessage = (TextMessage) message;
                    System.out.println(textMessage.getText());
                    textMessage.acknowledge();
                }
                //dequeue the itinerary steps at our own speed
                Thread.sleep(2000);
            }
        } catch (JMSException e) {
            //display the itinerary if we cannot access the receiving queue
            System.out.println("WARNING :Could not connect to the LetsGoBiking queue");
            System.out.println(getUserItinerary(inputData[0], inputData[1]));
        } catch (InterruptedException e) {
            e.printStackTrace();
        } finally {
            try{
                connection.close();
            } catch (JMSException e) {
                e.printStackTrace();
            }
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
        System.out.println("Looking for directions...");

        //get the LetsGoBiking service
        LetsGoBikingService l = new LetsGoBikingService();
        ILetsGoBikingService i = l.getBasicHttpBindingILetsGoBikingService();

        //pass the coordinates to the server
        return i.getItinerary(origin,destination);
    }
}
