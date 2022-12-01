package com.soap.test;

import com.soap.ws.client.generated.*;

public class Test {

    //COMMENT LE LANCER AU TOUT DEBUT :
    //mvn clean jaxws:wsimport
    //puis Build si ca marche toujours pas

    //Si maven trouve pas les plugins
    /*https://stackoverflow.com/questions/20496239/maven-plugins-can-not-be-found-in-intellij*/

    //si "couldn't find dependency/cannot resolve symbol dans les imports => File/Invalidate caches

    public static void main(String[] args) {
        System.out.println("Hello World! we are going to test a SOAP client written in Java");
        LetsGoBikingService l = new LetsGoBikingService();
        ILetsGoBikingService i = l.getBasicHttpBindingILetsGoBikingService();
        System.out.println(i.getItinerary("2400 route des dolines", "aix en provence"));
    }
}
