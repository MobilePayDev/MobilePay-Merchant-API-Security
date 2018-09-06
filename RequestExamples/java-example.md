```java
package mobilepay.client;

import io.jsonwebtoken.Jwts;
import io.jsonwebtoken.SignatureAlgorithm;
import org.apache.commons.codec.digest.DigestUtils;
import java.nio.ByteBuffer;
import java.nio.charset.StandardCharsets;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.security.KeyFactory;
import java.security.PrivateKey;
import java.security.spec.PKCS8EncodedKeySpec;
import java.util.Arrays;
import java.util.Base64;
import java.util.Optional;
import static org.apache.commons.codec.digest.MessageDigestAlgorithms.SHA_1;

public abstract class MobilePaySignatureMapper {

    public static Optional<String> getSignatureFor(
        final String url,
        final byte[] keyFile
    ) {
        try {
            final ByteBuffer byteBuffer = StandardCharsets.UTF_8.encode(url); // UTF8
            final byte [] digest = new DigestUtils(SHA_1).digest(byteBuffer); // SHA1
            final String payload = Base64.getEncoder().encodeToString(digest); // Base64
            final PrivateKey privateKey = getPrivateKey(keyFile);

            String signature = Jwts
                .builder()
                .setPayload(payload)
                .signWith(privateKey, SignatureAlgorithm.RS256)
                .compact();

            return Optional.of(signature);

        } catch (Exception e) {
            return Optional.empty();    
        }
    }

    private static PrivateKey getPrivateKey(final byte[] keyFile) throws Exception {
        final Path keyPath = Paths.get("Absolute/Path/To/Your/Key");
        final byte[] bytes = Files.readAllBytes(keyPath);
        final PKCS8EncodedKeySpec keySpec = new PKCS8EncodedKeySpec(bytes);
        final KeyFactory keyFactory = KeyFactory.getInstance("RSA");
        final PrivateKey privateKey = keyFactory.generatePrivate(keySpec);
        return privateKey;
    }
}
```