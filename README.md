# **Let's go biking project** :bike:
_RAGOZINA Yana - ROUX Elisa_

## Description :book:
This project is made up of a server that gives all the instructions necessary for a client to go from a point A to a point B using JCDecaux bikes and walking. 

## How to deploy the solution :hammer:


- Before starting anything, make sure ActiveMq is running (`activemq start`)

- Then open in **administrator mode** both of the server solutions, LetsGoBikingRoutingLibrary and GenericProxyCache

- Inside LetsGoBikingRoutingLibrary, to avoid any build errors, make sur the NuGet depency package `apache.nms.activemq` is installed 

- Start both of the servers

- Now execute the java command `mvn clean jaxws:wsimport`

- Finally launch the java client (run LetsGoBikingClient.java)


## Helpers in case of fails :link:

- If the ActiveMQProducer.cs file is not automatically imported when you open the solution, you can add it manually
- If there were build errors when launching the Java Client, try re-running `mvn clean jaxws:wsimport` then rebuild the project
- If Maven cannot find necessary dependencies (build error), do File -> Invalidate Caches / Restart

## Specifities 

 - The APIs used in this project are the JCDecaux Open API and the Open Route service API, as they are open sourced. 

- There are actually two servers, a main server handling all the logic around finding the routes and a proxy-cache making the requests to the JCDecaux API in order to avoid being banned because making too much requests. 

- The instructions are sent to the client using a queue every second using the ActiveMQ broker. The client receives the next step of their itinerary by reading the instructions enqueued by the server, every second as well. 


## Out of scope :x:

This project does not allow the client to update dynamically his itinerary. Once an itinerary is found and given to the the client, if he does not restart the whole process, the itinerary will not change. This could mean that by the time the client reaches the stations, bikes or stands might not be available anymore...
Also, we will consider that it is none of our responsibility to be within the legal limits that a client cannot use a bike outside of the city he picked it from because we believe it more interesting for him to have a complete itinerary.
Lastly, the client cannot go too far away from Europe. For sure , no contracts with stations will be proposed (we have removed them) but we do not prevent the customer
