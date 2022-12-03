package com.soap;

import com.soap.ws.client.generated.ILetsGoBikingService;
import com.soap.ws.client.generated.LetsGoBikingService;

import java.io.IOException;
import java.util.Scanner;

public class Main {
    public static void main(String[] args) throws IOException {

        String destination;
        String origin;

        System.out.println("Hi! Welcome to Lets Go Biking");
        System.out.println("Where are you heading today?");
        Scanner scanner = new Scanner(System.in);
        destination = scanner.nextLine();
        System.out.println("What is your starting point address?");
        origin = scanner.nextLine();
        System.out.println("Looking for directions...");
        LetsGoBikingService l = new LetsGoBikingService();
        ILetsGoBikingService i = l.getBasicHttpBindingILetsGoBikingService();
        System.out.println(i.getItinerary(origin, destination));
    }
}
