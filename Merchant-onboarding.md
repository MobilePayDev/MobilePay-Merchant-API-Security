# Technical documentation for onboarding merchants
This page offers a step by step guide on how to get onboarded as a user of one of MobilePay's APIs (e.g. AppSwitch or Connect). The technical documentation should be read as a supplement to [this guide](Merchant - API communication.pptx).

The overall steps involved in the onboarding process are outlined here:

1.	Sign up to the MobilePay API on the developer portal [here](https://developer.mobeco.dk)
2.	Login and navigate to Products and click to subscribe to "Merchant Security" product.
3.	Navigate to APIS and select the Merchant Security API
4.	Under the "Public key" endpoint press "Try it" and "Send" to receive the public key.
5.	Save the PublicKey value found in the response to a file (e.g. mobilepay.crt).
6.	Navigate to Products, click to subscribe to your API product of interest e.g. "MobilePay AppSwitch" or "MobilePay Connect" and await approval by a MobilePay API Administrator. If you cannot see your product of interest, contact MobilePay and we will make sure you have the necessary rights.
7.	Prepare [your own public key] (https://github.com/MobilePayDev/MobilePay-Merchant-API-Security/blob/master/Merchant-onboarding.md#your-own-public-key) for registration.
8.	Send your public key certificate (file with .enc ending) in a zip-file to help@mobilepay.dk. Please include your MobilePay API e-mail, merchant id (APP(DK/FI)XXXXXXXXXX), AppSwitch primary subscription key and your own certificate's expiration date*.

*You can find certificates expiration date by double clicking the created certificate and in the bottom of opened window you can find validation dates.

Attention: Please do not call "MobilePay AppSwich" or "Connect" endpoints from https://developer.mobeco.dk site. Calling endpoints within the site will result in "401 Unauthorized" response. 

Attention: If you are onboarding with several merchant id's, you will have to create an account and encrypted file for each of the merchant id's.

### Your own public key
If you do not have a certificate ready to use, one option is to buy one from a trusted authority like e.g. GlobalSign or Symantec. Another option is to create and sign a certificate yourself. Please remember to never disclose the private key. The private key is contained in the .pvk and .pfx files.

The public key certificate MUST be created to support the RSA 256 algorithm, and this means that the cryptograhic service provider should be specified to be "Microsoft Enhanced RSA and AES Cryptographic Provider" or Type 024. This is important in the certificate creation.

For examples of generating a public key certificate see
- [OpenSSL](https://github.com/MobilePayDev/MobilePay-Merchant-API-Security/blob/master/CreateCertificateExamples/OpenSSL.txt)
- [MakeCert](https://github.com/MobilePayDev/MobilePay-Merchant-API-Security/blob/master/CreateCertificateExamples/makeCert.cmd)
