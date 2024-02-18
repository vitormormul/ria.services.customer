[This link has the requirements for the programs in this repository.](https://docs.google.com/document/d/1uxOVrUi6g3qwTDAe8uAFMlJWVMhQDwXg/edit?usp=sharing&ouid=117061835670392908130&rtpof=true&sd=true)


To build the docker file.
```shell
docker build --file ./Customer.Client.Dockerfile . -t customer_client
```

```shell
docker build --file ./Customer.Web.Dockerfile . -t customer_web
```

```shell
docker build --file ./Payment.Denomination.Dockerfile . -t payment_denomination
```

> Obs: multiple containers might share the same volume for the same image.
Even though you might terminate and delete them, once you start a new one it will share
and have the same data as the previous one.


To build images before starting the containers.
```shell
docker-compose build --no-cache
```