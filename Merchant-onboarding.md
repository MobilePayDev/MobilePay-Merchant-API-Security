# Technical documentation for onboarding merchants
This page offers a step by step guide on how to get onboarded as a user of one of MobilePay's APIs (e.g. AppSwitch or Connect). The technical documentation should be read as a supplement to [this guide](Merchant - API communication.pptx).

The overall steps involved in the onboarding process are outlined here:

1.	Sign up to the MobilePay API on the developer portal [here](https://developer.mobeco.dk)
2.	Login and navigate to Products and click to subscribe to "Merchant Security" product.
3.	Navigate to APIS and select the Merchant Security API
4.	Under the "Public key" endpoint press "Try it" and "Send" to receive the public key.
5.	Save the PublicKey value found in the response to a file (e.g. mobilepay.crt).
6.	Navigate to Products, click to subscribe to your API product of interest e.g. "MobilePay AppSwitch" or "MobilePay Connect" and await approval by a MobilePay API Administrator. If you cannot see your product of interest, contact MobilePay and we will make sure you have the necessary rights.
7.	Prepare [your own public key] (https://github.com/MobilePayDev/MobilePay-Merchant-API-Security/blob/master/Merchant-onboarding.md#your-own-public-key) for registration. That is, use MobilePay's public key to encrypt your public key and your AppSwitch/Connect subscription key to a file. We provide a command line encryption tool to help you create the encrypted file. See how to use the tool [below] ( https://github.com/MobilePayDev/MobilePay-Merchant-API-Security/blob/master/Merchant-onboarding.md#encryption-tool).
8. After you have generated the encrypted file (file with .enc ending) you need to send it to developer@mobilepay.dk. In the email please add your Merchant Id (APP(DK/NO/FI)XXXXXXXXXX), AppSwitch primary subscription key and your own certificate's expiration date*.

*You can find certificates expiration date by double clicking the created certificate and in the bottom of opened window you can find validation dates.

Attention: Please do not call "MobilePay AppSwich" or "Connect" endpoints from https://developer.mobeco.dk site. Calling endpoints within the site will result in "401 Unauthorized" response. 

Attention: If you are onboarding with several merchant id's, you will have to create an account and encrypted file for each of the merchant id's.

### Your own public key
If you do not have a certificate ready to use, one option is to buy one from a trusted authority like e.g. GlobalSign or Symantec. Another option is to create and sign a certificate yourself. Please remember to never disclose the private key. The private key is contained in the .pvk and .pfx files.

The public key certificate MUST be created to support the RSA 256 algorithm, and this means that the cryptograhic service provider should be specified to be "Microsoft Enhanced RSA and AES Cryptographic Provider" or Type 024. This is important in the certificate creation.

For examples of generating a public key certificate see
- [OpenSSL](https://github.com/MobilePayDev/MobilePay-Merchant-API-Security/blob/master/CreateCertificateExamples/OpenSSL.txt)
- [MakeCert](https://github.com/MobilePayDev/MobilePay-Merchant-API-Security/blob/master/CreateCertificateExamples/makeCert.cmd)

### Encryption tool
We have created a tool to help you encrypt your public key and subscription key to a file. This tool is a small command line Windows application which takes a number of arguments. The argument list is as follows:</br>

	Argument 1: Your AppSwitch/Connect subscription key. Primary key found at https://developer.mobeco.dk/developer.
	Argument 2: Path to the crt file containing your public key. MobilePay uses your public key to verify all your future requests.
	Argument 3: Path to the crt file containing MobilePay's public key. MobilePay's public key is used to encrypt the payload.
	Argument 4 (Optional): An optional argument specifying the file location of the encrypted payload. The default is merchant.enc in the current directory.

The Windows based tool to do this can be found in the [source folder](https://github.com/MobilePayDev/MobilePay-Merchant-API-Security/tree/master/Encrypter/Source) of this repository.
The compiled windows binaries can be found [here](https://github.com/MobilePayDev/MobilePay-Merchant-API-Security/tree/master/Encrypter/Bin)

The source code is supplied with the tool, so it also serves as an example if using the tool is not an option. It is important to note that if the encryption is done without the use of the encreyption tool, your crt public key file must be base64 encoded.

### Encrypt your public key and subscription key

Below is an example of how the clear text format of the encrypted file you must send to MobilePay should be. Please be adviced, that you should never send us the clear text, but encrypt it using the provided encryption tool or similar code.

	Base64("eYour_Merchant_Public_Key") + ';' + "Your_MobilePay_Subscription_Key" E.g.:
	
	-----BEGIN CERTIFICATE-----
	MIIEkjCCA3qgAwIBAgIJAImpOqc7 ... eRjrnmQTCbAkw==
	-----END CERTIFICATE-----;c9808c643 ... 4bdc735281
