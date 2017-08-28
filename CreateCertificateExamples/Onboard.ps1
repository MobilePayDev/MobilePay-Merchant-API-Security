# This script creates a certificate, and encrypts the public key with MobilePay's public key and stores it in a .enc file.

Param(
[Parameter(Mandatory=$true,HelpMessage="MobilePay merchant to create a signature certificate for.")]
[string]$MerchantId,

[Parameter(Mandatory=$true,HelpMessage="Subscription id for this merchant")]
[string]$SubscriptionId,

[Parameter(Mandatory=$false,HelpMessage="Mobilepay public key certificate used to encrypt merchant certificate. Defaults to mobilepay.crt.")]
[System.IO.FileInfo]$MobilePayCertificate = [System.IO.FileInfo]::new("$PSScriptRoot\mobilepay.crt"),

[Parameter(Mandatory=$false,HelpMessage="Merchant certificate to use instead of creating a new one")]
[System.IO.FileInfo]$MerchantCertificate = $null)

# Check that the mobile pay public key certificate is available
if($MobilePayCertificate.Exists -ne $true) {
    Write-Error ("Could not locate the mobile pay certificate: " + $MobilePayCertificate.FullName)
    return
}

$mobilePayCert = [System.Security.Cryptography.X509Certificates.X509Certificate2]::new($MobilePayCertificate.FullName)

# Creates a certificate for the merchant, puts it in Local Machine\My, and returns it.
# It will have subject name set to AppSwitch-$subscriptionId
function Create-StoreCertificate
{
    $result = (New-SelfSignedCertificate -KeyExportPolicy Exportable -Provider "Microsoft Strong Cryptographic Provider" -Subject "CN=AppSwitch,CN=Subscription-$SubscriptionId,CN=Merchant-$MerchantId" -KeyAlgorithm RSA -KeyUsage KeyEncipherment,DigitalSignature -KeyLength 2048 -FriendlyName "AppSwitch Certificate for $MerchantId($SubscriptionId)" -HashAlgorithm Sha512)
    Write-Host "Created $($result.Subject) certificate in personal certificates for local machine."
    return $result
}

# Creates PEM from a X509Certificate2
function Export-Pem([System.Security.Cryptography.X509Certificates.X509Certificate2] $certificate) {
    return "-----BEGIN CERTIFICATE-----`r`n$([System.Convert]::ToBase64String($certificate.Export([System.Security.Cryptography.X509Certificates.X509ContentType]::Cert)))`r`n-----END CERTIFICATE-----"
}

# Encrypts data using the algorithm specified in Encrypter/Source/EncryptionHelpers/RSAEncryptionService.cs
function Encrypt-Data {
    Param([System.Security.Cryptography.RSACryptoServiceProvider]$rsa, $inputBytes)

    $input = [byte[]]$inputBytes
    [int]$keySize = $rsa.KeySize / 8
    $maxLength = $keySize - 42
    $dataLength = $input.length
    $iterations = [Math]::Floor($dataLength / $maxLength);
    
    $result = [System.Collections.Generic.List[Byte]]::new()

    For($i = 0; $i -le $iterations; $i++) {
        $len = $dataLength - ($maxLength * $i)
        if($len -gt $maxLength) {
            $len = $maxLength
        }
        
        $tempBytes = [byte[]][Array]::CreateInstance([Byte], $len)
        [Buffer]::BlockCopy($input, $maxLength * $i, $tempBytes, 0, $len)

        $encryptedBytes = $rsa.Encrypt($tempBytes, $true)
        $result.AddRange([BitConverter]::GetBytes([Int]$encryptedBytes.Length))
        $result.AddRange($encryptedBytes)
    }
    return $result.ToArray()
}

# If the user has specified a certificate use that instead of creating a new one.
if($MerchantCertificate -ne $null) {
    $merchantCert = [System.Security.Cryptography.X509Certificates.X509Certificate2]::new($MerchantCertificate.FullName)
} else {
    $merchantCert = Create-StoreCertificate
}

[string]$pem = (Export-Pem -certificate $merchantCert)
$encryptor = $mobilePayCert.PublicKey.Key -as [System.Security.Cryptography.RSACryptoServiceProvider]
$encryptedData = Encrypt-Data -rsa $encryptor -input ([System.Text.Encoding]::ASCII.GetBytes(("$pem;$SubscriptionId")))
[System.IO.File]::WriteAllBytes("$PSScriptRoot\merchant-$MerchantId-$SubscriptionId.enc", $encryptedData)
