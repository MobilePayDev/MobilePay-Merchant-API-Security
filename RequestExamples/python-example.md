The following shows how to create a signed payload that is send in the "AuthenticationSignature" header.
The jose package is from https://pypi.python.org/pypi/python-jose.


    from jose import jws
    import hashlib
    import base64
    
    privatekey = """-----BEGIN RSA PRIVATE KEY-----
    MIIEowIBAAKCAQEA4r+6Yn411yRI/ooFWHUVY0x7RAlfMeix4EVNjU1irR11cQqb
    VPF8nBsDooH6mxMrQXyWz190MlFR6LwVHz8CEtGQQiN+HrtX+2vaj1uIfQJC8RJe
        <--- Lines deleted ---->
    6BEYsxSIURry3fcQtY0uowKBgQDjLrbpkDlCwhyH17l7zssONI55XwvMx458ng2D
    oaOS+AC41xRBGm66KRcnASNXdLk4JCCJY7tNN5i6ATTcCjXpcScX
    -----END RSA PRIVATE KEY-----"""
    
    payload = 'https://api.mobeco.dk/appswitch/api/v1/merchants/your_merchant_id/orders/some_order_id'
    payload_utf8 = payload.encode("utf-8")
    payload_sha1 = hashlib.sha1(payload_utf8).digest()
    payload_base64 = base64.b64encode(payload_sha1)
    
    payload_signed = jws.sign(payload_base64, privatekey, algorithm='RS256')
    print(payload_signed)
