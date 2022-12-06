# **Let's go biking project** :bike:
_RAGOZINA Yana - ROUX Elisa_

## Description 
This project is made up of a that gives all the instructions necessary for a client to go from a point A to a point B using JCDecaux bikes and walking. 

## Specifities 

 - The APIs used in this project are the JCDecaux open API and the Open Route service API, as they are open sourced. 

- There are actually two servers, a main server handling all the logic around finding the routes and a proxy-cache making the requests to the JCDecaux API in order to avoid being banned by making too much requests. 

## Out of scope 

This project does not allow the client to update dynamically his itinerary. Once an itinerary is found and given to the the client, if he does not restart the whole process, the itinerary will not change. This could mean that by the time the client reaches the stations, bikes or stands might not be available anymore...
Also, we will consider that it is none of our responsibility to be within the legal limits that a client cannot use a bike outside of the city he picked it from.

## How to deploy the solution

## Helpers in case of fails

