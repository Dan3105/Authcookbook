### Definition
The `SameSite` attribute is ==a security feature for web cookies that controls when a browser sends a cookie with a cross-site request==

This primarily to mitigate the risk of cross-site request forgery (CSRF) attacks. It has three main values: `Strict`, `Lax`, and `None`

| Value        | Description                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                   |
| ------------ | ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **`Strict`** | The cookie is only sent in a same-site context. For example, if a user is on `siteA.com` and clicks a link to `siteA.com/page`, the cookie is sent. If they are on `siteB.com` and click a link to `siteA.com/page`, the cookie is not sent.                                                                                                                                                                                                                                                                                  |
| **`Lax`**    | The cookie is sent on **same-site** requests and **on top-level** navigations initiated by a **cross-site** request (e.g., clicking a link from `siteB.com` to `siteA.com`).<br><br>To able to sent cookie with **cross-site** it must satisfy:<br>**1**. The request must be a top-level navigation. You can think of this as equivalent to when the URL shown in the URL bar changes, e.g. a user clicking on a link to go to another site.<br><br>**2**. The request method must be safe (e.g. GET or HEAD, but not POST). |
| **`None`**   | The cookie is sent in all contexts, both same-site and cross-site. For this to work, the cookie must also be marked with the `Secure` attribute, meaning it can only be transmitted over an HTTPS connection.                                                                                                                                                                                                                                                                                                                 |

#### Top-level navigation meaning
TOP LEVEL navigation changes the URL in your address bar. Resources that are loaded by iframe, img tags, and script tags do not change the URL in the address bar so none of them cause TOP LEVEL navigation.

### CSRF (also known as XSRF) attack 
![[Pasted image 20251020101018.png]]
Example
a typical GET request for a $100 bank transfer might look like:
```sh
GET http://netbank.com/transfer.do?acct=PersonB&amount=$100 HTTP/1.1
```

A **hacker** can modify this script so it results in a $100 transfer to their own account. Now the malicious request might look like:

```sh
GET http://netbank.com/transfer.do?acct=AttackerA&amount=$100 HTTP/1.1
```

A bad actor can embed the request into an innocent looking hyperlink:

```html
<a href="http://netbank.com/transfer.do?acct=AttackerA&amount=$100">Read more!</a>
```

****