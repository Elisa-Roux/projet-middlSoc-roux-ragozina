package com.soap;

import com.soap.ws.client.generated.ILetsGoBikingService;
import com.soap.ws.client.generated.LetsGoBikingService;

public class Main {
    public static void main(String[] args) {

        System.out.println("Hello World! we are going to test a SOAP client written in Java");
        LetsGoBikingService l = new LetsGoBikingService();
        ILetsGoBikingService i = l.getBasicHttpBindingILetsGoBikingService();
        System.out.println(i.getItinerary("2400 route des dolines", "aix en provence"));
    }
}
