# **Let's go biking project** :bike:
_RAGOZINA Yana - ROUX Elisa_

## Description :book:
This project is made up of a server that gives all the instructions necessary for a client to go from a point A to a point B using JCDecaux bikes and walking. 

## How to deploy the solution :hammer:

### First method

- Launch the `... script` to launch the servers, make sur activeMq is activated (see last section for more information) 
- 

## Helpers in case of fails :link:

- If the ActiveMQProducer.cs file is not automatically imported when you open the solution, you can add it manually
- 
## Specifities 

 - The APIs used in this project are the JCDecaux Open API and the Open Route service API, as they are open sourced. 

- There are actually two servers, a main server handling all the logic around finding the routes and a proxy-cache making the requests to the JCDecaux API in order to avoid being banned because making too much requests. 

- The instructions are sent to the client using a queue. Every time the client asks (usually every second), the server will send him the next step of his itinerary. 

- 

## Out of scope :x:

This project does not allow the client to update dynamically his itinerary. Once an itinerary is found and given to the the client, if he does not restart the whole process, the itinerary will not change. This could mean that by the time the client reaches the stations, bikes or stands might not be available anymore...
Also, we will consider that it is none of our responsibility to be within the legal limits that a client cannot use a bike outside of the city he picked it from because we believe it more interesting for him to have a complete itinerary.
Lastly, the client cannot go too far away from Europe. For sure , no contracts with stations will be proposed (we have removed them) but we do not prevent the customer
