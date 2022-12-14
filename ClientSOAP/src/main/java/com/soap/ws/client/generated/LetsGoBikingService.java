
package com.soap.ws.client.generated;

import java.net.MalformedURLException;
import java.net.URL;
import javax.xml.namespace.QName;
import javax.xml.ws.Service;
import javax.xml.ws.WebEndpoint;
import javax.xml.ws.WebServiceClient;
import javax.xml.ws.WebServiceException;
import javax.xml.ws.WebServiceFeature;


/**
 * This class was generated by the JAX-WS RI.
 * JAX-WS RI 2.3.2
 * Generated source version: 2.2
 * 
 */
@WebServiceClient(name = "LetsGoBikingService", targetNamespace = "http://tempuri.org/", wsdlLocation = "http://localhost:8090/LetsGoBikingService?singleWsdl")
public class LetsGoBikingService
    extends Service
{

    private final static URL LETSGOBIKINGSERVICE_WSDL_LOCATION;
    private final static WebServiceException LETSGOBIKINGSERVICE_EXCEPTION;
    private final static QName LETSGOBIKINGSERVICE_QNAME = new QName("http://tempuri.org/", "LetsGoBikingService");

    static {
        URL url = null;
        WebServiceException e = null;
        try {
            url = new URL("http://localhost:8090/LetsGoBikingService?singleWsdl");
        } catch (MalformedURLException ex) {
            e = new WebServiceException(ex);
        }
        LETSGOBIKINGSERVICE_WSDL_LOCATION = url;
        LETSGOBIKINGSERVICE_EXCEPTION = e;
    }

    public LetsGoBikingService() {
        super(__getWsdlLocation(), LETSGOBIKINGSERVICE_QNAME);
    }

    public LetsGoBikingService(WebServiceFeature... features) {
        super(__getWsdlLocation(), LETSGOBIKINGSERVICE_QNAME, features);
    }

    public LetsGoBikingService(URL wsdlLocation) {
        super(wsdlLocation, LETSGOBIKINGSERVICE_QNAME);
    }

    public LetsGoBikingService(URL wsdlLocation, WebServiceFeature... features) {
        super(wsdlLocation, LETSGOBIKINGSERVICE_QNAME, features);
    }

    public LetsGoBikingService(URL wsdlLocation, QName serviceName) {
        super(wsdlLocation, serviceName);
    }

    public LetsGoBikingService(URL wsdlLocation, QName serviceName, WebServiceFeature... features) {
        super(wsdlLocation, serviceName, features);
    }

    /**
     * 
     * @return
     *     returns ILetsGoBikingService
     */
    @WebEndpoint(name = "BasicHttpBinding_ILetsGoBikingService")
    public ILetsGoBikingService getBasicHttpBindingILetsGoBikingService() {
        return super.getPort(new QName("http://tempuri.org/", "BasicHttpBinding_ILetsGoBikingService"), ILetsGoBikingService.class);
    }

    /**
     * 
     * @param features
     *     A list of {@link javax.xml.ws.WebServiceFeature} to configure on the proxy.  Supported features not in the <code>features</code> parameter will have their default values.
     * @return
     *     returns ILetsGoBikingService
     */
    @WebEndpoint(name = "BasicHttpBinding_ILetsGoBikingService")
    public ILetsGoBikingService getBasicHttpBindingILetsGoBikingService(WebServiceFeature... features) {
        return super.getPort(new QName("http://tempuri.org/", "BasicHttpBinding_ILetsGoBikingService"), ILetsGoBikingService.class, features);
    }

    private static URL __getWsdlLocation() {
        if (LETSGOBIKINGSERVICE_EXCEPTION!= null) {
            throw LETSGOBIKINGSERVICE_EXCEPTION;
        }
        return LETSGOBIKINGSERVICE_WSDL_LOCATION;
    }

}
