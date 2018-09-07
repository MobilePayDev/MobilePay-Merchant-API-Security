# Merchant requests to the MobilePay AppSwitch or Connect APIs

### Constructing a request to AppSwitch/Connect
In order to correctly identify and authenticate your call to the MobilePay API, two headers must be added to the Httprequest:
* Your merchant subscription key ("Ocp-Apim-Subscription-Key")
    NOTE: Please check if subscription key is the same as in encrypted file!
* Your signature ("AuthenticationSignature")

Your merchant subscription key was obtained when you subscribed to the requested API at https://developer.mobeco.dk 

The signature format must be of the type JWS (JSON Web Signature) which is specified [here](https://developer.pingidentity.com/en/resources/jwt-and-jose.html#jwt).

The JWS payload used to construct the token must be:

    Base64(SHA1HASH(UTF8("RequestURI"+"HTTP body")))

In case a GET is sent, the body is the empty string.

### Signature creation in different languages
Below you can find examples of how to create the signed payload used in the AuthenticationSignature header

* [.Net](RequestExamples/dotnet-example.md)
* [Python](RequestExamples/python-example.md)
* [Java](RequestExamples/java-example.md)
