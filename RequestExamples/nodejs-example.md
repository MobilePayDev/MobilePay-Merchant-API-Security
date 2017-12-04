The following shows how to create a signed payload that is send in the "AuthenticationSignature" header and sending a request with it.
The packages used can all be found in npm.

    'use strict';
    
    const fs = require('fs');
    const request = require('superagent');
    const pem = require('pem');
    const sha1 = require('sha1');
    const jws = require('jws');
    
    let privateKey = "";
    
    // NOTE: This is async, use properly. Path is relative to file location.
    fs.readFile(__dirname + "/path/to/your/merchant.pfx", (err, file) => {
      pem.readPkcs12(file, { p12Password: "{yourpasshere}" }, (err, cert) => {
        privateKey = cert.key;
      });
    });
    
    
    function createSignature(payload, cb) {
      // ensure utf8
      payload = Buffer.from(payload).toString();
      // sha1 (as an array of bytes and not string, which is default for sha1())
      payload = Buffer.from(sha1(payload), "hex");
      // base64
      payload = Buffer.from(payload).toString("base64");
      
      // and finally, the jws (not jwt!)
      return jws.createSign({
        header: { alg: "RS256" },
        privateKey: privateKey,
        payload: payload
      }).on('done', cb);
    }
    
    const url = "https://api.mobeco.dk/appswitch/api/v1/merchants/your_merchant_id/orders/some_order_id";
    createSignature(url, (signature) => {
      request
        .get(url)
        .set("Ocp-Apim-Subscription-Key", primaryKey)
        .set("AuthenticationSignature", signature)
        .end((err, data) => {
            data = JSON.parse(data.text);
        });
    });
