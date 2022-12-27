# MirrorMirror

A simple application to print HTTP requests received on specified port(s).

This project was quickly cobbled together to be used in an environment where a fully fledged proxy like Burp isn't feasible. Hence also the code quality, or lack thereof.

The binary is built as a single, self-contained binary for maximal portability.

## Preview

![](https://raw.githubusercontent.com/ZeMooX/MirrorMirror/master/md-img/startup.png)

![](https://raw.githubusercontent.com/ZeMooX/MirrorMirror/master/md-img/requests.png)

## Help

```bash
 __  __ _                     __  __ _
|  \/  (_)__ _ __ _  ___ __ _|  \/  (_)__ _ __ _  ___ __ _
| |\/| | |__` |__` |/ _ |__` | |\/| | |__` |__` |/ _ |__` |
| |  | | |  | |  | | (_) | | | |  | | |  | |  | | (_) | | |
|_|  |_|_|  |_|  |_|\___/  |_|_|  |_|_|  |_|  |_|\___/  |_|


──────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
-h               Print this help.
-p               Comma seperated list of ports to listen on. Default is 8080. (i.e. -p 1337,8080,1234)
-i               IP to bind to. Default is 127.0.0.1. (i.e. -i 192.168.1.123)
```
