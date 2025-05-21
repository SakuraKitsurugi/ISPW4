> [!IMPORTANT]
> Grade: 10/10
## Description:
**Your goal is to implement the RSA digital signature scheme. You are allowed to use cryptographic libraries.**
## Application 1 – Signing & Sending:
- Message Signing:
  - [x] Input a message and generate its RSA signature.
- Communication:
  - [x] Send the message, signature, and public key to Application 2 via sockets.
- Explanation:
  - [x] Explain how the signature is created.
## Application 2 – Tampering Proxy:
- Data Handling:
  - [x] Receive the message, signature, and public key from Application 1.
  - [x] Allow the user to modify the digital signature manually.
- Forwarding: 
  - [x] Send the (possibly modified) data to Application 3.
## Application 3 – Verification:
- Receiving Data:
  - [x] Receive the public key, message, and signature from Application 2 via sockets.
- Verification:
  - [x] Validate the signature and display whether it is correct.
- Explanation:
  - [x] Explain the verification process and how tampering affects it.
