**Veneer** is a web service for sharing common content between web sites.

The **veneer-client** repository contains a client API for consuming the **Veneer Service**

Disclaimer: This solution is currently under development and is not currently available for public consumption.

#Getting started

The solution contains the following projects:

##Veneer.Client

.NET Client API for retrieving content from the **Veneer Service**

##Veneer.Contracts

Data contracts representing the Veneer object model, used by **Veneer.Client**

##Veneer.Client.Web

Sample **ASP.NET MVC** web site implementation, demonstrating the use of the **Veneer.Client** API to retrieve content and integrate in a branded web site.

##Unit Tests

Each project has its own unit test project, named with a **.Tests** suffix.

#Usage

##Types of content available

The **Veneer Service** allows the following content types to be requested, which are represented in the **ContentTypes** enumeration in the **Veneer.Contracts** assembly:

 * Footer
 * FatFooter
 * HeaderWithMegaNav
 * HeaderWithoutMegaNav

##.NET

Content can be retrieved by creating an instance of the client and requesting the type of content required, as shown below:

		var client = new ContentClient();
		
		var content = client.Get(ContentTypes.Footer);

The **Content** object that is returned contains:

 * Date/time when the content was refreshed
 * A list of sections which contain:
	* An identifier
	* HTML for the requested content
	* A list of stylesheets (if appropriate) for the requested content
	* A list of scripts (if appropriate) for the requested content
	
Currently the **Veneer Service** will only return a single section for each type of content, this feature is left for future expansion.
		
##Other languages

The **Veneer Service** is presented as a REST-ful endpoint, so it can be consumed by other languages such as Javascript.

The service can return either XML or JSON to clients, depending on whether **application/xml** or **application/json** have been supplied in the Accept: and Content-Type: headers.

The service endpoint has the following format (the _serviceendpoint_ domain name will depend on the URL that you have been supplied with if you are granted access to the service):

		http://serviceendpoint/api/Content/Get?section=contentsection
		
The _contentsection_ value supplied should be one of the values in the **ContentTypes** enumeration.



